﻿using Assignment.Api.Interfaces.PlayerPulseInterfaces;
using Assignment.Api.Models;
using Assignment.Api.Models.PlayerPulseModel;
using Assignment.Api.Models.PlayerPulseModels;
using Assignment.Service.Model.PlayerPulseModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;

namespace Assignment.Service.Services.PlayerPulseServices
{
    public class PPTeamService
    {
        private readonly PPIDBTeamRepository _teamRepository;
        private readonly PPIDBAuctionRepository _auctionRepository;
        private readonly PPIDBUserRepository _userRepository;
        private readonly PPIDBPlayerRepository _playerRepository;

        public PPTeamService(PPIDBTeamRepository teamRepository, PPIDBAuctionRepository auctionRepository, PPIDBUserRepository userRepository, PPIDBPlayerRepository playerRepository)
        {
            _teamRepository = teamRepository;
            _auctionRepository = auctionRepository;
            _userRepository = userRepository;
            _playerRepository = playerRepository;
        }

        public async Task<PPTeamRS> CreateTeamAsync(PPTeamRQ team)
        {

            if (team.Logo == null)
            {
                throw new ArgumentException("Team Logo is required.");
            }

            byte[] logo;
            using (var stream = team.Logo.OpenReadStream())
            {
                using (var memoryStream = new MemoryStream())
                {
                    team.Logo.CopyTo(memoryStream);
                    logo = memoryStream.ToArray();
                }
            }

            var existingTeam = await _teamRepository.GetTeamByCodeAsync(team.TeamCode);

            if (existingTeam != null)
            {
                throw new ArgumentException("Team with this code already exists.");
            }


            var entity = new Team
            {
                TeamCode = team.TeamCode,
                Name = team.Name,
                Logo = logo,
                IsDeleted = false,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            var newTeam = await _teamRepository.CreateTeamAsync(entity);

            return new PPTeamRS
            {
                Id = newTeam.Id,
                TeamCode = newTeam.TeamCode,
                Name = newTeam.Name
            };
        }

        public async Task<IEnumerable<PPTeamRS>> GetAllTeamsAsync()
        {

            var teams =  await _teamRepository.GetAllTeamsAsync();

            return teams.Select(teams => new PPTeamRS
            {
                Id = teams.Id,
                TeamCode = teams.TeamCode,
                Name = teams.Name
                
            });
            
        }

        public async Task<PPTeamRS> GetTeamByCodeAsync(string teamCode)
        {
            var team =  await _teamRepository.GetTeamByCodeAsync(teamCode);

            if (team == null)
            {
                return null;
            }

            return new PPTeamRS
            {
                Id = team.Id,
                TeamCode = team.TeamCode,
                Name = team.Name
            };
        }

        public async Task DeleteTeamAsync(string teamCode)
        {
            var team = await _teamRepository.GetTeamByCodeAsync(teamCode);

            if (team == null)
            {
                throw new ArgumentException($"Team with code {teamCode} not found.");
            }

            team.IsDeleted = true;

            await _teamRepository.DeleteTeamAsync(team);

        }

        public async Task AssignTeamManagerAsync(string teamCode, int userId)
        {
            var user = await _userRepository.GetUserByIdAsync(userId);

            var existingTeamsManaged = await _teamRepository.GetTeamManagersByUserIdAsync(userId);

            if (existingTeamsManaged.Count >= 1)
            {
                throw new ArgumentException("User cannot manage more than one team");
            }

            if (user.RoleId != 3)
            {
                throw new ArgumentException("This user does not have permission of a Team Manager");
            }

            if (user == null)
            {
                throw new ArgumentException("User not found");
            }

            if (!user.IsVerified)
            {
                throw new ArgumentException("User verification required to assign as team manager");

            }

            var team = await _teamRepository.GetTeamByCodeAsync(teamCode);

            if (team == null)
            {
                throw new ArgumentException("Team not found");
                
            }

            var existingManager = await _teamRepository.IsUserTeamManagerAsync(userId, team.Id);

            if (existingManager)
            {
                throw new ArgumentException("User already manages a team");
            }

            await _teamRepository.AssignTeamManager(team.Id, userId);
        }

        public async Task<PPAuctionTeamRS> RegisterTeamForAuctionAsync(string teamCode, int auctionId)
        {
            var teamId = await _teamRepository.GetTeamIdByCodeAsync(teamCode);

            var auctionRule = await _auctionRepository.GetAuctionRuleByIdAsync(auctionId);

            var relevantRule = auctionRule.FirstOrDefault(rule => rule.RuleId == 3);

            var initialBudget = relevantRule?.RuleValue ?? 50000000;

            var team = await _teamRepository.GetTeamByCodeAsync(teamCode);

            if (team == null)
            {
                throw new ArgumentException("Team with this code does not exist.");
            }

            var auction = await _auctionRepository.GetAuctionByIdAsync(auctionId);

            if (auction == null)
            {
                throw new ArgumentException("Auction not found");
            }

            var currentTime = DateTime.UtcNow;

            if (currentTime < auction.RegistrationStartTime)
            {
                throw new ArgumentException("Registration for this auction hasn't started yet");
            }
            else if (currentTime > auction.RegistrationEndTime)
            {
                throw new ArgumentException("Registration for this auction has closed");
            }

            var teamAuctionRegistration = new AuctionTeam
            {
                TeamId = teamId,
                AuctionId = auctionId,
                RegistrationTime = DateTime.UtcNow,
                BudgetAmount = initialBudget,
                BalanceAmount = initialBudget,
                UpdatedAt = DateTime.UtcNow,
            };

            var auctionTeams = await _teamRepository.RegisterTeamForAuctionAsync(teamAuctionRegistration);

            return new PPAuctionTeamRS
            {
                Id = auctionTeams.Id,
                TeamId = auctionTeams.TeamId,
                AuctionId = auctionTeams.AuctionId,
                RegistrationTime = auctionTeams.RegistrationTime,
                BudgetAmount = auctionTeams.BudgetAmount,
                BalanceAmount = auctionTeams.BalanceAmount
            };
        }

        public async Task<IEnumerable<object>> GetAllTeamsAuctionDataByAuctionId(int auctionId)
        {
            var teams = await _teamRepository.GetAllTeamsAuctionDataByAuctionId(auctionId);

            var teamsDto = teams.Select(teams => new
            {
                teams.Id,
                teams.AuctionId,
                teams.TeamId,
                teams.BudgetAmount,
                teams.BalanceAmount
            });

            return teamsDto;
        }

        public async Task<PPAuctionTeamRS> GetTeamAuctionData(string teamCode, int auctionId)
        {
            var team = await _teamRepository.GetTeamAuctionData(teamCode, auctionId);

            return new PPAuctionTeamRS
            {
                Id = team.Id,
                AuctionId =  team.AuctionId,
                TeamId = team.TeamId,
                RegistrationTime = team.RegistrationTime,
                BudgetAmount = team.BudgetAmount,
                BalanceAmount = team.BalanceAmount
            };

        }

        public async Task<IEnumerable<PPTeamPlayerRS>> GetTeamRosterAsync(string teamCode)
        {
            var teamPlayers = await _teamRepository.GetTeamPlayersByTeamCodeAsync(teamCode);

            if (!teamPlayers.Any())
            {
                throw new ArgumentException("Player Details not found.");
            }

            return teamPlayers.Select(tp => new PPTeamPlayerRS
            {
                Id = tp.Id,
                PlayerId = tp.PlayerId,
                Status = tp.Status,
                ContractStartDate = tp.ContractStartDate,
                ContractEndDate = tp.ContractEndDate,
                PurchasedAmount = tp.PurchasedAmount,
            });
        }
    }
}

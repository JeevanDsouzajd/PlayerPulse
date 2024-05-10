using Assignment.Api.Interfaces.PlayerPulseInterfaces;
using Assignment.Api.Models.PlayerPulseModel;
using Assignment.Api.Models.PlayerPulseModels;
using Assignment.Service.Model.PlayerPulseModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Assignment.Service.Services.PlayerPulseServices
{
    public class PPAuctionService
    {
        private readonly PPIDBAuctionRepository _auctionRepository;
        private readonly PPIDBSportRepository _sportRepository;

        public PPAuctionService(PPIDBAuctionRepository auctionRepository, PPIDBSportRepository sportRepository)
        {
            _auctionRepository = auctionRepository;
            _sportRepository = sportRepository;
        }

        public async Task<PPAuctionRS> CreateAuctionAsync(BiddingType bidding, PPAuctionRQ auction)
        {
            var sport = await _sportRepository.GetSportIdByCodeAsync(auction.SportCode);

            var existingAuction = await _auctionRepository.GetAuctionByTitleAsync(auction.Title);

            if (existingAuction != null)
            {
                throw new ArgumentException("Auction with this title already exists");
            }

            if (auction.StartTime >= auction.EndTime)
            {
                throw new ArgumentException("Start time cannot be after or equal to end time.");
            }

            if (auction.RegistrationStartTime >= auction.RegistrationEndTime)
            {
                throw new ArgumentException("Registration start time cannot be after or equal to registration end time.");
            }

            if (auction.StartTime < DateTime.UtcNow)
            {
                throw new ArgumentException("Invalid Start time.");
            }

            if (auction.RegistrationStartTime < DateTime.UtcNow)
            {
                throw new ArgumentException("Invalid Registration start time.");
            }

            var auctionData = new Auction()
            {
                Title = auction.Title,
                League = auction.League,
                StartTime = auction.StartTime,
                EndTime = auction.EndTime,
                RegistrationStartTime = auction.RegistrationStartTime,
                RegistrationEndTime = auction.RegistrationEndTime,
                Status = StatusType.Upcoming,
                BiddingMechanism = bidding,
                IsActive = false,
                SportId = sport.Id
            };

            var createdAuction = await _auctionRepository.CreateAuctionAsync(auctionData);

            return new PPAuctionRS
            {
                Id = createdAuction.Id,
                Title = createdAuction.Title,
                League = createdAuction.League,
                StartTime = createdAuction.StartTime,
                EndTime = createdAuction.EndTime,
                RegistrationStartTime = createdAuction.RegistrationStartTime,
                RegistrationEndTime = createdAuction.RegistrationEndTime,
                Status = createdAuction.Status.ToString(),
                BiddingMechanism = createdAuction.BiddingMechanism.ToString(),
                SportId = createdAuction.SportId,
                IsActive = createdAuction.IsActive
            };
        }

        public async Task<IEnumerable<PPAuctionRS>> GetAllAuctionsAsync()
        {
            var auctions = await _auctionRepository.GetAllAuctionsAsync();

            return auctions.Select(auction => new PPAuctionRS
            {
                Id = auction.Id,
                Title = auction.Title,
                League = auction.League,
                StartTime = auction.StartTime,
                EndTime = auction.EndTime,
                RegistrationStartTime = auction.RegistrationStartTime,
                RegistrationEndTime = auction.RegistrationEndTime,
                Status = auction.Status.ToString(),
                BiddingMechanism = auction.BiddingMechanism.ToString(),
                SportId = auction.SportId,
                IsActive = auction.IsActive
            });
        }

        public async Task<PPAuctionRS> GetAuctionByIdAsync(int auctionId)
        {
            var auction =  await _auctionRepository.GetAuctionByIdAsync(auctionId);

            if (auction == null)
            {
                return null;
            }

            return new PPAuctionRS
            {
                Id = auction.Id,
                Title = auction.Title,
                League = auction.League,
                StartTime = auction.StartTime,
                EndTime = auction.EndTime,
                RegistrationStartTime = auction.RegistrationStartTime,
                RegistrationEndTime = auction.RegistrationEndTime,
                Status = auction.Status.ToString(),
                BiddingMechanism = auction.BiddingMechanism.ToString(),
                SportId = auction.SportId,
                IsActive = auction.IsActive
            };
        }

        public async Task<PPAuctionRuleRS> AssignRuleToAuctionAsync(RuleEnum rule, PPAuctionRuleRQ auctionRuleRQ)
        {

            var ruleId = await _auctionRepository.GetRuleIdByRuleType(rule);

            var existingRule = await _auctionRepository.GetAuctionRuleByAuctionIdAndRuleIdAsync(auctionRuleRQ.AuctionId, ruleId);
            if (existingRule != null)
            {
                throw new ArgumentException("This rule is already assigned to the auction.");
            }

            var auctionRule = new AuctionRule
            {
                AuctionId = auctionRuleRQ.AuctionId,
                RuleId = ruleId,
                RuleValue = auctionRuleRQ.RuleValue
            };

            var createdAuctionRule =  await _auctionRepository.AssignRuleToAuctionAsync(auctionRule);

            return new PPAuctionRuleRS
            {
                AuctionId = createdAuctionRule.AuctionId,
                RuleId = createdAuctionRule.RuleId,
                RuleValue = createdAuctionRule.RuleValue
            };
        }

        public async Task<List<PPAuctionRuleRS>> GetAuctionRuleByIdAsync(int auctionId)
        {
            var auctionRules = await _auctionRepository.GetAuctionRuleByIdAsync(auctionId);

            if(auctionRules == null)
            {
                throw new ArgumentException("Rule not found");
            }

            var auctionRuleResponses = auctionRules.Select(rule => new PPAuctionRuleRS
            {
                AuctionId = rule.AuctionId,
                RuleId = rule.RuleId,
                RuleValue = rule.RuleValue
            });

            return auctionRuleResponses.ToList();
        }

    }
}

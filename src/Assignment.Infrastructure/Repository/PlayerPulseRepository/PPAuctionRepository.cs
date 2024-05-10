using Assignment.Api.Interfaces.PlayerPulseInterfaces;
using Assignment.Api.Models.PlayerPulseModel;
using Assignment.Api.Models.PlayerPulseModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment.Infrastructure.Repository.PlayerPulseRepository
{
    public class PPAuctionRepository : PPIDBAuctionRepository
    {
        private readonly PlayerPulseContext _context;

        public PPAuctionRepository(PlayerPulseContext context)
        {
            _context = context;
        }

        public async Task<Auction> CreateAuctionAsync(Auction auction)
        {
            _context.Auctions.Add(auction);
            await _context.SaveChangesAsync();
            return auction;
        }

        public async Task<IEnumerable<Auction>> GetAllAuctionsAsync()
        {
            return await _context.Auctions.ToListAsync();
        }

        public async Task<Auction> GetAuctionByIdAsync(int auctionId)
        {
            return await _context.Auctions.FindAsync(auctionId);
        }

        public async Task<AuctionRule> AssignRuleToAuctionAsync(AuctionRule auctionRule)
        {

            _context.AuctionRules.Add(auctionRule);
            await _context.SaveChangesAsync();
            return auctionRule;
        }

        public async Task<List<AuctionRule>> GetAuctionRuleByIdAsync(int auctionId)
        {
            return await _context.AuctionRules.Where(ar => ar.AuctionId == auctionId).ToListAsync();
        }

        public async Task<int> GetRuleIdByRuleType(RuleEnum rule)
        {
            var rules =  await _context.Rules.ToListAsync();
            var ruleData = rules.FirstOrDefault(r => r.RuleType == rule);
            return ruleData.Id;
        }

        public async Task UpdateAuctionAsync(Auction auction)
        {
            _context.Auctions.Update(auction);
            await _context.SaveChangesAsync();
        }

        public async Task<Auction> GetAuctionByTitleAsync(string title)
        {
            return await _context.Auctions
                .FirstOrDefaultAsync(a => a.Title == title);
        }

        public async Task<AuctionRule> GetAuctionRuleByAuctionIdAndRuleIdAsync(int auctionId, int ruleId)
        {
            return await _context.AuctionRules
                .FirstOrDefaultAsync(ar => ar.AuctionId == auctionId && ar.RuleId == ruleId);
        }
    }
}

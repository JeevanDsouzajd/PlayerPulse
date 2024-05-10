using Assignment.Api.Models.PlayerPulseModel;
using Assignment.Api.Models.PlayerPulseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment.Api.Interfaces.PlayerPulseInterfaces
{
    public interface PPIDBAuctionRepository
    {
        Task<Auction> CreateAuctionAsync(Auction auction);

        Task<IEnumerable<Auction>> GetAllAuctionsAsync();

        Task<Auction> GetAuctionByIdAsync(int auctionId);

        Task<List<AuctionRule>> GetAuctionRuleByIdAsync(int auctionId);

        Task<AuctionRule> AssignRuleToAuctionAsync(AuctionRule auctionRule);

        Task<int> GetRuleIdByRuleType(RuleEnum rule);

        Task UpdateAuctionAsync(Auction auction);

        Task<Auction> GetAuctionByTitleAsync(string title);

        Task<AuctionRule> GetAuctionRuleByAuctionIdAndRuleIdAsync(int auctionId, int ruleId);

    }
}

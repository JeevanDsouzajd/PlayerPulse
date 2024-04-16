using System;
using System.Collections.Generic;

namespace Assignment.Api.Models;

public partial class AuctionTeam
{
    public int Id { get; set; }

    public int AuctionId { get; set; }

    public int TeamId { get; set; }

    public DateTime RegistrationTime { get; set; }

    public virtual Auction Auction { get; set; }

    public virtual Team Team { get; set; }
}

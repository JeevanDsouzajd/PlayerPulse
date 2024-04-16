using System;
using System.Collections.Generic;

namespace Assignment.Api.Models;

public partial class TeamPlayer
{
    public int Id { get; set; }

    public int TeamId { get; set; }

    public int PlayerId { get; set; }

    public string PlayerName { get; set; }

    public string Status { get; set; }

    public DateTime ContractStartDate { get; set; }

    public DateTime ContractEndDate { get; set; }

    public decimal? PurchasedAmount { get; set; }

    public virtual Player Player { get; set; }

    public virtual Team Team { get; set; }
}

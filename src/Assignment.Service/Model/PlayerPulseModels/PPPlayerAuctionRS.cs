﻿using Assignment.Api.Models.PlayerPulseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment.Service.Model.PlayerPulseModels
{
    public class PPPlayerAuctionRS
    {
        public int PlayerId { get; set; }

        public string Category { get; set; }

        public decimal? ValuatedPrice { get; set; }

        public PlayerAuctionStatus Status { get; set; }
    }
}

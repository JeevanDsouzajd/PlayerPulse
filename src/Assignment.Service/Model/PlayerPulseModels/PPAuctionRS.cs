using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment.Service.Model.PlayerPulseModels
{
    public class PPAuctionRS
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string League { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public DateTime RegistrationStartTime { get; set; }

        public DateTime RegistrationEndTime { get; set; }

        public string Status { get; set; }

        public bool? IsActive { get; set; }

        public string BiddingMechanism { get; set; }

        public int SportId { get; set; }
    }
}

using Assignment.Api.Models.PlayerPulseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment.Service.Model.PlayerPulseModels
{
    public class PPAuctionRQ
    {
        public string Title { get; set; }

        public string League { get; set; }

        public string SportCode { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public DateTime RegistrationStartTime { get; set; }

        public DateTime RegistrationEndTime { get; set; }

    }
}

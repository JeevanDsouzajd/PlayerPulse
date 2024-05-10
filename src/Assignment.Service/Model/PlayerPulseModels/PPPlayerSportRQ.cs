using Assignment.Api.Models;
using Assignment.Api.Models.PlayerPulseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment.Service.Model.PlayerPulseModels
{
    public class PPPlayerSportRQ
    {
        public string PlayerCode { get; set; }
        public string SportCode { get; set; }
        public LevelType Level { get; set; }
        
    }
}

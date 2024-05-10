using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment.Service.Model.PlayerPulseModels
{
    public class PPPlayerUpdateRQ
    {
        public string Name { get; set; }

        [Phone]
        public string Mobile { get; set; }

        public decimal? BasePrice { get; set; }
    }
}

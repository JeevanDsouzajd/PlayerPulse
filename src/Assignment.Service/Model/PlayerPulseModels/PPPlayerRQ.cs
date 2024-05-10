using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment.Service.Model.PlayerPulseModels
{
    public class PPPlayerRQ
    {
        public string PlayerCode { get; set; }

        public string Name { get; set; }

        public string Gender { get; set; }

        [Phone]
        public string Mobile { get; set; }

        public int Age { get; set; }

        public decimal? BasePrice { get; set; }
    }
}

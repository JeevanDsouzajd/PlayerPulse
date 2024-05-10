using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment.Service.Model.PlayerPulseModels
{
    public class PPPlayerRS
    {
        public int Id { get; set; }

        public string PlayerCode { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string Gender { get; set; }

        public string Mobile { get; set; }

        public int Age { get; set; }

        public decimal? BasePrice { get; set; }

        public bool? Sold { get; set; }

        public int? UserId { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

    }
}

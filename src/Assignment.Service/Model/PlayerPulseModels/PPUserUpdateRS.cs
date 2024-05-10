using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment.Service.Model.PlayerPulseModels
{
    public class PPUserUpdateRS
    {
        public string Username { get; set; }

        public string Email { get; set; }

        public DateTime UpdatedAt { get; set; }
    }
}

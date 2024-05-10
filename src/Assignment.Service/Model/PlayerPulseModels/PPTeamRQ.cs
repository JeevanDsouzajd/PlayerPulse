using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment.Service.Model.PlayerPulseModels
{
    public class PPTeamRQ
    {
        [Required]
        public string TeamCode { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public IFormFile Logo { get; set; }
    }
}

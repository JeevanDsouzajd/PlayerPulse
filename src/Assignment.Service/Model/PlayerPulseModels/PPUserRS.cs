using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment.Service.Model.PlayerPulseModels
{
    public class PPUserRS
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public int? RoleId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}

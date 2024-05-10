using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment.Service.Model.PlayerPulseModels
{
    public class PPPlayerStatisticsRS
    {
        public int Id { get; set; }

        public int StatisticTypeId { get; set; }

        public decimal Value { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Weather.Domain.Enums;

namespace Weather.Domain.Entities
{
    public class Weather : BaseEntity
    {
        public DateOnly Date { get; set; }
        public TimeOnly Time {get ; set; }
        public double? Temprature { get; set; }
        public double? Humidity { get; set; }
        public double? DewPoint { get; set; }
        public int? Pressure { get; set; }
        public string? WindDirection { get; set; }
        public int? WindSpeed { get; set; }
        public int? Cloudness { get; set; }
        public int? LowCloudBoundary { get; set; }
        public int? Visibility { get; set; }
        public string? Note { get; set; }
    }
}

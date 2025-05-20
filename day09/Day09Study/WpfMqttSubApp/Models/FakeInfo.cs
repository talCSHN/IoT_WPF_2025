using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfMqttSubApp.Models
{
    public class FakeInfo
    {
        [Key]
        public DateTime Sensing_dt { get; set; }
        [Key]
        public string Pub_Id { get; set; }
        public decimal Count { get; set; }
        public float Temp { get; set; }
        public float Humid { get; set; }
        public bool Light { get; set; }
        public bool Human { get; set; }
    }
}

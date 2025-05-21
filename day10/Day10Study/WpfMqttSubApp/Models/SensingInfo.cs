using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace WpfMqttSubApp.Models
{
    public class SensingInfo
    {
        public int L { get; set; }
        public int R { get; set; }
        public float T { get; set; }
        public float H { get; set; }
        public string F { get; set; }
        public string V { get; set; }
        public string RL { get; set; }
        public string CB { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusanRestaurantApp.Models
{
    public class BusanItem
    {
        public int Uc_Seq { get; set; }
        public string Main_Title { get; set; }
        public string Gugun_Nm { get; set; }
        public double Lat { get; set; }
        public double Lng { get; set; }
        public string Place { get; set; }
        public string Title { get; set; }
        public string SubTitle { get; set; }
        public string Addr1 { get; set; }
        public string Addr2 { get; set; }
        public string Cntct_Tel { get; set; }
        public string Homepage_Url { get; set; }
        public string Usage_Day_Week_And_Time { get; set; }
        public string Rprsntv_Menu { get; set; }
        public string Main_Img_Normal { get; set; }
        public string Main_Img_Thumb { get; set; }
        public string ItemCntnts { get; set; }
    }
}

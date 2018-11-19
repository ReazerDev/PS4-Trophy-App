using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace PS4Trophy
{
    class Trophy
    {
        public string trophyName { get; set; }
        public string trophyDescription { get; set; }
        public string status { get; set; }
        public string kind { get; set; }
    }
}
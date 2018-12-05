using System;
using Android.App;
using XMLParser;
using Android.OS;
using Android.Views;
using Android.Widget;
using System.IO;
using Android.Graphics;
using System.Drawing;

namespace PS4Trophy
{
    class addGameDialog : Dialog
    {
        public addGameDialog(Activity activity) : base(activity)
        {

        }

        Button ok;
        TextView gameName;
        TextView trophyGuide;
        TextView title;

        XML parser;

        string pathToFolder = Android.OS.Environment.ExternalStorageDirectory.AbsolutePath + "/PS4 Trophy";

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            RequestWindowFeature((int)WindowFeatures.NoTitle);
            SetContentView(Resource.Layout.GameDialog);

            ok = FindViewById<Button>(Resource.Id.okBtn);
            ok.Click += Ok_Click;
        }

        private void Ok_Click(object sender, EventArgs e)
        {
            title = FindViewById<TextView>(Resource.Id.textview_title);
            gameName = FindViewById<EditText>(Resource.Id.gameName);
            trophyGuide = FindViewById<TextView>(Resource.Id.trophyGuide);

            if(gameName.Text == "" || trophyGuide.Text == "")
            {
                title.Text = "Please give a Name and add a Guide";
                title.SetTextColor(Android.Graphics.Color.Red);
                return;
            }

            string directoryName = gameName.Text;

            if(Directory.Exists(pathToFolder + "/Games/" + directoryName))
            {
                Dismiss();
                return;
            }
            Directory.CreateDirectory(pathToFolder + "/Games/" + directoryName);

            parser = new XML();
            parser.createXML(pathToFolder + "/Games/" + directoryName + "/guide.xml");
            parser.addStartElement("Guide");
            parser.add("link", trophyGuide.Text);
            parser.addEndElement();
            parser.save();
            


            Dismiss();
        }
    }
}
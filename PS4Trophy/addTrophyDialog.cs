using System;
using Android.App;
using XMLParser;
using Android.OS;
using Android.Views;
using Android.Widget;
using System.IO;
using Android.Graphics;
using System.Collections.Generic;

namespace PS4Trophy
{
    class addTrophyDialog : Dialog
    {
        string gameName;

        public addTrophyDialog(Activity activity, string name) : base(activity)
        {
            gameName = name;
        }

        Button ok;
        TextView title;
        EditText trophyName;
        EditText trophyDescription;
        Spinner trophyKind;
        List<string> trophyKinds;
        

        string pathToFolder = Android.OS.Environment.ExternalStorageDirectory.AbsolutePath + "/PS4 Trophy";

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            RequestWindowFeature((int)WindowFeatures.NoTitle);
            SetContentView(Resource.Layout.TrophyDialog);

            ok = FindViewById<Button>(Resource.Id.okBtn);

            ok.Click += Ok_Click;

            trophyKind = FindViewById<Spinner>(Resource.Id.trophyKind);

            trophyKinds = new List<string>();
            trophyKinds.Add("Bronze");
            trophyKinds.Add("Silver");
            trophyKinds.Add("Gold");
            trophyKinds.Add("Platinum");
            ArrayAdapter<string> adapter = new ArrayAdapter<string>(Application.Context, Android.Resource.Layout.SimpleSpinnerItem, trophyKinds);
            adapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            trophyKind.Adapter = adapter;

            
        }

        private void Ok_Click(object sender, EventArgs e)
        {
            int num = (Directory.GetFiles(pathToFolder + "/Games/" + gameName, "*.xml").Length);
            title = FindViewById<TextView>(Resource.Id.textview_title);
            trophyName = FindViewById<EditText>(Resource.Id.trophyName);
            trophyDescription = FindViewById<EditText>(Resource.Id.trophyDescription);
            trophyKind = FindViewById<Spinner>(Resource.Id.trophyKind);

            if (trophyName.Text == "" || trophyDescription.Text == "")
            {
                title.Text = "Please give a Name and add a Description";
                title.SetTextColor(Color.Red);
                return;
            }

            if (File.Exists(pathToFolder + "/XML/" + "trophy" + num.ToString() + ".xml"))
            {
                Dismiss();
                return;
            }


            XML xml = new XML();
            xml.createXML(pathToFolder + "/Games/" + gameName + "/" + "trophy" + num.ToString() + ".xml");
            xml.addStartElement("Trophy");
            xml.add("name", trophyName.Text);
            xml.add("description", trophyDescription.Text);
            xml.add("kind", trophyKind.SelectedItem.ToString());
            xml.add("status", "Not Completed");
            xml.addEndElement();
            Dismiss();
        }
    }
}
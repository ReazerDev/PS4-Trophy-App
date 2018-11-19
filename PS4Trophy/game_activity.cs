using System;
using Android.App;
using Android.Widget;
using Android.OS;
using Android.Support.Design.Widget;
using Android.Support.V7.App;
using Android.Views;
using Android.Content;
using XMLParser;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace PS4Trophy
{
    
    [Activity(Label = "", Theme = "@style/AppTheme.NoActionBar")]
    public class game_activity : AppCompatActivity
    {
        private Android.Support.V7.Widget.Toolbar toolbar;
        string pathToFolder = Android.OS.Environment.ExternalStorageDirectory.AbsolutePath + "/PS4 Trophy";

        ListView trophyListView;
        List<Trophy> trophyList;
        TrophyListAdapter adapter;

        FloatingActionButton addTrophyBtn;

        string gameName = null;
        string trophyGuide = null;

        XML parser;
        XML parser2;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.game);

            gameName = Intent.GetStringExtra("gameName");
            toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.game_toolbar);
            SetSupportActionBar(toolbar);

            toolbar.Title = gameName;

            SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            SupportActionBar.SetHomeButtonEnabled(true);
            

            LinearLayout guideBtn = FindViewById<LinearLayout>(Resource.Id.GuideLayout);
            guideBtn.Click += GuideBtn_Click;

            addTrophyBtn = FindViewById<FloatingActionButton>(Resource.Id.addTrophyBtn);
            addTrophyBtn.Click += AddTrophyBtn_Click;

            trophyListView = FindViewById<ListView>(Resource.Id.trophyListView);
            trophyListView.ItemClick += TrophyListView_ItemClick;
            

            getTrophy();
        }

        public void getTrophy()
        {
            string name = null;
            string description = null;
            string xstatus = null;
            trophyListView = FindViewById<ListView>(Resource.Id.trophyListView);
            trophyList = new List<Trophy>();
            trophyList.Clear();

            if (trophyListView.Count > 0)
            {
                trophyListView.Adapter = null;
            }

            parser = new XML();

            try
            {
                trophyGuide = parser.read(pathToFolder + "/Games/" + gameName + "/guide.xml", "link");
            }
            catch(Exception ex)
            {
                TextWriter tw = new StreamWriter(pathToFolder + "/Games/" + gameName + "/log.txt");
                tw.WriteLine(ex.ToString());
                tw.Dispose();
            }

            DirectoryInfo di = new DirectoryInfo(pathToFolder + "/Games/" + gameName);
            FileInfo[] files = di.GetFiles("*.xml", SearchOption.TopDirectoryOnly);

            foreach (FileInfo file in files)
            {
                if (file.Name == "guide.xml")
                    continue;

                try
                {
                    name = parser.read(file.FullName, "name");
                    description = parser.read(file.FullName, "description");
                    xstatus = parser.read(file.FullName, "status");
                    trophyList.Add(new Trophy() { trophyName = name, trophyDescription = description, status = xstatus });
                }
                catch
                {
                    continue;
                }
            }



            adapter = new TrophyListAdapter(this, trophyList);
            trophyListView.Adapter = adapter;

        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.game_menu, menu);
            return true;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            if (item.ItemId == Android.Resource.Id.Home)
            {
                Intent test = new Intent(this, typeof(MainActivity));
                SetResult(Result.Ok, test);
                Finish();
            }

            if(item.ItemId == Resource.Id.report_problem)
                Toast.MakeText(this, "Report Problem", ToastLength.Short).Show();

            if (item.ItemId == Resource.Id.report_duplicate)
                Toast.MakeText(this, "Report Duplicate", ToastLength.Short).Show();

            if (item.ItemId == Resource.Id.refresh)
                getTrophy();
                Toast.MakeText(this, "Refreshing", ToastLength.Short).Show();

            return base.OnOptionsItemSelected(item);
        }

        private void GuideBtn_Click(object sender, EventArgs e)
        {
            try
            {
                Intent intent = new Intent(Intent.ActionView, Android.Net.Uri.Parse(trophyGuide));
                StartActivity(intent);
            }
            catch
            {
                Android.App.AlertDialog.Builder msg = new Android.App.AlertDialog.Builder(this);
                msg.SetTitle("Error!");
                msg.SetMessage("Link not found!");
                msg.SetPositiveButton("Ok", delegate
                {
                    msg.Dispose();
                });
                msg.Show();
            }
        }

        private void TrophyListView_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            parser = new XML();
            string path = pathToFolder + "/Games/" + gameName + "/trophy" + (e.Position + 1) + ".xml";
            if (parser.read(path, "status") == "Not Completed")
            {
                replace(path, "Not Completed", "Completed");
            }
            else 
            {
                replace(path, "Completed", "Not Completed");
            }
            Task.Delay(300).Wait();
            getTrophy();
        }

        private void AddTrophyBtn_Click(object sender, EventArgs e)
        {
            
            addTrophyDialog dialog = new addTrophyDialog(this, gameName);
            dialog.Show();
            dialog.DismissEvent += Dialog_DismissEvent;
        }

        private void Dialog_DismissEvent(object sender, EventArgs e)
        {
            getTrophy();
        }

        public void replace(string path, string oldChar, string newChar)
        {
            StreamReader tr = new StreamReader(path);
            string alreadyInFile = tr.ReadToEnd();
            tr.Dispose();
            tr.Close();
            TextWriter tw = new StreamWriter(path);
            alreadyInFile = alreadyInFile.Replace(oldChar, newChar);
            tw.WriteLine(alreadyInFile);

            tw.Dispose();
            tw.Close();
        }
    }
}
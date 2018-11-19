using System;
using Android.App;
using Android.Widget;
using Android.OS;
using Android.Support.Design.Widget;
using Android.Support.V7.App;
using Android.Views;
using System.IO;
using System.Collections.Generic;
using Android.Content.PM;
using Android.Runtime;
using Android.Support.V4.Content;
using Android.Support.V4.App;
using Android;
using XMLParser;
using Android.Content;

namespace PS4Trophy
{
	[Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar", MainLauncher = true)]
	public class MainActivity : AppCompatActivity
	{
        string pathToFolder = Android.OS.Environment.ExternalStorageDirectory.AbsolutePath + "/PS4 Trophy";
        List<Game> gamesList;
        ListView gameListView;
        GameListAdapter adapter;
        public FloatingActionButton addGameBtn;

        const int permission_Request = 101;
        private bool permissionGranted = false;
       

        public string name { get; set; }
        public string count { get; set; }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            RequestPermissions();
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.activity_main);
            
                
            Android.Support.V7.Widget.Toolbar toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);

            addGameBtn = FindViewById<FloatingActionButton>(Resource.Id.addGameBtn);
            addGameBtn.Click += addGameBtnOnClick;

            gameListView = FindViewById<ListView>(Resource.Id.gameListView);
            gameListView.ItemClick += GameListView_ItemClick;

            createDirectories();
            getGame();
        }

        private void GameListView_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            Intent intent = new Intent(this, typeof(game_activity));
            intent.PutExtra("gameName", gamesList[e.Position].gameName);
            StartActivityForResult(intent, 0);
        }

        public void getGame()
        {
            gameListView = FindViewById<ListView>(Resource.Id.gameListView);
            gamesList = new List<Game>();
            gamesList.Clear();

            if(gameListView.Count > 0)
            {
                gameListView.Adapter = null;
            }

            DirectoryInfo di = new DirectoryInfo(pathToFolder + "/Games/");
            string[] gameNames = Directory.GetDirectories(pathToFolder + "/Games");

            foreach (string directory in gameNames)
            { 
                    gamesList.Add(new Game() { gameName = directory.Replace(pathToFolder + "/Games/", ""), trophyCountTest = "Trophies: " + (Directory.GetFiles(directory).Length - 1).ToString() });
            }



            adapter = new GameListAdapter(this, gamesList);
            gameListView.Adapter = adapter;

        }
        
        #region RuntimePermissions

        public void RequestPermissions()
        {
            if (Build.VERSION.SdkInt >= BuildVersionCodes.M)
            {
                if (ContextCompat.CheckSelfPermission(this, Manifest.Permission.ReadExternalStorage) != (int)Permission.Granted && ContextCompat.CheckSelfPermission(this, Manifest.Permission.WriteExternalStorage) != (int)Permission.Granted)
                {
                    ActivityCompat.RequestPermissions(this, new String[] { Manifest.Permission.ReadExternalStorage, Manifest.Permission.WriteExternalStorage }, permission_Request);
                }
                else
                {
                    permissionGranted = true;
                }
            }
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Permission[] grantResults)
        {
            switch (requestCode)
            {
                case permission_Request:
                    {
                        // If request is cancelled, the result arrays are empty.
                        if (grantResults.Length > 0 && grantResults[0] == Permission.Granted)
                        {
                            permissionGranted = true;
                            getGame();
                        }
                        else
                        {
                            var msgBox = (new Android.App.AlertDialog.Builder(this)).Create();
                            msgBox.SetTitle("Warning");
                            msgBox.SetMessage("You need to accept the Permission, in order to use the App!");
                            msgBox.SetButton("Ok", delegate
                            {
                                RequestPermissions();
                            });
                            msgBox.SetButton2("Cancel", delegate
                            {
                                System.Environment.Exit(0);
                            });
                            msgBox.Show();
                        }
                        return;
                    }

                    // other 'case' lines to check for other
                    // permissions this app might request
            }
        }

        #endregion

        public void createDirectories()
        {
            if (!Directory.Exists(pathToFolder + "/Games"))
                Directory.CreateDirectory(pathToFolder + "/Games");

            if (!Directory.Exists(pathToFolder + "/Pictures"))
                Directory.CreateDirectory(pathToFolder + "/Pictures");
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.menu_main, menu);
            return true;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            int id = item.ItemId;
            if (id == Resource.Id.action_disable_ads)
            {
                Toast.MakeText(this, "Disable Ads", ToastLength.Short).Show();
            }
            if (id == Resource.Id.action_info)
            {
                Toast.MakeText(this, "Info", ToastLength.Short).Show();
            }
            if (id == Resource.Id.report_bug)
            {
                Toast.MakeText(this, "Report Bug", ToastLength.Short).Show();
            }
            if (id == Resource.Id.refresh)
            {
                getGame();
                Toast.MakeText(this, "Refreshing", ToastLength.Short).Show();
            }

            return base.OnOptionsItemSelected(item);
        }

        private void addGameBtnOnClick(object sender, EventArgs eventArgs)
        {
            addGameDialog dialog = new addGameDialog(this);
            dialog.Show();
            dialog.DismissEvent += Dialog_DismissEvent;
        }

        private void Dialog_DismissEvent(object sender, EventArgs e)
        {
            getGame();
        }

        protected override void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            if(resultCode == Result.Ok)
            {
                getGame();
            }
        }
    }
}


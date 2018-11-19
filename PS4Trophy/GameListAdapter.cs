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
    class GameListAdapter : BaseAdapter<Game>
    {
        public List<Game> finalFiles;
        private Context mcontext;
        string pathToFolder = Android.OS.Environment.ExternalStorageDirectory.AbsolutePath + "/PS4 Trophy/Pictures";

        public GameListAdapter(Context context, List<Game> files)
        {
            finalFiles = files;
            mcontext = context;
        }

        public override int Count
        {
            get { return finalFiles.Count; }
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override Game this[int position]
        {
            get { return finalFiles[position]; }
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            View row = convertView;
            if (row == null)
            {
                row = LayoutInflater.From(mcontext).Inflate(Resource.Layout.gameListViewLayout, null, false);
            }

            TextView txtName = row.FindViewById<TextView>(Resource.Id.gameName);
            txtName.Text = finalFiles[position].gameName;

            TextView txtCount = row.FindViewById<TextView>(Resource.Id.trophyCountTest);
            txtCount.Text = finalFiles[position].trophyCountTest;

            ImageView img = row.FindViewById<ImageView>(Resource.Id.imageView1);
            img.SetImageResource(Resource.Drawable.logo);
            return row;
        }
    }
}
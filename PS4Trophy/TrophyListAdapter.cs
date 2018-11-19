using System.Collections.Generic;
using Android.Graphics;
using Android.Content;
using Android.Views;
using Android.Widget;

namespace PS4Trophy
{
    class TrophyListAdapter : BaseAdapter<Trophy>
    {
        public List<Trophy> finalFiles;
        private Context mcontext;
        string pathToFolder = Android.OS.Environment.ExternalStorageDirectory.AbsolutePath + "/PS4 Trophy";

        public TrophyListAdapter(Context context, List<Trophy> files)
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

        public override Trophy this[int position]
        {
            get { return finalFiles[position]; }
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            View row = convertView;
            if (row == null)
            {
                row = LayoutInflater.From(mcontext).Inflate(Resource.Layout.trophyListViewLayout, null, false);
            }

            TextView txtName = row.FindViewById<TextView>(Resource.Id.trophyName);
            txtName.Text = finalFiles[position].trophyName;

            TextView txtDesc = row.FindViewById<TextView>(Resource.Id.trophyDescription);
            txtDesc.Text = finalFiles[position].trophyDescription;

            TextView txtStatus = row.FindViewById<TextView>(Resource.Id.status);

            if(finalFiles[position].status == "Completed")
            {
                txtStatus.SetTextColor(Color.Green);
            }
            if (finalFiles[position].status == "Not Completed")
            {
                txtStatus.SetTextColor(Color.Orange);
            }
            txtStatus.Text = finalFiles[position].status;

            ImageView imgKind = row.FindViewById<ImageView>(Resource.Id.imgKind);

            if (finalFiles[position].kind == "Platinum")
            {
                imgKind.SetImageResource(Resource.Drawable.platinum);
            }
            if (finalFiles[position].kind == "Gold")
            {
                imgKind.SetImageResource(Resource.Drawable.gold);
            }
            if (finalFiles[position].kind == "Silver")
            {
                imgKind.SetImageResource(Resource.Drawable.silver);
            }
            if (finalFiles[position].kind == "Bronze")
            {
                imgKind.SetImageResource(Resource.Drawable.bronze);
            }
            return row;
        }
    }
}
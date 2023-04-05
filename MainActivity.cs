using Android.Graphics;
using System.Net;

namespace RestSharp_JSON_Flickr_App
{
    [Activity(Label = "@string/app_name", MainLauncher = true)]
    public class MainActivity : Activity
    {
        int count = 0;

        ImageButton btnNext;
        ImageButton btnPrevious;
        ImageView imgPic;
        TextView txtCaption;
        RESTHandler objRest;
        Root objRootList;

        List<Photo> lstPhoto = new List<Photo>();

        protected override void OnCreate(Bundle? savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);
            btnNext = FindViewById<ImageButton>(Resource.Id.btnnext);
            btnPrevious = FindViewById<ImageButton>(Resource.Id.btnprev);
            txtCaption = FindViewById<TextView>(Resource.Id.textView1);
            imgPic = FindViewById<ImageView>(Resource.Id.imageView1);

            btnNext.Click += OnBtnNextClick;
            btnPrevious.Click += OnBtnPreviousClick;

            try
            {
                objRest = new RESTHandler(@"https://www.flickr.com/services/rest/?method=flickr.interestingness.getList");
                objRest.AddParameter("api_key", "cb43328245b41b0bd32729bb1f8474eb");
                objRest.AddParameter("format", "json");
                objRest.AddParameter("nojsoncallback", "1");
                objRest.AddParameter("auth_token", "72157720878338376-ad474be15a86294a");
                objRest.AddParameter("api_sig", "c1eda7313e6e981c06752cdb49eac333");

                objRootList = objRest.ExecuteRequest();
                lstPhoto = objRootList.photos.photo;

                GetImage(count);
            }
            catch(Exception ex)
            {
                Toast.MakeText(this,"Error :" + ex.Message, ToastLength.Long);
            }
        }

        public void GetImage(int count)
        {
            txtCaption.Text = lstPhoto[count].title;
            string imgUrl = "http://farm" + lstPhoto[count].farm.ToString() + ".staticflickr.com/"
                + lstPhoto[count].server + "/" + lstPhoto[count].id + "_" + lstPhoto[count].secret + "_b.jpg";

            Bitmap imageBitmap = null;
            if (!(imgUrl == "null"))
                using (var webClient = new WebClient())
                {
                    var imageBytes = webClient.DownloadData(imgUrl);
                    if (imageBytes != null && imageBytes.Length > 0)
                    {
                        imageBitmap = BitmapFactory.DecodeByteArray(imageBytes, 0, imageBytes.Length);
                    }
                }
            imgPic.SetImageBitmap(imageBitmap);
        }

        public void OnBtnNextClick(object sender, EventArgs e)
        {
            count++;
            if(count < lstPhoto.Count) 
            {
                GetImage(count);
            }
        }
        public void OnBtnPreviousClick (object sender, EventArgs e)
        {
            count--;
            if (count >= 0)
            {
                GetImage(count);
            }
        }
    }
}
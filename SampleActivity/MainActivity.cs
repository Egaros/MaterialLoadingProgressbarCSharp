using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using MaterialLoadingProgressbarCSharp;

namespace SampleActivity
{
    [Activity(Label = "SampleActivity", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        int progress = 0;
        Handler handler;
		CircleProgressBar progress1;
		CircleProgressBar progress2;
		CircleProgressBar progressWithArrow;
		CircleProgressBar progressWithoutBg;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.Main);
			progress1 = FindViewById<CircleProgressBar> (Resource.Id.progress1);
			progress2 = FindViewById<CircleProgressBar> (Resource.Id.progress2);
			progressWithArrow = FindViewById<CircleProgressBar> (Resource.Id.progressWithArrow);
			progressWithoutBg = FindViewById<CircleProgressBar> (Resource.Id.progressWithoutBg);

			progress2.SetColorSchemeResources (Android.Resource.Color.HoloGreenLight);

			progressWithArrow.SetColorSchemeResources (Android.Resource.Color.HoloOrangeLight);
			progressWithoutBg.SetColorSchemeResources (Android.Resource.Color.HoloRedLight);

			handler = new Handler ();
			for (int i = 0; i < 10; i++) {
				int finalI = i;
				handler.PostDelayed (() => {
					if (finalI * 10 >= 90) {
						progress2.Visibility = ViewStates.Invisible;
					} else {
						progress2.Progress = finalI *10;
					}
				}, 1000 * (i + 1));
			}
        }
    }
}


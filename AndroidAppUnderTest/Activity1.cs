using Android.App;
using Android.Util;
using Android.OS;

namespace AndroidAppUnderTest
{
    [Activity(Label = "AndroidAppUnderTest", MainLauncher = true, Icon = "@drawable/icon")]
    public class Activity1 : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.Main);
        }

        public override void OnWindowFocusChanged(bool hasFocus)
        {
            base.OnWindowFocusChanged(hasFocus);
            if (hasFocus)
            {
                Log.Debug("SPEED", "ON CREATE FINISHED");
            }
        }
    }
}


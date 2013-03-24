using System;

using Android.App;
using Android.Widget;
using Android.OS;

namespace Factorizer
{
    [Activity(Label = "Factorizer", MainLauncher = true, Icon = "@drawable/icon")]
    public class Activity1 : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.Main);

            var button = FindViewById<Button>(Resource.Id.MyButton);
            button.Click += RunFactorizer;
        }

        private void RunFactorizer(object sender, EventArgs e)
        {
            var factor = new Factorizer();
            factor.Run();

            Console.WriteLine("Average: {0}", factor.Average );
        }
    }
}


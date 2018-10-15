
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
using ModelLibrary;

namespace Android_NPCCMobileServices
{
    [Activity(MainLauncher = true, Theme = "@style/SplashTheme")]
    public class InitialActivity : Activity
    {
        public npcc_authentication oauth;
        protected override async void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            oauth = new npcc_authentication();
            bool isAuth = await oauth.IsAuthenticatedAsync();

            if (isAuth)
            {
                StartActivity(typeof(HomeActivity));
                Finish();
            }
            else
            {
                StartActivity(typeof(LoginActivity));
                Finish();
            }
        }
    }
}

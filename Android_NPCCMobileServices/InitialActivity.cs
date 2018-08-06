
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
        public MdlNpccAuthentication oauth;
        protected override async void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            oauth = new MdlNpccAuthentication();
            bool isAuth = await oauth.IsAuthenticatedAsync();

            if (isAuth)
            {
                StartActivity(typeof(MainActivity));
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

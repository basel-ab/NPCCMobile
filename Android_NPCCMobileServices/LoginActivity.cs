
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
using Dmax.Dialog;
using ModelLibrary;
using Xamarin.Essentials;
using static ModelLibrary.clsEnum;

namespace Android_NPCCMobileServices
{
    [Activity()]
    public class LoginActivity : Activity
    {
        Button btnLogin;
        EditText txtUsername, txtPassword;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.LoginLayout);

            txtUsername = FindViewById<EditText>(Resource.Id.txtUsername);
            txtPassword = FindViewById<EditText>(Resource.Id.txtPassword);
            btnLogin = FindViewById<Button>(Resource.Id.btnLogin);
            btnLogin.Click += BtnLogin_ClickAsync;
        }

        async void BtnLogin_ClickAsync(object sender, EventArgs e)
        {                   
            if (IsUserNameValid() && IsPasswordValid())
            {
                AlertDialog dialog = new SpotsDialog(this,Resource.Style.CustomDialog);
                dialog.SetMessage("Checking Your Details...");
                dialog.SetCancelable(false);
                dialog.Show();

                MdlNpccAuthentication oauth = new MdlNpccAuthentication();
                clsLoginInfo lg = await oauth.Login(txtUsername.Text, txtPassword.Text);

                dialog.Dismiss();

                //We have successfully authenticated a the user,
                //Now fire our OnLoginSuccess Event.
                if (lg.Authenticated == LoginResault.SuccessfullyAuthenticated)
                {
                    await SecureStorage.SetAsync("oauth_token", lg.Token);
                    //We have successfully authenticated a the user,
                    //Now fire our OnLoginSuccess Event.
                    StartActivity(typeof(MainActivity));
                    Finish();
                }
                else
                {
                    Toast.MakeText(this, "Wrong Username/ Password!", ToastLength.Short).Show();
                }
            }
            else
            {
                Toast.MakeText(this, "Empty Username/ Password!", ToastLength.Short).Show();
            }
        }

        private bool IsUserNameValid()
        {
            return !String.IsNullOrEmpty(txtUsername.Text.Trim());
        }

        private bool IsPasswordValid()
        {
            return !String.IsNullOrEmpty(txtPassword.Text.Trim());
        }


    }
}

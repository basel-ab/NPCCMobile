using Foundation;
using ModelLibrary;
using SVProgressHUDBinding;
using System;
using UIKit;
using Xamarin.Essentials;

namespace IOS_NPCCMobileServices
{
    public partial class LoginController : UIViewController
    {
        //Create an event when a authentication is successful
        public event EventHandler OnLoginSuccess;

        public LoginController (IntPtr handle) : base (handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            NavigationItem.Title = "Login to NPCC";

            btnLogin.TouchUpInside += BtnLogin_TouchUpInsideAsync;
            View.EndEditing(true);

            txtPassword.ShouldReturn+= TxtPassword_ShouldReturn;
        }

        async void BtnLogin_TouchUpInsideAsync(object sender, EventArgs e)
        {
            //Validate our Username & Password.
            if (IsUserNameValid() && IsPasswordValid())
            {
                SVProgressHUD.SetDefaultMaskType(SVProgressHUDMaskType.Black);
                SVProgressHUD.ShowWithStatus("Checking Your Details...");
                NPCCWebServicesModel ws = new NPCCWebServicesModel();
                LoginInfo lg = await ws.Login(txtUsername.Text, txtPassword.Text);
                SVProgressHUD.Dismiss();
                SVProgressHUD.SetDefaultMaskType(SVProgressHUDMaskType.None);
                if(lg.Authenticated == LoginResault.SuccessfullyAuthenticated)
                {
                    await SecureStorage.SetAsync("oauth_token", lg.Token);
                    //We have successfully authenticated a the user,
                    //Now fire our OnLoginSuccess Event.
                    if (OnLoginSuccess != null)
                    {
                        OnLoginSuccess(sender, new EventArgs());
                    }
                }
                else
                {
                    SVProgressHUD.ShowErrorWithStatus("Wrong Password");
                    SVProgressHUD.DismissWithDelay(10);

                }
            }
            else
            {
                SVProgressHUD.ShowErrorWithStatus( "Empty Username/Password");
                SVProgressHUD.DismissWithDelay(10);
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

        public override void TouchesBegan(NSSet touches, UIEvent evt)
        {
            View.EndEditing(true);
        }

        public bool TxtPassword_ShouldReturn(UITextField textField)
        {
            textField.ResignFirstResponder();
            btnLogin.SendActionForControlEvents(UIControlEvent.TouchUpInside);
            return true;
        }

    }
}
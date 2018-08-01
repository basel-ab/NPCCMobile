using CoreGraphics;
using Foundation;
using System;
using System.Collections.Generic;
using UIKit;
using ZXing;
using ZXing.Mobile;

namespace IOS_NPCCMobileServices
{
    public partial class HomeController : UIViewController
    {

        MobileBarcodeScanner scanner;
        CustomOverlayView customOverlay;
        public HomeController (IntPtr handle) : base (handle)
        {
            
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            NavigationItem.Title = "Home";
            scanner = new MobileBarcodeScanner();
            btnNormalScan.TouchUpInside += btnNormalScan_TouchUpInsideAsync;;
            btnAVScan.TouchUpInside += BtnAVScan_TouchUpInsideAsync;
            btnContScan.TouchUpInside += BtnContScan_TouchUpInside;
            btnCustomView.TouchUpInside += BtnCustomView_TouchUpInsideAsync;
        }

        async void btnNormalScan_TouchUpInsideAsync(object sender, EventArgs e)
        {
            //Tell our scanner to use the default overlay
            scanner.UseCustomOverlay = false;
            //We can customize the top and bottom text of the default overlay
            scanner.TopText = "Hold camera up to barcode to scan";
            scanner.BottomText = "Barcode will automatically scan";

            //Start scanning
            var result = await scanner.Scan();

            HandleScanResult(result);
        }

        async void BtnAVScan_TouchUpInsideAsync(object sender, EventArgs e)
        {
            //Tell our scanner to use the default overlay
            scanner.UseCustomOverlay = false;
            //We can customize the top and bottom text of the default overlay
            scanner.TopText = "Hold camera up to barcode to scan";
            scanner.BottomText = "Barcode will automatically scan";

            //Start scanning
            var result = await scanner.Scan (true);

            HandleScanResult (result);  
        }

        void BtnContScan_TouchUpInside(object sender, EventArgs e)
        {//Tell our scanner to use the default overlay
            //Tell our scanner to use the default overlay
            scanner.UseCustomOverlay = false;

            var opt = new MobileBarcodeScanningOptions();
            opt.DelayBetweenContinuousScans = 3000;

            //Start scanning
            scanner.ScanContinuously(opt, HandleScanResult);
        }

        async void BtnCustomView_TouchUpInsideAsync(object sender, EventArgs e)
        {
            //Create an instance of our custom overlay
            customOverlay = new CustomOverlayView();
            //Wireup the buttons from our custom overlay
            customOverlay.ButtonTorch.TouchUpInside += delegate {
                scanner.ToggleTorch();
            };
            customOverlay.ButtonCancel.TouchUpInside += delegate {
                scanner.Cancel();
            };

            //Tell our scanner to use our custom overlay
            scanner.UseCustomOverlay = true;
            scanner.CustomOverlay = customOverlay;

            var result = await scanner.Scan(new MobileBarcodeScanningOptions { AutoRotate = true });

            HandleScanResult(result);
        }



        void HandleScanResult(ZXing.Result result)
        {
            string msg = "";

            if (result != null && !string.IsNullOrEmpty(result.Text))
                msg = "Found Barcode: " + result.Text;
            else
                msg = "Scanning Canceled!";

            this.InvokeOnMainThread(async () => {
                Console.WriteLine(msg);
                var uiAlert = UIAlertController.Create("Scan Stoped", msg, UIAlertControllerStyle.Alert);
                uiAlert.AddAction(UIAlertAction.Create("OK", UIAlertActionStyle.Default, (alertAction) => {
                    alertAction.Dispose();
                    uiAlert.Dispose();
                }));
                await PresentViewControllerAsync(uiAlert, true);
                //var av = new UIAlertView("Barcode Result", msg, null, "OK", null);
                //av.Show();
            });
        }

        [Export("UITestBackdoorScan:")] // notice the colon at the end of the method name
        public NSString UITestBackdoorScan(NSString value)
        {
            UITestBackdoorScan(value.ToString());

            return new NSString();
        }

        public void UITestBackdoorScan(string param)
        {
            var expectedFormat = BarcodeFormat.QR_CODE;
            Enum.TryParse(param, out expectedFormat);
            var opts = new MobileBarcodeScanningOptions
            {
                PossibleFormats = new List<BarcodeFormat> { expectedFormat }
            };

            //Create a new instance of our scanner
            scanner = new MobileBarcodeScanner(this.NavigationController);
            scanner.UseCustomOverlay = false;

            Console.WriteLine("Scanning " + expectedFormat);

            //Start scanning
            scanner.Scan(opts).ContinueWith(t => {
                var result = t.Result;

                var format = result?.BarcodeFormat.ToString() ?? string.Empty;
                var value = result?.Text ?? string.Empty;

                BeginInvokeOnMainThread(() => {
                    var av = UIAlertController.Create("Barcode Result", format + "|" + value, UIAlertControllerStyle.Alert);
                    av.AddAction(UIAlertAction.Create("OK", UIAlertActionStyle.Cancel, null));
                    PresentViewController(av, true, null);
                });
            });
        }

    }
}
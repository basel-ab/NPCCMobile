// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace IOS_NPCCMobileServices
{
    [Register ("GenerateBarcodeController")]
    partial class GenerateBarcodeController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView imageBarcode { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (imageBarcode != null) {
                imageBarcode.Dispose ();
                imageBarcode = null;
            }
        }
    }
}
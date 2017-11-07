// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace DispatchQueueMemoryLeak
{
    [Register ("ViewController")]
    partial class ViewController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton btnTest { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel lblText { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (btnTest != null) {
                btnTest.Dispose ();
                btnTest = null;
            }

            if (lblText != null) {
                lblText.Dispose ();
                lblText = null;
            }
        }
    }
}
using System;
using System.IO;
using Xamarin.Forms; // for Dependecy attribute
using FieldConsole; // for Platform
using Foundation;
using UIKit;
using CoreGraphics;
using ObjCRuntime;

[assembly: Dependency(typeof(FieldConsole.Platform))]

namespace FieldConsole
{
    /// <summary>
    /// platform-SPECIFIC half of class which implements platform specific methods
    /// </summary>
    partial class Platform : NSObject, IPlatform, IUIDocumentInteractionControllerDelegate
    {
        string GetDataDir()
        {
            // use internal storage as that's all iOS devices have!
            return GetInternalPath();
        }

        static UIDocumentInteractionController DIC;

        IntPtr INativeObject.Handle
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public class UIDocumentInteractionControllerDelegateClass : UIDocumentInteractionControllerDelegate
        {
            UIViewController viewC;

            public UIDocumentInteractionControllerDelegateClass(UIViewController controller)
            {
                viewC = controller;
            }

            public override UIViewController ViewControllerForPreview(UIDocumentInteractionController controller)
            {
                return viewC;
            }

            public override UIView ViewForPreview(UIDocumentInteractionController controller)
            {
                return viewC.View;
            }

            public override CGRect RectangleForPreview(UIDocumentInteractionController controller)
            {
                return viewC.View.Frame;
            }
        }

        public bool OpenDataFile(string name, object o)
        {
            string x = Directory.GetFiles(GetDataDir())[0];
            NSUrl url = NSUrl.FromFilename(x);// Path.Combine(GetDataDir(), name));

            //UIActivityViewController activityVC = new UIActivityViewController(
            //                                        new NSObject[]
            //                                        {
            //                                            new NSString("KML Document"),
            //                                            url
            //                                            //new NSString(Path.Combine(GetDataDir(), name))
            //                                        },
            //                                        null);
            //UIViewController mainVC = ((Page)o).CreateViewController();
            ////App.Current.MainPage.cre
            ////UIViewController mainVC = Application.Current.MainPage.CreateViewController();
            //mainVC.PresentViewController(activityVC, true, null);
            //return true;

            //bool q = url.StartAccessingSecurityScopedResource();
            //Uri uri = new Uri(x);
            //Device.OpenUri(uri);
            //return true;
            //NSBundle b = NSBundle.MainBundle;
            //NSUrl u2 = b.GetUrlForResource(Path.GetFileNameWithoutExtension(name), Path.GetExtension(name));
            DIC = UIDocumentInteractionController.FromUrl(url);
            string s = url.ToString();
            DIC.Delegate = new UIDocumentInteractionControllerDelegateClass(((Page)o).CreateViewController());
            Page lp = (Page)o;
            var v = new UIView(new CGRect(lp.X, lp.Y, lp.Width, lp.Height));

            return DIC.PresentOpenInMenu(new UIBarButtonItem(v), true);
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~Platform() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        void IDisposable.Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion



    } // class
} // namespace

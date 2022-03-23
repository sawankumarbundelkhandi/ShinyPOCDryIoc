using Foundation;
using Prism;
using Prism.Ioc;
using UIKit;

[assembly: Shiny.ShinyApplication(
    ShinyStartupTypeName = "ShinyPOCDryIoc.Startup",
    XamarinFormsAppTypeName = "ShinyPOCDryIoc.App"
)]

namespace ShinyPOCDryIoc.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
    }
}

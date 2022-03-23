using Android.App;
using Android.Content.PM;
using Android.OS;
using Prism;
using Prism.Ioc;

[assembly: Shiny.ShinyApplication(
    ShinyStartupTypeName = "ShinyPOCDryIoc.Startup",
    XamarinFormsAppTypeName = "ShinyPOCDryIoc.App"
)]

namespace ShinyPOCDryIoc.Droid
{
    [Activity(Theme = "@style/MainTheme",
              MainLauncher = true,
              ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize)]
    public partial class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
    }
}


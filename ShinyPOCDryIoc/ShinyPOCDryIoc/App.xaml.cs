using Prism;
using Prism.Ioc;
using ShinyPOCDryIoc.ViewModels;
using ShinyPOCDryIoc.Views;
using Xamarin.Essentials;
using Xamarin.Essentials.Implementation;
using Xamarin.Essentials.Interfaces;
using Xamarin.Forms;

namespace ShinyPOCDryIoc
{
    public partial class App
    {
        protected override async void OnInitialized()
        {
            InitializeComponent();

            await NavigationService.NavigateAsync("NavigationPage/MainPage");
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton<IAppInfo, AppInfoImplementation>();
            containerRegistry.RegisterSingleton<ISecureStorage, SecureStorageImplementation>();

            containerRegistry.RegisterForNavigation<NavigationPage>();
            containerRegistry.RegisterForNavigation<MainPage, MainPageViewModel>();
            containerRegistry.RegisterForNavigation<LocationPage, LocationPageViewModel>();
            containerRegistry.RegisterForNavigation<PushPage, PushPageViewModel>();
            containerRegistry.RegisterForNavigation<SpeechPage, SpeechPageViewModel>();
        }

        protected override void OnStart()
        {
            base.OnStart();

            if (VersionTracking.IsFirstLaunchEver)
            {
                SecureStorage.RemoveAll();
            }
        }
    }
}

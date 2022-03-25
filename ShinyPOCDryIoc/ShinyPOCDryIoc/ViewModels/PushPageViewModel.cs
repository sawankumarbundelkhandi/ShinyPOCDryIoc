using System.Threading.Tasks;
using Prism.Commands;
using Prism.Navigation;
using Shiny;
using Shiny.Push;
using Xamarin.Essentials.Interfaces;

namespace ShinyPOCDryIoc.ViewModels
{
    public class PushPageViewModel : ViewModelBase
    {
        private string _token;
        private string _data;
        private readonly ISecureStorage _secureStorage;
        private readonly IPushManager _pushManager;

        public PushPageViewModel(ISecureStorage secureStorage, IPushManager pushManager)
        {
            _secureStorage = secureStorage;
            _pushManager = pushManager;
            RegisterCommand = new DelegateCommand(async () => await RegisterCommandHandler());
        }

        private async Task RegisterCommandHandler()
        {
            var result = await _pushManager.RequestAccess();

            if (result.Status == AccessState.Available)
            {
                await _secureStorage.SetAsync("token", result.RegistrationToken);
                Token = result.RegistrationToken;
            }
        }

        public DelegateCommand RegisterCommand { get; }

        public string Token
        {
            get => _token;
            set => SetProperty(ref _token, value);
        }

        public string Data
        {
            get => _data;
            set => SetProperty(ref _data, value);
        }

        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            Token = await _secureStorage.GetAsync("token");

            if (parameters.GetNavigationMode() == NavigationMode.New && parameters.ContainsKey("date"))
            {
                Data = parameters.GetValue<string>("data");
            }
        }
    }
}
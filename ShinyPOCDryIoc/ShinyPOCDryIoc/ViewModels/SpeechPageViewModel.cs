using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Prism.Commands;
using Prism.Services;
using Shiny.SpeechRecognition;
using Shiny;
using Prism.Navigation;

namespace ShinyPOCDryIoc.ViewModels
{
    public class SpeechPageViewModel : ViewModelBase
    {
        private readonly ISpeechRecognizer _speechRecognizer;
        private readonly IPageDialogService _pageDialogService;
        private readonly IDeviceService _deviceService;
        private string _recognizedText;
        private string _startOrStop;
        private IDisposable _continuousDictationSubscription;
        private IDisposable _listeningStatusChangedSubscription;

        public SpeechPageViewModel(ISpeechRecognizer speechRecognizer, IPageDialogService pageDialogService, IDeviceService deviceService)
        {
            Title = "Speech";
            _speechRecognizer = speechRecognizer;
            _pageDialogService = pageDialogService;
            _deviceService = deviceService;
            StartListingCommand = new DelegateCommand(async () => await StartListingCommandHandler());
        }

        private async Task StartListingCommandHandler()
        {
            var permissionStatus = await _speechRecognizer.RequestAccess();

            if (_continuousDictationSubscription != null)
            {
                _continuousDictationSubscription.Dispose();
                _continuousDictationSubscription = null;
                return;
            }

            if (permissionStatus == AccessState.Available)
            {
                _continuousDictationSubscription = _speechRecognizer.ContinuousDictation().Subscribe(x =>
                {
                    _deviceService.BeginInvokeOnMainThread(() =>
                    {
                        RecognizedText += " " + x;
                    });
                });
            }
            else
            {
                await _pageDialogService.DisplayPromptAsync("Error", "Permission not setup.");
            }
        }

        public ICommand StartListingCommand { get; }

        public string RecognizedText { get => _recognizedText; set => SetProperty(ref _recognizedText, value); }

        public string StartOrStop { get => _startOrStop; set => SetProperty(ref _startOrStop, value); }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            StartOrStop = "Start";

            if (parameters.GetNavigationMode() == NavigationMode.New)
            {
                _listeningStatusChangedSubscription = _speechRecognizer.WhenListeningStatusChanged().Subscribe(x =>
                {
                    StartOrStop = x ? "Stop" : "Start";
                });
            }
        }

        public override void OnNavigatedFrom(INavigationParameters parameters)
        {
            base.OnNavigatedFrom(parameters);

            if (_continuousDictationSubscription != null)
            {
                _continuousDictationSubscription.Dispose();
            }

            if (_listeningStatusChangedSubscription != null)
            {
                _listeningStatusChangedSubscription.Dispose();
            }
        }
    }
}

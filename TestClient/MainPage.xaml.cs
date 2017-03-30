using System;
using System.Linq;
using System.Threading.Tasks;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using TestClient.Services.Impl;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace TestClient
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private SettingProvider _settingService;

        public MainPage()
        {
            this.InitializeComponent();
            // TODO: deploy IoC and DI
            _settingService = new SettingProvider();

            ChangeProgressRingVisibility(false);
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                LoadLastSettingQueryResult();
                LoadWebServerUrl();
            }
            catch (Exception exception)
            {
                await ShowErrorDialog("Getting error while loading data from database...");
            }
        }

        private void LoadLastSettingQueryResult()
        {
            var settings = _settingService.GetSettingListFromDb();
            Settings.ItemsSource = settings.ArrayOfSetting != null && settings.ArrayOfSetting.Any()
                ? settings.ArrayOfSetting
                : null;
        }

        private void LoadWebServerUrl()
        {
            ServerUrl.Text = _settingService.GetWebServerUrl();
        }

        private async void Search_OnClick(object sender, RoutedEventArgs e)
        {
            ChangeProgressRingVisibility(true);

            try
            {
                var settingList = await _settingService.GetSettingList(SearchField.Text);
                Settings.ItemsSource = settingList.ArrayOfSetting.ToList();
            }
            catch (Exception exception)
            {
                await ShowErrorDialog("Getting error while loading settings from webService...");
            }
            
            ChangeProgressRingVisibility(false);
        }

        private async void SetServerUrl_Onclick(object sender, RoutedEventArgs e)
        {
            ChangeProgressRingVisibility(true);

            try
            {
                _settingService.AddOrUpdateWebServerUrl(ServerUrl.Text);
            }
            catch (Exception exception)
            {
                await ShowErrorDialog("Getting error while setting webServer url...");
            }

            ChangeProgressRingVisibility(false);
        }

        private void ChangeProgressRingVisibility(bool visible)
        {
            LoadingRing.IsActive = visible;
            LoadingRing.Visibility = visible ? Visibility.Visible : Visibility.Collapsed;
        }

        private async Task ShowErrorDialog(string error)
        {
            var messageDialog = new MessageDialog(error);
            await messageDialog.ShowAsync();
        }
    }
}

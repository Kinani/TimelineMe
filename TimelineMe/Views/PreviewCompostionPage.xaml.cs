using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using TimelineMe.Models;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Media.Core;
using Windows.Media.Editing;
using Windows.Storage;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace TimelineMe.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class PreviewCompostionPage : Page
    {
        private MediaStreamSource mediaStreamSource;
        private MediaComposition mediaComposition;
        private MediaGroup mediaGroup;
        private StorageFile cmpFile;
        
        public PreviewCompostionPage()
        {
            this.InitializeComponent();
            mediaComposition = new MediaComposition();
        }
        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            var currentView = SystemNavigationManager.GetForCurrentView();

            currentView.AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
            currentView.BackRequested += backButton_Tapped;
            mediaGroup = e.Parameter as MediaGroup;
            await UpdateMediaElementSource();
        }
        public async Task UpdateMediaElementSource()
        {
            cmpFile = await ApplicationData.Current.LocalFolder.GetFileAsync(mediaGroup.CompostionFileName);
            mediaComposition = await MediaComposition.LoadAsync(cmpFile);

            mediaStreamSource = mediaComposition.GeneratePreviewMediaStreamSource(
                (int)mediaElement.ActualWidth,
                (int)mediaElement.ActualHeight);

            mediaElement.SetMediaStreamSource(mediaStreamSource);
        }
        private void backButton_Tapped(object sender, BackRequestedEventArgs e)
        {
            if (App.ShellFrame.CanGoBack)
                App.ShellFrame.GoBack();
            else
                App.ShellFrame.Navigate(typeof(HomePage));
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
            var currentView = SystemNavigationManager.GetForCurrentView();

            currentView.AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;

            currentView.BackRequested -= backButton_Tapped;

        }
    }
}

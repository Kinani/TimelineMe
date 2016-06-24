using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace TimelineMe
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            App.MyStaticFrame = this.myFrame;
            App.dispatcher = Windows.UI.Core.CoreWindow.GetForCurrentThread().Dispatcher;
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Splitter.IsPaneOpen = !Splitter.IsPaneOpen;
        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            this.myFrame.Navigate(typeof(Views.HomePage));
            Splitter.IsPaneOpen = false;
            Header.Text = "Home";
        }
        private void SplitTogleBtn_Click(object sender, RoutedEventArgs e)
        {
            Splitter.IsPaneOpen = !Splitter.IsPaneOpen;
        }
        private void Home_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as RadioButton;
            if (button != null)
            {
                switch (button.Content.ToString())
                {
                    case "Home":
                        this.myFrame.Navigate(typeof(Views.HomePage));
                        break;
                    case "Capture me today!":
                        this.myFrame.Navigate(typeof(Views.CapturePage));
                        break;
                    case "Gallery":
                        this.myFrame.Navigate(typeof(Views.GalleryPage));
                        break;
                    case "Analytics":
                        this.myFrame.Navigate(typeof(Views.AnalyticsPage));
                        break;
                    case "Contact Developer":
                        this.myFrame.Navigate(typeof(Views.ContactDeveloper));
                        break;
                    case "Settings":
                        this.myFrame.Navigate(typeof(Views.Settings));
                        break;
                }
                Header.Text = button.Content.ToString();
            }
        }
    }
}

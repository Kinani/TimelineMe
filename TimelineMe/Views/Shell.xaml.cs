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

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace TimelineMe.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Shell : Page
    {
        public Frame AppFrame { get { return Content; } }

        private void Option1Button_Checked(object sender, RoutedEventArgs e)
        {
            Content.Navigate(typeof(CapturePage));
            NavigationPane.IsPaneOpen = false;
        }

        private void Option2Button_Checked(object sender, RoutedEventArgs e)
        {
            Content.Navigate(typeof(GalleryPage));
            NavigationPane.IsPaneOpen = false;
        }

        private void HamburgerButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationPane.IsPaneOpen = !NavigationPane.IsPaneOpen;
        }
        public Shell()
        {
            this.InitializeComponent();
        }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using TimelineMe.ViewModels;
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
    public sealed partial class GalleryPage : Page
    {
        public MediaViewModel Vm
        {
            get
            {
                return (MediaViewModel)DataContext;
            }
        }
        public GalleryPage()
        {
            this.InitializeComponent();
        }

      

        private void TimelineListView_ItemClick(object sender, ItemClickEventArgs e)
        {   
            (e.ClickedItem as MediaElement).TransportControls.IsEnabled = true;
            (e.ClickedItem as MediaElement).IsFullWindow = true;
        }
    }
}

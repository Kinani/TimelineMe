using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Views;
using Microsoft.Practices.ServiceLocation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimelineMe.ViewModels
{
   
    public class ViewModelLocator
    {
        public ViewModelLocator()
        {

            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);
            if (ViewModelBase.IsInDesignModeStatic)
            {
                // Create design time view services and models
            }
            else
            {
                // Create run time view services and models
            }

            //Register your services used here
            SimpleIoc.Default.Register<INavigationService, NavigationService>();
            SimpleIoc.Default.Register<CapturePageViewModel>();
            SimpleIoc.Default.Register<GalleryPageViewModel>();
            SimpleIoc.Default.Register<AnalyticsPageViewModel>();
            SimpleIoc.Default.Register<SettingsPageViewModel>();
        }



        public CapturePageViewModel CapturePageViewModelInstance
        {
            get
            {
                return ServiceLocator.Current.GetInstance<CapturePageViewModel>();
            }
        }

        public GalleryPageViewModel GalleryPageViewModelInstance
        {
            get
            {
                return ServiceLocator.Current.GetInstance<GalleryPageViewModel>();
            }
        }

        public AnalyticsPageViewModel AnalyticsPageViewModelInstance
        {
            get
            {
                return ServiceLocator.Current.GetInstance<AnalyticsPageViewModel>();
            }
        }

        public SettingsPageViewModel SettingsPageViewModelInstance
        {
            get
            {
                return ServiceLocator.Current.GetInstance<SettingsPageViewModel>();
            }
        }
        public static void Cleanup()
        {
            // TODO Clear the ViewModels
        }
    }
}

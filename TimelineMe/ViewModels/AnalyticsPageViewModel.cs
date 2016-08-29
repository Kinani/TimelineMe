using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimelineMe.Common;
using TimelineMe.Models;

namespace TimelineMe.ViewModels
{
    public class AnalyticsPageViewModel : ViewModelBase
    {
        private MediaAdmin mediaAdmin;

        #region Properties

        private ObservableCollection<Media> mediaCollection;
        public ObservableCollection<Media> MediaCollection
        {
            get
            {
                return mediaCollection;
            }
            set
            {
                mediaCollection = value;
                RaisePropertyChanged("MediaCollection");
            }
        }
        private ObservableCollection<MediaGroup> mediaGroupCollection;
        public ObservableCollection<MediaGroup> MediaGroupCollection
        {
            get
            {
                return mediaGroupCollection;
            }
            set
            {
                mediaGroupCollection = value;
                RaisePropertyChanged("MediaGroupCollection");
            }
        }
        #endregion

        #region Commands

        private RelayCommand analyticsPageLoaded;


        public RelayCommand AnalyticsPageLoaded
        {
            get
            {

                analyticsPageLoaded = new RelayCommand(() =>
                {
                    mediaAdmin.Initialize();
                    MediaCollection = new ObservableCollection<Media>(mediaAdmin.MediaList);
                    MediaGroupCollection = new ObservableCollection<MediaGroup>(mediaAdmin.MediaGroupList);

                });

                return analyticsPageLoaded;
            }
            set
            {
                analyticsPageLoaded = value;
            }
        }
        #endregion
        public AnalyticsPageViewModel() { }
    }
}

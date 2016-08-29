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

        private ObservableCollection<MediaGroup> selectedMediaGroup;
        public ObservableCollection<MediaGroup> SelectedMediaGroup
        {
            get
            {
                return selectedMediaGroup;
            }
            set
            {
                selectedMediaGroup = value;
                RaisePropertyChanged("SelectedMediaGroup");
            }
        }
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

        private RelayCommand<IList<object>> updateSelectedCMP;
        public RelayCommand<IList<object>> UpdateSelectedCMP
        {
            get
            {
                
                   updateSelectedCMP = new RelayCommand<IList<object>>(
                       selectedCMP =>
                       {
                           SelectedMediaGroup = new ObservableCollection<MediaGroup>(selectedCMP.Cast<MediaGroup>().ToList());
                       });
                return updateSelectedCMP;
            }
            set
            {
                updateSelectedCMP = value;
            }
        }


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
        public AnalyticsPageViewModel()
        {
            mediaAdmin = new MediaAdmin();
            SelectedMediaGroup = new ObservableCollection<MediaGroup>();
        }
    }
}

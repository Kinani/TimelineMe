﻿//    TODO: 
//    New model for merged videos?? or should we integrate the shit out of it
//    to the current only Model. + then we need to Imp. commands required for TimelineGrid

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
using TimelineMe.Views;
using Windows.Media.Capture;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace TimelineMe.ViewModels
{
    public class GalleryPageViewModel : ViewModelBase
    {
        private MediaAdmin mediaAdmin;
        private List<Media> selectedMediaCollection;
        private List<MediaGroup> selectedCMPCollection;
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

        private bool isLoading = false;
        public bool IsLoading
        {
            get
            {
                return isLoading;
            }
            set
            {
                isLoading = value;
                RaisePropertyChanged("IsLoading");

            }

        }

        private string youLastTime;

        public string YouLastTime
        {
            get
            {
                return youLastTime;
            }
            set
            {
                youLastTime = value;
                RaisePropertyChanged("YouLastTime");
            }
        }

        private ImageSource imageItem;

        public ImageSource ImageItem
        {
            get
            {
                return imageItem;
            }
            set
            {
                imageItem = value;
                RaisePropertyChanged("ImageItem");
            }
        }

        private Media selectedMediaForPreview;
        public Media SelectedMediaForPreview
        {
            get
            {
                return selectedMediaForPreview;
            }
            set
            {
                selectedMediaForPreview = value;
                RaisePropertyChanged("SelectedMediaForPreview");
            }
        }

        private bool previewEnable = false;

        public bool PreviewEnable
        {
            get
            {
                return previewEnable;
            }
            set
            {
                previewEnable = value;
                RaisePropertyChanged("PreviewEnable");
            }
        }

        private bool cmpPreview = false;

        public bool CMPPreview
        {
            get
            {
                return cmpPreview;
            }
            set
            {
                cmpPreview = value;
                RaisePropertyChanged("CMPPreview");
            }
        }
        private Visibility proBarVisibility = Visibility.Collapsed;

        public Visibility ProBarVisibility
        {
            get
            {
                return proBarVisibility;
            }
            set
            {
                proBarVisibility = value;
                RaisePropertyChanged("ProBarVisibility");
            }
        }
        private Visibility contentGridVisibility = Visibility.Visible;

        public Visibility ContentGridVisibility
        {
            get
            {
                return contentGridVisibility;
            }
            set
            {
                contentGridVisibility = value;
                RaisePropertyChanged("ContentGridVisibility");
            }
        }
        #endregion

        #region Commands
        private RelayCommand openCamera;

        public RelayCommand OpenCamera
        {
            get
            {
                if (openCamera == null)
                {
                    openCamera = new RelayCommand(async () =>
                    {
                        await OpenCameraUI();
                    });
                }
                return openCamera;
            }
            set
            {
                openCamera = value;
            }
        }

        private RelayCommand galleryPageLoaded;


        public RelayCommand GalleryPageLoaded
        {
            get
            {
                //if (galleryPageLoaded == null)
                //{
                galleryPageLoaded = new RelayCommand(() =>
                {
                    mediaAdmin.Initialize();
                    MediaCollection = new ObservableCollection<Media>(mediaAdmin.MediaList);
                    MediaGroupCollection = new ObservableCollection<MediaGroup>(mediaAdmin.MediaGroupList);

                });
                //}
                return galleryPageLoaded;
            }
            set
            {
                galleryPageLoaded = value;
            }
        }
        private RelayCommand<IList<object>> updateSelectedImages;
        public RelayCommand<IList<object>> UpdateSelectedImages
        {
            get
            {
                return updateSelectedImages
                   ?? (updateSelectedImages = new RelayCommand<IList<object>>(
                       selectedImages =>
                       {
                           selectedMediaCollection = selectedImages.Cast<Media>().ToList();
                           if (selectedMediaCollection.Count == 1)
                               PreviewEnable = true;
                           else
                               PreviewEnable = false;
                       }));
            }
            set
            {
                updateSelectedImages = value;
            }
        }
        private RelayCommand<IList<object>> updateSelectedCMP;
        public RelayCommand<IList<object>> UpdateSelectedCMP
        {
            get
            {
                return updateSelectedCMP
                   ?? (updateSelectedCMP = new RelayCommand<IList<object>>(
                       selectedCMP =>
                       {
                           selectedCMPCollection = selectedCMP.Cast<MediaGroup>().ToList();
                           if (selectedCMPCollection.Count == 1)
                               CMPPreview = true;
                           else
                               CMPPreview = false;
                       }));
            }
            set
            {
                updateSelectedCMP = value;
            }
        }
        private RelayCommand mergeSelectedImages;
        public RelayCommand MergeSelectedImages
        {
            get
            {
                mergeSelectedImages = new RelayCommand(async () =>
                {
                    await MergeImages();
                });
                return mergeSelectedImages;
            }
            set
            {
                mergeSelectedImages = value;
            }
        }
        private RelayCommand mediaClikcedForPreview;
        public RelayCommand MediaClikcedForPreview
        {
            get
            {

                mediaClikcedForPreview = new RelayCommand(async
                   () =>
                   {
                       Media mediaToPreview = null;
                       if (selectedMediaCollection.Count == 1)
                       {
                           mediaToPreview = selectedMediaCollection[0];
                       }
                       await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher
                       .RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                        {
                            App.ShellFrame.Navigate(typeof(PreviewMediaPage), mediaToPreview);

                        });

                   });
                return mediaClikcedForPreview;
            }
            set
            {
                mediaClikcedForPreview = value;
            }
        }

        private RelayCommand cmpClikcedForPreview;
        public RelayCommand CMPClikcedForPreview
        {
            get
            {

                cmpClikcedForPreview = new RelayCommand(async
                   () =>
                {
                    MediaGroup mediaGroupToPreview = null;
                    if (selectedCMPCollection.Count == 1)
                    {
                        mediaGroupToPreview = selectedCMPCollection[0];
                    }
                    await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher
                    .RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                    {
                        App.ShellFrame.Navigate(typeof(PreviewCompostionPage), mediaGroupToPreview);

                    });

                });
                return cmpClikcedForPreview;
            }
            set
            {
                cmpClikcedForPreview = value;
            }
        }

        #endregion
        public GalleryPageViewModel()
        {
            mediaAdmin = new MediaAdmin();

        }
        public async Task OpenCameraUI()
        {
            CameraCaptureUI captureUI = new CameraCaptureUI();
            captureUI.PhotoSettings.Format = CameraCaptureUIPhotoFormat.Jpeg;
            //captureUI.PhotoSettings.CroppedSizeInPixels = new Size(200, 200);

            StorageFile photo = await captureUI.CaptureFileAsync(CameraCaptureUIMode.Photo);
            if (photo != null)
            {

                //TODO: Make sure mediaFolder is initialized before moving.
                //await photo.MoveAsync(mediaAdmin.mediaFolder);

                await photo.MoveAsync(ApplicationData.Current.LocalFolder, "timelineme.jpg",
                    NameCollisionOption.GenerateUniqueName);
                await AddImage(photo);
            }
        }

        private async Task AddImage(StorageFile image)
        {
            //TODO: Add video resulted from merging
            await mediaAdmin.AddMedia(image);
        }

        private async Task RemoveImage(StorageFile image)
        {
            await mediaAdmin.RemoveMedia(image);
        }

        private async Task MergeImages()
        {
            ContentGridVisibility = Visibility.Collapsed;
            ProBarVisibility = Visibility.Visible;

            await mediaAdmin.MergeMedias(selectedMediaCollection);
            mediaAdmin.Initialize();
            MediaCollection = new ObservableCollection<Media>(mediaAdmin.MediaList);
            MediaGroupCollection = new ObservableCollection<MediaGroup>(mediaAdmin.MediaGroupList);

            ContentGridVisibility = Visibility.Visible;
            ProBarVisibility = Visibility.Collapsed;
        }


    }
}


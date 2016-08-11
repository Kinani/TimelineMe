//    TODO: 
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
using Windows.Media.Capture;
using Windows.Storage;
using Windows.UI.Xaml.Media;

namespace TimelineMe.ViewModels
{
    public class GalleryPageViewModel : ViewModelBase
    {
        private MediaAdmin mediaAdmin;
        private List<Media> selectedMediaCollection;
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
                       }));
            }
            set
            {
                updateSelectedImages = value;
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
            await mediaAdmin.MergeMedias(selectedMediaCollection);
        }


    }
}


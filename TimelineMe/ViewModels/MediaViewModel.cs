/*
 * Hello friend :)
 * mediaAdmin needs to be initialized with previous sessions(aka restore data from db)
 */
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimelineMe.Models;
using Windows.Media.Capture;
using Windows.Storage;
using Windows.UI.Xaml.Media;

namespace TimelineMe.ViewModels
{
    public class MediaViewModel : ViewModelBase
    {
        private MediaAdmin mediaAdmin;


        public ObservableCollection<Media> MediaCollection;

        #region Properties    
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
                if(openCamera == null)
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
                if(galleryPageLoaded == null)
                {
                    galleryPageLoaded = new RelayCommand(async () =>
                    {
                        await mediaAdmin.Initialize();
                    });
                }
                return galleryPageLoaded;
            }
            set
            {
                galleryPageLoaded = value;
            }
        }


        #endregion
        public MediaViewModel()
        {
            mediaAdmin = new MediaAdmin();
            MediaCollection = new ObservableCollection<Media>(mediaAdmin.MediaList);
        }
        public async Task OpenCameraUI()
        {
            CameraCaptureUI captureUI = new CameraCaptureUI();
            captureUI.PhotoSettings.Format = CameraCaptureUIPhotoFormat.Jpeg;
            //captureUI.PhotoSettings.CroppedSizeInPixels = new Size(200, 200);

            StorageFile photo = await captureUI.CaptureFileAsync(CameraCaptureUIMode.Photo);
            if (photo != null)
            {
                await AddImage(photo);
            }
        }
        
        private async Task AddImage(StorageFile image)
        {
            await mediaAdmin.AddMedia(image);
        }

        private async Task RemoveImage(StorageFile image)
        {
            await mediaAdmin.RemoveMedia(image);
        }

        private void MergeSelectedImages()
        {
            // TODO
        }


    }
}

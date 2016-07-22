/*
 * Hello friend :)
 * mediaAdmin needs to be initialized with previous sessions(aka restore data from db)
 */
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Media.Capture;
using Windows.Storage;

namespace TimelineMe.ViewModels
{
    public class MediaViewModel : ViewModelBase
    {
        private MediaAdmin mediaAdmin = new MediaAdmin();
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
            get { return youLastTime; }
            set
            {
                youLastTime = value;
                RaisePropertyChanged("YouLastTime");
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

        #endregion

        public async Task OpenCameraUI()
        {
            CameraCaptureUI captureUI = new CameraCaptureUI();
            captureUI.PhotoSettings.Format = CameraCaptureUIPhotoFormat.Jpeg;
            //captureUI.PhotoSettings.CroppedSizeInPixels = new Size(200, 200);

            StorageFile photo = await captureUI.CaptureFileAsync(CameraCaptureUIMode.Photo);
            if (photo != null)
            {

            }
        }
        public MediaViewModel()
        {
            
        }
        private void AddImage()
        {

        }

        private void RemoveImage()
        {

        }

        private void MergeSelectedImages()
        {

        }


    }
}

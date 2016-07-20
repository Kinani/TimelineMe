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

namespace TimelineMe.ViewModels
{
    public class MediaViewModel : ViewModelBase
    {
        private MediaAdmin mediaAdmin = new MediaAdmin();
        
        private bool _isLoading = false;
        public bool IsLoading
        {
            get
            {
                return _isLoading;
            }
            set
            {
                _isLoading = value;
                RaisePropertyChanged("IsLoading");

            }
            
        }



        public MediaViewModel()
        {
            
        }
        private void AddMedia()
        {

        }
    }
}

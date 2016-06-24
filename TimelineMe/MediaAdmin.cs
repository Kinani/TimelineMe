using Microsoft.ProjectOxford.Vision.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace TimelineMe
{
    public class MediaAdmin
    {
        private static MediaAdmin _mediaAdmin = new MediaAdmin();
        private StorageFile currentMedia;
        private AnalysisResult currentMediaCVAnalysis;
        public List<MediaAdmin> ProcessedMediaCollection = new List<MediaAdmin>();
        public MediaAdmin() { }

        public void AddImage(StorageFile media, AnalysisResult analysisResult)
        {
            MediaAdmin newMedia = new MediaAdmin();
            newMedia.currentMedia = media;
            newMedia.currentMediaCVAnalysis = analysisResult;
            _mediaAdmin.ProcessedMediaCollection.Add(newMedia);
        }


    }
}

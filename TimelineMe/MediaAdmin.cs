/*
 * Hello friend:)
 * We need to make use of Oxford's emotions API here.
 * Do we need to migirate this class to MediaViewModel ? overkill?
  TODO: TagsSpacesSeperated was intended to be used within search.
*/
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

        public ProcessedFeatures ExtractFeatures(AnalysisResult rawResult)
        {
            // TODO: Traverse .Categeroies IEnum (should we abandon this?!)
            ProcessedFeatures _processedFeatures = new ProcessedFeatures();
            _processedFeatures.adultScore = rawResult.Adult.AdultScore;
            if (rawResult.Description.Captions.Length > 0)
            {
                _processedFeatures.CaptionsSoloConf = rawResult.Description.Captions[0].Confidence;
                _processedFeatures.CaptionSoloText = rawResult.Description.Captions[0].Text;
            }
            if (rawResult.Tags.Length > 0)
            {
                StringBuilder tagsBuilder = new StringBuilder();
                foreach (var tag in rawResult.Tags)
                {
                    tagsBuilder.Append(tag.Name);
                    tagsBuilder.Append(" ");
                }
                _processedFeatures.TagsSpacesSeperated = tagsBuilder.ToString();
            }
            return _processedFeatures;
        }
    }
    public struct ProcessedFeatures
    {
        public double adultScore;
        public double CaptionsSoloConf;
        public string CaptionSoloText;
        public string TagsSpacesSeperated;
    }
}
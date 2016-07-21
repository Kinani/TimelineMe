/*
 * Hello friend:)
 * We need to make use of Oxford's emotions API here.
 * Do we need to migirate this class to MediaViewModel ? overkill?
  TODO: TagsSpacesSeperated was intended to be used within search.
*/
using Microsoft.ProjectOxford.Vision;
using Microsoft.ProjectOxford.Vision.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using System.IO;
using Microsoft.ProjectOxford.Emotion.Contract;
using Microsoft.ProjectOxford.Emotion;

namespace TimelineMe
{
    public class MediaAdmin
    {
        private static MediaAdmin _mediaAdmin = new MediaAdmin();
        private StorageFile currentMedia;
        private AnalysisResult currentMediaCVAnalysis;
        public List<MediaAdmin> ProcessedMediaCollection = new List<MediaAdmin>();
        public MediaAdmin()
        {
            
        }

        private async Task<AnalysisResult> DoVision(StorageFile file)
        {
            VisionServiceClient visonClient = new VisionServiceClient(MyCredentials.OxfordVision);
            VisualFeature[] visualFeatures = new VisualFeature[] { VisualFeature.Adult, VisualFeature.Description, VisualFeature.Tags };
            var fileStream = await file.OpenAsync(FileAccessMode.Read);
            return await visonClient.AnalyzeImageAsync(fileStream.AsStream(), visualFeatures);
        }

        private async Task<Emotion[]> DoFeel(StorageFile file)
        {
            EmotionServiceClient emotionClient = new EmotionServiceClient(MyCredentials.OxfordEmtoion);
            var fileStream = await file.OpenAsync(FileAccessMode.Read);
            return await emotionClient.RecognizeAsync(fileStream.AsStream());
        }

        public void AddMedia(StorageFile media, AnalysisResult analysisResult)
        {
            // Install DoFeel(), DoVision()
            // Install ExtractFeature()
            // For Performance we're going to ExtractFeature after capturing each image immediatly
            MediaAdmin newMedia = new MediaAdmin();
            newMedia.currentMedia = media;
            newMedia.currentMediaCVAnalysis = analysisResult;
            _mediaAdmin.ProcessedMediaCollection.Add(newMedia);
        }

        public ProcessedFeatures ExtractFeatures(AnalysisResult rawResult, Emotion[] emotions)
        {
            // TODO: Add Emotions extraction
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
        // Add emotions extractions
        public double adultScore;
        public double CaptionsSoloConf;
        public string CaptionSoloText;
        public string TagsSpacesSeperated;
    }
}
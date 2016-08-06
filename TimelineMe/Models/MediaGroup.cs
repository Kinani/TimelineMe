﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimelineMe.Models
{
    public class MediaGroup
    {
        [Key]
        public int MediaGroupId { get; set; }
        public DateTime FirstCreatedDate { get; private set; }
        public DateTime LastItemDate { get; set; }


        public double AngerScoreMean { get; set; }
        public double ContemptScoreMean { get; set; }
        public double DisgustScoreMean { get; set; }
        public double FearScoreMean { get; set; }
        public double HappinessScoreMean { get; set; }
        public double NeutralScoreMean { get; set; }
        public double SadnessScoreMean { get; set; }
        public double SurpriseScoreMean { get; set; }

        public string HighestEmotionMean { get; set; }


        public MediaGroup()
        {
            FirstCreatedDate = DateTime.Now;
        }

    }
}

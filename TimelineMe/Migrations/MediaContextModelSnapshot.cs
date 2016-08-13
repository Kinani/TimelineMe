using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using TimelineMe.Models;

namespace TimelineMe.Migrations
{
    [DbContext(typeof(MediaContext))]
    partial class MediaContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.0.0-rtm-21431");

            modelBuilder.Entity("TimelineMe.Models.Media", b =>
                {
                    b.Property<int>("MediaId")
                        .ValueGeneratedOnAdd();

                    b.Property<double>("AngerScore");

                    b.Property<string>("CaptionSoloText");

                    b.Property<double>("CaptionsSoloConf");

                    b.Property<DateTime>("CaptureDate");

                    b.Property<double>("ContemptScore");

                    b.Property<double>("DisgustScore");

                    b.Property<double>("FearScore");

                    b.Property<double>("HappinessScore");

                    b.Property<string>("HighestEmotion");

                    b.Property<bool>("IsMerged");

                    b.Property<string>("MediaName");

                    b.Property<double>("NeutralScore");

                    b.Property<double>("SadnessScore");

                    b.Property<double>("SurpriseScore");

                    b.Property<string>("TagsSpacesSeperated");

                    b.Property<double>("adultScore");

                    b.HasKey("MediaId");

                    b.ToTable("Medias");
                });

            modelBuilder.Entity("TimelineMe.Models.MediaGroup", b =>
                {
                    b.Property<int>("MediaGroupId")
                        .ValueGeneratedOnAdd();

                    b.Property<double>("AngerScoreMean");

                    b.Property<string>("CompostionFileName");

                    b.Property<double>("ContemptScoreMean");

                    b.Property<double>("DisgustScoreMean");

                    b.Property<double>("FearScoreMean");

                    b.Property<DateTime>("FirstCreatedDate");

                    b.Property<double>("HappinessScoreMean");

                    b.Property<string>("HighestEmotionMean");

                    b.Property<DateTime>("LastEditDate");

                    b.Property<double>("NeutralScoreMean");

                    b.Property<double>("SadnessScoreMean");

                    b.Property<double>("SurpriseScoreMean");

                    b.HasKey("MediaGroupId");

                    b.ToTable("MediaGroups");
                });
        }
    }
}

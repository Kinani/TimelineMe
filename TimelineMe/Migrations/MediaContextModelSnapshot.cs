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

                    b.Property<string>("CaptionSoloText");

                    b.Property<double>("CaptionsSoloConf");

                    b.Property<DateTime>("CaptureDate");

                    b.Property<bool>("IsMerged");

                    b.Property<string>("MediaName");

                    b.Property<string>("TagsSpacesSeperated");

                    b.Property<double>("adultScore");

                    b.HasKey("MediaId");

                    b.ToTable("Medias");
                });
        }
    }
}

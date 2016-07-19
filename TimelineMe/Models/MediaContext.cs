using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimelineMe.Models
{
    public class MediaContext : DbContext
    {
        public DbSet<Media> Medias { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Filename=Media.db");
        }
    }

    public class Media
    {
        //TODO: Add emotions props
        //TODO: Add media preview (think of db size plz)
        public string MediaName { get; set; }
        public DateTime CaptureDate { get; set; }
        public bool IsMerged { get; set; }
        public double adultScore { get; set; }
        public double CaptionsSoloConf { get; set; }
        public string CaptionSoloText { get; set; }
        public string TagsSpacesSeperated { get; set; }
    }
}

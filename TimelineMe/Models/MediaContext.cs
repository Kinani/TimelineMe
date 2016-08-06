using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimelineMe.Models
{
    public class MediaContext : DbContext
    {
        public DbSet<Media> Medias { get; set; }
        public DbSet<MediaGroup> MediaGroups { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Filename=MediaT.db");
        }
    }

    
}

using firefly.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace firefly.Data
{
    public class LuminarDbContext : DbContext
    {
        public LuminarDbContext(DbContextOptions<LuminarDbContext> options) : base(options) { }

        public DbSet<ImageGenerationJob> ImageGenerationJobs => Set<ImageGenerationJob>();
        public DbSet<ImageAsset> ImageAssets => Set<ImageAsset>();
    }
}
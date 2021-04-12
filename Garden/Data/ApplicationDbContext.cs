using Garden.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Garden.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Garden.Models.Attachment> Attachment { get; set; }
        public DbSet<Garden.Models.BaseSubType> BaseSubType { get; set; }
        public DbSet<Garden.Models.BaseType> BaseType { get; set; }
        public DbSet<Garden.Models.GardenAttachMap> GardenAttachMap { get; set; }
        public DbSet<Garden.Models.GardenRole> GardenRole { get; set; }
        public DbSet<Garden.Models.GardenSpace> GardenSpace { get; set; }
        public DbSet<Garden.Models.GardenTask> GardenTask { get; set; }
        public DbSet<Garden.Models.GardenTaskAttachMap> GardenTaskAttachMap { get; set; }
        public DbSet<Garden.Models.GardenUser> GardenUser { get; set; }
        public DbSet<Garden.Models.GardenUserTaskMap> GardenUserTaskMap { get; set; }
        public DbSet<Garden.Models.GardenWorkDay> GardenWorkDay { get; set; }
        public DbSet<Garden.Models.GardenWorkTime> GardenWorkTime { get; set; }
    }
}

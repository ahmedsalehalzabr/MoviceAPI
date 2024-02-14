﻿using Microsoft.EntityFrameworkCore;
using MoviceAPI.Models.Data;

namespace MoviceAPI.Models
{
    public class AppDbContext : DbContext 
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) :base(options) 
        {
            
        }

        public DbSet<Genre> Genres { get; set; }
    }
}
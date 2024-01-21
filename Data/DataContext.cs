using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InterviewAssistantAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace InterviewAssistantAPI.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        public DbSet<Tag> Tags { get; set; }
        public DbSet<Card> Cards { get; set; }
    }
}
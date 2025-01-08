using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Domain.Models;

public class CsvDataManagerDbContext : DbContext
{

    public CsvDataManagerDbContext(DbContextOptions<CsvDataManagerDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<CsvUploader> CsvUploaders { get; set; }
    public DbSet<FileData> FileData { get; set; }
    

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Data Source=ABDUL-SAMAD;Initial Catalog=CsvDataManagerDb;Integrated Security=True;Trust Server Certificate=True");
 
    
}

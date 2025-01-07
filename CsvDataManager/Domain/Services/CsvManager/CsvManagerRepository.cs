using Domain.Models;
using Domain.Services.CsvManager.DTOs;
using Domain.Services.CsvManager.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Services.CsvManager
{
    public class CsvManagerRepository : ICsvManagerRepository
    {
        private readonly CsvDataManagerDbContext _context;

        public CsvManagerRepository(CsvDataManagerDbContext context)
        {
            _context = context;
        }

        public async Task AddCsvUploaderAsync(CsvUploader csvUploader)
        {
            await _context.Set<CsvUploader>().AddAsync(csvUploader);
        }

        public Task AddCsvUploaderAsync(CsvDataDto request)
        {
            throw new NotImplementedException();
        }

        public async Task AddFileDataAsync(FileData fileData)
        {
            await _context.Set<FileData>().AddAsync(fileData);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}

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

        public async Task SaveCsvUploaderAsync(CsvUploader csvUploader)
        {
            await _context.CsvUploaders.AddAsync(csvUploader);
            await _context.SaveChangesAsync();
        }

        public async Task SaveFileDataAsync(FileData fileData)
        {
            await _context.FileData.AddAsync(fileData);
            await _context.SaveChangesAsync();
        }

        public async Task SaveBatchFileDataAsync(FileData fileDataBatch)
        {
            await _context.FileData.AddRangeAsync(fileDataBatch);
            await _context.SaveChangesAsync();
        }

    }
}

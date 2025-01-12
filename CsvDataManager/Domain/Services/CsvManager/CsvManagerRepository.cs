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

        //public async Task SaveFileDataAsync(FileData fileData)
        //{
        //    await _context.FileData.AddAsync(fileData);
        //    await _context.SaveChangesAsync();
        //}

        public async Task SaveBatchFileDataAsync(FileData fileDataBatch)
        {
            await _context.FileData.AddRangeAsync(fileDataBatch);
            await _context.SaveChangesAsync();

            await CsvUpdate(fileDataBatch.FileId);

        }

        public async Task CsvUpdate(Guid fileId)
        {
            var csvUpdater = _context.CsvUploaders.Where(x => x.Id == fileId).FirstOrDefault();

            if (csvUpdater != null)
            {
                csvUpdater.Status = "Completed";
                _context.Update(csvUpdater);
                _context.SaveChanges();
            }
        }

        public async Task<List<FileData>> GetFileDataByUserIdAsync(Guid userId)
        {
            var csvFiles = await _context.CsvUploaders
                                         .Where(x => x.UserId == userId)
                                         .ToListAsync();

            if (!csvFiles.Any())
                return new List<FileData>();

            var fileIds = csvFiles.Select(f => f.Id).ToList();

            return await _context.FileData
                                 .Where(x => fileIds.Contains(x.FileId))
                                 .ToListAsync();
        }

        public async Task<List<CsvUploader>> GetCsvFileByUserIdAsync(Guid userId)
        {
            return await _context.CsvUploaders
                .Where(c => c.UserId == userId)
                .ToListAsync();
        }

    }
}

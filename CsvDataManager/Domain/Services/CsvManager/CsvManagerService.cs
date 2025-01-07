using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Models;
using Domain.Services.CsvManager.DTOs;
using Domain.Services.CsvManager.Interface;

namespace Domain.Services.CsvManager
{
    public class CsvManagerService : ICsvManagerService
    {
        private readonly ICsvManagerRepository _repository;

        public CsvManagerService(ICsvManagerRepository repository)
        {
            _repository = repository;
        }

        public async Task SaveCsvDataAsync(CsvDataDto request)
        {
            // Save CsvUploader record
            await _repository.AddCsvUploaderAsync(request);
            //lis
            //// Save FileData records
            //foreach (var data in )
            //{
            //    await _repository.AddFileDataAsync(data);
            //}

            // Commit changes
            await _repository.SaveChangesAsync();
        }



        /*public async Task SaveCsvDataAsync(CsvUploader csvUploader, IEnumerable<FileData> fileData)
        {
            // Save CsvUploader record
            await _repository.AddCsvUploaderAsync(csvUploader);

            // Save FileData records
            foreach (var data in fileData)
            {
                await _repository.AddFileDataAsync(data);
            }

            // Commit changes
            await _repository.SaveChangesAsync();
        }*/
    }
}

using System;
using System.Collections.Generic;

namespace Domain.Models;

public partial class User
{
    public Guid Id { get; set; } 

    public string? Name { get; set; } 

    public string Email { get; set; } = null!; 

    public string Password { get; set; } = null!; 

    public ICollection<CsvUploader> CsvUploaders { get; set; } = new List<CsvUploader>();
}

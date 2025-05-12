using System.Globalization;
using CsvHelper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace MonappolyLibrary.Services;

public class CsvReader<T>
{
    private readonly ILogger<CsvReader<T>> _logger;

    public CsvReader() { }
    
    public CsvReader(ILogger<CsvReader<T>> logger)
    {
        _logger = logger;
    }
    
    public IEnumerable<T>? UploadFile(IFormFile file)
    {
        try
        {
            using var reader = new StreamReader(file.OpenReadStream());
            using var csvReader = new CsvReader(reader, CultureInfo.InvariantCulture);

            return csvReader.GetRecords<T>();
        }
        catch (Exception ex)
        {
            _logger.LogError($"Unable to read CSV and extract records. File name: {file.Name}, Record Type: {typeof(T)}.\n{ex}");
            return null;
        }
    }

    public IEnumerable<T>? UploadFile(FileStream file)
    {
        using var reader = new StreamReader(file);
        using var csvReader = new CsvReader(reader, CultureInfo.InvariantCulture);

        return csvReader.GetRecords<T>().ToList();
    }
}
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Dor.Services;

public class FileService
{
    private readonly string _fileStoragePath;

    public FileService(string fileStoragePath)
    {
        _fileStoragePath = fileStoragePath;
    }

    public async Task<string> SaveFileAsync(IFormFile file, string subDirectory = "")
    {
        if (file == null || file.Length == 0)
            throw new ArgumentException("Invalid file.");

        var directoryPath = Path.Combine(_fileStoragePath, subDirectory);
        if (!Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
        }

        var fileName = Path.GetFileName(file.FileName);
        var filePath = Path.Combine(directoryPath, fileName);

        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        return Path.Combine(subDirectory, fileName).Replace("\\", "/");
    }

    public void DeleteFile(string filePath)
    {
        var fullPath = Path.Combine(_fileStoragePath, filePath);
        if (File.Exists(fullPath))
        {
            File.Delete(fullPath);
        }
    }
}
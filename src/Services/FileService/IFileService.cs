using System;
using System.Threading.Tasks;
using MopsterTeams.Models;

namespace MopsterTeams.Services
{
    public interface IFileService
    {
        Task<AOResult<string>> SaveFileAsync(string fileName, byte[] data);
        Task<AOResult<bool>> ClearDocumentsFolderAsync();
    }
}
 
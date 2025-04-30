using System;
using System.Threading.Tasks;

namespace MopsterTeams.Services
{
    public interface IInternetConnectionService
    {
        Task<bool> CheckInternetConnectionAsync();
        Task<bool> CheckInternetConnectionWithoutErrorMessageAsync();
    }
}

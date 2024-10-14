using System.Threading.Tasks;

namespace HashGen.Services
{
    public interface IFileService
    {
        public Task<bool> SaveFileAsync(string userData);
    }
}

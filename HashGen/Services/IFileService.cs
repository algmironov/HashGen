using System.Threading.Tasks;

namespace HashGen.Services
{
    public interface IFileService
    {
        public Task SaveFileAsync(string userData);
    }
}

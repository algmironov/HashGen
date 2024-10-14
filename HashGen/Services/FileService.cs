using System;
using System.IO;
using System.Threading.Tasks;

using static HashGen.Helpers.StorageProviderHelper;

namespace HashGen.Services
{
    public class FileService : IFileService
    {
        private readonly string _fileName = "userData.json";

        public async Task<bool> SaveFileAsync(string userData)
        {
            var destination = await OpenFolderPickerAsync("Выберите папку для сохранения данных");

            try
            {
                if (destination is not null)
                {
                    var fileToSave = Path.Combine(destination.Path.LocalPath, _fileName);
                    await File.WriteAllTextAsync(fileToSave, userData, System.Text.Encoding.UTF8);
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}

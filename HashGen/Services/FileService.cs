using System.IO;
using System.Threading.Tasks;

using Avalonia.Controls;
using Avalonia.Platform.Storage;

namespace HashGen.Services
{
    public class FileService: IFileService
    {
        private readonly Window _target;
        private readonly string _fileName = "userData.json";

        public FileService(Window target)
        {
            _target = target;
        }

        public async Task SaveFileAsync(string userData)
        {
            //var file = await _target.StorageProvider.SaveFilePickerAsync(new FilePickerSaveOptions
            //{
            //    Title = "Куда сохранить файл",
            //    DefaultExtension = "json",

            //});

            //if (file is not null)
            //{
            //    await using var stream = await file.OpenWriteAsync();
            //    using var streamWriter = new StreamWriter(stream);
            //    await streamWriter.WriteLineAsync(userData);
            //    return ;
            //}

            var destination = await _target.StorageProvider.OpenFolderPickerAsync(new FolderPickerOpenOptions
            {
                AllowMultiple = false,
                SuggestedFileName = "userData.json",
                Title = "Выберите папку для сохранения файла"
            });

            if (destination is not null && destination.Count > 0)
            {
                var fileToSave = Path.Combine(destination[0].Path.AbsolutePath.ToString(), _fileName);
                await File.WriteAllTextAsync(fileToSave, userData, System.Text.Encoding.UTF8);
            }
        }
    }
}

﻿using System;
using System.Threading.Tasks;

using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Platform.Storage;

namespace HashGen.Helpers
{
    public static class StorageProviderHelper
    {
        private static IStorageProvider GetStorageProvider()
        {
            if (Application.Current?.ApplicationLifetime is not IClassicDesktopStyleApplicationLifetime desktop || desktop.MainWindow?.StorageProvider is not { } provider)
            {
                throw new NullReferenceException("Missing StorageProvider instance.");
            }

            return provider;
        }

        public static async Task<IStorageItem?> OpenFilePickerAsync()
        {
            var files = await GetStorageProvider().OpenFilePickerAsync(new FilePickerOpenOptions()
            {
                Title = "Select File",
                AllowMultiple = false,
                FileTypeFilter = [FilePickerFileTypes.Pdf]
            });

            return files?.Count >= 1 ? files[0] : null;
        }

        public static async Task<IStorageItem?> OpenFolderPickerAsync()
        {
            var folder = await GetStorageProvider().OpenFolderPickerAsync(new FolderPickerOpenOptions()
            {
                Title = "Select Folder",
                AllowMultiple = false
            });

            return folder?.Count >= 1 ? folder[0] : null;
        }

        public static async Task<IStorageItem?> OpenFolderPickerAsync(string title)
        {
            var folder = await GetStorageProvider().OpenFolderPickerAsync(new FolderPickerOpenOptions()
            {
                Title = title,
                AllowMultiple = false
            });

            return folder?.Count >= 1 ? folder[0] : null;
        }
    }
}

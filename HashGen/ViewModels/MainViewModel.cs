using System;
using System.Security.Cryptography;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Threading.Tasks;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using HashGen.Models;
using HashGen.Services;

using Microsoft.AspNetCore.Cryptography.KeyDerivation;

using static HashGen.Helpers.MessageBoxHelper;

namespace HashGen.ViewModels;
public partial class MainViewModel : ViewModelBase
{
    private readonly IFileService? _fileService;

    [ObservableProperty]
    private string _login = string.Empty;

    [ObservableProperty]
    private string _password = string.Empty;

    [ObservableProperty]
    private string _salt = string.Empty;

    [ObservableProperty]
    private string _hashedLogin = string.Empty;

    [ObservableProperty]
    private string _hashedPassword = string.Empty;

    private readonly JsonSerializerOptions options = new()
    {
        WriteIndented = true,
        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
    };

    public MainViewModel()
    {

    }

    public MainViewModel(IFileService fileService) : this()
    {
        _fileService = fileService;
    }

    [RelayCommand]
    private async Task SaveDataAsync()
    {
        UserData userData = new()
        {
            Login = Login,
            Password = Password,
            Salt = Salt,
            HashedLogin = HashedLogin,
            HashedPassword = HashedPassword
        };

        string json = JsonSerializer.Serialize(userData, options);
        var isSuccess = await _fileService.SaveFileAsync(json);

        if (isSuccess)
        {
            await ShowMessage("Файл с данными успешно сохранен!", Helpers.MessageBoxType.Success);
        }
        else
        {
            await ShowMessage("Не удалось сохранить файл с данными!", Helpers.MessageBoxType.Error);
        }
    }

    [RelayCommand]
    private void ClearData()
    {
        Login = "";
        Password = "";
        Salt = "";
        HashedLogin = "";
        HashedPassword = "";
    }

    [RelayCommand]
    private void GeneratePassword()
    {
        Password = Guid.NewGuid().ToString("N")[..12];
    }

    [RelayCommand]
    private void GenerateSalt()
    {
        byte[] saltBytes = new byte[16];
        using (var range = RandomNumberGenerator.Create())
        {
            range.GetBytes(saltBytes);
        }
        Salt = Convert.ToBase64String(saltBytes);
    }

    [RelayCommand]
    private async Task GenerateHash()
    {
        try
        {
            byte[] saltBytes = Convert.FromBase64String(Salt);

            byte[] hashedLogin = KeyDerivation.Pbkdf2(
                password: Login,
                salt: saltBytes,
                prf: KeyDerivationPrf.HMACSHA1,
                iterationCount: 1000,
                numBytesRequested: 256 / 8
            );

            byte[] hashedPassword = KeyDerivation.Pbkdf2(
                password: Password,
                salt: saltBytes,
                prf: KeyDerivationPrf.HMACSHA1,
                iterationCount: 1000,
                numBytesRequested: 256 / 8
            );

            HashedLogin = Convert.ToBase64String(hashedLogin);
            HashedPassword = Convert.ToBase64String(hashedPassword);
        }
        catch (Exception ex)
        {
            await ShowMessage($"Возникла ошибка: {ex} ", Helpers.MessageBoxType.Error);
        }
    }
}

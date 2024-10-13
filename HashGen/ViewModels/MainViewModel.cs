using System;
using System.IO;
using System.Security.Cryptography;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Input;

using CommunityToolkit.Mvvm.Input;

using HashGen.Models;
using HashGen.Services;

using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace HashGen.ViewModels;
public class MainViewModel : ViewModelBase
{
    private string _login = string.Empty;
    private string _password = string.Empty;
    private string _salt = string.Empty;
    private string _hashedLogin = string.Empty;
    private string _hashedPassword = string.Empty;
    private readonly IFileService _fileService;

    public string Login
    {
        get { return _login; }
        set { _login = value; OnPropertyChanged(nameof(Login)); }
    }

    public string Password
    {
        get { return _password; }
        set { _password = value; OnPropertyChanged(nameof(Password)); }
    }

    public string Salt
    {
        get { return _salt; }
        set { _salt = value; OnPropertyChanged(nameof(Salt)); }
    }

    public string HashedLogin
    {
        get { return _hashedLogin; }
        set { _hashedLogin = value; OnPropertyChanged(nameof(HashedLogin)); }
    }

    public string HashedPassword
    {
        get { return _hashedPassword; }
        set { _hashedPassword = value; OnPropertyChanged(nameof(HashedPassword)); }
    }

    public ICommand GeneratePasswordCommand { get; }
    public ICommand GenerateSaltCommand { get; }
    public ICommand GenerateHashCommand { get; }
    public ICommand SaveDataCommand { get; }

    public MainViewModel(IFileService fileService)
    {
        GeneratePasswordCommand = new RelayCommand(GeneratePassword);
        GenerateSaltCommand = new RelayCommand(GenerateSalt);
        GenerateHashCommand = new RelayCommand(GenerateHash);
        SaveDataCommand = new AsyncRelayCommand(SaveDataAsync);
        _fileService = fileService;
    }

    private async Task SaveDataAsync()
    {
        // Create a new instance of UserData
        UserData userData = new()
        {
            Login = Login,
            Password = Password,
            Salt = Salt,
            HashedLogin = HashedLogin,
            HashedPassword = HashedPassword
        };

        string json = JsonSerializer.Serialize(userData, new JsonSerializerOptions() { WriteIndented = true });
        await _fileService.SaveFileAsync(json);

    }

    private void GeneratePassword()
    {
        // Generate a random password
        Password = Guid.NewGuid().ToString("N")[..12];
    }

    private void GenerateSalt()
    {
        // Generate a random salt
        byte[] saltBytes = new byte[16];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(saltBytes);
        }
        Salt = Convert.ToBase64String(saltBytes);
    }

    private void GenerateHash()
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
}

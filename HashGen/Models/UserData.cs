namespace HashGen.Models
{
    public class UserData
    {
        public string Login { get; set; }
        public string Password { get; set; }
        public string Salt { get; set; }
        public string HashedLogin { get; set; }
        public string HashedPassword { get; set; }

        public UserData(string login, string password, string salt, string hashedLogin, string hashedPassword)
        {
            Login = login;
            Password = password;
            Salt = salt;
            HashedLogin = hashedLogin;
            HashedPassword = hashedPassword;
        }

        public UserData()
        {
            
        }
    }
}

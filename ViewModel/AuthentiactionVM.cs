using ProductsClient.Classes;
using ProductsClient.Model;
using ProductsClient.Views;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ProductsClient.ViewModel
{
    public class AuthentiactionVM : BaseVM
    {
        private string _userLogin;
        private string _userPassword;

        private WindowState _authWindowState;
        private Task<HttpResponseMessage> _responseMessage;
        private string _message;

        public AuthentiactionVM()
        {
            WindowMinimisizeCommand = new RelayCommand(WindowMinimisize);
            ApplicationCloseCommand = new RelayCommand(ApplicationClose);
            TelegramAuthCommand = new RelayCommand(TelegramAuth);
            AuthenticateCommand = new RelayCommand(Authenticate);
        }

        public string UserLogin
        {
            get => _userLogin;
            set { _userLogin = value; OnPropertyChanged(); }
        }
        public string UserPassword
        {
            get => _userPassword;
            set { _userPassword = value; OnPropertyChanged(); }
        }

        public WindowState AuthWindowState
        {
            get => _authWindowState;
            set { _authWindowState = value; OnPropertyChanged(); }
        }
        public Task<HttpResponseMessage> ResponseMessage
        {
            get => _responseMessage;
            set { _responseMessage = value; OnPropertyChanged(); }
        }
        public string Message
        {
            get => _message;
            set { _message = value; OnPropertyChanged(); }
        }

        public ICommand WindowMinimisizeCommand { get; set; }
        public ICommand ApplicationCloseCommand { get; set; }
        public ICommand TelegramAuthCommand { get; set; }
        public ICommand AuthenticateCommand { get; set; }

        private void WindowMinimisize(object obj)
        {
            AuthWindowState = WindowState.Minimized;
        }
        private void ApplicationClose(object obj)
        {
            Application.Current.Shutdown();
        }
        private void TelegramAuth(object obj)
        {
            MessageBox.Show("Авторизация");
        }
        private async void Authenticate(object obj)
        {
            UserLogin userLogin = new UserLogin()
            {
                Login = UserLogin,
                Password = (obj as PasswordBox)?.Password
            };

            if (!ValidateFields(userLogin))
                return;

            Message = string.Empty;

            ResponseMessage = new Task<HttpResponseMessage>(() => new HttpResponseMessage());
            ResponseMessage = await Task.WhenAny(DataInfo.Instance.Identity(userLogin));

            if (ResponseMessage.Exception != null)
            {
                Message = "Connection problems";
                return;
            }

            HttpResponseMessage response = ResponseMessage.Result;
            if (response.IsSuccessStatusCode)
            {
                new MainWindow().Show();
                Application.Current.Windows.OfType<AuthenticationView>().First().Close();
            }
            else
            {
                Message = await response.Content.ReadAsStringAsync();
                //throw new HttpRequestException($"Status code: {(int)response.StatusCode} ({response.StatusCode})\nMessage: {Message = await response.Content.ReadAsStringAsync()}");
            }
        }

        private bool ValidateFields(UserLogin userLogin)
        {
            if (string.IsNullOrWhiteSpace(userLogin.Login) && string.IsNullOrWhiteSpace(userLogin.Password))
            {
                Message = "UserLogin is empty";
                return false;
            }
            else if (string.IsNullOrWhiteSpace(userLogin.Login))
            {
                Message = "Login is empty";
                return false;
            }
            else if (string.IsNullOrWhiteSpace(userLogin.Password))
            {
                Message = "Password is empty";
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
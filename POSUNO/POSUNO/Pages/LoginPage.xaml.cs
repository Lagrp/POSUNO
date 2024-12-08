using POSUNO.Helpers;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace POSUNO.Pages;

/// <summary>
/// An empty page that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class LoginPage : Page
{
    public LoginPage()
    {
        this.InitializeComponent();
        EmailTextBox.Text = "lagrp03@gmail.com";
        PasswordPasswordBox.Password = "luan3306";
    }

    private async void LoginButton_Click(object sender, RoutedEventArgs e)
    {
        bool isValid = await ValidForm();
        if (!isValid)
        {
            return;
        }

        //Loader loader = new Loader("Por favor espere...");
        //loader.Show();
        Response response = await ApiService.LoginAsync(new LoginRequest
        {
            Email = EmailTextBox.Text,
            Password = PasswordPasswordBox.Password
        });
        //loader.Close();

        if (!response.IsSuccess)
        {
            var messageDialog = new ContentDialog
            {
                Title = "Error",
                Content = response.Message,
                CloseButtonText = "OK",
                XamlRoot = this.XamlRoot
            };
            await messageDialog.ShowAsync();
            return;
        }

        User user = (User)response.Result;
        if (user != null)
        {
            var messageDialog = new ContentDialog
            {
                Title = "Error",
                Content = "Usuario o contraseña incorrectos",
                CloseButtonText = "OK",
                XamlRoot = this.XamlRoot
            };
            await messageDialog.ShowAsync();
            return;
        }

        var contentDialog = new ContentDialog
        {
            Content = $"Bienvenido {user.FullName}",
            CloseButtonText = "OK",
            XamlRoot = this.XamlRoot
        };

        await contentDialog.ShowAsync();

        //TokenResponse tokenResponse = (TokenResponse)response.Result;
        //Frame.Navigate(typeof(MainPage), tokenResponse);
    }

    private async Task<bool> ValidForm()
    {
        ContentDialog messageDialog;

        if (string.IsNullOrEmpty(EmailTextBox.Text))
        {
            messageDialog = new ContentDialog
            {
                Title = "Error",
                Content = "Debes ingresar tu email.",
                CloseButtonText = "OK",
                XamlRoot = this.XamlRoot
            };
            await messageDialog.ShowAsync();
            return false;
        }

        if (!RegexUtilities.IsValidEmail(EmailTextBox.Text))
        {
            messageDialog = new ContentDialog
            {
                Title = "Error",
                Content = "Debes ingresar un email válido.",
                CloseButtonText = "Ok",
                XamlRoot = this.XamlRoot
            };
            await messageDialog.ShowAsync();
            return false;
        }

        if (PasswordPasswordBox.Password.Length < 6)
        {
            messageDialog = new ContentDialog
            {
                Title = "Error",
                Content = "Debes ingresar tu contraseña de al menos seis (6) carátertes.",
                CloseButtonText = "Ok",
                XamlRoot = this.XamlRoot
            };
            await messageDialog.ShowAsync();
            return false;
        }

        return true;
    }
}

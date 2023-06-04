using SecretsSharing.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SecretsSharing.Pages
{
    /// <summary>
    /// Interaction logic for AuthPage.xaml
    /// </summary>
    public partial class AuthPage : Page
    {
        public AuthPage()
        {
            InitializeComponent();
        }

        private void btnEnter_Click(object sender, RoutedEventArgs e)
        {
            var login = tbxLogin.Text;
            var password = pbxPassword.Password;

            if (string.IsNullOrWhiteSpace(login) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Логин и пароль - обязательные поля");
                return;
            }

            HttpResponseMessage response;
            using(var client = new HttpClient())
            {
                response = client.GetAsync($"https://localhost:44306/api/Users/GetUser/login={login}&password={password}").Result;
            }

            var result = response.Content.ReadAsStringAsync().Result;
            if (response.IsSuccessStatusCode)
                NavigationService.Navigate(new FilesPage(Convert.ToInt32(result)));
            else
                MessageBox.Show(result, "Api says");
        }

        private void btnToRegPage_Click(object sender, RoutedEventArgs e) => NavigationService.Navigate(new RegPage());

        private void btnGetFileByUrl_Click(object sender, RoutedEventArgs e) => new GetByUrlWindow().ShowDialog();
    }
}

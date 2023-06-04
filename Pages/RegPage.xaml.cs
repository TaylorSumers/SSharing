using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
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
using SecretsSharing.Models;
using System.Net.Http;
using System.Net.Http.Json;
using Newtonsoft.Json;
using System.Net;

namespace SecretsSharing.Pages
{
    /// <summary>
    /// Interaction logic for RegPage.xaml
    /// </summary>
    public partial class RegPage : Page
    {
        public RegPage()
        {
            InitializeComponent();
        }

        private void btnRegistrate_Click(object sender, RoutedEventArgs e)
        {
            var login = tbxLogin.Text;
            var password = pbxPass.Password;
            // Validate
            if (string.IsNullOrWhiteSpace(login) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Логин и пароль - обязательные поля");
                return;
            }
            if(password != pbxRepPass.Password)
            {
                MessageBox.Show("Пароли не совпадают");
                return;
            }

            // Create new user
            var newUser = new RequiredUser()
            {
                Login = login,
                Password = password
            };

            HttpResponseMessage response;
            using (var client = new HttpClient())
            {
                JsonContent content = JsonContent.Create(newUser);
                response = client.PostAsync("https://localhost:44306/api/Users/PostUser", content).Result;
            }
            var result =response.Content.ReadAsStringAsync().Result;
            if(response.IsSuccessStatusCode)
                NavigationService.Navigate(new FilesPage(Convert.ToInt32(result)));
            else
                MessageBox.Show(result, "Api says");
        }

        private void btnGoBack_Click(object sender, RoutedEventArgs e) => NavigationService.GoBack();
    }
}
    
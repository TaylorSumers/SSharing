using Newtonsoft.Json;
using SecretsSharing.Models;
using SecretsSharing.Windows;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
    /// Interaction logic for FilesPage.xaml
    /// </summary>
    public partial class FilesPage : Page
    {
        private readonly int _userId;

        public FilesPage(int userId)
        {
            InitializeComponent();
            _userId = userId;
            UpdateDatagrid();
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            var selectedFile = dtgFiles.SelectedItem as ResponseFile;
            if (selectedFile == null) 
            {
                MessageBox.Show("Выберите файл");
                return;
            }

            var code = selectedFile.Code;
            HttpResponseMessage response;
            using ( var client = new HttpClient())
            {
                response = client.DeleteAsync($"https://localhost:44306/api/Files/DeleteFile/code={code}").Result;
            }

            MessageBox.Show(response.Content.ReadAsStringAsync().Result);

            UpdateDatagrid();
        }

        private void UpdateDatagrid()
        {
            HttpResponseMessage response;
            using (var client = new HttpClient())
            {
                response = client.GetAsync($"https://localhost:44306/api/Files/GetFilesList/userId={_userId}").Result;
            }

            var result = response.Content.ReadAsStringAsync().Result;

            if (!response.IsSuccessStatusCode)
            {
                MessageBox.Show(result, "Api says");
                return;
            }

            var filesList = JsonConvert.DeserializeObject<List<ResponseFile>>(result);
            dtgFiles.ItemsSource = filesList;
        }

        private void btnDownload_Click(object sender, RoutedEventArgs e)
        {
            var selectedFile = dtgFiles.SelectedItem as ResponseFile;
            if (selectedFile == null)
            {
                MessageBox.Show("Выберите файл");
                return;
            }

            if (selectedFile.FileType == "Строка")
                MessageBox.Show(selectedFile.Name);
            
            if(selectedFile.FileType == "Файл")
            {
                var cloudFileName = $"{selectedFile.Code}{selectedFile.Name.Substring(selectedFile.Name.Contains('.') ? selectedFile.Name.LastIndexOf('.') : 0)}";

                System.Diagnostics.Process.Start("explorer.exe", $"https://storage.yandexcloud.net/ssharing-storage/{cloudFileName}");
            }
            UpdateDatagrid();
        }

        private void btnExit_Click(object sender, RoutedEventArgs e) => NavigationService.GoBack();

        private void btnUpload_Click(object sender, RoutedEventArgs e)
        {
            new AddFileWindow(_userId).ShowDialog();
            UpdateDatagrid();
        }

    }
}

using Microsoft.Win32;
using SecretsSharing.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
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
using System.Windows.Shapes;
using System.IO;

namespace SecretsSharing.Windows
{
    /// <summary>
    /// Interaction logic for AddFileWindow.xaml
    /// </summary>
    public partial class AddFileWindow : Window
    {
        private readonly int _userId;

        string _filePath;

        public AddFileWindow(int userId)
        {
            InitializeComponent();
            _userId = userId;
        }

        private void rbString_Checked(object sender, RoutedEventArgs e) 
        {
            if (tbHeader != null)
                tbHeader.Text = "Текст:";
        }

        private void rbFile_Checked(object sender, RoutedEventArgs e)
        {
            if (tbHeader != null)
                tbHeader.Text = "Путь к файлу:";
        }

        private void tbxVal_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (rbString.IsChecked == true) // String type is checked
                tbxVal.IsReadOnly = false;
            if(rbFile.IsChecked == true) // File type is checked
            {
                tbxVal.IsReadOnly = true;
                var fileDialog = new OpenFileDialog();
                if(fileDialog.ShowDialog() == true)
                {
                    _filePath = fileDialog.FileName;
                    tbxVal.Text = _filePath;
                }
            }
        }

        private void btnUpload_Click(object sender, RoutedEventArgs e)
        {
            if(rbString.IsChecked == true)
            {
                if (string.IsNullOrWhiteSpace(tbxVal.Text))
                {
                    MessageBox.Show("Строка не может быть пустой");
                    return;
                }

                var uploadStr = new RequiredString()
                {
                    Content = tbxVal.Text,
                    Uploader = _userId,
                    DeleteAfterDownload = (bool)chbxDeleteAfterDownload.IsChecked
                };

                HttpResponseMessage response;
                using (var client = new HttpClient())
                {
                    JsonContent content = JsonContent.Create(uploadStr);
                    response = client.PostAsync("https://localhost:44306/api/Files/PostString", content).Result;
                }

                MessageBox.Show(response.Content.ReadAsStringAsync().Result, "Api says");

                Close();
            }

            if(rbFile.IsChecked == true)
            {
                var content = new MultipartFormDataContent();
                var fileContent = new ByteArrayContent(File.ReadAllBytes(_filePath));
                
                content.Add(fileContent, name:"FileToUpload", fileName: System.IO.Path.GetFileName(_filePath));
                content.Add(new StringContent(_userId.ToString()), name: "Uploader");
                content.Add(new StringContent(((bool)chbxDeleteAfterDownload.IsChecked).ToString()), name: "DeleteAfterDownload");

                HttpResponseMessage response;
                using (var client = new HttpClient())
                {
                    response = client.PostAsync("https://localhost:44306/api/Files/PostFile", content).Result;
                }

                MessageBox.Show(response.Content.ReadAsStringAsync().Result, "Api says");

                Close();
            }
        }
    }
}

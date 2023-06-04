using Newtonsoft.Json;
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
using System.Windows.Shapes;

namespace SecretsSharing.Windows
{
    /// <summary>
    /// Interaction logic for GetByUrlWindow.xaml
    /// </summary>
    public partial class GetByUrlWindow : Window
    {
        public GetByUrlWindow()
        {
            InitializeComponent();
        }

        private void btnDownload_Click(object sender, RoutedEventArgs e)
        {
            var downloadUrl = tbxUrl.Text;
            if (string.IsNullOrWhiteSpace(downloadUrl))
            {
                MessageBox.Show("Введите ссылку");
                return;
            }

            if (downloadUrl.Contains("GetFile"))
            {
                HttpResponseMessage response;
                using (var client = new HttpClient())
                {
                    try
                    {
                        response = client.GetAsync(downloadUrl).Result;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                        return;
                    }
                }

                var result = response.Content.ReadAsStringAsync().Result;

                if (!response.IsSuccessStatusCode)
                {
                    MessageBox.Show(result);
                    return;
                }

                var defenition = new { fileName = "", bytes = new byte[0] };

                var recievedFile = JsonConvert.DeserializeAnonymousType(result, defenition);

                File.WriteAllBytes(System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), recievedFile.fileName), recievedFile.bytes);

                MessageBox.Show("Файл сохранен на рабочем столе");
                Close();
            }
            else if (downloadUrl.Contains("GetString"))
            {
                HttpResponseMessage response;
                using (var client = new HttpClient())
                {
                    try
                    {
                        response = client.GetAsync(downloadUrl).Result;
                    }
                    catch(Exception ex) 
                    {
                        MessageBox.Show(ex.Message);
                        return;
                    }
                }
                var result = response.Content.ReadAsStringAsync().Result;
                MessageBox.Show(result);
                if(response.IsSuccessStatusCode)
                    Close();
            }
            else
            {
                MessageBox.Show("Ссылка введена некорректно");
            }

        }
    }
}

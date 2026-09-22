using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SharedLibrary.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Windows;

namespace LabWork3Task2.ViewModels
{
    public partial class MainViewModel : ViewModelBase
    {
        [ObservableProperty]
        private ObservableCollection<User> _users = new();


        [RelayCommand]
        private async Task ProcessResponseAsync()
        {
            try
            {
                using var tcpClient = new TcpClient();
                await tcpClient.ConnectAsync(IPAddress.Parse("127.0.0.1"), 8888);

                var networkStream = tcpClient.GetStream();
                var memoryStream = new MemoryStream();
                var buffer = new byte[1024];

                int bytesRead = 0;

                while ((bytesRead = await networkStream.ReadAsync(buffer, 0, buffer.Length)) > 0)
                {
                    await memoryStream.WriteAsync(buffer, 0, bytesRead);
                }

                memoryStream.Position = 0;

                using var streamReader = new StreamReader(memoryStream, Encoding.UTF8);
                var response = await streamReader.ReadToEndAsync();

                var jsonOptions = new JsonSerializerOptions();
                jsonOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                var users = JsonSerializer.Deserialize<List<User>>(response, jsonOptions);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        //[RelayCommand]
        //private void ReadExcelFile(string excelFilePath)
        //{
        //    if (!File.Exists(excelFilePath))
        //    {
        //        MessageBox.Show("Файл не найден!",
        //                        "Ошибка",
        //                        MessageBoxButton.OK,
        //                        MessageBoxImage.Error);
        //        return;
        //    }

        //    try
        //    {
        //        var csvFile = File.ReadAllLines(filePath);
        //        foreach (var line in csvFile)
        //        {
        //            if (line == null)
        //                return;

        //            var userData = line.Split(";");
        //            User user = new()
        //            {
        //                Id = Convert.ToInt32(userData[0] + 1),
        //                Login = userData[1],
        //                Password = userData[2],
        //                Email = userData[3],
        //            };
        //            Users.Add(user);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.Message,
        //                        "Ошибка",
        //                        MessageBoxButton.OK,
        //                        MessageBoxImage.Error);
        //    }
        //}
    }
}

using SharedLibrary.Models;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;

class Server
{
    const string _filePath = "users.csv";
    private static async Task Main(string[] args)
    {
        var tcpListener = new TcpListener(IPAddress.Parse("127.0.0.1"), 8888);


        try
        {
            tcpListener.Start();
            Console.WriteLine("Сервер запущен");

            var users = await ParseToCsvAsync(_filePath);

            string jsonUsers = JsonSerializer.Serialize(users);
            byte[] sendData = Encoding.UTF8.GetBytes(jsonUsers);

            while (true)
            {
                var tcpClient = await tcpListener.AcceptTcpClientAsync();

                await Task.Run(async () => ClientResponseAsync(tcpClient, sendData));

                tcpClient.Close();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
        finally
        {
            tcpListener.Stop();
        }
    }

    private static async Task ClientResponseAsync(TcpClient tcpClient, byte[] sendData)
    {
        using var networkStream = tcpClient.GetStream();
        await networkStream.WriteAsync(sendData, 0, sendData.Length);
    }

    private static async Task<List<User>> ParseToCsvAsync(string filePath)
    {
        var users = new List<User>();
        using (StreamReader reader = new StreamReader(filePath))
        {
            string? line;

            while ((line = await reader.ReadLineAsync()) != null)
            {
                var userParts = line.Split(';');
                users.Add(new User
                {
                    Id = Convert.ToInt32(userParts[0]),
                    Login = userParts[1],
                    Password = userParts[2],
                    Email = userParts[3]
                });
            }
        }
        return users;
    }
}
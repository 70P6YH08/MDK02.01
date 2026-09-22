using SharedTask2.Models;
using System.Net;
using System.Net.Sockets;
using System.Text;

class Server
{

    private readonly string _filePath = "";
    private static async Task Main(string[] args)
    {
        var tcpListener = new TcpListener(IPAddress.Parse("127.0.0.1"), 8888);

        try
        {
            tcpListener.Start();
            Console.WriteLine("Сервер запущен");

            while (true)
            {
                var tcpClient = await tcpListener.AcceptTcpClientAsync();

                await Task.Run(async () => ClientResponseAsync(tcpClient));

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

        static async Task ClientResponseAsync(TcpClient tcpClient)
        {
            var users = new User
        }
    }
}
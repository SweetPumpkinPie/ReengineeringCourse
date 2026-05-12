using System.Net;
using System.IO;
using System.Net.Sockets;
using System.Threading.Tasks;

public class RealTcpClientWrapper : ITcpClientWrapper
{
    private readonly TcpClient _client;
    public RealTcpClientWrapper(TcpClient client) => _client = client;
    public NetworkStream GetStream() => _client.GetStream();
    public void Close() => _client.Close();
}
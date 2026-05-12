using System.IO;
using System.Net.Sockets;
using System.Threading.Tasks;

public interface ITcpListenerWrapper
{
    void Start();
    void Stop();
    Task<ITcpClientWrapper> AcceptTcpClientAsync();
}
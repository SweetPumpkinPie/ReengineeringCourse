using System.IO;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace EchoTcpServerApp.Wrappers;

public interface ITcpListenerWrapper
{
    void Start();
    void Stop();
    Task<ITcpClientWrapper> AcceptTcpClientAsync();
}
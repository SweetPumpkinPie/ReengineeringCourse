using System.IO;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace EchoTcpServerApp.Wrappers;

public interface ITcpClientWrapper
{
    NetworkStream GetStream();
    void Close();
}
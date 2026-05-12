using System.IO;
using System.Net.Sockets;
using System.Threading.Tasks;

public interface ITcpClientWrapper
{
    NetworkStream GetStream();
    void Close();
}
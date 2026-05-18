using System.Net;
using System.Net.Sockets;
using System.Diagnostics.CodeAnalysis;

namespace EchoTcpServerApp.Wrappers;

[ExcludeFromCodeCoverage]
public class RealTcpListenerWrapper : ITcpListenerWrapper
{
    private readonly TcpListener _listener;
    
    public RealTcpListenerWrapper(int port)
    {
        _listener = new TcpListener(IPAddress.Any, port);
    }

    public void Start() => _listener.Start();
    public void Stop() => _listener.Stop();
    
    public async Task<ITcpClientWrapper> AcceptTcpClientAsync()
    {
        var client = await _listener.AcceptTcpClientAsync();
        return new RealTcpClientWrapper(client);
    }
}
using NUnit.Framework;
using Moq;
using System.Threading.Tasks;
using System.Threading;

namespace EchoTcpServerApp.Wrappers;

public class EchoServerLogicTests
{
    [Test]
    public async Task StartAsync_ShouldCallListenerStart()
    {
        // Arrange
        var listenerMock = new Mock<ITcpListenerWrapper>();
        
        listenerMock.Setup(l => l.AcceptTcpClientAsync())
            .Returns(async () => { await Task.Delay(-1); return new Mock<ITcpClientWrapper>().Object; });

        var server = new EchoServer(listenerMock.Object);
        
        // Act
        _ = Task.Run(() => server.StartAsync());
        
        await Task.Delay(50);

        // Assert
        listenerMock.Verify(l => l.Start(), Times.Once);
        
        server.Stop(); 
    }
}
using NUnit.Framework;
using Moq;
using System.Threading.Tasks;
using System.Threading;

namespace EchoServerTests;

public class EchoServerLogicTests
{
    [Test]
    public void StartAsync_ShouldCallListenerStart()
    {
        // Arrange
        var listenerMock = new Mock<ITcpListenerWrapper>();
        
        listenerMock.Setup(l => l.AcceptTcpClientAsync())
            .Returns(async () => { await Task.Delay(-1); return null; });

        var server = new EchoServer(listenerMock.Object);
        
        // Act
        _ = Task.Run(() => server.StartAsync());
        
        Thread.Sleep(50); 

        // Assert
        listenerMock.Verify(l => l.Start(), Times.Once);
        
        server.Stop(); 
    }
}
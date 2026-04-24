using NetArchTest.Rules;
using NetSdrClientApp;

namespace NetSdrClientAppTests;

public class ArchitectureTests
{
    [Test]
    public void MessagesNamespace_ShouldNot_DependOnNetworking()
    {
        // Arrange
        var assembly = typeof(NetSdrClient).Assembly;

        // Act
        var result = Types.InAssembly(assembly)
            .That()
            .ResideInNamespace("NetSdrClientApp.Messages")
            .ShouldNot()
            .HaveDependencyOn("NetSdrClientApp.Networking")
            .GetResult();

        // Assert
        Assert.That(result.IsSuccessful, Is.True, "An architectural rule has been violated! The Messages module must not depend on Networking.");
    }
}
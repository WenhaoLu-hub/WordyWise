using FluentAssertions;
using NetArchTest.Rules;

namespace Architecture.Tests;

public class ArchitectureTest2s
{
    private const string DomainNamespace = "Domain";
    private const string ApplicationNamespace = "Application";
    private const string InfrastructureNamespace = "Infrastructure";
    private const string PersistenceNamespace = "Persistence";
    private const string PresentationNamespace = "Presentation";
    private const string WebAPINamespace = "WebAPI";

    [Fact]
    public void Domain_Should_Not_HaveDependencyOnOtherProjects()
    {
        //Arrange
        var assembly = Domain.AssemblyReference.Assembly();
        var otherProjects = new[]
        {
            ApplicationNamespace,
            InfrastructureNamespace,
            PersistenceNamespace,
            PresentationNamespace,
            WebAPINamespace
        };
        // Act
        var testResult = Types
            .InAssembly(assembly)
            .ShouldNot()
            .HaveDependencyOnAll(otherProjects)
            .GetResult();

        // Assert
        testResult.IsSuccessful.Should().BeTrue();
    }
    
    [Fact]
    public void Application_Should_Not_HaveDependencyOnOtherProjects()
    {
        //Arrange
        var assembly = Application.AssemblyReference.Assembly();
        var otherProjects = new[]
        {
            InfrastructureNamespace,
            PersistenceNamespace,
            PresentationNamespace,
            WebAPINamespace
        };
        // Act
        var testResult = Types
            .InAssembly(assembly)
            .ShouldNot()
            .HaveDependencyOnAll(otherProjects)
            .GetResult();

        // Assert
        testResult.IsSuccessful.Should().BeTrue();
    }

    [Fact]
    public void Handler_Should_Have_DependencyOnDomain()
    {
        //Arrange
        var assembly = Application.AssemblyReference.Assembly();
        
        //Act
        var testResult = Types
            .InAssembly(assembly)
            .That()
            .HaveNameEndingWith("Handler")
            .Should()
            .HaveDependencyOn(DomainNamespace)
            .GetResult();

        //Assert
        testResult.IsSuccessful.Should().BeTrue();
    }
    
    [Fact]
    public void Infrastructure_Should_Not_HaveDependencyOnOtherProjects()
    {
        //Arrange
        var assembly = Infrastructure.AssemblyReference.Assembly();
        var otherProjects = new[]
        {
            PersistenceNamespace,
            PresentationNamespace,
            WebAPINamespace
        };
        // Act
        var testResult = Types
            .InAssembly(assembly)
            .ShouldNot()
            .HaveDependencyOnAll(otherProjects)
            .GetResult();

        // Assert
        testResult.IsSuccessful.Should().BeTrue();
    }
    
    [Fact]
    public void Persistence_Should_Not_HaveDependencyOnOtherProjects()
    {
        //Arrange
        var assembly = Persistence.AssemblyReference.Assembly();
        var otherProjects = new[]
        {
            InfrastructureNamespace,
            PresentationNamespace,
            WebAPINamespace
        };
        // Act
        var testResult = Types
            .InAssembly(assembly)
            .ShouldNot()
            .HaveDependencyOnAll(otherProjects)
            .GetResult();

        // Assert
        testResult.IsSuccessful.Should().BeTrue();
    }
    
    [Fact]
    public void Presentation_Should_Not_HaveDependencyOnOtherProjects()
    {
        //Arrange
        var assembly = Presentation.AssemblyReference.Assembly();
        var otherProjects = new[]
        {
            InfrastructureNamespace,
            PersistenceNamespace,
            WebAPINamespace
        };
        // Act
        var testResult = Types
            .InAssembly(assembly)
            .ShouldNot()
            .HaveDependencyOnAll(otherProjects)
            .GetResult();

        // Assert
        testResult.IsSuccessful.Should().BeTrue();
    }

    [Fact]
    public void Controller_Should_Have_DependencyOnMediatR()
    {
        //Arrange
        var assembly = Presentation.AssemblyReference.Assembly();
        
        //Act
        var testResult = Types
            .InAssembly(assembly)
            .That()
            .HaveNameEndingWith("Controller")
            .Should()
            .HaveDependencyOn("MediatR")
            .GetResult();
        
        //Assert
        testResult.IsSuccessful.Should().BeTrue();
    }
    
}
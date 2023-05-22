using Application.Users.Commands;
using Domain.Entities.UserAggregate;
using Domain.Errors;
using Domain.Repositories;
using Domain.ValueObjects;
using FluentAssertions;
using Moq;

namespace WebAPI.Application.Test.Users.Commands;

public class CreateUserCommandHandlerTest
{
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;

    public CreateUserCommandHandlerTest()
    {
        _userRepositoryMock = new();
        _unitOfWorkMock = new();
    }

    [Fact]
    public async void Handle_Should_ReturnFailureResult_WhenPhoneNumberIsNotUnique()
    {
        //Arrange
        var command = new CreateUserCommand("Test","123456789");
        _userRepositoryMock.Setup(
                x => x.IsPhoneNumberUniqueAsync(
                    It.IsAny<PhoneNumber>(),
                    It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);
        var handler = new CreateUserCommandHandler(
            _userRepositoryMock.Object,
            _unitOfWorkMock.Object);
        
        //Act
        var result = await handler.Handle(command,default);
        
        //Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().BeEquivalentTo(DomainErrors.User.DuplicatePhoneNumber);
    }
    
    [Fact]
    public async void Handle_Should_ReturnTrueResult_WhenPhoneNumberIsUnique()
    {
        //Arrange
        var command = new CreateUserCommand("Test","123456789");
        _userRepositoryMock.Setup(
                x => x.IsPhoneNumberUniqueAsync(
                    It.IsAny<PhoneNumber>(),
                    It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);
        var handler = new CreateUserCommandHandler(
            _userRepositoryMock.Object,
            _unitOfWorkMock.Object);
        
        //Act
        var result = await handler.Handle(command,default);
        
        //Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeEmpty();
    }
    
    
    [Fact]
    public async void Handle_Should_CallRepository_WhenPhoneNumberIsUnique()
    {
        //Arrange
        var command = new CreateUserCommand("Test","123456789");
        _userRepositoryMock.Setup(
                x => x.IsPhoneNumberUniqueAsync(
                    It.IsAny<PhoneNumber>(),
                    It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);
        var handler = new CreateUserCommandHandler(
            _userRepositoryMock.Object,
            _unitOfWorkMock.Object);
        
        //Act
        var result = await handler.Handle(command,default);
        
        //Assert
        _userRepositoryMock.Verify(
            x => x.Add(It.Is<User>(x => x.Id == result.Value)),
            Times.Once);

    }
    
    [Fact]
    public async void Handle_Should_Not_CallUnitOfWork_WhenPhoneNumberIsNotUnique()
    {
        //Arrange
        var command = new CreateUserCommand("Test","123456789");
        _userRepositoryMock.Setup(
                x => x.IsPhoneNumberUniqueAsync(
                    It.IsAny<PhoneNumber>(),
                    It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);
        var handler = new CreateUserCommandHandler(
            _userRepositoryMock.Object,
            _unitOfWorkMock.Object);
        
        //Act
        await handler.Handle(command,default);
        
        //Assert
        _unitOfWorkMock.Verify(
            x=>x.SaveChangesAsync(It.IsAny<CancellationToken>()),Times.Never);

    }
}
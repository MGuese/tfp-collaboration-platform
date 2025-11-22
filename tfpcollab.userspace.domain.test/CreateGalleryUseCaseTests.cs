using Microsoft.Extensions.Logging;
using NSubstitute;
using Shouldly;
using FluentResults;
using NSubstitute.ExceptionExtensions;
using tfp_collab_userspace_domain.DomainObjects;
using tfp_collab_userspace_domain.Request;
using tfp_collab_userspace_domain.UseCase;
using tfp_collab_userspace_domain.Service;

namespace tfp_collab_userspace_domain_test;

[TestFixture]
public class CreateGalleryUseCaseTests
{
    [Test]
    public async Task Handle_ValidRequest_CallsDatabaseServiceAndOutputPortWithSuccess()
    {
        // Arrange
        var logger = Substitute.For<ILogger<CreateGalleryUseCase>>();
        var mockGalleryRepository = Substitute.For<IGalleryRepository>();
        var mockUnitOfWork = Substitute.For<IUnitOfWork>();
        mockUnitOfWork.GalleryRepository = mockGalleryRepository;
        
        var request = new CreateGalleryRequest { Name = "Test Gallery", OwnerId = Guid.NewGuid() };
        
        // Setup the mock to return a specific Id when CreateGalleryAsync is called
        GalleryDo galleryDo = new()
        {
            Name = "Meine erste Gallery",
            OwnerId = OwnerId.New(),
            AddedOn = DateTime.UtcNow
        };
        mockGalleryRepository
            .CreateAsync(Arg.Any<GalleryDo>())
            .Returns(Task.FromResult(Result.Ok(galleryDo)));

        var useCase = new CreateGalleryUseCase(mockUnitOfWork, logger);

        // Act
        var createGalleryResponse = await useCase.Handle(request);

        // Assert
        // Verify that the database service was called with the correct GalleryDto
        await mockGalleryRepository
            .Received(1)
            .CreateAsync(Arg.Is<GalleryDo>(dto =>
                dto.Name == request.Name &&
                dto.OwnerId == request.OwnerId
            ));

        // Verify that the output port was called with a successful response and the correct Id
        createGalleryResponse.IsSuccessul.ShouldBeTrue();
        createGalleryResponse.Id.ShouldBe(galleryDo.Id.Value);
        createGalleryResponse.ErrorMessage.ShouldBeEmpty();
    }

    [Test]
    public async Task Handle_DatabaseServiceThrowsException_CallsOutputPortWithError()
    {
        // Arrange
        var mockUnitOfWork = Substitute.For<IUnitOfWork>();
        var mockGalleryRepository = Substitute.For<IGalleryRepository>();
        mockUnitOfWork.GalleryRepository = mockGalleryRepository;
        var logger = Substitute.For<ILogger<CreateGalleryUseCase>>();
        var request = new CreateGalleryRequest { Name = "Test Gallery", OwnerId = Guid.NewGuid() };
        const string expectedErrorMessage = "Database error occurred.";
        var databaseException = new Exception(expectedErrorMessage);

        // Setup the mock to throw an exception
        mockGalleryRepository
            .CreateAsync(Arg.Any<GalleryDo>())
            .ThrowsAsync(databaseException);

        var useCase = new CreateGalleryUseCase(mockUnitOfWork, logger);

        // Act
        var createGalleryResponse = await useCase.Handle(request);

        // Assert
        // Verify that the database service was called
        await mockGalleryRepository.Received(1).CreateAsync(Arg.Any<GalleryDo>());

        createGalleryResponse.IsSuccessul.ShouldBeFalse();
        createGalleryResponse.Id.ShouldBe(Guid.Empty);
        createGalleryResponse.ErrorMessage.ShouldStartWith($"Error creating gallery:");
        createGalleryResponse.ErrorMessage.ShouldContain(expectedErrorMessage);
    }

    [Test]
    public async Task Handle_NullRequest_DoesNotCallDatabaseServiceButStillCallsOutputPortWithError()
    {
        // Arrange
        var mockUnitOfWork = Substitute.For<IUnitOfWork>();
        var mockGalleryRepository = Substitute.For<IGalleryRepository>();
        mockUnitOfWork.GalleryRepository = mockGalleryRepository;
        var logger = Substitute.For<ILogger<CreateGalleryUseCase>>();
        CreateGalleryRequest request = null!; // Simulate a null request
        const string expectedErrorMessage = "Object reference not set to an instance of an object."; // Default exception message for null reference

        var useCase = new CreateGalleryUseCase(mockUnitOfWork, logger);

        // Act
        var createGalleryResponse = await useCase.Handle(request);

        // Assert
        await mockUnitOfWork.GalleryRepository.DidNotReceive().CreateAsync(Arg.Any<GalleryDo>());

        // Verify that the output port was called with an error response due to the exception
        
        createGalleryResponse.IsSuccessul.ShouldBeFalse();
        createGalleryResponse.Id.ShouldBe(Guid.Empty);
        createGalleryResponse.ErrorMessage.ShouldStartWith("Error creating gallery:");
        createGalleryResponse.ErrorMessage.ShouldContain(expectedErrorMessage);
    }
    
    [Test]
    public async Task Handle_NullRequest_RepositoryCreateGalleryReturnsResultFailed()
    {
        // Arrange
        var mockUnitOfWork = Substitute.For<IUnitOfWork>();
        var mockGalleryRepository = Substitute.For<IGalleryRepository>();
        mockUnitOfWork.GalleryRepository = mockGalleryRepository;
        mockGalleryRepository
            .CreateAsync(Arg.Any<GalleryDo>())
            .Returns(Task.FromResult(Result.Fail<GalleryDo>(["Unique Key Exception."])));
        var logger = Substitute.For<ILogger<CreateGalleryUseCase>>();
        CreateGalleryRequest request = new () { Name = "Test Gallery", OwnerId = Guid.NewGuid() };

        var useCase = new CreateGalleryUseCase(mockUnitOfWork, logger);

        // Act
        var createGalleryResponse = await useCase.Handle(request);

        // Assert
        createGalleryResponse.IsSuccessul.ShouldBeFalse();
        createGalleryResponse.Id.ShouldBe(Guid.Empty);
        createGalleryResponse.ErrorMessage.ShouldStartWith("Unique Key Exception.");
    }
}
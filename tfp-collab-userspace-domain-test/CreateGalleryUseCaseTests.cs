using Microsoft.Extensions.Logging;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using tfp_collab_userspace_domain.dto;
using tfp_collab_userspace_domain.OutputPorts;
using tfp_collab_userspace_domain.Request;
using tfp_collab_userspace_domain.Response;
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
        var mockDatabaseService = Substitute.For<IDatabaseService>();
        var mockOutputPort = Substitute.For<ICreateGalleryOutputPort>();
        var logger = Substitute.For<ILogger<CreateGalleryUseCase>>();
        var request = new CreateGalleryRequest { Name = "Test Gallery", OwnerId = Guid.NewGuid() };
        var expectedGalleryId = Guid.NewGuid();

        // Setup the mock to return a specific Id when CreateGalleryAsync is called
        mockDatabaseService.CreateGalleryAsync(Arg.Any<GalleryDto>())
            .Returns(Task.CompletedTask)
            .AndDoes(info =>
            {
                // Simulate the database setting the Id after creation
                var galleryDto = info.Arg<GalleryDto>();
                galleryDto.Id = expectedGalleryId;
            });

        var useCase = new CreateGalleryUseCase(mockDatabaseService, mockOutputPort, logger);

        // Act
        await useCase.Handle(request);

        // Assert
        // Verify that the database service was called with the correct GalleryDto
        await mockDatabaseService
            .Received(1)
            .CreateGalleryAsync(Arg.Is<GalleryDto>(dto =>
                dto.Name == request.Name &&
                dto.OwnerId == request.OwnerId &&
                dto.Id == expectedGalleryId // Verify Id was set
            ));

        // Verify that the output port was called with a successful response and the correct Id
        await mockOutputPort
            .Received(1)
            .Handle(Arg.Is<CreateGalleryResponse>(response =>
                response.Success &&
                response.Id == expectedGalleryId &&
                response.FailureNumber == null
            ));
    }

    [Test]
    public async Task Handle_DatabaseServiceThrowsException_CallsOutputPortWithError()
    {
        // Arrange
        var mockDatabaseService = Substitute.For<IDatabaseService>();
        var mockOutputPort = Substitute.For<ICreateGalleryOutputPort>();
        var logger = Substitute.For<ILogger<CreateGalleryUseCase>>();
        var request = new CreateGalleryRequest { Name = "Test Gallery", OwnerId = Guid.NewGuid() };
        var expectedErrorMessage = "Database error occurred.";
        var databaseException = new Exception(expectedErrorMessage);

        // Setup the mock to throw an exception
        mockDatabaseService.CreateGalleryAsync(Arg.Any<GalleryDto>())
            .ThrowsAsync(databaseException);

        var useCase = new CreateGalleryUseCase(mockDatabaseService, mockOutputPort, logger);

        // Act
        await useCase.Handle(request);

        // Assert
        // Verify that the database service was called
        await mockDatabaseService.Received(1).CreateGalleryAsync(Arg.Any<GalleryDto>());

        // Verify that the output port was called with an error response and the correct message
        await mockOutputPort.Received(1).Handle(Arg.Is<CreateGalleryResponse>(response =>
            !response.Success &&
            response.Id == null &&
            response.FailureNumber == $"Error creating gallery: {expectedErrorMessage}"
        ));
    }

    [Test]
    public async Task Handle_NullRequest_DoesNotCallDatabaseServiceButStillCallsOutputPortWithError()
    {
        // Arrange
        var mockDatabaseService = Substitute.For<IDatabaseService>();
        var mockOutputPort = Substitute.For<ICreateGalleryOutputPort>();
        var logger = Substitute.For<ILogger<CreateGalleryUseCase>>();
        CreateGalleryRequest request = null; // Simulate a null request
        var expectedErrorMessage = "Object reference not set to an instance of an object."; // Default exception message for null reference

        var useCase = new CreateGalleryUseCase(mockDatabaseService, mockOutputPort, logger);

        // Act
        await useCase.Handle(request);

        // Assert
        // Verify that the database service was NOT called
        await mockDatabaseService.DidNotReceive().CreateGalleryAsync(Arg.Any<GalleryDto>());

        // Verify that the output port was called with an error response due to the exception
        await mockOutputPort.Received(1).Handle(Arg.Is<CreateGalleryResponse>(response =>
            !response.Success &&
            response.Id == null &&
            response.FailureNumber.StartsWith("Error creating gallery:") &&
            response.FailureNumber.Contains(expectedErrorMessage)
        ));
    }
}
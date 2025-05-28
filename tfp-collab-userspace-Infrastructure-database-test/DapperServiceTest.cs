using System.Reflection;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Shouldly;
using tfp_collab_userspace_interfaces.dto;
using tfp_collab_userspace_storage_database;
using tfp_collab_userspace_storage_database.Model;
using tfp_collab_userspace_storage_database.Repositories;

namespace tfp_collab_userspace_storage_database_test;

public class DapperServiceTest
{
    [SetUp]
    public void OneTimeSetup()
    {
        DapperExtensions.DapperExtensions.SetMappingAssemblies([Assembly.GetExecutingAssembly()]);
        Dapper.SqlMapper.AddTypeHandler(typeof(Guid), new GuidTypeHandler());
    }

    [Test]
    public async Task CreateGallery()
    {
        // Arrange
        GalleryDto gallery = new ()
        { 
            Name = "Meine erste Gallery", 
            OwnerId = Guid.NewGuid()
        };
        InMemoryDatabase db = new ();
        var connectionFactoryMock = Substitute.For<IDbConnectionFactory>();
        var logging = Substitute.For<ILogger<DapperService>>();
        using var connection = db.OpenConnection();
        connectionFactoryMock.CreateConnection().Returns(connection);
        using var galleryRepository = new GalleryRepository(connectionFactoryMock);
        DapperService service = new(galleryRepository, logging);
        
        // Act
        await service.CreateGalleryAsync(gallery);
        
        // Assert
        gallery.Id.ShouldNotBe(Guid.Empty);
        gallery.Name.ShouldBe(gallery.Name);
        gallery.OwnerId.ShouldBe(gallery.OwnerId);
    }
    
    [Test]
    public async Task CreateGalleryFailed()
    {
        // Arrange
        Gallery gallery = new ()
        { 
            Id = Guid.NewGuid(),
            AddedOn = DateTime.Now,
            Name = "Meine erste Gallery", 
            OwnerId = Guid.NewGuid()
        };
        InMemoryDatabase db = new ();
        db.Insert([gallery]);
        
        GalleryDto galleryDto = new ()
        { 
            Name = "Meine erste Gallery", 
            OwnerId = gallery.OwnerId,
            AddedOn = DateTime.Now
        };
        var connectionFactoryMock = Substitute.For<IDbConnectionFactory>();
        var logging = Substitute.For<ILogger<DapperService>>();
        using var connection = db.OpenConnection();
        connectionFactoryMock.CreateConnection().Returns(connection);
        using var galleryRepository = new GalleryRepository(connectionFactoryMock);
        DapperService service = new(galleryRepository, logging);
        
        // Act
        var galleryDtoResult = await service.CreateGalleryAsync(galleryDto);
        
        // Assert
        galleryDtoResult.ShouldBeNull();
    }
    
    [Test]
    public async Task GetGallery()
    {
        // Arrange
        Gallery gallery = new()
        {
            Id = Guid.NewGuid(),
            Name = "Meine erste Gallery",
            OwnerId = Guid.NewGuid(),
            AddedOn = DateTime.UtcNow
        };
        var db = new InMemoryDatabase();
        db.Insert<Gallery>([gallery]);
        var connectionFactoryMock = Substitute.For<IDbConnectionFactory>();
        var logging = Substitute.For<ILogger<DapperService>>();
        using var connection = db.OpenConnection();
        connectionFactoryMock.CreateConnection().Returns(connection);
        using var galleryRepository = new GalleryRepository(connectionFactoryMock);
        DapperService service = new(galleryRepository, logging);
        
        // Act
        var galleryDto = await service.GetAsync(gallery.OwnerId, gallery.Id);
        
        // Assert
        galleryDto.ShouldNotBeNull();
        galleryDto.Id.ShouldBe(gallery.Id);
        galleryDto.Name.ShouldBe(gallery.Name);
        galleryDto.OwnerId.ShouldBe(gallery.OwnerId);
        galleryDto.AddedOn.ShouldBe(gallery.AddedOn);
    }
    
    [Test]
    public async Task GetGalleryFailed()
    {
        // Arrange
        var db = new InMemoryDatabase();
        var connectionFactoryMock = Substitute.For<IDbConnectionFactory>();
        var logging = Substitute.For<ILogger<DapperService>>();
        using var connection = db.OpenConnection();
        connectionFactoryMock.CreateConnection().Returns(connection);
        using var galleryRepository = new GalleryRepository(connectionFactoryMock);
        DapperService service = new(galleryRepository, logging);
        
        // Act
        var galleryDto = await service.GetAsync(Guid.NewGuid(), Guid.NewGuid());
        
        // Assert
        galleryDto.ShouldBeNull();
    }
    
    [Test]
    public async Task DeleteGallery()
    {
        // Arrange
        Gallery gallery = new()
        {
            Id = Guid.NewGuid(),
            Name = "Meine erste Gallery",
            OwnerId = Guid.NewGuid(),
            AddedOn = DateTime.UtcNow
        };
        var db = new InMemoryDatabase();
        db.Insert<Gallery>([gallery]);
        var connectionFactoryMock = Substitute.For<IDbConnectionFactory>();
        var logging = Substitute.For<ILogger<DapperService>>();
        using var connection = db.OpenConnection();
        connectionFactoryMock.CreateConnection().Returns(connection);
        using var galleryRepository = new GalleryRepository(connectionFactoryMock);
        DapperService service = new(galleryRepository, logging);
        
        // Act
        await service.DeleteGalleryAsync(gallery.Id);
        
        // Assert
        var galleryDto = await service.GetAsync(gallery.Id);
        galleryDto.ShouldBeNull();
    }
}
using System.Reflection;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Shouldly;
using tfp_collab_userspace_domain.DomainObjects;
using tfp_collab_userspace_domain.Service;
using tfp_collab_userspace_storage_database;
using tfp_collab_userspace_storage_database.Model;
using tfp_collab_userspace_storage_database.Repositories;

namespace tfp_collab_userspace_storage_database_test;

public class UnitOfWorkTest
{
    [SetUp]
    public void Setup()
    {
        DapperExtensions.DapperExtensions.SetMappingAssemblies([Assembly.GetExecutingAssembly()]);
        Dapper.SqlMapper.AddTypeHandler(new GuidTypeHandler());
    }

    [Test]
    public async Task CreateGallery()
    {
        // Arrange
        GalleryDo galleryDo = new ()
        { 
            Name = "Meine erste Gallery", 
            OwnerId = OwnerId.New()
        };
        InMemoryDatabase db = new ();
        var connectionFactoryMock = Substitute.For<IDbConnectionFactory>();
        var logging = Substitute.For<ILogger<IUnitOfWork>>();
        using var connection = db.OpenConnection();
        connectionFactoryMock.CreateConnection().Returns(connection);
        using UnitOfWork uof = new (connectionFactoryMock, logging);
        
        // Act
        var galleryDoResult = await uof.GalleryRepository.CreateAsync(galleryDo);
        await uof.SaveAsync();
        
        // Assert
        galleryDoResult.IsSuccess.ShouldBeTrue();
        
        galleryDoResult.Value.Id.ShouldNotBe((GalleryId)Guid.Empty);
        galleryDoResult.Value.Name.ShouldBe(galleryDo.Name);
        galleryDoResult.Value.OwnerId.ShouldBe(galleryDo.OwnerId);
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
        
        GalleryDo galleryDo = new ()
        { 
            Name = "Meine erste Gallery", 
            OwnerId = OwnerId.New(),
            AddedOn = DateTime.Now
        };
        var connectionFactoryMock = Substitute.For<IDbConnectionFactory>();
        var logging = Substitute.For<ILogger<IUnitOfWork>>();
        using var connection = db.OpenConnection();
        connectionFactoryMock.CreateConnection().Returns(connection);
        using UnitOfWork uof = new (connectionFactoryMock, logging);
        
        // Act
        var galleryDtoResult = await uof.GalleryRepository.CreateAsync(galleryDo);
        await uof.SaveAsync();
        
        // Assert
        galleryDtoResult.IsSuccess.ShouldBeTrue();
    }
    
    [Test]
    public async Task GetGalleryByOwnerIdAndGalleryId()
    {
        var galleryId = GalleryId.New();
        var ownerId = OwnerId.New();
        // Arrange
        Gallery gallery = new()
        {
            Id = galleryId,
            Name = "Meine erste Gallery",
            OwnerId = ownerId,
            AddedOn = DateTime.UtcNow
        };
        var db = new InMemoryDatabase();
        db.Insert<Gallery>([gallery]);
        var connectionFactoryMock = Substitute.For<IDbConnectionFactory>();
        var logging = Substitute.For<ILogger<IUnitOfWork>>();
        using var connection = db.OpenConnection();
        connectionFactoryMock.CreateConnection().Returns(connection);
        using UnitOfWork uof = new (connectionFactoryMock, logging);
        
        // Act
        var galleryDoResult = await uof.GalleryRepository.GetAsync(ownerId, galleryId);
        await uof.SaveAsync();
        
        // Assert
        galleryDoResult.IsSuccess.ShouldBeTrue();
        var galleryDo = galleryDoResult.Value;
        galleryDo.ShouldNotBeNull();
        galleryDo.Id.ShouldBe((GalleryId)gallery.Id);
        galleryDo.Name.ShouldBe(gallery.Name);
        galleryDo.OwnerId.ShouldBe((OwnerId)gallery.OwnerId);
        galleryDo.AddedOn.ShouldBe(gallery.AddedOn);
    }
    
    [Test]
    public async Task GetGallery()
    {
        var galleryId = GalleryId.New();
        var ownerId = OwnerId.New();
        // Arrange
        Gallery gallery = new()
        {
            Id = galleryId,
            Name = "Meine erste Gallery",
            OwnerId = ownerId,
            AddedOn = DateTime.UtcNow
        };
        var db = new InMemoryDatabase();
        db.Insert<Gallery>([gallery]);
        var connectionFactoryMock = Substitute.For<IDbConnectionFactory>();
        var logging = Substitute.For<ILogger<IUnitOfWork>>();
        using var connection = db.OpenConnection();
        connectionFactoryMock.CreateConnection().Returns(connection);
        using UnitOfWork uof = new (connectionFactoryMock, logging);
        
        // Act
        var galleryDoResult = await uof.GalleryRepository.GetAsync(ownerId);
        await uof.SaveAsync();
        
        // Assert
        galleryDoResult.IsSuccess.ShouldBeTrue();
        var galleryDo = galleryDoResult.Value.FirstOrDefault();
        galleryDo.ShouldNotBeNull();
        galleryDo.Id.ShouldBe((GalleryId)gallery.Id);
        galleryDo.Name.ShouldBe(gallery.Name);
        galleryDo.OwnerId.ShouldBe((OwnerId)gallery.OwnerId);
        galleryDo.AddedOn.ShouldBe(gallery.AddedOn);
    }
    
    [Test]
    public async Task GetGalleryFailed()
    {
        // Arrange
        var db = new InMemoryDatabase();
        using var connection = db.OpenConnection();
        var connectionFactoryMock = Substitute.For<IDbConnectionFactory>();
        var logging = Substitute.For<ILogger<IUnitOfWork>>();
        
        connectionFactoryMock.CreateConnection().Returns(connection);
        using UnitOfWork uof = new (connectionFactoryMock, logging);
        
        // Act
        var galleryDtoResult = await uof.GalleryRepository.GetAsync(OwnerId.New(), GalleryId.New());
        await uof.SaveAsync();
        
        // Assert
        galleryDtoResult.IsFailed.ShouldBeTrue();
    }
    
    [Test]
    public async Task DeleteGallery()
    {
        // Arrange
        var galleryId = GalleryId.New();
        var ownerId = OwnerId.New();
        Gallery gallery = new()
        {
            Id = galleryId,
            Name = "Meine erste Gallery",
            OwnerId = ownerId,
            AddedOn = DateTime.UtcNow
        };
        
        var db = new InMemoryDatabase();
        using var connection = db.OpenConnection();
        db.Insert<Gallery>([gallery]);
        var connectionFactoryMock = Substitute.For<IDbConnectionFactory>();
        var logging = Substitute.For<ILogger<IUnitOfWork>>();
        
        connectionFactoryMock.CreateConnection().Returns(connection);
        using UnitOfWork uof = new (connectionFactoryMock, logging);
        
        // Act
        var deleteGalleryResult = await uof.GalleryRepository.DeleteAsync(galleryId);
        await uof.SaveAsync();
        
        // Assert
        deleteGalleryResult.IsSuccess.ShouldBeTrue();
    }
}
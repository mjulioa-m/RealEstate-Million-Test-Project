using NUnit.Framework;
using Moq;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq; 
using RealEstate.Api.Controllers;
using RealEstate.Api.Domain;
using RealEstate.Api.Repositories;
using RealEstate.Api.Dtos; 

namespace RealEstate.Tests
{
    [TestFixture]
    public class PropertiesControllerTests
    {
        private Mock<IPropertyRepository> _repoMock = null!;
        private Mock<ILogger<PropertiesController>> _loggerMock = null!;
        private PropertiesController _controller = null!;

        [SetUp]
        public void Setup()
        {
            _repoMock = new Mock<IPropertyRepository>();
            _loggerMock = new Mock<ILogger<PropertiesController>>();
            _controller = new PropertiesController(_repoMock.Object, _loggerMock.Object);
        }

        [Test]
        public async Task Get_ReturnsOkWithDtoList_WhenPropertiesExist()
        {
            var properties = new List<Property>
            {
                new Property { 
                    Id = "1", IdOwner = "owner1", Name = "Casa Bella", 
                    Address = "Calle Falsa 123", Price = 120000, 
                    Image = new PropertyImage { File = "url1" } 
                },
                new Property { 
                    Id = "2", IdOwner = "owner2", Name = "Casa Linda", 
                    Address = "Avenida Real 45", Price = 220000, 
                    Image = new PropertyImage { File = "url2" } 
                }
            };

            _repoMock
                .Setup(r => r.GetPropertiesAsync(
                    It.IsAny<string?>(), It.IsAny<string?>(), It.IsAny<decimal?>(), 
                    It.IsAny<decimal?>(), It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(properties);

            var result = await _controller.Get(null, null, null, null, 1, 10);

            Assert.That(result, Is.TypeOf<OkObjectResult>());
            var okResult = result as OkObjectResult;
            Assert.That(okResult, Is.Not.Null);

            var dtos = okResult!.Value as List<PropertyDto>; 
            Assert.That(dtos, Is.Not.Null);
            Assert.That(dtos.Count, Is.EqualTo(2));

            Assert.That(dtos[0].Name, Is.EqualTo(properties[0].Name));
            Assert.That(dtos[0].ImageUrl, Is.EqualTo(properties[0].Image?.File)); 
            Assert.That(dtos[1].IdOwner, Is.EqualTo(properties[1].IdOwner));
            Assert.That(dtos[1].ImageUrl, Is.EqualTo(properties[1].Image?.File));
        }

        [Test]
        public async Task GetById_ReturnsOkWithDto_WhenPropertyExists()
        {
            var property = new Property
            {
                Id = "1",
                IdOwner = "owner1",
                Name = "Casa Bella",
                Address = "Calle Falsa 123",
                Price = 120000,
                Image = new PropertyImage { File = "url1", Enabled = true } 
            };
            _repoMock.Setup(r => r.GetByIdAsync("1")).ReturnsAsync(property);

            var result = await _controller.GetById("1");

            Assert.That(result, Is.TypeOf<OkObjectResult>());
            var okResult = result as OkObjectResult;
            Assert.That(okResult, Is.Not.Null);

            var dto = okResult!.Value as PropertyDto; 
            Assert.That(dto, Is.Not.Null);

            Assert.That(dto.Id, Is.EqualTo(property.Id));
            Assert.That(dto.Name, Is.EqualTo(property.Name));
            Assert.That(dto.ImageUrl, Is.EqualTo(property.Image.File)); 
        }

        [Test]
        public async Task GetById_ReturnsNotFound_WhenPropertyDoesNotExist()
        {
            _repoMock.Setup(r => r.GetByIdAsync("99")).ReturnsAsync((Property?)null);

            var result = await _controller.GetById("99");

            Assert.That(result, Is.TypeOf<NotFoundResult>());
        }

        [Test]
        public async Task Create_ReturnsCreatedAtActionWithDto()
        {
            var property = new Property
            {
                IdOwner = "owner3",
                Name = "Casa Nueva",
                Address = "Calle Luna 12",
                Price = 150000,
                Image = new PropertyImage { File = "url3" } 
            };
            _repoMock.Setup(r => r.CreateAsync(It.IsAny<Property>())).Returns(Task.CompletedTask)
                .Callback<Property>(p => p.Id = "newId"); 

            var result = await _controller.Create(property);

            Assert.That(result, Is.TypeOf<CreatedAtActionResult>());
            var created = result as CreatedAtActionResult;
            Assert.That(created, Is.Not.Null);
            Assert.That(created!.ActionName, Is.EqualTo(nameof(_controller.GetById)));

            var dto = created.Value as PropertyDto; 
            Assert.That(dto, Is.Not.Null);
            Assert.That(dto.Name, Is.EqualTo(property.Name));
            Assert.That(dto.ImageUrl, Is.EqualTo(property.Image.File)); 
            Assert.That(dto.Id, Is.EqualTo("newId"));
        }

        [Test]
        public async Task CreateBatch_ReturnsOk_WhenAllPropertiesAdded()
        {
            var properties = new List<Property>
            {
                new Property { Name = "Casa 1", Image = new PropertyImage { File = "url1" } }, 
                new Property { Name = "Casa 2", Image = new PropertyImage { File = "url2" } } 
            };

            _repoMock.Setup(r => r.CreateManyAsync(properties)).Returns(Task.CompletedTask);

            var result = await _controller.CreateBatch(properties);

            Assert.That(result, Is.TypeOf<OkObjectResult>());
            _repoMock.Verify(r => r.CreateManyAsync(properties), Times.Once);
        }

        [Test]
        public async Task CreateBatch_ReturnsServerError_WhenExceptionThrown()
        {
            var properties = new List<Property> { new Property { Name = "Test" } };
            _repoMock.Setup(r => r.CreateManyAsync(properties)).ThrowsAsync(new System.Exception("Error"));

            var result = await _controller.CreateBatch(properties);


            var statusResult = result as ObjectResult;
            Assert.That(statusResult, Is.Not.Null);
            Assert.That(statusResult!.StatusCode, Is.EqualTo(500));
        }

     

        [Test]
        public async Task UpdateImage_ReturnsNoContent_WhenPropertyExists()
        {
            var propertyId = "1";
            var newImage = new PropertyImage { File = "new_url.jpg", Enabled = true };

            _repoMock.Setup(r => r.GetByIdAsync(propertyId)).ReturnsAsync(new Property { Id = propertyId });
            _repoMock.Setup(r => r.AddImageAsync(propertyId, newImage)).Returns(Task.CompletedTask);

            var result = await _controller.UpdateImage(propertyId, newImage);

            Assert.That(result, Is.TypeOf<NoContentResult>());
            _repoMock.Verify(r => r.AddImageAsync(propertyId, newImage), Times.Once);
        }

        [Test]
        public async Task UpdateImage_ReturnsNotFound_WhenPropertyDoesNotExist()
        {
            var propertyId = "99";
            var newImage = new PropertyImage { File = "new_url.jpg", Enabled = true };

            _repoMock.Setup(r => r.GetByIdAsync(propertyId)).ReturnsAsync((Property?)null);

            var result = await _controller.UpdateImage(propertyId, newImage);

            Assert.That(result, Is.TypeOf<NotFoundResult>());
            _repoMock.Verify(r => r.AddImageAsync(It.IsAny<string>(), It.IsAny<PropertyImage>()), Times.Never);
        }
    }
}
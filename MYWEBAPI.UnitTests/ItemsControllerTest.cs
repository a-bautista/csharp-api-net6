using MYWEBAPI.Controllers;
using MYWEBAPI.Entities;
using MYWEBAPI.Repositories;
using MYWEBAPI.Dtos;
using Xunit;
using Moq;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc; 
using FluentAssertions;

namespace MYWEBAPI.UnitTests {

    public class ItemsControllerTest
    {
        private readonly Mock<IItemRepository> repositoryStub = new();
        private readonly Mock<ILogger<ItemsController>> loggerStub = new();
        private readonly Random rand = new();

        [Fact]
        public async Task GetItemAsync_WithUnexistingItem_ReturnsNotFound() {
            // Arrange
            repositoryStub.Setup(repo => repo.GetItemAsync(It.IsAny<Guid>())).ReturnsAsync((Item)null);
            var controller = new ItemsController(repositoryStub.Object, loggerStub.Object);
            
            // Act
            var result = await controller.GetItemAsync(Guid.NewGuid());

            // Assert
            result.Result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task GetItemAsync_WithExistingItem_ReturnsExpectedItem() {
            // Arrange
            var expectedItem = CreateRandomItem();
            var expectedDto = new ItemDto {
                Id = expectedItem.Id,
                Name = expectedItem.Name,
                Price = expectedItem.Price,
                CreatedDate = expectedItem.CreatedDate
            };
            repositoryStub.Setup(repo => repo.GetItemAsync(It.IsAny<Guid>())).ReturnsAsync(expectedItem);
            
            var controller = new ItemsController(repositoryStub.Object, loggerStub.Object);

            // Act
            var result = await controller.GetItemAsync(Guid.NewGuid());

            // Assert
            result.Value.Should().BeEquivalentTo(
                expectedItem
                );
        }

        private Item CreateRandomItem(){
            return new(){
                Id = Guid.NewGuid(),
                Name = Guid.NewGuid().ToString(),
                Price = rand.Next(1000),
                CreatedDate = DateTimeOffset.UtcNow
            };
        }
    }
}

// public void public void UnitOfWorks_StateUnderTest_ExpectedBehavior()

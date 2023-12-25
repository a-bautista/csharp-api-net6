using MYWEBAPI.Controllers;
using MYWEBAPI.Entities;
using MYWEBAPI.Repositories;
using MYWEBAPI.Dtos;
using Xunit;
using Moq;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;
using FluentAssertions;

namespace MYWEBAPI.UnitTests
{

    public class ItemsControllerTest
    {
        private readonly Mock<IItemRepository> repositoryStub = new();
        private readonly Mock<ILogger<ItemsController>> loggerStub = new();
        private readonly Random rand = new();

        [Fact]
        public async Task GetItemAsync_WithUnexistingItem_ReturnsNotFound()
        {
            // Arrange
            repositoryStub.Setup(repo => repo.GetItemAsync(It.IsAny<Guid>())).ReturnsAsync((Item)null);
            var controller = new ItemsController(repositoryStub.Object, loggerStub.Object);

            // Act
            var result = await controller.GetItemAsync(Guid.NewGuid());

            // Assert
            result.Result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task GetItemAsync_WithExistingItem_ReturnsExpectedItem()
        {
            // Arrange
            var expectedItem = CreateRandomItem();
            var expectedDto = new ItemDto
            {
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

        // [Fact]
        // public async Task GetItemAsync_WithUnexistingItem_ReturnsAllItems() {
        //     // Arrange
        //     var expectedItems = new[] { CreateRandomItem(), CreateRandomItem(), CreateRandomItem() };

        //     repositoryStub.Setup(repo => repo.GetItemsAsync())  
        //         .ReturnsAsync(expectedItems);

        //     var controller = new ItemsController(repositoryStub.Object, loggerStub.Object);

        //     // Act
        //     var actualItems = await controller.GetItemAsync(Guid.NewGuid());

        //     // Assert
        //     actualItems.Should().BeEquivalentTo(
        //         expectedItems
        //         );
        // }

        [Fact]
        public async Task CreateItemAsync_WithItemToCreate_ReturnsCreatedItem()
        {
            // Arrange
            var itemToCreate = new CreateItemDto
            {
                Name = Guid.NewGuid().ToString(),
                Price = rand.Next(1000)
            };
            var controller = new ItemsController(repositoryStub.Object, loggerStub.Object);

            // Act
            var result = await controller.CreateItemAsync(itemToCreate);

            // Assert
            var createdItem = (result.Result as CreatedAtActionResult).Value as ItemDto;
            itemToCreate.Should().BeEquivalentTo(
                createdItem,
                options => options.ComparingByMembers<ItemDto>().ExcludingMissingMembers()
            );
            createdItem.Id.Should().NotBeEmpty();
            createdItem.CreatedDate.Should().BeCloseTo(DateTimeOffset.UtcNow, TimeSpan.FromMilliseconds(1000));
        }

        [Fact]
        public async Task UpdateItemAsync_WithExistingItem_ReturnsNoContent()
        {
            // Arrange
            var existingItem = CreateRandomItem();
            repositoryStub.Setup(repo => repo.GetItemAsync(It.IsAny<Guid>())).ReturnsAsync(existingItem);

            var itemId = existingItem.Id;
            var itemToUpdate = new UpdateItemDto()
            {
                Name = Guid.NewGuid().ToString(),
                Price = existingItem.Price + 3
            };
            var controller = new ItemsController(repositoryStub.Object, loggerStub.Object);
            // ACT
            var result = await controller.UpdateItemAsync(itemId, itemToUpdate);

            // Assert
            result.Result.Should().BeOfType<NoContentResult>();
        }

        [Fact]
        public async Task DeleteItemAsync_WithExistingItem_ReturnsNoContent()
        {
            // Arrange
            var existingItem = CreateRandomItem();
            repositoryStub.Setup(repo => repo.GetItemAsync(It.IsAny<Guid>())).ReturnsAsync(existingItem);

            var controller = new ItemsController(repositoryStub.Object, loggerStub.Object);
            // ACT
            var result = await controller.DeleteItemAsync(existingItem.Id);

            // Assert
            result.Result.Should().BeOfType<NoContentResult>();
        }


        private Item CreateRandomItem()
        {
            return new()
            {
                Id = Guid.NewGuid(),
                Name = Guid.NewGuid().ToString(),
                Price = rand.Next(1000),
                CreatedDate = DateTimeOffset.UtcNow
            };
        }
    }
}

// public void public void UnitOfWorks_StateUnderTest_ExpectedBehavior()

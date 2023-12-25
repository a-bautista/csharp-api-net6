using Microsoft.AspNetCore.Mvc;
using MYWEBAPI.Dtos;
using MYWEBAPI.Entities;
using MYWEBAPI.Repositories;

namespace MYWEBAPI.Controllers
{
    [ApiController]
    [Route("items")]
    public class ItemsController: ControllerBase
    {
        private readonly IItemRepository repository; 
        private readonly ILogger<ItemsController> logger;

        // dependency injection
        public ItemsController(IItemRepository repository, ILogger<ItemsController> logger){
            this.repository = repository; // this class doesn't know which repository is being used
            //repository = new InMemRepository(); // add an interface instead of adding this instance
            this.logger = logger;
        }

        [HttpGet]
        public async Task<IEnumerable<ItemDto>> GetItemsAsync()
        {
            // Get the item with Dto because 
            var items = (await repository.GetItemsAsync())
                        .Select(item => item.AsDto());
            logger.LogInformation($"{DateTime.UtcNow:hh:mm:ss}: Retrieved {items.Count()} items");
            return items;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ItemDto>> GetItemAsync(Guid id)
        {
            var item = await repository.GetItemAsync(id);
            if (item == null)
            {
                return NotFound();
            }
            
            return item.AsDto();
        }

        [HttpPost]
        public async Task<ActionResult<ItemDto>> CreateItemAsync(CreateItemDto itemDto){
            Item item = new(){
                Id = Guid.NewGuid(),
                Name = itemDto.Name,
                Price = itemDto.Price,
                CreatedDate = DateTimeOffset.UtcNow
            };
            await repository.CreateItemAsync(item);
            return CreatedAtAction(nameof(GetItemAsync), new {id = item.Id}, item.AsDto());
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ItemDto>> UpdateItemAsync(Guid id, UpdateItemDto itemDto)
        {
            var existingItem = await repository.GetItemAsync(id);
            if (existingItem is null) {
                return NotFound();
            }
            Item updatedItem = existingItem with 
            {
                Name = itemDto.Name,
                Price = itemDto.Price
            };
            await repository.UpdateItemAsync(updatedItem);
            return NoContent();
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult<ItemDto>> DeleteItemAsync(Guid id)
        {
            var existingItem = await repository.GetItemAsync(id);
            
            if (existingItem is null) {
                return NotFound();
            }
            await repository.DeleteItemAsync(id);
            return NoContent();
        }


    }
}
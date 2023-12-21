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

        // dependency injection
        public ItemsController(IItemRepository repository){
            this.repository = repository; // this class doesn't know which repository is being used
            //repository = new InMemRepository(); // add an interface instead of adding this instance
        }

        [HttpGet]
        public IEnumerable<ItemDto> GetItems()
        {
            // Get the item with Dto because 
            var items = repository.GetItems().Select(item => item.AsDto());
            return items;
        }

        [HttpGet("{id}")]
        public ActionResult<ItemDto> GetItem(Guid id)
        {
            var item = repository.GetItemAsync(id);
            if (item == null)
            {
                return NotFound();
            }
            
            return item.AsDto();
        }

        [HttpPost]
        public ActionResult<ItemDto> CreateItem(CreateItemDto itemDto){
            Item item = new(){
                Id = Guid.NewGuid(),
                Name = itemDto.Name,
                Price = itemDto.Price,
                CreatedDate = DateTimeOffset.UtcNow
            };
            repository.CreateItem(item);
            return CreatedAtAction(nameof(GetItem), new {id = item.Id}, item.AsDto());
        }

        [HttpPut("{id}")]
        public ActionResult<ItemDto> UpdateItem(Guid id, UpdateItemDto itemDto)
        {
            var existingItem = repository.GetItemAsync(id);
            if (existingItem is null) {
                return NotFound();
            }
            Item updatedItem = existingItem with 
            {
                Name = itemDto.Name,
                Price = itemDto.Price
            };
            repository.UpdateItem(updatedItem);
            return NoContent();
        }
        [HttpDelete("{id}")]
        public ActionResult<ItemDto> DeleteItem(Guid id)
        {
            var existingItem = repository.GetItemAsync(id);
            
            if (existingItem is null) {
                return NotFound();
            }
            repository.DeleteItem(id);
            return NoContent();
        }


    }
}
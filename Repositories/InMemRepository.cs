using MYWEBAPI.Entities;
namespace MYWEBAPI.Repositories 
{
    public class InMemRepository: IItemRepository {
        private readonly List<Item> items = new()
        {
            new Item {Id = Guid.NewGuid(), Name = "Potion", Price = 9, CreatedDate = DateTimeOffset.UtcNow},
            new Item {Id = Guid.NewGuid(), Name = "Elixir", Price = 30, CreatedDate = DateTimeOffset.UtcNow},
            new Item {Id = Guid.NewGuid(), Name = "Stamina", Price = 15, CreatedDate = DateTimeOffset.UtcNow}
        };

        public IEnumerable<Item> GetItems()
        {
            return items;
        }

        public Item GetItemAsync(Guid Id)
        {
            return items.Where(item => item.Id == Id).SingleOrDefault();
        }

        public void CreateItemAsync(Item item)
        {
            items.Add(item);
        }

        public void UpdateItemAsync(Item item)
        {
            var index = items.FindIndex(existingItem => existingItem.Id == item.Id);
            items[index] = item;
        }
        public void DeleteItemAsync(Guid id)
        {
            var index = items.FindIndex(existingItem => existingItem.Id == id);
            items.RemoveAt(index);
        }
    }
}
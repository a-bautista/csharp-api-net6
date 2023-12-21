using MYWEBAPI.Entities;
using System.Threading.Tasks;

namespace MYWEBAPI.Repositories 
{
        public interface IItemRepository {
        Task<Item> GetItemAsync(Guid id);
        Task<IEnumerable<Item>> GetItems();
        Task CreateItemAsync(Item item);
        Task UpdateItem(Item item);
        Task DeleteItem(Guid item);
    }
}
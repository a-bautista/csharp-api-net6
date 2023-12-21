using MYWEBAPI.Dtos;
using MYWEBAPI.Entities;

namespace MYWEBAPI
{
    public static class Extensions // extensions are static
    {
        public static ItemDto AsDto(this Item item)
        {
            return new ItemDto
            {
                Id = item.Id,
                Name = item.Name,
                Price = item.Price,
                CreatedDate = item.CreatedDate
            };
        }
    }
}
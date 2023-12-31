namespace MYWEBAPI.Entities 
{
    public record Item{
        public Guid Id { get; init; }

        public string Name { get; init; }

        public decimal Price { get; init; }

        public DateTimeOffset CreatedDate { get; init; }
    }

}

// Record type instead of class
/*
    Use for immutable objects
    With expression support
    Value based equality support
*/

// init properties
/*
    After the creation you cannot 
*/


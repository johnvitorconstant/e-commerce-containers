using ECC.Core.DomainObjects;

namespace ECC.Catalog.API.Models
{
    public class Product:Entity, IAggregateRoot
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public bool Active { get; set; }
        public decimal Price { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Image { get; set; }
        public int QuantityStock { get; set; }
    }
}

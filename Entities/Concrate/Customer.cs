using Core.Entities;

namespace Entities.Concrate
{
    public class Customer:IEntity
    {
        public int CustomerId { get; set; }
        public string ContactName { get; set; }
        public string CompanyName { get; set; }
        public string City { get; set; }

    }
}

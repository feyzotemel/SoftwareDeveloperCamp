using Core.Entities;

namespace Entities.Concrate
{
    ///Çıplak Class Kalmasın
    public class Category:IEntity
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
    }
}

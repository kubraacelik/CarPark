using CarPark.Entities.Concrete;

namespace CarPark.Users.Models
{
    public class cities : BaseModel
    {
        public string name { get; set; }
        public string plate { get; set; }
        public string latitude { get; set; }
        public string longitude { get; set; }
        public ICollection<County> counties { get; set; }
    }
}

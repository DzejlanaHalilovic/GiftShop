using System.ComponentModel.DataAnnotations;

namespace Micro.Sinhro.Gift.Models
{
    public class Gift
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public float Price { get; set; }
        public string ImagePath { get; set; }
    }
}

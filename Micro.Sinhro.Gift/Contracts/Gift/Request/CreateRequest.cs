namespace Micro.Sinhro.Gift.Contracts.Gift.Request
{
    public class CreateRequest
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public float Price { get; set; }
        public IFormFile Path { get; set; }
    }
}

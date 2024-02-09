namespace Micro.Sinhro.Gift.ImageUploadPhoto
{
    public class Upload
    {
        public static string SaveFile(string root, IFormFile imageFile, string path)
        {
            string imageName = new string(Path.GetFileNameWithoutExtension(imageFile.Name).Take(10).ToArray()).Replace(' ', '-');

            imageName = imageName + DateTime.Now.ToString("yymmssfff") + Path.GetExtension(imageFile.FileName);

            var imagePath = Path.Combine(root, $"wwwroot/{path}", imageName);

            using (var fileStream = new FileStream(imagePath, FileMode.Create))
            {
                imageFile.CopyTo(fileStream);
            }

            return imageName;
        }
    }
}

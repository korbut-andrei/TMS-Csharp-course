using Final_project.Entities;

namespace Final_project.Models.GETmodels
{
    public class ImageResponseModel
    {
        public bool Success { get; set; }
        public string ServerMessage { get; set; }
        public int ImageId { get; set; }
        public IFormFile? Image { get; set; }
    }
}

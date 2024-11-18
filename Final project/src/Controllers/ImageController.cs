using AndreiKorbut.CareerChoiceBackend.Models.Auth;
using AndreiKorbut.CareerChoiceBackend.Models.POST;
using AndreiKorbut.CareerChoiceBackend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace AndreiKorbut.CareerChoiceBackend.Controllers
{
    [Route("api/Images")]
    [ApiController]
    public class ImageController
    {
        private readonly ImageService _imageService;
        public ImageController(ImageService imageService)
        {
            _imageService = imageService;
        }

        //[Authorize(Roles = UserRoles.Admin)]
        [HttpPost]
        [Route("GetImage")]
        public async Task<IActionResult> GetImage([FromQuery] int imageId)
        {
            var result = await _imageService.GetImage(imageId);

            if (result.Success)
            {
                return new OkObjectResult(new
                {
                    message = result.ServerMessage,
                    imageId = result.ImageId,
                    image = result.Image
                });
            }
            else
            {
                return new ObjectResult(new { error = result.ServerMessage })
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError
                };
            }
        }
    }
}

using AutoMapper;
using Micro.Sinhro.Gift.Contracts.Gift.Request;
using Micro.Sinhro.Gift.ImageUploadPhoto;
using Micro.Sinhro.Gift.Interfaces;
using Micro.Sinhro.Gift.Persistance;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Micro.Sinhro.REST.Gift.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GiftController : ControllerBase
    {
        private readonly IGiftService giftService;
        private readonly IMapper mapper;
        private readonly IMessageBroker broker;
        private readonly Microsoft.AspNetCore.Hosting.IHostingEnvironment hostingEnvironment;

        public GiftController(IGiftService carService, IMapper mapper, IMessageBroker messageBroker, Microsoft.AspNetCore.Hosting.IHostingEnvironment _hostingEnvironment)
        {
            this.giftService = carService;
            this.mapper = mapper;
            this.broker = messageBroker;
            this.hostingEnvironment = _hostingEnvironment;

        }

        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(giftService.GetGifts());
        }

        [HttpGet("{id}")]
        public IActionResult GetGiftById(int id)
        {
            var gift = giftService.GetGift(id);
            if (gift == null)
            {
                return NotFound();
            }
            return Ok(gift);
        }


        [HttpDelete("{id}")]
        public IActionResult DeleteGift(int id)
        {
            var result = giftService.DeleteGift(id);
            if (!result.success)
            {
                return BadRequest(result);
            }
            //broker.Consume();
            return Ok(result);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateGift([FromForm] UpdateRequest request, int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var gift = giftService.GetGift(id);
            mapper.Map<UpdateRequest, Sinhro.Gift.Models.Gift>(request, gift);
            var result = giftService.UpdateGift(gift);
            if (!result.success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpPost]
        public IActionResult CreateGift([FromForm] CreateRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var gift = mapper.Map<Sinhro.Gift.Models.Gift>(request);
            gift.ImagePath = Upload.SaveFile(hostingEnvironment.ContentRootPath, request.Path, "images");

            var result = giftService.CreateGift(gift);
            if (!result.success)
            {
                return BadRequest(result);
            }
           // broker.Publish(gift);
            return Ok(result);
        }
    }
}

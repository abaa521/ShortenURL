using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShortenURLApi.Services;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel;

namespace ShortenURLApi.Controllers
{


    [Route("")]
    [ApiController]
    public class ShortUrlController : ControllerBase
    {
        private readonly ShortUrlService _shortUrlService;

        public ShortUrlController(ShortUrlService shortUrlService)
        {
            _shortUrlService = shortUrlService;
        }

        [HttpPost("create")]
        public ActionResult<string> CreateShortUrl([FromBody][DefaultValue("https://www.google.com/")] string longUrl)
        {
            if (string.IsNullOrEmpty(longUrl))
            {
                return BadRequest("Long URL is required");
            }
            var shortCode = _shortUrlService.CreateShortUrl(longUrl);
            return Ok($"Short URL: {Request.Scheme}://{Request.Host}/{shortCode}");
        }

        [HttpGet("{shortCode}")]
        public ActionResult RedirectShortUrl(string shortCode)
        {
            var longUrl = _shortUrlService.GetLongUrl(shortCode);
            if (string.IsNullOrEmpty(longUrl))
            {
                return NotFound();
            }
            return Redirect(longUrl);
        }
    }

}

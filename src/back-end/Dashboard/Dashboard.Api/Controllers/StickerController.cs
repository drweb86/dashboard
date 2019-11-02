using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Dashboard.Dtos.Stickers;
using Dashboard.Services;
using Dashboard.ViewModels.Stickers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dashboard.Api.Controllers.Auth
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class StickerController : ControllerBase
    {
        private readonly IStickerService _stickerService;
        private readonly IMapper _mapper;

        public StickerController(IMapper mapper,
            IStickerService stickerService)
        {
            _mapper = mapper;
            _stickerService = stickerService;
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] StickerAddInputModel sticker)
        {
            var stickerDto = _mapper.Map<StickerAddInputModel, StickerAddDto>(sticker);
            var ownerId = int.Parse(HttpContext.User.Claims.Single(c => c.Type == ClaimTypes.Name).Value);
            var addedStickerDto = await _stickerService.AddAsync(ownerId, stickerDto);

            return Ok(_mapper.Map<StickerDto,StickerResultModel>(addedStickerDto));
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] StickerUpdateInputModel sticker)
        {
            var stickerDto = _mapper.Map<StickerUpdateInputModel, StickerUpdateDto>(sticker);
            var ownerId = int.Parse(HttpContext.User.Claims.Single(c => c.Type == ClaimTypes.Name).Value);
            var updatedStickerDto = await _stickerService.UpdateAsync(ownerId, stickerDto);

            if (updatedStickerDto == null)
                return NotFound();

            return Ok(_mapper.Map<StickerDto, StickerResultModel>(updatedStickerDto));
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var ownerId = int.Parse(HttpContext.User.Claims.Single(c => c.Type == ClaimTypes.Name).Value);

            if (!await _stickerService.DeleteAsync(ownerId, id))
                return NotFound();

            return NoContent();
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var ownerId = int.Parse(HttpContext.User.Claims.Single(c => c.Type == ClaimTypes.Name).Value);
            var allStickers = await _stickerService.GetAllAsync(ownerId);

            return Ok(_mapper.Map<IEnumerable<StickerDto>, IEnumerable<StickerResultModel>>(allStickers));
        }
    }
}

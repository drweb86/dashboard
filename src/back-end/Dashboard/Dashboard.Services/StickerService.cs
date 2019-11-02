using AutoMapper;
using Dashboard.Dtos.Stickers;
using Dashboard.Entitites;
using Dashboard.Repositories.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dashboard.Services
{
    public interface IStickerService
    {
        Task<StickerDto> AddAsync(int ownerId, StickerAddDto stickerDto);
        Task<StickerDto> UpdateAsync(int ownerId, StickerUpdateDto stickerDto);
        Task<bool> DeleteAsync(int ownerId, int stickerId);
        Task<IEnumerable<StickerDto>> GetAllAsync(int ownerId);
    }

    public class StickerService : IStickerService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public StickerService(
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<StickerDto> AddAsync(int ownerId, StickerAddDto stickerDto)
        {
            var sticker = _mapper.Map<StickerAddDto, Sticker>(stickerDto);
            sticker.OwnerId = ownerId;
            _unitOfWork.StickerRepository.Add(sticker);
            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<Sticker, StickerDto>(sticker);
        }

        public async Task<StickerDto> UpdateAsync(int ownerId, StickerUpdateDto stickerDto)
        {
            var sticker = _mapper.Map<StickerUpdateDto, Sticker>(stickerDto);
            sticker.OwnerId = ownerId;
            if (!await _unitOfWork.StickerRepository.UpdateAsync(x => x.Id == sticker.Id && x.OwnerId == sticker.OwnerId, sticker))
                return null;

            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<Sticker, StickerDto>(sticker);
        }

        public async Task<bool> DeleteAsync(int ownerId, int stickerId)
        {
            if (!await _unitOfWork.StickerRepository.DeleteAsync(x => x.Id == stickerId && x.OwnerId == ownerId))
                return false;

            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<StickerDto>> GetAllAsync(int ownerId)
        {
            var stickers = await _unitOfWork.StickerRepository.GetAllByUserIdAsync(ownerId);
            return _mapper.Map<IEnumerable<Sticker>, IEnumerable<StickerDto>>(stickers);
        }
    }
}

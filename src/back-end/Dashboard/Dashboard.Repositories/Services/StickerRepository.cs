using Dashboard.Entitites;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dashboard.Repositories.Services
{
    public interface IStickerRepository: IBaseRepository<Sticker>
    {
        Task<IEnumerable<Sticker>> GetAllByUserIdAsync(int ownerId);
    }

    public class StickerRepository : BaseRepository<Sticker>, IStickerRepository
    {
        public StickerRepository(DashboardDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<IEnumerable<Sticker>> GetAllByUserIdAsync(int ownerId)
        {
            return await Db.Stickers
                .AsNoTracking()
                .Where(z => z.OwnerId == ownerId)
                .ToListAsync();
        }
    }
}

using Microsoft.EntityFrameworkCore;
using WorkNest.Config;
using WorkNest.Models.Board;

namespace WorkNest.Repositories
{
    public class BoardRepository : Repository<Board>, IBoardRepository
    {
        private readonly ApplicationDbContext _db;

        public BoardRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        
        public async Task<Board> GetBoardWithDetailsAsync(int boardId)
        {
            return await _db.Boards
                .Include(b => b.Columns.OrderBy(c => c.Order))
                    .ThenInclude(c => c.Tasks) 
                        .ThenInclude(t => t.AssignedUser) 
                .FirstOrDefaultAsync(b => b.Id == boardId);
        }
    }
}

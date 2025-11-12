using WorkNest.Models.Board;

namespace WorkNest.Repositories
{
    public interface IBoardRepository : IRepository<Board>
    {
        Task<Board> GetBoardWithDetailsAsync(int boardId);
    }
}

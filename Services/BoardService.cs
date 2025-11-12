using WorkNest.Enums;
using WorkNest.Models.Board;
using WorkNest.Models.Column;
using WorkNest.Models.Dto;
using WorkNest.Repositories;
using WorkNest.Utils;
using AutoMapper;
using System.Net;
using WorkNest.Models.Board;
using WorkNest.Models.Column;
using WorkNest.Models.Dto;
using WorkNest.Repositories;

namespace WorkNest.Services
{
    public class BoardService
    {
        private readonly IBoardRepository _boardRepo;
        private readonly IMapper _mapper;

        public BoardService(IBoardRepository boardRepo, IMapper mapper)
        {
            _boardRepo = boardRepo;
            _mapper = mapper;
        }

        public async Task<List<BoardDTO>> GetAllBoards()
        {
            var boards = await _boardRepo.GetAllAsync();
            return _mapper.Map<List<BoardDTO>>(boards);
        }

        public async Task<BoardDetailsDTO> GetBoardDetails(int boardId)
        {
            var board = await _boardRepo.GetBoardWithDetailsAsync(boardId);
            if (board == null)
            {
                throw new HttpResponseError(HttpStatusCode.NotFound, "Board not found");
            }
            return _mapper.Map<BoardDetailsDTO>(board);
        }

        public async Task<BoardDetailsDTO> CreateBoard(CreateBoardDTO dto)
        {
            var board = _mapper.Map<Board>(dto);

            board.Columns = new List<Column>
            {
                new Column { Name = "Backlog", Order = 1 },
                new Column { Name = "To Do", Order = 2 },
                new Column { Name = "In Progress", Order = 3 },
                new Column { Name = "Done", Order = 4 }
            };

            await _boardRepo.CreateOneAsync(board);

            return _mapper.Map<BoardDetailsDTO>(board);
        }

        public async Task DeleteBoard(int boardId)
        {
            var board = await _boardRepo.GetOneAsync(b => b.Id == boardId);
            if (board == null)
            {
                throw new HttpResponseError(HttpStatusCode.NotFound, "Board not found");
            }
            await _boardRepo.DeleteOneAsync(board);
        }
    }
}

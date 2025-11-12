using WorkNest.Enums;
using WorkNest.Models.Dto;
using WorkNest.Services;
using WorkNest.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Net;
using WorkNest.Models.Dto;
using WorkNest.Services;

namespace WorkNest.Controllers
{
    [Route("api/boards")]
    [ApiController]
    [Authorize]
    public class BoardController : ControllerBase
    {
        private readonly BoardService _boardService;

        public BoardController(BoardService boardService)
        {
            _boardService = boardService;
        }

        [HttpGet]
        public async Task<ActionResult<List<BoardDTO>>> GetBoards()
        {
            var boards = await _boardService.GetAllBoards();
            return Ok(boards);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BoardDetailsDTO>> GetBoardDetails(int id)
        {
            try
            {
                var board = await _boardService.GetBoardDetails(id);
                return Ok(board);
            }
            catch (HttpResponseError ex)
            {
                return StatusCode((int)ex.StatusCode, new HttpMessage(ex.Message));
            }
        }

        [HttpPost]
        [Authorize(Roles = $"{ROLE.MOD}, {ROLE.ADMIN}")] 
        public async Task<ActionResult<BoardDetailsDTO>> CreateBoard([FromBody] CreateBoardDTO dto)
        {
            var board = await _boardService.CreateBoard(dto);
            return CreatedAtAction(nameof(GetBoardDetails), new { id = board.Id }, board);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = $"{ROLE.MOD}, {ROLE.ADMIN}")] 
        public async Task<ActionResult> DeleteBoard(int id)
        {
            try
            {
                await _boardService.DeleteBoard(id);
                return NoContent();
            }
            catch (HttpResponseError ex)
            {
                return StatusCode((int)ex.StatusCode, new HttpMessage(ex.Message));
            }
        }
    }
}

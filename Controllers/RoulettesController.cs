using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using RouletteApi.Helpers;
using RouletteApi.Models;
using RouletteApi.Services;
using RouletteApi.Exceptions;
using System.Threading.Tasks;

namespace RouletteApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RoulettesController : ControllerBase
    {
        private readonly RouletteService _rouletteService;

        public RoulettesController(RouletteService rouletteService)
        {
            _rouletteService = rouletteService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateRoulette()
        {
            try
            {
                CustomResponse<string> response = await _rouletteService.CreateRoulette();
                return Ok(response);
            }
            catch (MessageException exception)
            {
                return Problem(
                detail: exception.Message,
                title: exception.StatusName);
            }
        }

        [HttpPut("open")]
        public async Task<IActionResult> OpenRoulette([FromBody] dynamic body)
        {
            try
            {
                CustomResponse<string> response = await _rouletteService.UpdateStatus(body.rouletteId.Value, true);
                _rouletteService.CleanBets(body.rouletteId.Value);
                return Ok(response);
            }
            catch (MessageException exception)
            {
                return Problem(
                detail: exception.Message,
                title: exception.StatusName);
            }
        }

        [HttpPost("placeBet")]
        public async Task<IActionResult> PlaceBet([FromBody] dynamic body, [FromHeader(Name = "userId")] string userId)
        {
            try
            {
                CustomResponse<Bet> response = await _rouletteService.PlaceBet(body.rouletteId.Value, body.betPlace.Value, body.amount.Value, userId);
                return Ok(response);
            }
            catch (MessageException exception)
            {
                return Problem(
                detail: exception.Message,
                title: exception.StatusName);
            }
        }

        [HttpPut("close")]
        public async Task<IActionResult> CloseRoulette([FromBody] dynamic body)
        {
            try
            {
                CustomResponse<List<Bet>> response = await _rouletteService.CloseBets(body.rouletteId.Value);
                return Ok(response);
            }
            catch (MessageException exception)
            {
                return Problem(
                detail: exception.Message,
                title: exception.StatusName);
            }
        }

        [HttpGet]
        public ActionResult<List<Roulette>> GetRoulettes() =>
             _rouletteService.GetRoulettes();
    }
}

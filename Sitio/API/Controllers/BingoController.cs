using Microsoft.AspNetCore.Mvc;
using Modelos.Models;
using Newtonsoft.Json;
using Servicios.Intefaces;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BingoController : ControllerBase
    {
        private readonly IBingoService _bingoService;

        public BingoController(IBingoService bingoService)
        {
            _bingoService = bingoService;
        }

        [HttpGet(Name = "GetCartones")]
        public IActionResult GetCartones()
        {
            try
            {
                var cartones = _bingoService.GenerarCartones(4);

                return Ok(JsonConvert.SerializeObject(cartones));
            }
            catch (Exception ex)
            { 
                return BadRequest(ex.Message);
            }
        }

        [HttpPost(Name = "PostResult")]
        public IActionResult PostResult(Datos dto)
        {
            try
            {
                _bingoService.GuardarResultado(dto);
                return Ok();
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}
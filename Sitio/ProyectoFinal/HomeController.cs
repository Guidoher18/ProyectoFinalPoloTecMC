using Microsoft.AspNetCore.Mvc;
using Modelos.Models;
using Newtonsoft.Json;
using RestSharp;

namespace ProyectoFinal
{
    public class HomeController : Controller
    {
        private readonly IConfiguration _configuration;

        public HomeController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IActionResult Index() => View("Bingo");

        public IActionResult ObtenerCartones()
        {
            try
            {
                var client = new RestClient($"{_configuration.GetValue<string>("API_URL")}/Bingo");
                var request = new RestRequest();
                var response = client.ExecuteGet(request);

                if (!response.IsSuccessStatusCode)
                    return BadRequest();

                string cartones = "";

                if (response != null && response.Content != null)
                    cartones = JsonConvert.DeserializeObject<string>(response.Content);    

                return Ok(cartones);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        public IActionResult GuardarDatos(Datos dto)
        {
            try
            {
                var client = new RestClient($"{_configuration.GetValue<string>("API_URL")}/Bingo");
                var request = new RestRequest();
                request.AddBody(dto);
                var response = client.ExecutePost(request);

                if (!response.IsSuccessStatusCode)
                    return BadRequest();

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}

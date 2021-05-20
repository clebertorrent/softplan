using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.VisualBasic;
using Newtonsoft.Json;

namespace WebCustomAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CalculaJurosController : ControllerBase
    {
        private readonly ILogger<CalculaJurosController> _logger;
        static HttpClient client = new HttpClient();

        public CalculaJurosController(ILogger<CalculaJurosController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> CalculaJuros(double valorinicial, int meses)
        {
            client.BaseAddress = new Uri("https://localhost:44348/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            HttpResponseMessage response = await client.GetAsync("/taxajuros");

            var ValorFinal = 0.0;

            if (response.IsSuccessStatusCode)
            {
                var vJson = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                var vTaxa = JsonConvert.DeserializeObject<double>(vJson);

                var taxadeJuros = vTaxa * 100;
                var tempo = meses;
                ValorFinal = valorinicial * Math.Pow((double)1 + (double)(taxadeJuros / 100), (double)tempo);

            }

            return Ok(ValorFinal.ToString("N2"));
        }
    }
}

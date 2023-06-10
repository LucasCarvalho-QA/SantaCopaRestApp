using Microsoft.AspNetCore.Mvc;
using SantaCopaRestApp.Model;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SantaCopaRestApp.Controllers
{
    public class Mata
    {
        public int Rodada { get; set; }

        public string JogadorCasa { get; set; }
        public string JogadorVisitante { get; set; }
    }

    [Route("api/[controller]")]
    [ApiController]
    public class TorneioController : ControllerBase
    {
        Torneio torneio = new();
        Gerenciador gerenciador = new Gerenciador();

        

        // GET: api/<TorneioController>
        // [HttpGet(Name = "GetWeatherForecast")]
        //[HttpGet]
        //public List<Torneio> Get()
        //{                      
        //    return torneio.SelecionarTorneios();
        //}

        [HttpGet]
        public void Get()
        {
            Torneio torneio = new Torneio();
            torneio.IniciarTorneio();
        }

        // GET api/<TorneioController>/5
        [HttpGet("{id}")]
        public Torneio Get(string id)
        {
            return torneio.SelecionarTorneios(id).First();
        }

        // POST api/<TorneioController>/5
        [HttpPost("mata")]
        public void Torneio([FromBody] Mata mata)
        {  
            gerenciador.SortearPartidas_Mata($"Rodada {mata.Rodada}", mata.JogadorCasa, mata.JogadorVisitante);
        }

        // POST api/<TorneioController>
        [HttpPost]
        public Torneio Post([FromBody] Torneio value)
        {
            torneio.CriarTorneio(value);
            return torneio.SelecionarTorneios().Last();
        }

        // PUT api/<TorneioController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<TorneioController>/5
        [HttpDelete("{id}")]
        public void Delete(string id)
        {
            torneio.ExcluirTorneio(id);
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using SantaCopaRestApp.Model;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SantaCopaRestApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PartidaController : ControllerBase
    {
        Partida partida = new Partida();
        Classificacao classificacao = new Classificacao();
        // GET: api/<PartidaController>
        [HttpGet]
        public List<Partida> Get()
        {
            return partida.SelecionarPartidas();
        }

        // GET api/<PartidaController>/5
        [HttpGet("{id}")]
        public List<Partida> Get(string id)
        {         
            return partida.SelecionarPartidas(id);
        }

        // POST api/<PartidaController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<PartidaController>/5
        [HttpPut("alterar")]
        public List<Partida> Put([FromBody] Partida partida)
        {            
            partida.AlterarPartida(partida);
            //classificacao.AtualizarClassificacao(partida);
            return partida.SelecionarPartidas(null, partida.PartidaID.ToString());
        }

        // DELETE api/<PartidaController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}

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
        public List<Partida> Put([FromBody] Partida partidaJogada)
        {
            partidaJogada.PartidaRealizada = "Sim";

            
            partida.AlterarPartida(partidaJogada);
            //SELECT * FROM Partida WHERE PartidaRealizada = 'Sim' AND PartidaID = 1

            var partidaEditada = partida.SelecionarPartidas(null, partidaJogada.PartidaID.ToString());
            partidaEditada[0].JogadorCasaGols = partidaJogada.JogadorCasaGols;
            partidaEditada[0].JogadorVisitanteGols = partidaJogada.JogadorVisitanteGols;
            partidaEditada[0].PartidaRealizada = "Sim";
            classificacao.AtualizarClassificacao(partidaEditada[0]);

            return partidaEditada;
        }

        // DELETE api/<PartidaController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}

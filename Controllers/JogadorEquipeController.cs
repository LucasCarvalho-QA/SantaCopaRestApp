using Microsoft.AspNetCore.Mvc;
using SantaCopaRestApp.Model;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SantaCopaRestApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JogadorEquipeController : ControllerBase
    {
        JogadorEquipe jogadorEquipe = new JogadorEquipe();
        Gerenciador gerenciador = new Gerenciador();
        // GET: api/<JogadorEquipeController>
        [HttpGet]
        public List<Jogador> Get()
        {
            gerenciador.GetJogador_PorId();
            gerenciador.GetEstrelas();
            //gerenciador.SortearJogadorEquipe();
            return null;

        }

        // GET api/<JogadorEquipeController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<JogadorEquipeController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<JogadorEquipeController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<JogadorEquipeController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}

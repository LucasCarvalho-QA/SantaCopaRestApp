using SantaCopaRestApp.Model;
using System;

namespace SantaCopaRestApp
{
    public class Gerenciador
    {
        Classificacao classificacao = new Classificacao();
        Equipe equipe = new Equipe();
        Jogador jogador = new Jogador();
        JogadorEquipe jogadorEquipe = new JogadorEquipe();
        Partida partida = new Partida();
        Torneio torneio = new Torneio();

        internal static JogadorEquipe jogadorEquipeSorteados = null;
        internal static int qtdJogosPorRodadaNaSala;

        public string SelectRandomString(List<string> strings)
        {
            Random random = new();
            int index = random.Next(strings.Count);
            return strings[index];
        }

        public Equipe SelecionarEquipeAleatoria(List<Equipe> equipes)
        {
            Random random = new();
            return equipes[random.Next(equipes.Count)];
        }

        public List<string> GetRodadas()
        {
            return new() { "Rodada 1", "Rodada 2", "Rodada 3" };
        }

        public List<string> GetLocais()
        {
            return new() { "Sala", "Sala", "Sala", "Escritório", "Escritório", "Escritório"};
        }

        public List<string> GetEstrelas()
        {
            return new() { "5", "4.5", "4.5", "4", "4", "4" };
        }

        public List<Jogador> GetJogador_PorId(string? id = null)
        {
            return jogador.SelecionarJogador_PorId(id);
        }

        public List<Jogador> GetJogador_PorNome(string? nome = null)
        {
            return jogador.SelecionarJogador_PorNome(nome);
        }

        public List<Equipe> GetEquipes(string? equipeID = null, string? estrelas = null)
        {
            return equipe.SelecionarEquipes(equipeID, estrelas);
        }

        public Equipe GetEquipeAleatoria(string? estrelas)
        {
            Random random = new Random();
            var equipes = equipe.SelecionarEquipes(null, estrelas);
            return equipes[random.Next(equipes.Count)];
        }

        public string SortearLocal()
        {
            return SelectRandomString(GetLocais());
        }

        public string SortearEstrelas()
        {
            return SelectRandomString(GetEstrelas());
        }

        public string SortearEquipe()
        {
            return SelectRandomString(GetEstrelas());
        }

        public void SortearPartidas(string rodada)
        {
            int i = 0;            
            List<Jogador> jogadores = GetJogador_PorId();
            var locais = GetLocais();

            while (i < (GetJogador_PorId().Count / 2))
            {
                Partida partida = new Partida();
                string nivelEstrelas = SortearEstrelas();
                partida.Rodada = rodada;
                partida.Local = SelectRandomString(locais);
                partida.NivelEstrelas = nivelEstrelas;

                var jogadorCasa = SortearJogadorEquipe(nivelEstrelas, jogadores, rodada);                    
                partida.JogadorCasa = GetJogador_PorId(jogadorCasa.JogadorID.ToString())[0].Nome;
                partida.EquipeCasa = GetEquipes(jogadorCasa.EquipeID.ToString())[0].Nome;

                var jogadorVisitante = SortearJogadorEquipe(nivelEstrelas, jogadores, rodada);                
                    
                while(partida.EquipeVisitante == partida.EquipeCasa)
                    jogadorVisitante.EquipeID = GetEquipeAleatoria(nivelEstrelas).EquipeID;

                partida.JogadorVisitante = GetJogador_PorId(jogadorVisitante.JogadorID.ToString())[0].Nome;
                partida.EquipeVisitante = GetEquipes(jogadorVisitante.EquipeID.ToString())[0].Nome;

                partida.CriarPartida(partida);
                locais.Remove(partida.Local);
                i++;
            }            
        }

        public void SortearPartidas_Mata(string rodada, string jogadorCasa_Nome, string jogadorVisitante_Nome)
        {
            int i = 0;
            List<Jogador> jogadores = GetJogador_PorId();
            var locais = GetLocais();

            
            Partida partida = new Partida();
            string nivelEstrelas = SortearEstrelas();
            partida.Rodada = rodada;
            partida.Local = SelectRandomString(locais);
            partida.NivelEstrelas = nivelEstrelas;                
                
            JogadorEquipe jogadorCasa = new JogadorEquipe
            {
                Rodada = rodada,
                EquipeID = GetEquipeAleatoria(nivelEstrelas).EquipeID,
                JogadorID = GetJogador_PorNome(jogadorCasa_Nome)[0].JogadorID
            };
                
            partida.JogadorCasa = GetJogador_PorId(jogadorCasa.JogadorID.ToString())[0].Nome;
            partida.EquipeCasa = GetEquipes(jogadorCasa.EquipeID.ToString())[0].Nome;

            JogadorEquipe jogadorVisitante = new JogadorEquipe
            {
                Rodada = rodada,
                EquipeID = GetEquipeAleatoria(nivelEstrelas).EquipeID,
                JogadorID = GetJogador_PorNome(jogadorVisitante_Nome)[0].JogadorID
            };

            //Avaliar comportamento
            while (partida.EquipeVisitante == partida.EquipeCasa)
                jogadorVisitante.EquipeID = GetEquipeAleatoria(nivelEstrelas).EquipeID;

            partida.JogadorVisitante = GetJogador_PorId(jogadorVisitante.JogadorID.ToString())[0].Nome;
            partida.EquipeVisitante = GetEquipes(jogadorVisitante.EquipeID.ToString())[0].Nome;

            partida.CriarPartida(partida);
            locais.Remove(partida.Local);
            i++;
            
        }

        


        public JogadorEquipe SortearJogadorEquipe(string nivelEstrelas, List<Jogador> jogadores, string? rodada)
        {
            Random random = new();
            JogadorEquipe jogadorEquipe = new JogadorEquipe();

            Equipe equipe = GetEquipeAleatoria(nivelEstrelas);

            if (jogadorEquipe.VerificarTimesCriadosNaRodada(rodada, equipe.EquipeID.ToString()).Count > 1)
                SortearJogadorEquipe(nivelEstrelas, jogadores, rodada);

            Jogador jogador = jogadores[random.Next(jogadores.Count)];
            

            JogadorEquipe jogadorEquipeLocal = new()
            {
                Rodada = rodada,
                EquipeID = equipe.EquipeID,
                JogadorID = jogador.JogadorID
            };

            if (jogadorEquipe.SelecionarJogadorEquipe(jogador.JogadorID.ToString(), equipe.EquipeID.ToString()).Count > 0)
                SortearJogadorEquipe(nivelEstrelas, jogadores, rodada);
            
            jogadorEquipe.CriarJogadorEquipe(jogadorEquipeLocal);
            jogadorEquipeLocal.JogadorEquipeID = jogadorEquipe.RetornarUltimoItemCriado().JogadorEquipeID;

            jogadores.Remove(jogador);

            return jogadorEquipeLocal;
        }    
        
        

    }
}

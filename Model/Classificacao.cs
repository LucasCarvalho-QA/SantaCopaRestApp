using System.Data.SqlClient;

namespace SantaCopaRestApp.Model
{
    public class Classificacao
    {
        public int Pontos { get; set; }
        public int Jogos { get; set; }
        public int Vitorias { get; set; }
        public int Empates { get; set; }
        public int GolsPro { get; set; }
        public int GolsContra { get; set; }
        public int Saldo { get; set; }        
        public string JogadorNome { get; set; }
        public int TorneioID { get; set; }
        public int JogadorID { get; set; }
        private string connectionString = @"Server=LEKTOP\SQLEXPRESS; Database=SantaCopa;Integrated Security=true";

        public void AtualizarClassificacao(Partida partida)
        {
            

            if (partida.JogadorCasaGols > partida.JogadorVisitanteGols)
            {
                AplicarResultadosVencedor(partida, partida.JogadorCasa);
                AplicarResultadosPerdedor(partida, partida.JogadorVisitante);
            }                    

            if (partida.JogadorCasaGols < partida.JogadorVisitanteGols)
            {
                AplicarResultadosVencedor(partida, partida.JogadorVisitante);
                AplicarResultadosPerdedor(partida, partida.JogadorCasa);
            }                    

            if (partida.JogadorCasaGols == partida.JogadorVisitanteGols)
                AplicarResultadosEmpate(partida);  
            
        }

        public void AplicarResultadosVencedor(Partida partida, string jogador)
        {
            Classificacao classificacao = new Classificacao();
            var classificado = classificacao.ConsultarClassificacao(jogador);
            //classificacao.GolsPro =+ parti 

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = $@"UPDATE Classificacao
                                 SET Pontos = Pontos + 3,
                                     Vitorias = Vitorias + 1,
                                     GolsPro = GolsPro + @GolsPro,
                                     GolsContra = GolsContra + @GolsContra,
                                     Saldo = Saldo + @Saldo,
                                    Jogos = Jogos + 1
                                 WHERE TorneioID = @TorneioID AND JogadorNome = '{jogador}'";

                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@GolsPro", partida.JogadorCasaGols);
                command.Parameters.AddWithValue("@GolsContra", partida.JogadorVisitanteGols);
                command.Parameters.AddWithValue("@Saldo", partida.JogadorCasaGols - partida.JogadorVisitanteGols);
                command.Parameters.AddWithValue("@TorneioID", 1);

                command.ExecuteNonQuery();
            }
        }

        public void AplicarResultadosPerdedor(Partida partida, string jogador)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = $@"UPDATE Classificacao
                                 SET Derrotas = Derrotas + 1,
                                     GolsPro = GolsPro + @GolsContra,
                                     GolsContra = GolsContra + @GolsPro,
                                     Saldo = Saldo - @Saldo,
                                    Jogos = Jogos + 1
                                 WHERE TorneioID = @TorneioID AND JogadorNome = '{jogador}'";

                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@GolsPro", partida.JogadorCasaGols);
                command.Parameters.AddWithValue("@GolsContra", partida.JogadorVisitanteGols);
                command.Parameters.AddWithValue("@Saldo", partida.JogadorCasaGols - partida.JogadorVisitanteGols);
                command.Parameters.AddWithValue("@TorneioID", 1);

                command.ExecuteNonQuery();
            }
        }

        public void AplicarResultadosEmpate(Partida partida)
        {
            //SELECT* FROM Classificacao WHERE JogadorNome = ''
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = $@"UPDATE Classificacao
                                 SET Empates = Empates + 1,
                                     GolsPro = GolsPro + @GolsPro,
                                     GolsContra = GolsContra + @GolsContra,
                                     Saldo = Saldo + @Saldo,
                                    Pontos = Pontos + 1,
                                    Jogos = Jogos + 1
                                 WHERE TorneioID = @TorneioID AND JogadorNome IN ('{partida.JogadorCasa}', '{partida.JogadorVisitante}')";

                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@GolsPro", partida.JogadorCasaGols);
                command.Parameters.AddWithValue("@GolsContra", partida.JogadorVisitanteGols);
                command.Parameters.AddWithValue("@Saldo", partida.JogadorCasaGols - partida.JogadorVisitanteGols);
                command.Parameters.AddWithValue("@TorneioID", 1);

                command.ExecuteNonQuery();
            }
        }

        public void PopularClassificacao()
        {
            Jogador jogadores = new Jogador();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                foreach (Jogador jogador in jogadores.SelecionarJogador_PorNome())
                {
                    // Verificar se o jogador já possui registro na tabela de classificação
                    string queryVerificar = @"SELECT COUNT(*) FROM Classificacao WHERE JogadorNome = @JogadorNome AND TorneioID = @TorneioID";
                    SqlCommand commandVerificar = new SqlCommand(queryVerificar, connection);
                    commandVerificar.Parameters.AddWithValue("@JogadorNome", jogador.Nome);
                    commandVerificar.Parameters.AddWithValue("@TorneioID", 1);
                    int count = (int)commandVerificar.ExecuteScalar();

                    if (count == 0)
                    {
                        // Inserir novo registro na tabela de classificação para o jogador
                        string queryInserir = @"INSERT INTO Classificacao (Pontos, Jogos,Vitorias, Empates, Derrotas, GolsPro, GolsContra, Saldo, JogadorNome, TorneioID, JogadorID)
                                            VALUES (@Pontos, @Jogos, @Vitorias, @Empates, @Derrotas, @GolsPro, @GolsContra, @Saldo, @JogadorNome, @TorneioID, @JogadorID)";
                        SqlCommand commandInserir = new SqlCommand(queryInserir, connection);
                        commandInserir.Parameters.AddWithValue("@Pontos", 0);
                        commandInserir.Parameters.AddWithValue("@Jogos", 0);
                        commandInserir.Parameters.AddWithValue("@Vitorias", 0);
                        commandInserir.Parameters.AddWithValue("@Empates", 0);
                        commandInserir.Parameters.AddWithValue("@Derrotas", 0);
                        commandInserir.Parameters.AddWithValue("@GolsPro", 0);
                        commandInserir.Parameters.AddWithValue("@GolsContra", 0);
                        commandInserir.Parameters.AddWithValue("@Saldo", 0);
                        commandInserir.Parameters.AddWithValue("@JogadorNome", jogador.Nome);
                        commandInserir.Parameters.AddWithValue("@TorneioID", 1);
                        commandInserir.Parameters.AddWithValue("@JogadorID", jogador.JogadorID);

                        commandInserir.ExecuteNonQuery();
                    }
                }
            }
        }

        public List<Classificacao> ConsultarClassificacao(string jogadorID)
        {
            List<Classificacao> classificacaoList = new List<Classificacao>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = @"SELECT Pontos, Vitorias, Empates, GolsPro, GolsContra, Saldo, JogadorNome, TorneioID, JogadorID
                             FROM Classificacao
                             WHERE TorneioID = 1 AND JogadorID = @JogadorID
                             ORDER BY Pontos DESC";

                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@JogadorID", JogadorID);

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Classificacao classificacao = new Classificacao();

                    classificacao.Pontos = reader.GetInt32(0);
                    classificacao.Jogos = reader.GetInt32(1);
                    classificacao.Vitorias = reader.GetInt32(2);
                    classificacao.Empates = reader.GetInt32(3);
                    classificacao.GolsPro = reader.GetInt32(4);
                    classificacao.GolsContra = reader.GetInt32(5);
                    classificacao.Saldo = reader.GetInt32(6);
                    classificacao.JogadorNome = reader.GetString(7);
                    classificacao.TorneioID = reader.GetInt32(8);
                    classificacao.JogadorID = reader.GetInt32(9);

                    classificacaoList.Add(classificacao);
                }

                reader.Close();
            }

            return classificacaoList;
        }

    }
}

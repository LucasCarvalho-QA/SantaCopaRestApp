using System.Data.SqlClient;

namespace SantaCopaRestApp.Model
{
    public class Classificacao
    {        
        public int Pontos { get; set; }
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
            string jogadorVencedor = null;
            string jogadorPerdedor = null;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // Verificar o vencedor da partida
                string vencedor = "";
                if (partida.JogadorCasaGols > partida.JogadorVisitanteGols)
                {
                    vencedor = partida.JogadorCasa;
                    jogadorVencedor = partida.JogadorCasa;
                    jogadorPerdedor = partida.JogadorVisitante;

                }                    
                else if (partida.JogadorCasaGols < partida.JogadorVisitanteGols)
                {
                    vencedor = partida.JogadorVisitante;
                    jogadorVencedor = partida.JogadorVisitante;
                    jogadorPerdedor = partida.JogadorCasa;
                }
                else
                {
                    vencedor = "empate";
                }
                    

                // Atualizar tabela de classificação para o vencedor
                if (!string.IsNullOrEmpty(vencedor))
                {
                    string query = $@"UPDATE Classificacao
                                 SET Pontos = Pontos + 3,
                                     Vitorias = Vitorias + 1,
                                     GolsPro = GolsPro + @GolsPro,
                                     GolsContra = GolsContra + @GolsContra,
                                     Saldo = Saldo + @Saldo
                                 WHERE TorneioID = @TorneioID AND JogadorNome = '{jogadorVencedor}'";

                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@GolsPro", partida.JogadorCasaGols);
                    command.Parameters.AddWithValue("@GolsContra", partida.JogadorVisitanteGols);
                    command.Parameters.AddWithValue("@Saldo", partida.JogadorCasaGols - partida.JogadorVisitanteGols);
                    command.Parameters.AddWithValue("@TorneioID", 1);
                    

                    command.ExecuteNonQuery();
                }

                // Atualizar tabela de classificação para o perdedor
                if (string.IsNullOrEmpty(vencedor))
                {
                    string query = $@"UPDATE Classificacao
                                 SET GolsPro = GolsPro + @GolsPro,
                                     GolsContra = GolsContra + @GolsContra,
                                     Saldo = Saldo + @Saldo
                                 WHERE TorneioID = @TorneioID AND JogadorNome = '{jogadorVencedor}'";

                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@GolsPro", partida.JogadorCasaGols);
                    command.Parameters.AddWithValue("@GolsContra", partida.JogadorVisitanteGols);
                    command.Parameters.AddWithValue("@Saldo", partida.JogadorCasaGols - partida.JogadorVisitanteGols);
                    command.Parameters.AddWithValue("@TorneioID", 1);                    

                    command.ExecuteNonQuery();
                }

                if (vencedor == "empate")
                {
                    string query = $@"UPDATE Classificacao
                                 SET Empates = Empates + 1,
                                     GolsPro = GolsPro + @GolsPro,
                                     GolsContra = GolsContra + @GolsContra,
                                     Saldo = Saldo + @Saldo
                                 WHERE TorneioID = @TorneioID AND JogadorNome IN ('{partida.JogadorCasa}','{partida.JogadorVisitante}')";

                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@GolsPro", partida.JogadorCasaGols);
                    command.Parameters.AddWithValue("@GolsContra", partida.JogadorVisitanteGols);
                    command.Parameters.AddWithValue("@Saldo", partida.JogadorCasaGols - partida.JogadorVisitanteGols);
                    command.Parameters.AddWithValue("@TorneioID", 1);

                    command.ExecuteNonQuery();
                }
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
                        string queryInserir = @"INSERT INTO Classificacao (Pontos, Vitorias, Empates, GolsPro, GolsContra, Saldo, JogadorNome, TorneioID, JogadorID)
                                            VALUES (@Pontos, @Vitorias, @Empates, @GolsPro, @GolsContra, @Saldo, @JogadorNome, @TorneioID, @JogadorID)";
                        SqlCommand commandInserir = new SqlCommand(queryInserir, connection);
                        commandInserir.Parameters.AddWithValue("@Pontos", 0);
                        commandInserir.Parameters.AddWithValue("@Vitorias", 0);
                        commandInserir.Parameters.AddWithValue("@Empates", 0);
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
    }
}

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
        public int TorneioID { get; set; }
        public int JogadorID { get; set; }
        private string connectionString = @"Server=LEKTOP\SQLEXPRESS; Database=SantaCopa;Integrated Security=true";

        public void AtualizarClassificacao(Partida partida)
        {

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // Verificar o vencedor da partida
                string vencedor = "";
                if (partida.JogadorCasaGols > partida.JogadorVisitanteGols)
                    vencedor = partida.EquipeCasa;
                else if (partida.JogadorCasaGols < partida.JogadorVisitanteGols)
                    vencedor = partida.EquipeVisitante;

                // Atualizar tabela de classificação para o vencedor
                if (!string.IsNullOrEmpty(vencedor))
                {
                    string query = @"UPDATE Classificacao
                                 SET Pontos = Pontos + 3,
                                     Vitorias = Vitorias + 1,
                                     GolsPro = GolsPro + @GolsPro,
                                     GolsContra = GolsContra + @GolsContra,
                                     Saldo = Saldo + @Saldo
                                 WHERE TorneioID = @TorneioID AND JogadorID = @JogadorID";

                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@GolsPro", partida.JogadorCasaGols);
                    command.Parameters.AddWithValue("@GolsContra", partida.JogadorVisitanteGols);
                    command.Parameters.AddWithValue("@Saldo", partida.JogadorCasaGols - partida.JogadorVisitanteGols);
                    command.Parameters.AddWithValue("@TorneioID", partida.TorneioID);
                    command.Parameters.AddWithValue("@JogadorID", partida.JogadorCasa);

                    command.ExecuteNonQuery();
                }

                // Atualizar tabela de classificação para o perdedor ou empate
                if (!string.IsNullOrEmpty(vencedor))
                {
                    string query = @"UPDATE Classificacao
                                 SET Empates = Empates + 1,
                                     GolsPro = GolsPro + @GolsPro,
                                     GolsContra = GolsContra + @GolsContra,
                                     Saldo = Saldo + @Saldo
                                 WHERE TorneioID = @TorneioID AND JogadorID = @JogadorID";

                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@GolsPro", partida.JogadorCasaGols);
                    command.Parameters.AddWithValue("@GolsContra", partida.JogadorVisitanteGols);
                    command.Parameters.AddWithValue("@Saldo", partida.JogadorCasaGols - partida.JogadorVisitanteGols);
                    command.Parameters.AddWithValue("@TorneioID", partida.TorneioID);
                    command.Parameters.AddWithValue("@JogadorID", partida.JogadorVisitante);

                    command.ExecuteNonQuery();
                }
            }
        }

    }
}

using System.Data.SqlClient;

namespace SantaCopaRestApp.Model
{
    public class Jogador
    {
        public int JogadorID { get; set; }
        public string Nome { get; set; }

        private string connectionString = @"Server=LEKTOP\SQLEXPRESS; Database=SantaCopa;Integrated Security=true";

        public List<Jogador> SelecionarJogador_PorId(string? id = null)
        {
            List<Jogador> jogadores = new();
            string query = "SELECT * FROM Jogador";

            if (id != null)
                query += $" WHERE JogadorID = {id}";

            using (SqlConnection connection = new(connectionString))
            {
                SqlCommand command = new(query, connection);

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Jogador jogador = new()
                    {
                        JogadorID = (int)reader["JogadorID"],
                        Nome = (string)reader["Nome"],                        
                    };

                    jogadores.Add(jogador);
                }

                reader.Close();
            }

            return jogadores;
        }

        public List<Jogador> SelecionarJogador_PorNome(string? nomeJogador = null)
        {
            List<Jogador> jogadores = new();
            string query = "SELECT * FROM Jogador";

            if (nomeJogador != null)
                query += $" WHERE Nome = '{nomeJogador}'";

            using (SqlConnection connection = new(connectionString))
            {
                SqlCommand command = new(query, connection);

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Jogador jogador = new()
                    {
                        JogadorID = (int)reader["JogadorID"],
                        Nome = (string)reader["Nome"],
                    };

                    jogadores.Add(jogador);
                }

                reader.Close();
            }

            return jogadores;
        }
    }
}

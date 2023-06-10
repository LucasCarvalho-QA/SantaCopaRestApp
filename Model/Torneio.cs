using System.Data.SqlClient;

namespace SantaCopaRestApp.Model
{
    public class Torneio
    {
        public int TorneioID { get; set; }
        public string Nome { get; set; }  
        public DateTime RealizadoEm { get; set; }

        private string connectionString = @"Server=LEKTOP\SQLEXPRESS; Database=SantaCopa;Integrated Security=true";    

        public List<Torneio> SelecionarTorneios(string? id = null)
        {
            List<Torneio> torneios = new();
            string query = "SELECT * FROM Torneio";

            if (id != null)
                query += $" WHERE TorneioID = {id}";

            using (SqlConnection connection = new(connectionString))
            {
                SqlCommand command = new(query, connection);

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Torneio torneio = new()
                    {
                        TorneioID = (int)reader["TorneioID"],
                        Nome = (string)reader["Nome"],
                        RealizadoEm = (DateTime)reader["RealizadoEm"]
                    };

                    torneios.Add(torneio);
                }

                reader.Close();
            }

            return torneios;
        }

        public void CriarTorneio(Torneio torneio)
        {
            using SqlConnection connection = new(connectionString);
            string query = "INSERT INTO Torneio (Nome, RealizadoEm) VALUES (@Nome, @RealizadoEm)";

            SqlCommand command = new(query, connection);
            command.Parameters.AddWithValue("@Nome", torneio.Nome);
            command.Parameters.AddWithValue("@RealizadoEm", torneio.RealizadoEm);

            connection.Open();
            command.ExecuteNonQuery();
        }

        public void ExcluirTorneio(string torneioID)
        {
            using SqlConnection connection = new(connectionString);
            string query = "DELETE FROM Torneio WHERE TorneioID = @TorneioID";

            SqlCommand command = new(query, connection);
            command.Parameters.AddWithValue("@TorneioID", torneioID);

            connection.Open();
            command.ExecuteNonQuery();
        }

        public void IniciarTorneio()
        {
            Gerenciador gerenciador = new Gerenciador();
            gerenciador.SortearPartidas("Rodada 1");
            gerenciador.SortearPartidas("Rodada 2");
            gerenciador.SortearPartidas("Rodada 3");            
        }
    }
}

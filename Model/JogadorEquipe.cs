using System.Data.SqlClient;

namespace SantaCopaRestApp.Model
{
    public class JogadorEquipe
    {
        public int JogadorEquipeID { get; set; }
        public int JogadorID { get; set; }
        public int EquipeID { get; set; }
        public string? Rodada { get; set; }

        private string connectionString = @"Server=LEKTOP\SQLEXPRESS; Database=SantaCopa;Integrated Security=true";

        public List<JogadorEquipe> SelecionarJogadorEquipe(string? id = null, string? equipeID = null)
        {
            List<JogadorEquipe> jogadorEquipes = new();
            string query = "SELECT * FROM JogadorEquipe";

            if (id != null && equipeID != null)
                query += $" WHERE JogadorID = {id} AND EquipeID = {equipeID}";

            if (id != null && equipeID == null)
                query += $" WHERE JogadorID = {id}";            

            using (SqlConnection connection = new(connectionString))
            {
                SqlCommand command = new(query, connection);

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    JogadorEquipe jogadorEquipe = new()
                    {
                        JogadorEquipeID = (int)reader["JogadorEquipeID"],
                        JogadorID = (int)reader["JogadorID"],
                        EquipeID = (int)reader["EquipeID"],
                        Rodada = (string)reader["Rodada"],
                    };

                    jogadorEquipes.Add(jogadorEquipe);
                }

                reader.Close();
            }

            return jogadorEquipes;
        }

        public void CriarJogadorEquipe(JogadorEquipe jogadorEquipe)
        {
            using SqlConnection connection = new(connectionString);
            string query = "INSERT INTO JogadorEquipe (JogadorID, EquipeID, Rodada) VALUES (@JogadorID, @EquipeID, @Rodada)";

            SqlCommand command = new(query, connection);
            command.Parameters.AddWithValue("@JogadorID", jogadorEquipe.JogadorID);
            command.Parameters.AddWithValue("@EquipeID", jogadorEquipe.EquipeID);
            command.Parameters.AddWithValue("@Rodada", jogadorEquipe.Rodada);

            connection.Open();
            command.ExecuteNonQuery();
        }
        
        public JogadorEquipe RetornarUltimoItemCriado()
        {
            List<JogadorEquipe> jogadorEquipes = new();
            string query = "SELECT TOP 1 * FROM JogadorEquipe ORDER BY 1 DESC";
            
            using (SqlConnection connection = new(connectionString))
            {
                SqlCommand command = new(query, connection);

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    JogadorEquipe jogadorEquipe = new()
                    {
                        JogadorEquipeID = (int)reader["JogadorEquipeID"],
                        JogadorID = (int)reader["JogadorID"],
                        EquipeID = (int)reader["EquipeID"],
                        Rodada = (string)reader["Rodada"],
                    };

                    jogadorEquipes.Add(jogadorEquipe);
                }

                reader.Close();
            }

            return jogadorEquipes.First();
        }

        public List<JogadorEquipe> VerificarTimesCriadosNaRodada(string rodada, string equipeId)
        {
            List<JogadorEquipe> jogadorEquipes = new();
            string query = $"SELECT * FROM JogadorEquipe WHERE Rodada = '{rodada}' AND EquipeID = '{equipeId}'";

            using (SqlConnection connection = new(connectionString))
            {
                SqlCommand command = new(query, connection);

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    JogadorEquipe jogadorEquipe = new()
                    {
                        JogadorEquipeID = (int)reader["JogadorEquipeID"],
                        JogadorID = (int)reader["JogadorID"],
                        EquipeID = (int)reader["EquipeID"],
                        Rodada = (string)reader["Rodada"],
                    };

                    jogadorEquipes.Add(jogadorEquipe);
                }

                reader.Close();
            }

            return jogadorEquipes;
        }
    }
}


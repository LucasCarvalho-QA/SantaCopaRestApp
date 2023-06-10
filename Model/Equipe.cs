using System.Data.SqlClient;

namespace SantaCopaRestApp.Model
{
    public class Equipe
    {
        public int EquipeID { get; set; }
        public string Nome { get; set; }
        public string Estrelas { get; set; }

        private string connectionString = @"Server=LEKTOP\SQLEXPRESS; Database=SantaCopa;Integrated Security=true";


        public List<Equipe> SelecionarEquipes(string? id = null, string? estrelas = null)
        {
            List<Equipe> equipes = new();
            string query = "SELECT * FROM Equipe";

            if (id != null)
                query += $" WHERE EquipeID = '{id}'";

            if (estrelas != null)
                query += $" WHERE Estrelas = '{estrelas}'";

            using (SqlConnection connection = new(connectionString))
            {
                SqlCommand command = new(query, connection);

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Equipe equipe = new()
                    {
                        EquipeID = (int)reader["EquipeID"],
                        Nome = (string)reader["Nome"],
                        Estrelas = (string)reader["Estrelas"]
                    };

                    equipes.Add(equipe);
                }

                reader.Close();
            }

            return equipes;
        }
    }
}

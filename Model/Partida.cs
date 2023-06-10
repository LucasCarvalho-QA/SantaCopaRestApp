using System.Data.SqlClient;
using System.Reflection;

namespace SantaCopaRestApp.Model
{
    public class Partida
    {
        public int PartidaID { get; set; }
        public string Rodada { get; set; }
        public string? NivelEstrelas { get; set; }        
        public string? Local { get; set; }

        public string? JogadorCasa { get; set; }
        public string? EquipeCasa { get; set; }
        public int JogadorCasaGols { get; set; }

        public string? JogadorVisitante { get; set; }
        public string? EquipeVisitante { get; set; }
        public int JogadorVisitanteGols { get; set; }

        public int TorneioID { get; set; }


        private string connectionString = @"Server=LEKTOP\SQLEXPRESS; Database=SantaCopa;Integrated Security=true";

        public List<Partida> SelecionarPartidas(string? rodadaId = null, string? partidaId = null)
        {
            List<Partida> partidas = new();
            string query = "SELECT * FROM Partida";

            if (rodadaId != null && partidaId == null)
                query += $" WHERE Rodada = 'Rodada {rodadaId}'";

            if (rodadaId == null && partidaId != null)
                query += $" WHERE PartidaID = '{partidaId}'";

            using (SqlConnection connection = new(connectionString))
            {
                SqlCommand command = new(query, connection);

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Partida partida = new()
                    {
                        PartidaID = (int)reader["PartidaID"],
                        Rodada = (string)reader["Rodada"],
                        Local = (string)reader["Local"],
                        NivelEstrelas = (string)reader["NivelEstrelas"],

                        JogadorCasaGols = (int)reader["JogadorCasaGols"],
                        JogadorCasa = (string)reader["JogadorCasa"],
                        EquipeCasa = (string)reader["EquipeCasa"],

                        JogadorVisitanteGols = (int)reader["JogadorVisitanteGols"],
                        JogadorVisitante = (string)reader["JogadorVisitante"],
                        EquipeVisitante = (string)reader["EquipeVisitante"],

                        TorneioID = (int)reader["TorneioID"],
                    };

                    partidas.Add(partida);
                }

                reader.Close();
            }

            return partidas;
        }

        public void CriarPartida(Partida partida)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = @"INSERT INTO Partida (Rodada, NivelEstrelas, Local, JogadorCasa, EquipeCasa, JogadorCasaGols, JogadorVisitante, EquipeVisitante, JogadorVisitanteGols, TorneioID)
                             VALUES (@Rodada, @NivelEstrelas, @Local, @JogadorCasa, @EquipeCasa, @JogadorCasaGols, @JogadorVisitante, @EquipeVisitante, @JogadorVisitanteGols, @TorneioID)";

                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Rodada", partida.Rodada);
                command.Parameters.AddWithValue("@NivelEstrelas", partida.NivelEstrelas);
                command.Parameters.AddWithValue("@Local", partida.Local);
                command.Parameters.AddWithValue("@JogadorCasa", partida.JogadorCasa);
                command.Parameters.AddWithValue("@EquipeCasa", partida.EquipeCasa);
                command.Parameters.AddWithValue("@JogadorCasaGols", partida.JogadorCasaGols);
                command.Parameters.AddWithValue("@JogadorVisitante", partida.JogadorVisitante);
                command.Parameters.AddWithValue("@EquipeVisitante", partida.EquipeVisitante);
                command.Parameters.AddWithValue("@JogadorVisitanteGols", partida.JogadorVisitanteGols);
                command.Parameters.AddWithValue("@TorneioID", 1);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public void AlterarPartida(Partida partida)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "UPDATE Partida SET ";
                SqlCommand command = new SqlCommand();
                command.Connection = connection;

                if (!string.IsNullOrEmpty(partida.Rodada))
                {
                    query += "Rodada = @Rodada, ";
                    command.Parameters.AddWithValue("@Rodada", partida.Rodada);
                }

                if (!string.IsNullOrEmpty(partida.Local))
                {
                    query += "Local = @Local, ";
                    command.Parameters.AddWithValue("@Local", partida.Local);
                }

                if (!string.IsNullOrEmpty(partida.JogadorCasa))
                {
                    query += "JogadorCasa = @JogadorCasa, ";
                    command.Parameters.AddWithValue("@JogadorCasa", partida.JogadorCasa);
                }

                if (!string.IsNullOrEmpty(partida.EquipeCasa))
                {
                    query += "EquipeCasa = @EquipeCasa, ";
                    command.Parameters.AddWithValue("@EquipeCasa", partida.EquipeCasa);
                }

                if (partida.JogadorCasaGols >= 0)
                {
                    query += "JogadorCasaGols = @JogadorCasaGols, ";
                    command.Parameters.AddWithValue("@JogadorCasaGols", partida.JogadorCasaGols);
                }

                if (!string.IsNullOrEmpty(partida.JogadorVisitante))
                {
                    query += "JogadorVisitante = @JogadorVisitante, ";
                    command.Parameters.AddWithValue("@JogadorVisitante", partida.JogadorVisitante);
                }

                if (!string.IsNullOrEmpty(partida.EquipeVisitante))
                {
                    query += "EquipeVisitante = @EquipeVisitante, ";
                    command.Parameters.AddWithValue("@EquipeVisitante", partida.EquipeVisitante);
                }

                if (partida.JogadorVisitanteGols >= 0)
                {
                    query += "JogadorVisitanteGols = @JogadorVisitanteGols, ";
                    command.Parameters.AddWithValue("@JogadorVisitanteGols", partida.JogadorVisitanteGols);
                }

                query = query.TrimEnd(',', ' ');
                query += " WHERE PartidaID = @PartidaID";
                command.Parameters.AddWithValue("@PartidaID", partida.PartidaID);

                command.CommandText = query;

                connection.Open();
                command.ExecuteNonQuery();
            }
        }
    }
}


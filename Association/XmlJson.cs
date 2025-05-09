using System;

using System.IO;
using MySql.Data.MySqlClient;
using System.Text.Json;
using System.Xml.Serialization;


namespace Association
{
    public class Champ
    {
        public string Nom { get; set; }
        public string Valeur { get; set; }
    }

    public class Ligne
    {
        public List<Champ> Champs { get; set; }
    }

    internal class XmlJson
	{
		
		public static void MainXmlJson(MySqlConnection conn)
		{
			try
			{
				/// Demander à l'utilisateur le nom de la table à traiter
				Console.Write("Quelle table souhaitez-vous changer de format ? ");
				string tableName = Console.ReadLine()?.Trim();  /// Éliminer les espaces superflus

				while (string.IsNullOrEmpty(tableName))
				{
					Console.WriteLine("Vous n'avez donné aucune table, veuillez essayer à nouveau.");
					tableName = Console.ReadLine()?.Trim();
				}

				/// Récupérer les données de la table spécifiée
				List<object> data = GetTable(conn, tableName);

				/// Sérialisation des données en XML
				serialize_XML(data, tableName);

				/// Sérialisation des données en JSON
				serialize_JSON(data, tableName);
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Erreur : {ex.Message}");
			}
		}

		public static List<object> GetTable(MySqlConnection conn, string nomTable)
		{
			List<object> resultat = new List<object>();

            /// Exécution de la requête SQL pour récupérer toutes les données d'une table
            MySqlCommand cmd = new MySqlCommand($"SELECT * FROM {nomTable}", conn);
            var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                var colonne = new Dictionary<string, object>();

                for (int i = 0; i < reader.FieldCount; i++)
                {
                    colonne[reader.GetName(i)] = reader.GetValue(i);  /// Dynamique : Ajoute le nom de la colonne et sa valeur
                }

                resultat.Add(colonne);  /// Ajouter la ligne au résultat
            }
            reader.Close();

			return resultat;
		}

		/// Sérialisation des données en XML (adaptée pour les dictionnaires)
		public static void serialize_XML(List<object> list, string table)
		{
            var serializableList = ConvertToSerializable(list);

            /// Sérialisation de la liste des données en XML
            XmlSerializer serializer = new XmlSerializer(typeof(List<Ligne>));  /// Modifier le type de sérialisation
			using (var stream = new FileStream($"{table}.xml", FileMode.Create))
			{
				serializer.Serialize(stream, serializableList);
			}

			Console.WriteLine("Fichier XML généré avec succès dans le dossier Debug.");
		}

        private static List<Ligne> ConvertToSerializable(List<object> data)
        {
            var result = new List<Ligne>();

            foreach (var item in data)
            {
                if (item is Dictionary<string, object> ligne)
                {
                    var champs = new List<Champ>();
                    foreach (var kvp in ligne)
                    {
                        champs.Add(new Champ
                        {
                            Nom = kvp.Key,
                            Valeur = kvp.Value?.ToString() ?? ""
                        });
                    }
                    result.Add(new Ligne { Champs = champs });
                }
            }

            return result;
        }


        /// Sérialisation des données en JSON
        public static void serialize_JSON(List<object> list, string table)
		{
			/// Sérialisation des données en JSON
			string json = JsonSerializer.Serialize(list, new JsonSerializerOptions { WriteIndented = true });

			/// Sauvegarde dans un fichier JSON
			File.WriteAllText($"{table}.json", json);

			Console.WriteLine("Fichier JSON généré avec succès dans le dossier Debug.");
		}
	}
}

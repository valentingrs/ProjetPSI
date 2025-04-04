using MySql.Data.MySqlClient;
using System.Transactions;

namespace Association
{
	internal class Bdd
	{

		#region Tiers
		public static void CreerUnCompte(MySqlConnection conn, int IDTiers, string CodeP, string Ville, string Email, string Tel, string Nom, string Adresse, string Prenom)
		{
			
			if (Existe(conn, "Tiers", "IDTiers", IDTiers)) { Console.WriteLine("Compte déjà existant !"); return; }
			else
			{
				try
				{
					string query = $"INSERT INTO Tiers (IDTiers, CodePostal, Ville, Email, Tel, Nom, Adresse, Prenom) " +
								   $"VALUES ({IDTiers}, '{CodeP}', '{Ville}', '{Email}', '{Tel}', '{Nom}', '{Adresse}', '{Prenom}');";

					using (MySqlCommand cmd = new MySqlCommand(query, conn))
					{
						cmd.ExecuteNonQuery();
						Console.WriteLine("Compte créé avec succès !");
					}
				}
				catch (MySqlException e)
				{
					Console.WriteLine($"Erreur MySQL : {e.Message}");
				}
			}
			
		}

		public static bool Existe(MySqlConnection conn, string Table, string AttributElement, object element)
		{
			bool ElementExiste = false;

			// Utilisation de paramètres pour éviter les erreurs de syntaxe et les injections SQL
			string query = $"SELECT COUNT(*) FROM {Table} WHERE {AttributElement} = @element;";

			try
			{
				using (MySqlCommand cmd = new MySqlCommand(query, conn))
				{
					// Vérifie si l'élément est un nombre ou une chaîne de caractères
					if (element is int || element is long)
						cmd.Parameters.AddWithValue("@element", Convert.ToInt64(element));
					else
						cmd.Parameters.AddWithValue("@element", element.ToString());

					// Exécute la requête et vérifie le nombre d'occurrences
					int count = Convert.ToInt32(cmd.ExecuteScalar());
					ElementExiste = (count > 0);
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine("Erreur lors de la vérification de l'existence : " + ex.Message);
			}
			return ElementExiste;
		}

		public static void SupprimerUnCompte(MySqlConnection conn, string Table, int Identifiant)
		{
			try
			{
				string query = $"DELETE FROM {Table} WHERE ID{Table} = {Identifiant};";

				using (MySqlCommand cmd = new MySqlCommand(query, conn))
				{
					cmd.ExecuteNonQuery();
					Console.WriteLine("Compte supprimé avec succès !");
				}
			}
			catch (MySqlException e)
			{
				Console.WriteLine($"Erreur MySQL : {e.Message}");
			}
		}

        #endregion

        #region Cuisinier
        public static void AjouterClient(bool VeuxEtreClient, int IDTiers, MySqlConnection conn)
		{
			if (VeuxEtreClient)
			{
				try
				{
					string query = $"INSERT INTO Client (IDTiers) VALUES ({IDTiers});";

					using (MySqlCommand cmd = new MySqlCommand(query, conn))
					{
						cmd.ExecuteNonQuery();
						Console.WriteLine("Vous êtes un client !");
					}
				}
				catch (MySqlException e)
				{
					Console.WriteLine($"Erreur MySQL : {e.Message}");
				}
			}
			else
			{
				Console.WriteLine("Vous n'avez pas pu être ajouté à la table 'Client'.");
			}
		}
		public static void SupprimerClient(MySqlConnection conn, string Table, int Identifiant)
		{
			try
			{
				string query = $"DELETE FROM {Table} WHERE ID{Table} = {Identifiant};";

				using (MySqlCommand cmd = new MySqlCommand(query, conn))
				{
					cmd.ExecuteNonQuery();
					Console.WriteLine("Client supprimé avec succès !");
				}
			}
			catch (MySqlException e)
			{
				Console.WriteLine($"Erreur MySQL : {e.Message}");
			}
		}

        #endregion

        #region Cuisinier
        public static void AjouterCuisinier(bool VeuxEtreCuisinier, int IDTiers, MySqlConnection conn)
		{
			if (VeuxEtreCuisinier)
			{
				try
				{
					string query = $"INSERT INTO Cuisinier VALUES ({IDTiers});";

					using (MySqlCommand cmd = new MySqlCommand(query, conn))
					{
						cmd.ExecuteNonQuery();
						Console.WriteLine("Vous êtes maintenant cuisinier !");
					}
				}
				catch (MySqlException e)
				{
					Console.WriteLine($"Erreur MySQL : {e.Message}");
				}
			}
			else
			{
				Console.WriteLine("Vous n'avez pas pu être ajouté à la table 'Cuisinier'.");
			}
		}

        public static void SupprimerCuisinier(MySqlConnection conn, string Table, int Identifiant)
        {
            try
            {
                string query = $"DELETE FROM {Table} WHERE ID{Table} = {Identifiant};";

                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.ExecuteNonQuery();
                    Console.WriteLine("Cuisinier supprimé avec succès !");
                }
            }
            catch (MySqlException e)
            {
                Console.WriteLine($"Erreur MySQL : {e.Message}");
            }
        }
		#endregion

		public static void FairePlat(bool EtreCuisinier, MySqlConnection conn, int IDPlat, string TypePlat, DateTime DateFabrication, DateTime DatePeremption, string Nationalite, string Regime, string Ingredients, float PrixPlat, int NombrePersonnes, int idCuisinier)
		{
			MySqlTransaction transaction = conn.BeginTransaction();

			if (EtreCuisinier)
			{
				try
				{
					string query = $"INSERT INTO Plat (IDPlat, TypePlat, DateFabrication, DatePeremption, Nationalite, Regime, Ingredients, PrixPlat, NombrePersonnes) " +
								   $"VALUES ({IDPlat}, '{TypePlat}', '{DateFabrication:yyyy-MM-dd}', '{DatePeremption:yyyy-MM-dd}', '{Nationalite}', '{Regime}', '{Ingredients}', {PrixPlat}, {NombrePersonnes}, {idCuisinier});";

					using (MySqlCommand cmd = new MySqlCommand(query, conn, transaction))
					{
						cmd.ExecuteNonQuery();
						transaction.Commit();
						Console.WriteLine("Plat ajouté avec succès !");
					}
				}
				catch (MySqlException e)
				{
					Console.WriteLine($"Erreur MySQL : {e.Message}");
					transaction.Rollback();
				}
			}
			else
			{
				Console.WriteLine("Vous n'avez pas pu ajouter ce plat.");
			}
		}

		static void FaireUneCommande(bool EtreClient, MySqlConnection conn, int IDCommande, DateTime DateCommande, DateTime HeureCommande, int IDTiers)
		{
			MySqlTransaction transaction = conn.BeginTransaction();

			if (EtreClient && IDTiers != 0)
			{
				try
				{
					string query = $"INSERT INTO Commande (IDCommande, DateCommande, HeureCommande, IDTiers) " +
								   $"VALUES ({IDCommande}, '{DateCommande:yyyy-MM-dd}', '{HeureCommande:HH:mm:ss}', {IDTiers});";

					using (MySqlCommand cmd = new MySqlCommand(query, conn, transaction))
					{
						cmd.ExecuteNonQuery();
						transaction.Commit();
						Console.WriteLine("Commande ajoutée avec succès !");
					}
				}
				catch (MySqlException e)
				{
					Console.WriteLine($"Erreur MySQL : {e.Message}");
					transaction.Rollback();
				}
			}
			else
			{
				Console.WriteLine("Vous n'avez pas pu faire cette commande.");
			}
		}

		static void MainSQL(MySqlConnection conn)
		{
			try
			{
				conn.Open();
				// Insérer element qu'on sauhaite faire
				conn.Close();
			}
			catch (MySqlException e)
			{
				Console.WriteLine("Erreur de Connexion : " + e.ToString());
			}
		}
	}
}
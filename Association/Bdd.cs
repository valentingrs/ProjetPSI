using MySql.Data.MySqlClient;
using MySqlX.XDevAPI.Relational;
using System.ComponentModel;
using System.Data.Common;
using System.Transactions;

namespace Association
{
	internal class Bdd
	{

        #region Méthodes générales
        public static bool Existe(MySqlConnection conn, string Table, string AttributElement, object element)
        {
            bool ElementExiste = false;

            // Nombre de fois qu'existe l'attribut avec cette valeur
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

        static void AffichageElementsOrdre(MySqlConnection conn, string table, string colonne, string colonneOrdre, bool croissant)
        {
            try
            {
                string query = $"SELECT '{colonne}' FROM {table} ORDER BY {colonneOrdre};";
				if (!croissant) { query += " DESC"; }

                MySqlCommand cmd = new MySqlCommand(query, conn);
                MySqlDataReader reader = cmd.ExecuteReader();

                int ligne = 0;
                while (reader.Read())
                {
                    ligne++;
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        Console.Write($"{reader.GetName(i)}: {reader[i]}\t");
                    }
                    Console.WriteLine();
                }

                if (ligne == 0)
                    Console.WriteLine("Aucun résultat.");
            }
            catch (MySqlException e)
            {
                Console.WriteLine($"Erreur MySQL : {e.Message}");
            }
        }

        #endregion

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

        public static void ModifierCompte(MySqlConnection conn, string ElementAChanger, string NouvelElement, int identifiant, string Table)
        { 
            try
            {
                string query = $"UPDATE {Table} SET {ElementAChanger}='{NouvelElement}' WHERE ID{Table} = {identifiant};";
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.ExecuteNonQuery();
                    Console.WriteLine("Votre modification a bien été prise en compte.");
                }
            }
            catch (MySqlException e)
            {
                Console.WriteLine($"Erreur MySQL : {e.Message}");
            }
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
					string query = $"INSERT INTO Client (IDClient) VALUES ({IDTiers});";

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
        
        public static void AffichageClientsSql(MySqlConnection conn, string donnees, bool croissant)
        {
            string query = "SELECT Tiers.IDTiers, Tiers.Prenom, Tiers.Nom, Tiers.Adresse FROM Client " +
                           "JOIN Tiers ON Client.IDClient = Tiers.IDTiers " + "ORDER BY ";
            
            if (donnees == "Ordre alpha") { query += "Nom ASC, Prenom ASC;"; }
            else if (donnees == "Rue") { query += "Adresse ASC;"; }
            else if (donnees == "Achats")
            {
                query = "SELECT Tiers.IDTiers, Tiers.Nom, Tiers.Prenom, SUM(Plat.PrixPlat) AS MontantTotalAchats " +
                        "FROM  Client " +
                        "JOIN Tiers ON Client.IDClient = Tiers.IDTiers " +
                        "JOIN Commande ON Client.IDClient = Commande.IDClient " +
                        "JOIN PlatCommande ON Commande.IDCommande = PlatCommande.IDCommande " +
                        "JOIN Plat ON PlatCommande.IDPlat = Plat.IDPlat GROUP BY Tiers.IDTiers, Tiers.Nom, Tiers.Prenom " +
                        "ORDER BY MontantTotalAchats DESC;";
            }
            else { Console.WriteLine("Mauvais paramètre entré"); return; }
            try
            {
                MySqlCommand cmd = new MySqlCommand(query, conn);
                MySqlDataReader reader = cmd.ExecuteReader();

                int ligne = 0;
                while (reader.Read())
                {
                    ligne++;
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        Console.Write($"{reader.GetName(i)}: {reader[i]}\t");
                    }
                    Console.WriteLine();
                }
                reader.Close(); /// fermer le reader car il ne peut y avoir qu'un reader ouvert à la fois
                if (ligne == 0)
                    Console.WriteLine("Aucun résultat.");
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
					string query = $"INSERT INTO Cuisinier(IDCuisinier) VALUES ({IDTiers});";

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

        public static void FairePlat(bool EtreCuisinier, MySqlConnection conn, int IDPlat, string TypePlat, string nomPlat, DateTime DateFabrication, DateTime DatePeremption, string Nationalite, string Regime, string Ingredients, double PrixPlat, int NombrePersonnes, int idCuisinier)
        {

            if (EtreCuisinier)
            {
                string Formatfabrication = DateFabrication.ToString("yyyy-MM-dd"); // formatage du typeDateTime pour qu'il convienne au type DATE de Sql
                string Formatperemption = DatePeremption.ToString("yyyy-MM-dd");
				string prix = PrixPlat.ToString().Replace(",", "."); // formatage du prix pour qu'il convienne à Sql
                try
                { 
                    string query = $"INSERT INTO Plat " +
                                   $"VALUES ({IDPlat}, '{TypePlat}', '{nomPlat}', '{Formatfabrication}', '{Formatperemption}', '{Nationalite}', '{Regime}', '{Ingredients}', {prix}, {NombrePersonnes}, {idCuisinier});";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.ExecuteNonQuery();
                        Console.WriteLine("Plat ajouté avec succès !");
                    }
                }
                catch (MySqlException e)
                {
                    Console.WriteLine($"Erreur MySQL : {e.Message}");
                }
            }
            else
            {
                Console.WriteLine("Vous n'avez pas pu ajouter ce plat.");
            }
        }


        public static void ClientsServisCuisinier(MySqlConnection conn, bool temps, string date, int idCuisinier)
        {
            try
            {
                string query = "SELECT Tiers.IDTiers, Tiers.Nom, Tiers.Prenom FROM Commande " +
                               "JOIN Client ON Commande.IDClient = Client.IDClient " +
                               "JOIN Tiers ON Client.IDClient = Tiers.IDTiers " +
                              $"WHERE Commande.IDCuisinier = {idCuisinier}";
                if (temps)
                {
                    query += $"WHERE Commande.DateCommande >=  '{date}';";
                }
                else { query += ";"; }

                MySqlCommand cmd = new MySqlCommand(query, conn);
                MySqlDataReader reader = cmd.ExecuteReader();

                int ligne = 0;
                while (reader.Read())
                {
                    ligne++;
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        Console.Write($"{reader.GetName(i)}: {reader[i]}\t");
                    }
                    Console.WriteLine();
                }

                if (ligne == 0)
                    Console.WriteLine("Aucun résultat.");
            }
            catch (MySqlException e)
            {
                Console.WriteLine($"Erreur MySQL : {e.Message}");
            }
        }
        #endregion

        #region Commande

        public static bool PlatCuisinierValide(MySqlConnection conn, int idCuisinier, int idPlat)
        {
            try
            {
                string query = $"SELECT IDPlat FROM Plat WHERE IDCuisinier = {idCuisinier};";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                MySqlDataReader reader = cmd.ExecuteReader();
                List<int> idPlatsCuisinier = new List<int>();

                using (reader)
                {
                    while (reader.Read())
                    {
                        int platId = Convert.ToInt32(reader["IDPlat"]);
                        idPlatsCuisinier.Add(platId);
                    }
                }
                reader.Close();
                return idPlatsCuisinier.Contains(idPlat);
                
            }
            catch (MySqlException e)
            {
                Console.WriteLine($"Erreur MySQL : {e.Message}");
                return false;
            }
        }
        public static void FaireUneCommande(bool EtreClient, MySqlConnection conn, int IDCommande, DateTime DateCommande, DateTime HeureCommande, int idClient, int idCuisinier)
		{
            string Formatfabrication = DateCommande.ToString("yyyy-MM-dd"); // formatage du typeDateTime pour qu'il convienne au type DATE de Sql
            string Formatperemption = HeureCommande.ToString("hh:mm:ss");
            if (EtreClient)
			{
				try
				{
                    Console.WriteLine(IDCommande + " " + idClient + " " + idCuisinier);
					string query = $"INSERT INTO Commande " +
								   $"VALUES ({IDCommande}, '{DateCommande:yyyy-MM-dd}', '{HeureCommande:HH:mm:ss}', {idClient}, {idCuisinier});";
					using (MySqlCommand cmd = new MySqlCommand(query, conn))
					{
						cmd.ExecuteNonQuery();
						Console.WriteLine("Commande ajoutée avec succès !");
					}
				}
                catch (MySqlException e)
                {
                    Console.WriteLine($"Erreur MySQL : {e.Message}");
                }
            }
			else
			{
				Console.WriteLine("Vous n'avez pas pu faire cette commande.");
			}
		}

		public static void CommandePlat(MySqlConnection conn, int idCommande, int idPlat)
		{
			try
			{
				string query = $"INSERT INTO PlatCommande (IDCommande, IDPlat) " +
							   $"VALUES ({idCommande}, {idPlat});";
                Console.WriteLine("Check 3");
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.ExecuteNonQuery();
                    Console.WriteLine("Plat ajouté la commande n°" + idCommande+ " avec succès !");
                }
            }
            catch (MySqlException e)
            {
                Console.WriteLine($"Erreur MySQL : {e.Message}");
            }
        }

        public static double CalculerPrixCommande(int idCommande, MySqlConnection conn)
        {
            double prixC = 0;

            string query = "SELECT SUM(p.PrixPlat) AS PrixTotal FROM PlatCommande c JOIN Plat p ON c.IDPlat = p.IDPlat WHERE c.IDCommande = ?IDCommande;";

            using (var commande = new MySqlCommand(query, conn))
            {
                commande.Parameters.Add(new MySqlParameter("?IDCommande", MySqlDbType.Int32) { Value = idCommande });

                object result = commande.ExecuteScalar();
                if (result != DBNull.Value && result != null)
                {
                    prixC = Convert.ToSingle(result);
                }
            }

            return prixC;
        }
        #endregion

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
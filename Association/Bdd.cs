using MySql.Data.MySqlClient;
using MySqlX.XDevAPI.Relational;
using SkiaSharp;
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

                if (ligne == 0) { Console.WriteLine("Aucun résultat."); }
                reader.Close();
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
                              $"WHERE Commande.IDCuisinier = {idCuisinier} ";
                if (temps)
                {
                    query += $"AND Commande.DateCommande >=  '{date}'";
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

                if (ligne == 0) { Console.WriteLine("Aucun résultat."); }
                reader.Close();
            }
            catch (MySqlException e)
            {
                Console.WriteLine($"Erreur MySQL : {e.Message}");
            }
        }

        public static void PlatsAffichage(MySqlConnection conn, int id, string param)
        {
            try
            {
                string query = "";
                if (param == "PlatsFreq")
                {
                    query = "SELECT Plat.NomPlat AS Plat, COUNT(pc.IDPlat) AS Nombre FROM Plat " +
                               "JOIN PlatCommande pc ON pc.IDPlat = Plat.IDPlat " +
                               $"WHERE Plat.IDCuisinier = {id} " +
                               "GROUP BY Plat.NomPlat";
                }
                if (param == "PlatDuJour")
                {
                    query = "SELECT Plat.IDPlat AS n°, Plat.NomPlat AS Nom FROM Plat " +
                           $"WHERE Plat.IDCuisinier = {id} " +
                            "AND Plat.IDPlat NOT IN(SELECT PlatCommande.IDPlat FROM PlatCommande);";
                }
                if (param == "InfosPlat")
                {
                    query = $"SELECT * FROM Plat WHERE Plat.IDPlat = {id}";
                }
                MySqlCommand cmd = new MySqlCommand(query, conn);
                MySqlDataReader reader = cmd.ExecuteReader();
                using (reader)
                {
                    // En-tête
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        Console.Write($"{reader.GetName(i),-20}");
                    }
                    Console.WriteLine();
                    Console.WriteLine(new string('-', 20 * reader.FieldCount));

                    // Données
                    while (reader.Read())
                    {
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            Console.Write($"{reader[i],-20}");
                        }
                        Console.WriteLine();
                    }

                    reader.Close();
                }
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

        public static void PlatsDispos(MySqlConnection conn)
        {
            try
            {
                string query = "SELECT Plat.NomPlat AS Plat, Plat.Ingredients AS Ingredients, Plat.IDPlat AS IDPlat, Tiers.Prenom, Tiers.Nom, " +
                    "Tiers.IDTiers AS Identifiant, Tiers.CodePostal AS Arondissement FROM Plat " +
                    "JOIN Tiers ON Plat.IDCuisinier = Tiers.IDTiers " +
                    "WHERE Plat.IDPlat NOT IN (SELECT PlatCommande.IDPlat FROM PlatCommande);";

                MySqlCommand cmd = new MySqlCommand(query, conn);
                MySqlDataReader reader = cmd.ExecuteReader();
                using (reader)
                {
                    Console.WriteLine("{0,-20} {1, -35} {2,-8} {3,-10} {4,-12} {5,-12} {6,-10}",
                        "Plat", "Ingredients", "IDPlat", "Prenom", "Nom", "Identifiant", "Arrondissement");
                    Console.WriteLine(new string('-', 120));

                    while (reader.Read())
                    {
                        Console.WriteLine("{0,-20} {1, -35} {2,-8} {3,-10} {4,-12} {5,-12} {6,-10}",
                            reader["Plat"],
                            reader["Ingredients"],
                            reader["IDPlat"],
                            reader["Prenom"],
                            reader["Nom"],
                            reader["Identifiant"],
                            reader["Arondissement"]);
                    }
                }
            }
            catch (MySqlException e)
            {
                Console.WriteLine($"Erreur MySQL : {e.Message}");
            }
        }

        public static void CommandePlat(MySqlConnection conn, int idCommande, int idPlat)
        {
            try
            {
                string query = $"INSERT INTO PlatCommande (IDCommande, IDPlat) " +
                               $"VALUES ({idCommande}, {idPlat});";
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.ExecuteNonQuery();
                    Console.WriteLine("Plat ajouté la commande n°" + idCommande + " avec succès !");
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

            string query = $"SELECT SUM(p.PrixPlat) AS PrixTotal FROM PlatCommande c JOIN Plat p ON c.IDPlat = p.IDPlat WHERE c.IDCommande = {idCommande};";

            using (var commande = new MySqlCommand(query, conn))
            {
                object result = commande.ExecuteScalar();
                if (result != DBNull.Value && result != null)
                {
                    prixC = Convert.ToSingle(result);
                }
            }

            return prixC;
        }


        /// utile pour GrapheTiers 
        public struct Commande
        {
            public int idCommande;
            public int idClient;
            public int idCuisinier;
        }

        public static List<int> RecupererTiers(MySqlConnection conn)
        {
            List<int> listeIDTiers = new List<int>();
            try
            {
                string queryTiers = "SELECT IDTiers FROM Tiers;";
                MySqlCommand cmd = new MySqlCommand(queryTiers, conn);
                MySqlDataReader reader = cmd.ExecuteReader();
                using (reader)
                {
                    while (reader.Read())
                    {
                        int idTiers = (int)reader["IDTiers"];

                        listeIDTiers.Add(idTiers);
                    }
                    
                }
            }
            catch (MySqlException e)
            {
                Console.WriteLine($"Erreur MySQL : {e.Message}");
            }
            return listeIDTiers;
        }
        public static List<Commande> RecupererCommande(MySqlConnection conn)
        {

            List<Commande> commandes = new List<Commande>();

            try
            {
                string query = "SELECT IDCommande, IDClient, IDCuisinier FROM Commande;";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                MySqlDataReader reader = cmd.ExecuteReader();
                using (reader)
                {
                    while (reader.Read())
                    {
                        int idCommande = Convert.ToInt32(reader["IDCommande"]);
                        int idClient = Convert.ToInt32(reader["IDClient"]);
                        int idCuisinier = Convert.ToInt32(reader["IDCuisinier"]);

                        Commande commande = new Commande();
                        commande.idCommande = idCommande;
                        commande.idClient = idClient;
                        commande.idCuisinier = idCuisinier;

                        commandes.Add(commande);
                    }
                    
                }
            }

            catch (MySqlException e)
            {
                Console.WriteLine($"Erreur MySQL : {e.Message}");
            }
            return commandes;
                
        }
        #endregion

        #region Statistiques

        public static void NombreCommandes(MySqlConnection conn)
        {
            try
            {
                string query = "SELECT Tiers.Prenom, Tiers.Nom, Count(Commande.IDCommande) " +
                            "AS NombreCommandes FROM Commande " +
                            "JOIN Tiers ON Commande.IDCuisinier = Tiers.IDTiers " +
                            "GROUP BY Tiers.IDTiers " +
                            "ORDER BY NombreCommandes DESC";

                MySqlCommand cmd = new MySqlCommand(query, conn);
                MySqlDataReader reader = cmd.ExecuteReader();
                using (reader)
                {
                    while (reader.Read())
                    {
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            Console.Write($"{reader.GetName(i)}: {reader[i]}\t");
                        }
                        Console.WriteLine();
                    }
                }
            }
            catch (MySqlException e)
            {
                Console.WriteLine($"Erreur MySQL : {e.Message}");
            }
        }

        public static void AfficherCommandes(MySqlConnection conn, DateTime debut, DateTime fin, string duree, int? idClient=null) // paramètre idClient facultatif
        {
            string query = "SELECT c.IDCommande, c.DateCommande, c.IDClient, c.IDCuisinier FROM Commande c ";


            if (duree == "Toujours") { query += ""; }
            if (duree == "PasFin")
            {
                string formatDebut = debut.ToString("yyyy-MM-dd"); // formatage du typeDateTime pour qu'il convienne au type DATE de Sql
                query += $"WHERE DateCommande >= '{formatDebut}'";
            }
            if (duree == "PasDebut")
            {
                string formatFin = fin.ToString("yyyy-MM-dd"); // formatage du typeDateTime pour qu'il convienne au type DATE de Sql
                query += $"WHERE DateCommande <= '{formatFin}'";
            }
            if (duree == "DebutEtFin")
            {
                string formatDebut = debut.ToString("yyyy-MM-dd"); // formatage du typeDateTime pour qu'il convienne au type DATE de Sql
                string formatFin = fin.ToString("yyyy-MM-dd"); // formatage du typeDateTime pour qu'il convienne au type DATE de Sql
                query += $"WHERE DateCommande >= '{formatDebut}' AND DateCommande <= '{formatFin}'";
            }

            if (idClient.HasValue) // paramètre supplémentaire si on veut la requête pour qu'un client précis
            {
                if (duree == "Toujours") { Console.Write("check tojours");  query += $" WHERE c.IDClient = {idClient};"; }
                else
                {
                    query += $" AND c.IDClient = {idClient};";
                }
            }
            else { query += ";"; }

            MySqlCommand cmd = new MySqlCommand(query, conn);
            MySqlDataReader reader = cmd.ExecuteReader();
            using (reader)
            {
                while (reader.Read())
                {
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        Console.Write($"{reader.GetName(i)}: {reader[i]}\t");
                    }
                    Console.WriteLine();
                }
            }
        }

        public static double CalculerMoyenne(MySqlConnection conn, string table, string colonne)
        {
            double moyenne = 0;

            string query = $"SELECT AVG({colonne}) AS Moyenne FROM {table};";

            using (MySqlCommand cmd = new MySqlCommand(query, conn))
            {
                object result = cmd.ExecuteScalar();
                if (result != DBNull.Value && result != null)
                    moyenne = Convert.ToSingle(result);
            }

            return moyenne;
        }

        public static void MoyenneStats(MySqlConnection conn, string param)
        {
            string query = "";
            if (param == "PrixCommandes")
            {
                query = "SELECT AVG(PrixTotal) AS Prix FROM (SELECT c.IDCommande, SUM(p.PrixPlat) AS PrixTotal FROM PlatCommande c " + 
                        "JOIN Plat p ON c.IDPlat = p.IDPlat GROUP BY c.IDCommande) AS Moyenne;";
            }
            if (param == "MoyenneClients")
            {
                query = "SELECT AVG(MontantTotalAchats) AS Moyenne FROM " +
                        "(SELECT SUM(Plat.PrixPlat) AS MontantTotalAchats, Client.IDClient FROM  Client " +
                        "JOIN Tiers ON Client.IDClient = Tiers.IDTiers " +
                        "JOIN Commande ON Client.IDClient = Commande.IDClient " +
                        "JOIN PlatCommande ON Commande.IDCommande = PlatCommande.IDCommande " +
                        "JOIN Plat ON PlatCommande.IDPlat = Plat.IDPlat " +
                        "GROUP BY Client.IDClient) AS MoyenneComptesClient; ";
            }
            MySqlCommand cmd = new MySqlCommand(query, conn);
            MySqlDataReader reader = cmd.ExecuteReader();
            using (reader)
            {
                while (reader.Read())
                {
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        Console.Write($"{reader.GetName(i)}: {reader[i]}\t");
                    }
                    Console.Write(" euros");
                    Console.WriteLine();
                }
            }
            reader.Close();
        }

        public static void NatioPlats(MySqlConnection conn, string nationalite, int idClient)
        {
            string query = "SELECT p.NomPlat AS Nom, p.Regime, p.Ingredients, p.PrixPlat as Prix FROM Plat p " +
                           "JOIN PlatCommande pc ON p.IDPlat = pc.IDPlat " +
                           "JOIN Commande c ON pc.IDCommande = c.IDCommande " +
                          $"WHERE c.IDClient = {idClient} " +
                          $"AND p.Nationalite = '{nationalite}';";

            MySqlCommand cmd = new MySqlCommand(query, conn);
            MySqlDataReader reader = cmd.ExecuteReader();
            using (reader)
            {
                while (reader.Read())
                {
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        Console.Write($"{reader.GetName(i)}: {reader[i]}\t");
                    }
                    Console.Write(" euros");
                    Console.WriteLine();
                }
            }
            reader.Close();
        }


        #endregion

    }
}
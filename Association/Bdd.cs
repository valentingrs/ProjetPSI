using MySql.Data.MySqlClient;
using MySqlX.XDevAPI.Relational;
using SkiaSharp;
using System.ComponentModel;
using System.Data.Common;
using System.Net;
using System.Reflection.Metadata.Ecma335;
using System.Transactions;

namespace Association
{
    internal class Bdd
    {

        #region M�thodes g�n�rales
        public static bool Existe(MySqlConnection conn, string Table, string AttributElement, object element)
        {
            bool ElementExiste = false;

            // Nombre de fois qu'existe l'attribut avec cette valeur
            string query = $"SELECT COUNT(*) FROM {Table} WHERE {AttributElement} = @element;";

            try
            {
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    // V�rifie si l'�l�ment est un nombre ou une cha�ne de caract�res
                    if (element is int || element is long)
                        cmd.Parameters.AddWithValue("@element", Convert.ToInt64(element));
                    else
                        cmd.Parameters.AddWithValue("@element", element.ToString());

                    // Ex�cute la requ�te et v�rifie le nombre d'occurrences
                    int count = Convert.ToInt32(cmd.ExecuteScalar());
                    ElementExiste = (count > 0);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erreur lors de la v�rification de l'existence : " + ex.Message);
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

                if (ligne == 0) { Console.WriteLine("Aucun r�sultat."); }
                reader.Close();
            }
            catch (MySqlException e)
            {
                Console.WriteLine($"Erreur MySQL : {e.Message}");
            }
        }

        #endregion
        public static bool VerfierConnexion(MySqlConnection conn, string email, string mdp)
        {
            bool connecte = false;
            try
            {
                string query = "SELECT MotDePasse FROM Tiers WHERE email = '" + email + "';";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                MySqlDataReader reader = cmd.ExecuteReader();

                using (reader)
                {
                    if (reader.Read()) /// car au plus un �l�ment (cl� primaire) dans le reader donc pas probl�matique
                    {
                        string motdepasse = (string)reader["MotDePasse"];
                        if (mdp == motdepasse) { connecte = true; }
                        else { Console.WriteLine("Mot de passe faux"); }
                    }
                    else { Console.WriteLine("Pas de compte existant avec cet email."); }
                }
            }
            catch (MySqlException e) { Console.WriteLine($"Erreur MySQL : {e.Message}"); }
            return connecte;
       
        }

        static public int RecupIdMail(MySqlConnection conn, string email)
        {
            int id = 0;
            try
            {
                string queryId = "SELECT IDTiers FROM Tiers WHERE Email = '" + email + "';";
                MySqlCommand cmd = new MySqlCommand(queryId, conn); MySqlDataReader reader = cmd.ExecuteReader();
                using (reader)
                {
                    while (reader.Read())
                    {
                        id = (int)reader["IDTiers"];
                    }
                }
            }
            catch (MySqlException e) { Console.WriteLine($"Erreur MySQL : {e.Message}"); }
            return id;
        }

        #region Tiers
        public static void CreerUnCompte(MySqlConnection conn, int IDTiers, string mdp, string CodeP, string Ville, string Email, string Tel, string Nom, string Adresse, string Prenom)
        {

            if (Existe(conn, "Tiers", "IDTiers", IDTiers)) { Console.WriteLine("Compte d�j� existant !"); return; }
            else
            {
                try
                {
                    string query = $"INSERT INTO Tiers (IDTiers, MotDePasse, CodePostal, Ville, Email, Tel, Nom, Adresse, Prenom) " +
                                   $"VALUES ({IDTiers}, '{mdp}', '{CodeP}', '{Ville}', '{Email}', '{Tel}', '{Nom}', '{Adresse}', '{Prenom}');";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.ExecuteNonQuery();
                        Console.WriteLine("Compte cr�� avec succ�s !");
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
                    Console.WriteLine("Votre modification a bien �t� prise en compte.");
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
                    Console.WriteLine("Compte supprim� avec succ�s !");
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
                        Console.WriteLine("Vous �tes un client !");
                    }
                }
                catch (MySqlException e)
                {
                    Console.WriteLine($"Erreur MySQL : {e.Message}");
                }
            }
            else
            {
                Console.WriteLine("Vous n'avez pas pu �tre ajout� � la table 'Client'.");
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
                    Console.WriteLine("Client supprim� avec succ�s !");
                }
            }
            catch (MySqlException e)
            {
                Console.WriteLine($"Erreur MySQL : {e.Message}");
            }
        }

        public static void AffichageClientsSql(MySqlConnection conn, string donnees, bool croissant)
        {
            string query = "SELECT Tiers.Prenom, Tiers.Nom, Tiers.Adresse FROM Client " +
                           "JOIN Tiers ON Client.IDClient = Tiers.IDTiers " + "ORDER BY ";

            if (donnees == "Ordre alpha") { query += "Nom ASC, Prenom ASC;"; }
            else if (donnees == "Rue") { query += "Adresse ASC;"; }
            else if (donnees == "Achats")
            {
                query = "SELECT Tiers.Nom, Tiers.Prenom, , Tiers.Adresse, SUM(Plat.PrixPlat) AS MontantTotalAchats " +
                        "FROM  Client " +
                        "JOIN Tiers ON Client.IDClient = Tiers.IDTiers " +
                        "JOIN Commande ON Client.IDClient = Commande.IDClient " +
                        "JOIN PlatCommande ON Commande.IDCommande = PlatCommande.IDCommande " +
                        "JOIN Plat ON PlatCommande.IDPlat = Plat.IDPlat GROUP BY Tiers.IDTiers, Tiers.Nom, Tiers.Prenom " +
                        "ORDER BY MontantTotalAchats DESC;";
            }
            else { Console.WriteLine("Mauvais param�tre entr�"); return; }
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
            reader.Close(); /// fermer le reader car il ne peut y avoir qu'un reader ouvert � la fois
            if (ligne == 0)
                Console.WriteLine("Aucun r�sultat.");
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
                        Console.WriteLine("Vous �tes maintenant cuisinier !");
                    }
                }
                catch (MySqlException e)
                {
                    Console.WriteLine($"Erreur MySQL : {e.Message}");
                }
            }
            else
            {
                Console.WriteLine("Vous n'avez pas pu �tre ajout� � la table 'Cuisinier'.");
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
                    Console.WriteLine("Cuisinier supprim� avec succ�s !");
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
                string Formatfabrication = DateFabrication.ToString("yyyy-MM-dd"); /// formatage du typeDateTime pour qu'il convienne au type DATE de Sql
                string Formatperemption = DatePeremption.ToString("yyyy-MM-dd");
                string prix = PrixPlat.ToString().Replace(",", "."); /// formatage du prix pour qu'il convienne � Sql
                try
                {
                    if (!Existe(conn, "Plat", "IDPlat", IDPlat))
                    {
                        string query = $"INSERT INTO Plat VALUES ({IDPlat}, '{nomPlat}', '{TypePlat}', '{Ingredients}', '{Nationalite}', '{Regime}');";
                        using (MySqlCommand cmd = new MySqlCommand(query, conn))
                        {
                            cmd.ExecuteNonQuery();
                            Console.WriteLine("Plat ajout� avec succ�s !");
                        }
                    }

                    
                    Random random = new Random();
                    int idPlatCuisinier = random.Next(1, 1001);
                    while (Existe(conn, "PlatCuisinier", "IDPlatCuisinier", idPlatCuisinier))
                    {
                        idPlatCuisinier = random.Next(1, 1001);
                    }
                    string query2 = $"INSERT INTO PlatCuisinier " +
                                   $"VALUES ({idPlatCuisinier}, {IDPlat}, '{Formatfabrication}', '{Formatperemption}', '{Ingredients}', '{prix}', '{NombrePersonnes}', '{idCuisinier}');";

                    using (MySqlCommand cmd = new MySqlCommand(query2, conn))
                    {
                        cmd.ExecuteNonQuery();
                        Console.WriteLine("Plat ajout� avec succ�s !");
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

                if (ligne == 0) { Console.WriteLine("Aucun r�sultat."); }
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
                    query = "SELECT Plat.NomPlat AS Plat, COUNT(PlatCommande.IDCommande) AS NombreCommandes FROM PlatCuisinier " +
                               "JOIN Plat ON PlatCuisinier.IDPlat = Plat.IDPlat " +
                               "LEFT JOIN PlatCommande ON PlatCommande.IDPlatCuisinier = PlatCuisinier.IDPlatCuisinier " +
                               $"WHERE PlatCuisinier.IDCuisinier = {id} " +
                               "GROUP BY Plat.NomPlat;";
                }
                if (param == "PlatDuJour")
                {
                    query = "SELECT Plat.IDPlat AS n�, Plat.NomPlat AS Nom FROM PlatCuisinier pc " +
                            "JOIN Plat ON pc.IDPlat = Plat.IDPlat " +
                            $"WHERE pc.IDCuisinier = {id} AND pc.IDPlatCuisinier NOT IN " +
                            "(SELECT IDPlatCuisinier FROM PlatCommande);";
                }
                if (param == "InfosPlat")
                {
                    query = $"SELECT * FROM Plat WHERE Plat.IDPlat = {id}";
                }
                MySqlCommand cmd = new MySqlCommand(query, conn);
                MySqlDataReader reader = cmd.ExecuteReader();
                using (reader)
                {
                    // En-t�te
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        Console.Write($"{reader.GetName(i),-20}");
                    }
                    Console.WriteLine();
                    Console.WriteLine(new string('-', 20 * reader.FieldCount));

                    // Donn�es
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

        public static bool CuisinierValide(MySqlConnection conn, int idCuisinier)
        {
            try
            {
                string query = "SELECT DISTINCT pc.IDCuisinier\r\nFROM PlatCuisinier pc\r\nLEFT JOIN PlatCommande pcom ON pc.IDPlatCuisinier = pcom.IDPlatCuisinier\r\nWHERE pcom.IDPlatCuisinier IS NULL;";
                MySqlCommand cmd = new MySqlCommand(query, conn);

                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int id = reader.GetInt32("IDCuisinier");
                        if (id == idCuisinier)
                            return true;
                    }
                    return false;
                }
            }
            catch (MySqlException e)
            {
                Console.WriteLine($"Erreur MySQL : {e.Message}");
                return false;
            }
        }

        public static bool PlatCuisinierValide(MySqlConnection conn, int idCuisinier, int idPlatCuisinier)
        {
            try
            {
                string query = $"SELECT COUNT(*) FROM PlatCuisinier WHERE IDCuisinier = {idCuisinier} AND IDPlatCuisinier = {idPlatCuisinier};";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                
                int count = Convert.ToInt32(cmd.ExecuteScalar()); /// en th�orie 1
                return count > 0;

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
                        Console.WriteLine("Commande ajout�e avec succ�s !");
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
                string query = "SELECT p.NomPlat AS Plat, p.Ingredients, pc.IDPlatCuisinier AS IDPlat, t.Prenom, t.Nom, t.IDTiers AS Identifiant, t.CodePostal AS Arrondissement " +
                               "FROM PlatCuisinier pc " +
                               "JOIN Plat p ON pc.IDPlat = p.IDPlat " +
                               "JOIN Cuisinier c ON pc.IDCUisinier = c.IDCuisinier " +
                               "JOIN Tiers t ON c.IDCuisinier = t.IDTiers " +
                               "WHERE pc.IDPlatCuisinier NOT IN (SELECT IDPlatCuisinier FROM PlatCommande);";

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
                            reader["Arrondissement"]);
                    }
                }
            }
            catch (MySqlException e)
            {
                Console.WriteLine($"Erreur MySQL : {e.Message}");
            }
        }

        public static void CommandePlat(MySqlConnection conn, int idCommande, int idPlatCuisinier)
        {
            try
            {
                string query = $"INSERT INTO PlatCommande (IDCommande, IDPlatCuisinier) " +
                               $"VALUES ({idCommande}, {idPlatCuisinier});";
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.ExecuteNonQuery();
                    Console.WriteLine("Plat ajout� la commande n�" + idCommande + " avec succ�s !");
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

            string query = $"SELECT SUM(pc.PrixPlat) AS PrixTotal FROM PlatCommande c JOIN PlatCuisinier pc ON c.IDPlatCuisinier = pc.IDPlatCuisinier WHERE c.IDCommande = {idCommande};";

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
                            "AS NombreDeCommandes FROM Commande " +
                            "JOIN Tiers ON Commande.IDCuisinier = Tiers.IDTiers " +
                            "GROUP BY Tiers.IDTiers, Tiers.Prenom, Tiers.Nom " +
                            "ORDER BY NombreDeCommandes DESC;";

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

        public static void AfficherCommandes(MySqlConnection conn, DateTime debut, DateTime fin, string duree, int? idClient=null) // param�tre idClient facultatif
        {
            string query = "SELECT c.IDCommande, c.DateCommande, c.HeureCommande, c.IDClient, c.IDCuisinier FROM Commande c ";


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

            if (idClient.HasValue) // param�tre suppl�mentaire si on veut la requ�te pour qu'un client pr�cis
            {
                if (duree == "Toujours") { query += $" WHERE c.IDClient = {idClient};"; }
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
                        object value = reader[i];

                        if (value is DateTime dt)
                        {
                            /// Affiche uniquement la date (sans l'heure) si c�est DateCommande
                            if (reader.GetName(i) == "DateCommande")
                                Console.Write($"{reader.GetName(i)}: {dt.ToString("dd/MM/yyyy")}\t");
                            else if (reader.GetName(i) == "HeureCommande")
                                Console.Write($"{reader.GetName(i)}: {dt.ToString("HH:mm")}\t");
                            else
                                Console.Write($"{reader.GetName(i)}: {dt}\t");
                        }
                        else
                        {
                            Console.Write($"{reader.GetName(i)}: {value}\t");
                        }
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
                query = "SELECT AVG(PrixTotal) AS Prix FROM (SELECT pc.IDCommande, SUM(pcui.PrixPlat) AS PrixTotal " +
                        "FROM PlatCommande pc " +
                        "JOIN PlatCuisinier pcui ON pc.IDPlatCuisinier = pcui.IDPlatCuisinier " +
                        "GROUP BY pc.IDCommande) AS Moyenne;";
            }
            if (param == "MoyenneClients")
            {
                query = "SELECT AVG(MontantTotalAchats) AS Moyenne FROM " +
                        "(SELECT SUM(pcui.PrixPlat) AS MontantTotalAchats, c.IDClient FROM Client c " +
                        "JOIN Commande com ON c.IDClient = com.IDClient " +
                        "JOIN PlatCommande pc ON com.IDCommande = pc.IDCommande " +
                        "JOIN PlatCuisinier pcui ON pc.IDPlatCuisinier = pcui.IDPlatCuisinier " +
                        "GROUP BY c.IDClient) AS MoyennneComptesClient;";
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
                    Console.WriteLine(" euros");
                }
            }
            reader.Close();
        }

        public static void NatioPlats(MySqlConnection conn, string nationalite, int idClient)
        {
            string query = "SELECT pl.NomPlat AS Nom, pl.Regime, pl.Ingredients, pcui.PrixPlat as Prix FROM PlatCommande pc " +
                           "JOIN PlatCuisinier pcui ON pc.IDPlatCuisinier = pcui.IDPlatCuisinier " +           
                           "JOIN Plat pl ON pcui.IDPlat = pl.IDPlat " +
                           "JOIN Commande c ON pc.IDCommande = c.IDCommande " +
                          $"WHERE c.IDClient = {idClient} " +
                          $"AND pl.Nationalite = '{nationalite}';";

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
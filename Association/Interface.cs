using static Association.GrapheStation;
using static Association.Bdd;
using MySql.Data.MySqlClient;
using System.Text;

namespace Association
{
	internal class Interface
	{
		public static void GererInterface(MySqlConnection conn)
		{
			Console.Clear();
			Menu();
			ChoixModule(conn);
		}

		static void Menu()
		{
			Console.WriteLine("LIVIN PARIS\n\n");
			Console.WriteLine("-> Module Tiers\n-> Module Client\n-> Module Cuisinier\n-> Module Commande\n-> Module Statistiques\n");
		}

        static void InterfaceModifierCompte(MySqlConnection conn, string type)
        {
            Console.Write("\nModfier un compte ");
            if (type == "Client") { Console.WriteLine("client : "); }
            if (type == "Cuisinier") { Console.WriteLine("cuisinier : "); }
            Console.Write("Entrer l'id du compte à modiifer : "); int idTiers = Convert.ToInt32(Console.ReadLine());

            if (!Existe(conn, type, "ID"+type, idTiers)) { Console.WriteLine("Compte " + type + " inexistant !"); }
            else
            {
                Console.Write("Entrer la composante à modifier : nom, prénom, adresse, téléphone, code postal, ville ? ");
                string colonne = Console.ReadLine().Trim().ToLower().Replace("é", "e"); // enlever majuscules, espaces, accents
                Console.Write("Entrer la nouvelle composante modifée : ");
                string modifiee = Console.ReadLine();
                Console.WriteLine(colonne);
                switch (colonne)
                {
                    case "nom":
                        ModifierCompte(conn, "Nom", modifiee, idTiers, "Tiers");
                        break;
                    case "prenom":
                        ModifierCompte(conn, "Prenom", modifiee, idTiers, "Tiers");
                        break;
                    case "adresse":
                        ModifierCompte(conn, "Adresse", modifiee, idTiers, "Tiers");
                        break;
                    case "telephone":
                        ModifierCompte(conn, "Tel", modifiee, idTiers, "Tiers");
                        break;
                    case "code postal":
                        ModifierCompte(conn, "CodePostal", modifiee, idTiers, "Tiers");
                        break;
                    case "ville":
                        ModifierCompte(conn, "Ville", modifiee, idTiers, "Tiers");
                        break;
                    default:
                        Console.WriteLine("Paramètre rentré invalide");
                        break;

                }
            }
        }

		static void ChoixModule(MySqlConnection conn)
		{
			Console.Write("Choisir un module : ");
			string mod = (Console.ReadLine()).ToLower().Trim();
			while (mod != "tiers" && mod != "client" && mod != "cuisinier" && mod != "commande" && mod != "statistiques")
			{
				Console.WriteLine("Rentrer un nom valide de module");
				Console.Write("Choisir un module : ");
				mod = (Console.ReadLine()).ToLower().Trim();
			}
			
			switch (mod)
			{
				case "tiers":
					ModuleTiers(conn); break;
				case "client":
					ModuleClient(conn); break;
				case "cuisinier":
					ModuleCuisinier(conn); break;
				case "commande":
					ModuleCommande(conn); break;
				case "statistiques":
					ModuleStatistiques(conn); break;
			}
		}

		static void RetourMenu(MySqlConnection conn)
		{
			GererInterface(conn);
		}

		static void ModuleTiers(MySqlConnection conn)
		{
            Console.Clear(); Console.WriteLine("Bienvenue sur le module tiers\n");

            Console.WriteLine("Que voulez-faire ? Selectionner le numéro assoicé à l'option ou entrer 'r' pour retourner en arrière: ");
            Console.WriteLine("1 - Ajouter un compte");
			Console.WriteLine("2 - Supprimer un compte");
            Console.WriteLine("3 - Modifier un compte");
            Console.Write("\nChoix : ");
            string choix = Console.ReadLine();
            while (choix != "1" && choix != "2" && choix != "3" && choix != "r") { Console.Write("\nRentre un choix valide : "); choix = Console.ReadLine(); }
			while (choix != "r")
			{
                if (choix == "1")
                {
                    Console.WriteLine("\nCréation d'un compte : ");
                    Console.Write("Entrer un id : "); int idTiers = Convert.ToInt32(Console.ReadLine());
                    if (Existe(conn, "Tiers", "IDTiers", idTiers)) { Console.WriteLine("Compte déjà existant !");}
                    else
                    {
                        Console.Write("Entrer un nom : "); string nom = Console.ReadLine(); nom = nom.Replace("'", "''"); // pour éviter les conflits avec les apostrophes dans les données
                        Console.Write("Entrer un prénom : "); string prenom = Console.ReadLine();
                        Console.Write("Entrer un code postal : "); string cp = Console.ReadLine();
                        Console.Write("Entrer une ville : "); string ville = Console.ReadLine(); ville = ville.Replace("'", "''");
                        Console.Write("Entrer une adresse : "); string adresse = Console.ReadLine(); adresse = adresse.Replace("'", "''");
                        Console.Write("Entrer une adresse mail: "); string email = Console.ReadLine();
                        Console.Write("Entrer un numéro de téléphone : "); string tel = Console.ReadLine();
                        CreerUnCompte(conn, idTiers, cp, ville, email, tel, nom, adresse, prenom);
                    }
                }

				if (choix == "2")
				{
					Console.WriteLine("\nSupprimer un compte : ");
					Console.WriteLine("Entrer l'id du compte : "); int idTiers = Convert.ToInt32(Console.ReadLine());
                    if (!Existe(conn, "Tiers", "IDTiers", idTiers)) { Console.WriteLine("Compte inexistant !"); }
                    else { SupprimerUnCompte(conn, "Tiers", idTiers); }

                }
                
                if (choix == "3")
                {
                    InterfaceModifierCompte(conn, "Tiers");
                }
                Console.Write("\nChoix : ");
                choix = Console.ReadLine();
                while (choix != "1" && choix != "2" && choix != "3" && choix != "r") { Console.Write("\nRentrer un choix valide : "); choix = Console.ReadLine(); }
            }
            RetourMenu(conn);
        }

		static void ModuleClient(MySqlConnection conn)
		{
			Console.Clear(); Console.WriteLine("Bienvenue sur le module client\n");

			Console.WriteLine("Que voulez-faire ? Selectionner le numéro assoicé à l'option ou entrer 'r' pour retourner en arrière: ");
			Console.WriteLine("1 - Ajouter un client");
            Console.WriteLine("2 - Supprimer un client");
            Console.WriteLine("3 - Afficher les clients");
            Console.WriteLine("4 - Modifier un client");
            Console.Write("\nChoix : ");
			string choix = Console.ReadLine();
			while (choix != "1" && choix != "2" && choix != "3" && choix != "4" && choix != "r") { Console.Write("\nRentre un choix valide : "); choix = Console.ReadLine(); }

            while (choix != "r")
            {
                if (choix == "1")
                {
                    Console.WriteLine("\nCréation d'un client : ");
                    Console.Write("Entrer l'id du compte Tiers : "); int idClient = Convert.ToInt32(Console.ReadLine());
                    bool clientExiste = Existe(conn, "Client", "IDClient", idClient);
                    bool tiersExiste = Existe(conn, "Tiers", "IDTiers", idClient);
                    while (tiersExiste == false || clientExiste == true)
                    {
                        Console.WriteLine("Compte tiers inexistant ou compte client déjà existant, rentrer un nouvel id");
                        Console.Write("Entrer l'id du compte Tiers : "); idClient = Convert.ToInt32(Console.ReadLine());
                        tiersExiste = Existe(conn, "Tiers", "IDTiers", idClient);
                        clientExiste = Existe(conn, "Client", "IDClient", idClient);
                    }
                    AjouterClient(true, idClient, conn);
                    
                }

                if (choix == "2")
                {
                    Console.WriteLine("\nSuppression d'un client : ");
                    Console.Write("Entrer l'id du compte Tiers pour supprimer : "); int idClient = Convert.ToInt32(Console.ReadLine());

                    bool clientExiste = Existe(conn, "Client", "IDClient", idClient);
                    bool tiersExiste = Existe(conn, "Tiers", "IDTiers", idClient);
                    if (clientExiste == false || tiersExiste == false)
                    {
                        Console.WriteLine("Compte tiers inexistant ou compte client inexistant");
                       
                    }
                    else
                    {
                        SupprimerClient(conn, "Client", idClient);

                    }
                }

                if (choix == "3")
                {
                    Console.WriteLine(""); AffichageClients(conn);
                }

                if (choix == "4")
                {
                    InterfaceModifierCompte(conn, "Client");
                }

                Console.WriteLine("\nQue voulez-faire ? Selectionner le numéro assoicé à l'option ou entrer 'r' pour retourner en arrière: ");
                Console.Write("Choix : ");
                choix = Console.ReadLine();
                while (choix != "1" && choix != "2" && choix != "3" && choix != "4" && choix != "r") { Console.Write("\nRentre un choix valide : "); choix = Console.ReadLine(); }
            }
            RetourMenu(conn);
        }

        static void AffichageClients(MySqlConnection conn)
        {
            Console.WriteLine("\nSelon quel ordre afficher les clients (entrer le numéro) : ");
            Console.WriteLine("1 - Ordre alphabétique");
            Console.WriteLine("2 - Rue");
            Console.WriteLine("3 - Montant des achats cumulés");
            Console.Write("Choix : "); int choix = Convert.ToInt32(Console.ReadLine());
            while (choix > 3 || choix < 1) { Console.Write("Rentrer un choix valide : "); choix = Convert.ToInt32(Console.ReadLine()); }
            switch (choix)
            {
                case 1:
                    Console.WriteLine("Par ordre alphabétique : \n");
                    AffichageClientsSql(conn, "Ordre alpha", true);
                    break;
                case 2:
                    Console.WriteLine("Par rue");
                    AffichageClientsSql(conn, "Rue", true);
                    break;
                case 3:
                    Console.WriteLine("Par montant des achats cumulés");
                    AffichageClientsSql(conn, "Achats", true);
                    break;
                default:
                    break;
            }
        }


        static void ModuleCuisinier(MySqlConnection conn)
		{
            Console.Clear(); Console.WriteLine("Bienvenue sur le module cuisinier\n");

            Console.WriteLine("Que voulez-faire ? Selectionner le numéro assoicé à l'option ou entrer 'r' pour retourner en arrière: ");
            Console.WriteLine("1 - Ajouter un cuisinier");
            Console.WriteLine("2 - Supprimer un cuisinier");
			Console.WriteLine("3 - En tant que cuisinier, ajouter un plat");
            Console.WriteLine("4 - Modifier un cuisinier");
            Console.WriteLine("5 - Afficher les données d'un cuisinier");
            Console.Write("\nChoix : ");
            string choix = Console.ReadLine();
            while (choix != "1" && choix != "2" && choix != "3" && choix != "4" && 
                choix != "5" && choix != "r") { Console.Write("\nRentre un choix valide : "); choix = Console.ReadLine(); }

            while (choix != "r")
            {
                if (choix == "1")
                {
                    Console.WriteLine("\nCréation d'un cuisinier : ");
                    Console.Write("Entrer l'id du compte Tiers : "); int id = Convert.ToInt32(Console.ReadLine());

                    bool cuisinierExiste = Existe(conn, "Cuisinier", "IDCuisinier", id);
                    bool tiersExiste = Existe(conn, "Tiers", "IDTiers", id);
                    while (tiersExiste == false || cuisinierExiste == true)
                    {
                        Console.WriteLine("Compte tiers inexistant ou compte cuisinier déjà existant, rentrer un nouvel id");
                        Console.Write("Entrer l'id du compte Tiers : "); id = Convert.ToInt32(Console.ReadLine());
                        tiersExiste = Existe(conn, "Tiers", "IDTiers", id);
                        cuisinierExiste = Existe(conn, "Cuisinier", "IDCuisinier", id);
                    }
                    AjouterCuisinier(true, id, conn);

                }

                if (choix == "2")
                {
                    Console.WriteLine("\nSuppression d'un cuisinier : ");
                    Console.Write("Entrer l'id du compte Tiers pour supprimer : "); int id = Convert.ToInt32(Console.ReadLine());

                    bool cuisinierExiste = Existe(conn, "Cuisinier", "IDCuisinier", id);
                    bool tiersExiste = Existe(conn, "Tiers", "IDTiers", id);

                    if (cuisinierExiste == false) { Console.WriteLine("Cusinier inexistant"); }
                    else if (tiersExiste == false) { Console.WriteLine("Tiers inexistant"); }
                    else
                    {
                        SupprimerCuisinier(conn, "Cuisinier", id);
                    }
                    
                }

                if (choix == "3")
                {
                    Console.WriteLine("\nConfection d'un plat : ");
                    Console.Write("Entrer l'id du compte cuisinier: "); string console = Console.ReadLine();
                    if (console == "r") { RetourMenu(conn); break; return; }
                    int id = Convert.ToInt32(console);

                    bool cuisinierExiste = Existe(conn, "Cuisinier", "IDCuisinier", id);
                    while (cuisinierExiste == false)
                    {
                        Console.WriteLine("Compte cuisinier inexistant, rentrer un nouvel id");
                        Console.Write("Entrer l'id du compte Cuisinier : "); id = Convert.ToInt32(Console.ReadLine());
                        cuisinierExiste = Existe(conn, "Cuisinier", "IDCuisinier", id);
                    }
                    Console.WriteLine("");
                    ModulePlat(conn, id);
                }

                if (choix == "4")
                {
                    InterfaceModifierCompte(conn, "Cuisinier");
                }

                if (choix == "5")
                {
                    Console.Write("Entrer l'id du Cuisinier dont vous voulez afficher les infos : ");
                    int idCuisinier = Convert.ToInt32(Console.ReadLine());
                    while (!Existe(conn, "Cuisinier", "IDCuisinier", idCuisinier)) 
                    {
                        Console.Write("Entrer un id valide : ");
                        idCuisinier = Convert.ToInt32(Console.ReadLine());
                    }
                    ChoixAffichageCuisinier(conn, idCuisinier);

                }

            }
            RetourMenu(conn);
        }

        public static void ChoixAffichageCuisinier(MySqlConnection conn, int idCuisinier)
        {
            Console.WriteLine("Quelle information voulez-vous ? Séléctionner le numéro associé");
            Console.WriteLine("1 - Clients servis");

            Console.Write("\nChoix : ");
            string choix = Console.ReadLine();
            while (choix != "1" ) { Console.Write("\nRentre un choix valide : "); choix = Console.ReadLine(); }
            if (choix == "1")
            {
                Console.Write("Depuis une date précise ? O ou N (N par défaut) : ");
                string date = Console.ReadLine().Trim().ToLower();
                if (date == "o")
                {
                    Console.Write("Entrer une date au format yyyy-mm-jj : ");
                    date = Console.ReadLine();
                    ClientsServisCuisinier(conn, true, date, idCuisinier);
                }
                else
                {
                    ClientsServisCuisinier(conn, false, "", idCuisinier);
                }
            }
        }
        static void ModulePlat(MySqlConnection conn, int idCuisinier)
        {
            Console.WriteLine("Ajout d'un plat");
            Console.Write("Id du plat : "); int idplat = Convert.ToInt32(Console.ReadLine());
            Console.Write("Nom du plat : "); string nomPlat = Console.ReadLine();
            Console.Write("Type du plat : "); string type = Console.ReadLine();
            Console.Write("Date de fabrication du plat (JJ-MM-AAAA) : "); DateTime fabrication = Convert.ToDateTime(Console.ReadLine());
            Console.Write("Date de peremption du plat (JJ-MM-AAAA) : "); DateTime peremption = Convert.ToDateTime(Console.ReadLine());
            Console.Write("nationalité du plat : "); string natio = Console.ReadLine();
            Console.Write("ingredients du plat : "); string ingredients = Console.ReadLine();
            Console.Write("regime du plat : "); string regime = Console.ReadLine();
            Console.Write("prix du plat (avec une virgule et pas un point) : "); string prixLu = Console.ReadLine(); prixLu.Replace(".", ","); double prix = Convert.ToDouble(prixLu);
            Console.Write("Nombre de personnes pour le plat : "); int nbPersonnes = Convert.ToInt32(Console.ReadLine());

            FairePlat(true, conn, idplat, type, nomPlat, fabrication, peremption, natio, regime, ingredients, prix, nbPersonnes, idCuisinier);
        }

        static void ModuleCommande(MySqlConnection conn)
		{
			Console.Clear(); Console.WriteLine("Bienvenue sur le module commande\nOptions : \n");
            Console.WriteLine("1 - Faire une commande");

			Console.WriteLine("Pour choisir une option, rentrer le numéro au début de l'option");
			Console.WriteLine("Pour retourner au menu principal, entrer 'r'");
			Console.Write("\nChoix : ");
			
			string choix = Console.ReadLine();

			while (choix != "1" && choix != "r") 
			{
				Console.Write("\nChoix invalide, rentrer un nouveau choix : ");
				choix = Console.ReadLine();
			}
			
			while (choix != "r")
			{
                if (choix == "1")
                {
                    Console.WriteLine("Ajout d'une commande : ");
                    Console.Write("Entrer un id de commande : "); int idCommande = Convert.ToInt32(Console.ReadLine());
                    Console.Write("Entrer une date de commande : "); DateTime date = Convert.ToDateTime(Console.ReadLine());
                    Console.Write("Entrer une heure de commande : "); DateTime heure = Convert.ToDateTime(Console.ReadLine());
                    Console.Write("Entrer un id de client : "); int idClient = Convert.ToInt32(Console.ReadLine());
                    Console.Write("Entrer un id de cuisinier : "); int idCuisinier = Convert.ToInt32(Console.ReadLine());
                    Console.Write("Entrer un id de plat : "); int idPlat = Convert.ToInt32(Console.ReadLine());
                    FaireUneCommande(true, conn, idCommande, date, heure, idClient, idCuisinier);
                    bool valide = PlatCuisinierValide(conn, idCuisinier, idPlat);

                    if (valide == false) { Console.WriteLine("Ce plat n'est pas disponible pour ce cuisinier"); }
                    else
                    {
                        CommandePlat(conn, idCommande, idPlat);
                    }

                    Console.Write("Voulez-vous ajouter d'autres plats à la commande (oui 'O' ou non 'N') ? "); string rep = Console.ReadLine().ToUpper();
                    while (rep == "O")
                    {
                        Console.Write("\nEntrer un id de plat : "); idPlat = Convert.ToInt32(Console.ReadLine());
                        bool PlatDejaCommande = Existe(conn, "PlatCommande", "IDPlat", idPlat);
                        valide = PlatCuisinierValide(conn, idCuisinier, idPlat);
                        if (PlatDejaCommande) { Console.WriteLine("Ce plat a déjà été commandé, il n'est plus disponible malhereusement :("); }
                        else if (valide == false) { Console.WriteLine("Ce plat n'est pas disponible pour ce cuisinier"); }

                        else { CommandePlat(conn, idCommande, idPlat); }
                        Console.Write("Voulez-vous ajouter d'autres plats à la commande (oui 'O' ou non 'N') ? "); rep = Console.ReadLine().ToUpper();
                    }

                    Console.WriteLine("Commande terminée, on peut passer à la transaction : ");
                    Console.WriteLine("Prix de la commande : " + CalculerPrixCommande(idCommande, conn) + " euros");
                    Console.WriteLine("\nPour faciliter la livraison, donner les stations de métro (attention à l'orthographe et mettre des espaces entre les tirets) les plus proches du : ");
                    Console.Write("Client : "); string station1 = Console.ReadLine();
                    Console.Write("Cuisinier : "); string station2 = Console.ReadLine();
                    MetroParis(station1, station2);
                }

                Console.Write("\nChoix : ");
				choix = Console.ReadLine();

				while (choix != "1" && choix != "r")
				{
					Console.Write("\nChoix invalide, rentrer un nouveau choix : ");
					choix = Console.ReadLine();
				}

			}
			RetourMenu(conn); 

			//RetourMenu();
		}

		static void ModuleStatistiques(MySqlConnection conn)
		{
			Console.Clear(); Console.WriteLine("Bienvenue sur le module statistiques");
			RetourMenu(conn);
		}
	}
}
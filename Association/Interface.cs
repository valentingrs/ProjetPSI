using static Association.GrapheStation;
using static Association.Bdd;
using MySql.Data.MySqlClient;
using System.Text;
using System.Security.Cryptography.X509Certificates;

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
            Console.Write("Entrer l'id du compte � modiifer : "); int idTiers = Convert.ToInt32(Console.ReadLine());

            if (!Existe(conn, type, "ID"+type, idTiers)) { Console.WriteLine("Compte " + type + " inexistant !"); }
            else
            {
                Console.Write("Entrer la composante � modifier : nom, pr�nom, adresse, t�l�phone, code postal, ville ? ");
                string colonne = Console.ReadLine().Trim().ToLower().Replace("�", "e"); // enlever majuscules, espaces, accents
                Console.Write("Entrer la nouvelle composante modif�e : ");
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
                        Console.WriteLine("Param�tre rentr� invalide");
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

            Console.WriteLine("Que voulez-faire ? Selectionner le num�ro assoic� � l'option ou entrer 'r' pour retourner en arri�re: ");
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
                    Console.WriteLine("\nCr�ation d'un compte : ");
                    Console.Write("Entrer un id : "); int idTiers = Convert.ToInt32(Console.ReadLine());
                    if (Existe(conn, "Tiers", "IDTiers", idTiers)) { Console.WriteLine("Compte d�j� existant !");}
                    else
                    {
                        Console.Write("Entrer un nom : "); string nom = Console.ReadLine(); nom = nom.Replace("'", "''"); // pour �viter les conflits avec les apostrophes dans les donn�es
                        Console.Write("Entrer un pr�nom : "); string prenom = Console.ReadLine();
                        Console.Write("Entrer un code postal : "); string cp = Console.ReadLine();
                        Console.Write("Entrer une ville : "); string ville = Console.ReadLine(); ville = ville.Replace("'", "''");
                        Console.Write("Entrer une adresse : "); string adresse = Console.ReadLine(); adresse = adresse.Replace("'", "''");
                        Console.Write("Entrer une adresse mail: "); string email = Console.ReadLine();
                        Console.Write("Entrer un num�ro de t�l�phone : "); string tel = Console.ReadLine();
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

			Console.WriteLine("Que voulez-faire ? Selectionner le num�ro assoic� � l'option ou entrer 'r' pour retourner en arri�re: ");
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
                    Console.WriteLine("\nCr�ation d'un client : ");
                    Console.Write("Entrer l'id du compte Tiers : "); int idClient = Convert.ToInt32(Console.ReadLine());
                    bool clientExiste = Existe(conn, "Client", "IDClient", idClient);
                    bool tiersExiste = Existe(conn, "Tiers", "IDTiers", idClient);
                    while (tiersExiste == false || clientExiste == true)
                    {
                        Console.WriteLine("Compte tiers inexistant ou compte client d�j� existant, rentrer un nouvel id");
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

                Console.WriteLine("\nQue voulez-faire ? Selectionner le num�ro assoic� � l'option ou entrer 'r' pour retourner en arri�re: ");
                Console.Write("Choix : ");
                choix = Console.ReadLine();
                while (choix != "1" && choix != "2" && choix != "3" && choix != "4" && choix != "r") { Console.Write("\nRentre un choix valide : "); choix = Console.ReadLine(); }
            }
            RetourMenu(conn);
        }

        static void AffichageClients(MySqlConnection conn)
        {
            Console.WriteLine("\nSelon quel ordre afficher les clients (entrer le num�ro) : ");
            Console.WriteLine("1 - Ordre alphab�tique");
            Console.WriteLine("2 - Rue");
            Console.WriteLine("3 - Montant des achats cumul�s");
            Console.Write("Choix : "); int choix = Convert.ToInt32(Console.ReadLine());
            while (choix > 3 || choix < 1) { Console.Write("Rentrer un choix valide : "); choix = Convert.ToInt32(Console.ReadLine()); }
            switch (choix)
            {
                case 1:
                    Console.WriteLine("Par ordre alphab�tique : \n");
                    AffichageClientsSql(conn, "Ordre alpha", true);
                    break;
                case 2:
                    Console.WriteLine("Par rue");
                    AffichageClientsSql(conn, "Rue", true);
                    break;
                case 3:
                    Console.WriteLine("Par montant des achats cumul�s");
                    AffichageClientsSql(conn, "Achats", true);
                    break;
                default:
                    break;
            }
        }


        static void ModuleCuisinier(MySqlConnection conn)
		{
            Console.Clear(); Console.WriteLine("Bienvenue sur le module cuisinier\n");

            Console.WriteLine("Que voulez-faire ? Selectionner le num�ro assoic� � l'option ou entrer 'r' pour retourner en arri�re: ");
            Console.WriteLine("1 - Ajouter un cuisinier");
            Console.WriteLine("2 - Supprimer un cuisinier");
			Console.WriteLine("3 - En tant que cuisinier, ajouter un plat");
            Console.WriteLine("4 - Modifier un cuisinier");
            Console.WriteLine("5 - Afficher les donn�es d'un cuisinier");
            Console.Write("\nChoix : ");
            string choix = Console.ReadLine();
            while (choix != "1" && choix != "2" && choix != "3" && choix != "4" && 
                choix != "5" && choix != "r") { Console.Write("\nRentre un choix valide : "); choix = Console.ReadLine(); }

            while (choix != "r")
            {
                if (choix == "1")
                {
                    Console.WriteLine("\nCr�ation d'un cuisinier : ");
                    Console.Write("Entrer l'id du compte Tiers : "); int id = Convert.ToInt32(Console.ReadLine());

                    bool cuisinierExiste = Existe(conn, "Cuisinier", "IDCuisinier", id);
                    bool tiersExiste = Existe(conn, "Tiers", "IDTiers", id);
                    while (tiersExiste == false || cuisinierExiste == true)
                    {
                        Console.WriteLine("Compte tiers inexistant ou compte cuisinier d�j� existant, rentrer un nouvel id");
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

                Console.WriteLine("\nQue voulez-faire ? Selectionner le num�ro assoic� � l'option ou entrer 'r' pour retourner en arri�re: ");
                Console.Write("Choix : ");
                choix = Console.ReadLine();
                while (choix != "1" && choix != "2" && choix != "3" && choix != "4" && choix != "5" && choix != "r") { Console.Write("\nRentre un choix valide : "); choix = Console.ReadLine(); }
            }
            RetourMenu(conn);
        }

        public static void ChoixAffichageCuisinier(MySqlConnection conn, int idCuisinier)
        {
            Console.WriteLine("Quelle information voulez-vous ? S�l�ctionner le num�ro associ�");
            Console.WriteLine("1 - Clients servis");
            Console.WriteLine("2 - Plats r�alis�s et leur fr�quence");
            Console.WriteLine("3 - Plat du jour propos�(s) : ");

            Console.Write("\nChoix d'information : : ");
            string choix = Console.ReadLine();
            while (choix != "1" && choix != "2" && choix != "3") { Console.Write("\nRentre un choix valide : "); choix = Console.ReadLine(); }
            if (choix == "1")
            {
                Console.Write("Depuis une date pr�cise ? O ou N (N par d�faut) : ");
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

            if (choix == "2")
            {
                Console.WriteLine("Plats propos�s et leur f�quence du cuisinier n�" + idCuisinier  + " : ");
                PlatsAffichage(conn, idCuisinier, "PlatsFreq");

            }

            if (choix == "3")
            {
                Console.WriteLine("Plat du jours propos�s par le cuisinier n� " + idCuisinier + " : ");
                PlatsAffichage(conn, idCuisinier, "PlatDuJour");
            }
        }
        static void ModulePlat(MySqlConnection conn, int idCuisinier)
        {
            Console.WriteLine("Ajout d'un plat");
            Random random = new Random();
            int idplat = new Random().Next(1, 1001);
            while (Existe(conn, "Plat", "IDPlat", idplat)) { idplat = new Random().Next(1, 1001); }
            Console.Write("Nom du plat : "); string nomPlat = Console.ReadLine();
            Console.Write("Type du plat : "); string type = Console.ReadLine();
            Console.Write("Date de fabrication du plat (JJ-MM-AAAA) : "); DateTime fabrication = Convert.ToDateTime(Console.ReadLine());
            Console.Write("Date de peremption du plat (JJ-MM-AAAA) : "); DateTime peremption = Convert.ToDateTime(Console.ReadLine());
            Console.Write("nationalit� du plat : "); string natio = Console.ReadLine();
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
            Console.WriteLine("2 - Plats disponibles");
            Console.WriteLine("3 - Informations sur un plat");
			Console.WriteLine("Pour choisir une option, rentrer le num�ro au d�but de l'option");
			Console.WriteLine("Pour retourner au menu principal, entrer 'r'");
			Console.Write("\nChoix : ");
			
			string choix = Console.ReadLine();

			while (choix != "1" && choix != "2" && choix != "3" && choix != "r") 
			{
				Console.Write("\nChoix invalide, rentrer un nouveau choix : ");
				choix = Console.ReadLine();
			}
			
			while (choix != "r")
			{
                if (choix == "1")
                {
                    Console.WriteLine("Ajout d'une commande : ");
                    Random random = new Random();
                    int idCommande = random.Next(1, 1001);
                    while(Existe(conn, "Commande", "IDCommande", idCommande))
                    {
                        idCommande = random.Next(1, 1001);
                    }
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

                    Console.Write("Voulez-vous ajouter d'autres plats � la commande (oui 'O' ou non 'N') ? "); string rep = Console.ReadLine().ToUpper();
                    while (rep == "O")
                    {
                        Console.Write("\nEntrer un id de plat : "); idPlat = Convert.ToInt32(Console.ReadLine());
                        bool PlatDejaCommande = Existe(conn, "PlatCommande", "IDPlat", idPlat);
                        valide = PlatCuisinierValide(conn, idCuisinier, idPlat);
                        if (PlatDejaCommande) { Console.WriteLine("Ce plat a d�j� �t� command�, il n'est plus disponible malhereusement :("); }
                        else if (valide == false) { Console.WriteLine("Ce plat n'est pas disponible pour ce cuisinier"); }

                        else { CommandePlat(conn, idCommande, idPlat); }
                        Console.Write("Voulez-vous ajouter d'autres plats � la commande (oui 'O' ou non 'N') ? "); rep = Console.ReadLine().ToUpper();
                    }

                    Console.WriteLine("Commande termin�e, on peut passer � la transaction : ");
                    Console.WriteLine("Prix de la commande : " + CalculerPrixCommande(idCommande, conn) + " euros");
                    Console.WriteLine("\nPour faciliter la livraison, donner les stations de m�tro (attention � l'orthographe et mettre des espaces entre les tirets) les plus proches du : ");
                    Console.Write("Client : "); string station1 = Console.ReadLine();
                    Console.Write("Cuisinier : "); string station2 = Console.ReadLine();
                    MetroParis(station1, station2);
                }

                if (choix == "2")
                {
                    Console.WriteLine("Plats disponibles : ");
                    PlatsDispos(conn);
                }

                if (choix == "3")
                {
                    Console.Write("Identifiant du plat dont vous voulez avoir des infos (consulter la rubrique Plats disponibles si besoin) : ");
                    int idPlatInfo = Convert.ToInt32(Console.ReadLine());
                    while (!Existe(conn, "Plat", "IDPlat", idPlatInfo)) { Console.Write("Entrer un id de plat valide : "); idPlatInfo = Convert.ToInt32(Console.ReadLine()); }
                    PlatsAffichage(conn, idPlatInfo, "InfosPlat");


                }

                Console.Write("\nChoix : ");
				choix = Console.ReadLine();

				while (choix != "1" && choix != "2" && choix != "3" && choix != "r")
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
            Console.Clear(); Console.WriteLine("Bienvenue sur le module statistiques\nOptions : \n");
            Console.WriteLine("1 - Nombre de commandes effectu�es par cuisinier");
            Console.WriteLine("2 - Commandes selon une p�riode de temps");
            Console.WriteLine("3 - Moyenne des prix des commandes");
            Console.WriteLine("4 - Moyenne des comptes clients");
            Console.WriteLine("5 - Liste des commandes pour un client selon la nationalit� des plats, la p�riode ");
            Console.WriteLine("Pour choisir une option, rentrer le num�ro au d�but de l'option");
            Console.WriteLine("Pour retourner au menu principal, entrer 'r'");
            Console.Write("\nChoix : ");

            string choix = Console.ReadLine();

            while (choix != "1" && choix != "2" && choix != "3" && choix != "4" && choix != "5" && choix != "r")
            {
                Console.Write("\nChoix invalide, rentrer un nouveau choix : ");
                choix = Console.ReadLine();
            }

            while (choix != "r")
            {
                if (choix == "1")
                {
                    Console.WriteLine("Nombre de commandes effecut�es par cuisinier : ");
                    NombreCommandes(conn);
                }

                if (choix == "2")
                {
                    CommandesTemps(conn);
                }

                if (choix == "3")
                {
                    Console.WriteLine("Moyenne des prix des commandes : ");

                    MoyenneStats(conn, "PrixCommandes");
                }
                if (choix == "4")
                {
                    Console.WriteLine("Moyenne de comptes clients");
                    MoyenneStats(conn, "MoyenneClients");
                }

                if (choix == "5")
                {
                    Console.Write("Entrer l'identifiant du client : "); int idclient = Convert.ToInt32(Console.ReadLine());
                    while (!Existe(conn, "Client", "IDClient", idclient))
                    {
                        Console.WriteLine("Entrer un identifiant valide de client : "); idclient = Convert.ToInt32(Console.ReadLine());
                    }
                    Console.WriteLine("Afficher la liste des commandes pour un client selon : ");
                    Console.WriteLine("1 - Nationalit�");
                    Console.WriteLine("2 - P�riode");
                    Console.Write("Choix (entrer le num�ro) : "); string choixListe = Console.ReadLine().Trim();
                    while (choixListe != "1" && choixListe != "2")
                    {
                        Console.Write("Rentrer un choix valide : ");
                        choixListe = Console.ReadLine();
                    }
                    if (choixListe == "1")
                    {
                        Console.Write("Entrer une nationalit� : ");
                        string nationalite = Console.ReadLine();
                        NatioPlats(conn, nationalite, idclient);
                    }
                    if (choixListe == "2")
                    {
                        CommandesTemps(conn, idclient);

                    }
                }

                Console.Write("ok");
                Console.Write("\nChoix : ");
                choix = Console.ReadLine();

                while (choix != "1" && choix != "2" && choix != "3" && choix != "4" && choix != "5" && choix != "r")
                {
                    Console.Write("\nChoix invalide, rentrer un nouveau choix : ");
                    choix = Console.ReadLine();
                }

            }
            RetourMenu(conn);
        }

        public static void CommandesTemps(MySqlConnection conn, int? idClient=null)
        {
            Console.WriteLine("Affiches les commandes selon : ");
            Console.WriteLine("1 - Toutes les commandes r�alis�es");
            Console.WriteLine("2 - Sur une p�riode donn�es");
            Console.WriteLine("3 - Depuis un certain temps");
            Console.WriteLine("4 - Jusqu'� un certain temps");

            Console.Write("Choix : "); string choix = Console.ReadLine();
            while (choix != "1" && choix != "2" && choix != "3" && choix != "4")
            {
                Console.Write("Rentrer un num�ro valide : "); choix = Console.ReadLine();
            }

            if (choix == "1")
            {
                DateTime date1 = Convert.ToDateTime("1900-01-01"); // date tr�s dans le pass�
                DateTime date2 = Convert.ToDateTime("2100-01-01");
                Console.Write("ok");
                AfficherCommandes(conn, date1, date2, "Toujours", idClient);   
            }

            if (choix == "2")
            {
                Console.Write("Entrer une date de d�but : ");
                DateTime debut = Convert.ToDateTime(Console.ReadLine());
                Console.Write("Entrer une date de fin : ");
                DateTime fin = Convert.ToDateTime(Console.ReadLine());
                AfficherCommandes(conn, debut, fin, "DebutEtFin", idClient);
            }

            if (choix == "3")
            {
                Console.Write("Entrer une date de d�but : ");
                DateTime debut = Convert.ToDateTime(Console.ReadLine());
                DateTime fin = Convert.ToDateTime("2100-01-01");
                AfficherCommandes(conn, debut, fin, "PasFin", idClient);
            }

            if (choix == "4")
            {
                Console.Write("Entrer une date de fin : ");
                DateTime fin = Convert.ToDateTime(Console.ReadLine());
                DateTime debut = Convert.ToDateTime("1900-01-01");
                AfficherCommandes(conn, debut, fin, "PasDebut", idClient);
            }
        }
    }
}
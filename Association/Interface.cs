using static Association.GrapheStation;
using static Association.Bdd;
using MySql.Data.MySqlClient;
using System.Text;
using System.Security.Cryptography.X509Certificates;
using System.ComponentModel.DataAnnotations;

namespace Association
{
	internal class Interface
	{
		public static void GererInterface(MySqlConnection conn)
		{
			Console.Clear();
			int idCompteActif = MenuConnexion(conn);
			ChoixModule(conn, idCompteActif);
		}

		static int MenuConnexion(MySqlConnection conn)
		{
            int idCompteConnecte = 0; /// id par d�faut, en principe jamais attribu�

            Console.WriteLine("LIVIN'PARIS \n\n");
            bool connecte = false;

            while (connecte == false)
            {
                Console.Write("Avez-vous d�j� un compte existant Oui (O) / Non (N) ? ");
                string choix = Console.ReadLine().ToLower().Trim();

                while (choix != "o" && choix != "n")
                {
                    Console.Write("Avez-vous d�j� un compte existant Oui (O) / Non (N) ? ");
                    choix = Console.ReadLine().ToLower().Trim();
                }


                if (choix == "n")
                {
                    AjouterUnCompte(conn);

                }
                
                if (choix == "o")
                {
                    Console.Write("\n\nEntrer un identifiant de connexion (email) : "); string email = Console.ReadLine().Trim();
                    Console.Write("Entrer un mot de passe : "); string motdepasse = Console.ReadLine();
                    connecte = VerfierConnexion(conn, email, motdepasse);
                    if (connecte == true) { idCompteConnecte = RecupIdMail(conn, email); Console.Clear(); }
                }
            }
            return idCompteConnecte;
		}


        static void InterfaceModifierCompte(MySqlConnection conn, string type, int idCompteModif=0)
        {
            Console.Write("\nModfier un compte ");
            if (type == "Client") { Console.WriteLine("client : "); }
            if (type == "Cuisinier") { Console.WriteLine("cuisinier : "); }
            if (type == "Tiers") { Console.WriteLine("tiers : "); }
            int idTiers;
            if (idCompteModif == 0)
            {
                Console.Write("Entrer l'id du compte � modiifer : "); idTiers = Convert.ToInt32(Console.ReadLine());
            }
            else
            {
                idTiers = idCompteModif;
            }
            if (!Existe(conn, type, "ID"+type, idTiers)) { Console.WriteLine("Compte " + type + " inexistant !"); }
            else
            {
                Console.Write("Entrer la composante � modifier : nom, pr�nom, adresse, t�l�phone, code postal, ville ? ");
                string colonne = Console.ReadLine().Trim().ToLower().Replace("�", "e"); // enlever majuscules, espaces, accents
                Console.Write("Entrer la nouvelle composante modif�e : ");
                string modifiee = Console.ReadLine();
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

		static void ChoixModule(MySqlConnection conn, int idCompteActif)
		{
			Console.WriteLine("LIVIN PARIS\n\n");
            Console.WriteLine("Connect�. Bievenue sur votre compte LIVIN'PARIS");
            Console.WriteLine("-> 1 : Param�tres du compte\n-> 2 : Interface Client\n-> 3 : Interface Cuisinier\n-> 4 : G�rer les commandes\n-> 5 : Quelques statistiques\n-> 6 : Param�tres administrateur");
       
            Console.Write("Choisir un module (rentrer le chiffre associ�) : ");
			string mod = (Console.ReadLine()).ToLower().Trim();
			while (mod != "1" && mod != "2" && mod != "3" && mod != "4" && mod != "5" && mod != "6")
			{
				Console.WriteLine("Rentrer un nombre valide");
				Console.Write("Choisir un module : ");
				mod = (Console.ReadLine()).ToLower().Trim();
			}
			
			switch (mod)
			{
				case "1":
					ModuleTiers(conn, idCompteActif); break;
				case "2":
					ModuleClient(conn, idCompteActif); break;
				case "3":
					ModuleCuisinier(conn, idCompteActif); break;
				case "4":
					ModuleCommande(conn, idCompteActif); break;
				case "5":
					ModuleStatistiques(conn, idCompteActif); break;
                case "6":
                    ModuleAdmin(conn, idCompteActif); break;
            }
		}

		static void RetourMenu(MySqlConnection conn, int idCompteActif)
        {
            Console.Clear();
			ChoixModule(conn, idCompteActif);
		}

        static void AjouterUnCompte(MySqlConnection conn)
        {
            Console.WriteLine("\nCr�ation d'un compte : ");
            int idTiers = new Random().Next(1, 1001);
            while (Existe(conn, "Tiers", "IDTiers", idTiers)) { idTiers = new Random().Next(1, 1001); }

            Console.Write("Entrer une adresse mail: "); string email = Console.ReadLine();
            while (Existe(conn, "Tiers", "Email", email)) { Console.WriteLine("Mail d�j� existant. Rentrez un nouvel email: "); email = Console.ReadLine(); }
            Console.Write("Entrer un mot de passe : "); string mdp = Console.ReadLine();
            Console.Write("Entrer un nom : "); string nom = Console.ReadLine(); nom = nom.Replace("'", "''"); /// pour �viter les conflits avec les apostrophes dans les donn�es
            Console.Write("Entrer un pr�nom : "); string prenom = Console.ReadLine();
            Console.Write("Entrer un code postal : "); string cp = Console.ReadLine();
            Console.Write("Entrer une ville : "); string ville = Console.ReadLine(); ville = ville.Replace("'", "''");
            Console.Write("Entrer une adresse : "); string adresse = Console.ReadLine(); adresse = adresse.Replace("'", "''");

            

            Console.Write("Entrer un num�ro de t�l�phone : "); string tel = Console.ReadLine();
            CreerUnCompte(conn, idTiers, mdp, cp, ville, email, tel, nom, adresse, prenom);
        }


		static void ModuleTiers(MySqlConnection conn, int idCompteActif)
		{
            Console.Clear(); Console.WriteLine("Param�tres du compte\n");

            Console.WriteLine("Que voulez-faire ? Selectionner le num�ro assoic� � l'option ou entrer 'r' pour retourner en arri�re: ");
            Console.WriteLine("1 - Modifier mon compte");
			Console.WriteLine("2 - Supprimer mon compte");
            Console.WriteLine("3 - Se d�connecter");
            Console.Write("\nChoix : ");
            string choix = Console.ReadLine();
            while (choix != "1" && choix != "2" && choix != "3" && choix != "r") { Console.Write("\nRentre un choix valide : "); choix = Console.ReadLine(); }
			while (choix != "r")
			{
                if (choix == "3")
                {
                    Console.Clear();
                    MenuConnexion(conn);
                    GererInterface(conn); /// appel r�cursif finalement
                    /// apr�s il n'y pas r�ellement de risque de rappels infinis car on imagine bien qu'une fois que
                    /// l'utilisateur quitte l'application tout s'arr�te et lorsqu'il se d�connecte et reconnecte
                    /// ce ne sera pas un nombre tr�s grand de fois

                }

				if (choix == "2")
				{
					Console.WriteLine("\nSupprimer du compte : ");
                    if (!Existe(conn, "Tiers", "IDTiers", idCompteActif)) { Console.WriteLine("Compte inexistant !"); }
                    else { SupprimerUnCompte(conn, "Tiers", idCompteActif); GererInterface(conn); }
                }
                
                if (choix == "1")
                {
                    InterfaceModifierCompte(conn, "Tiers", idCompteActif);
                }

                Console.Write("\nChoix : ");
                choix = Console.ReadLine();
                while (choix != "1" && choix != "2" && choix != "3" && choix != "r") { Console.Write("\nRentrer un choix valide : "); choix = Console.ReadLine(); }
            }
            RetourMenu(conn, idCompteActif);
        }

        static void ModuleAdmin(MySqlConnection conn, int idCompteActif)
        {

            if (idCompteActif != 1)
            {
                Console.Clear();
                Console.WriteLine("Acc�s impossible � cette page, vous n'�tes pas un compte administrateur");
                Console.WriteLine("Retour au menu : ");
                object retourMenu = Console.ReadLine(); /// quand l'utilisateur clique sur entr�e il retourne automatiquement au menu
                RetourMenu(conn, idCompteActif);
            }
            else
            {
                Console.Clear(); Console.WriteLine("Param�tres administrateur\n");

                Console.WriteLine("Que voulez-faire ? Selectionner le num�ro assoic� � l'option ou entrer 'r' pour retourner en arri�re: ");
                Console.WriteLine("1 - Modifier un compte");
                Console.WriteLine("2 - Supprimer un compte");
                Console.WriteLine("3 - Ajouter un compte client");
                Console.WriteLine("4 - Suppression d'un compte client");
                Console.Write("\nChoix : ");
                string choix = Console.ReadLine();
                while (choix != "1" && choix != "2" && choix != "3" && choix != "4" && choix != "r") { Console.Write("\nRentre un choix valide : "); choix = Console.ReadLine(); }
                while (choix != "r")
                {
                    if (choix == "1")
                    {
                        InterfaceModifierCompte(conn, "Tiers");
                    }
                    else if (choix == "2")
                    {
                        Console.WriteLine("\nSupprimer un compte : ");
                        Console.WriteLine("Entrer l'id du compte : "); 
                        int idTiers = Convert.ToInt32(Console.ReadLine());
                        if (!Existe(conn, "Tiers", "IDTiers", idTiers)) { Console.WriteLine("Compte inexistant !"); }
                        else { SupprimerUnCompte(conn, "Tiers", idTiers); }
                    }

                    else if (choix == "3")
                    {
                        Console.WriteLine("\nCr�ation d'un client : ");
                        Console.Write("Entrer le mail du compte Tiers : "); string email = Console.ReadLine();
                        while (!Existe(conn, "Tiers", "Email", email)) { Console.Write("\nEntrer le mail du compte Tiers : "); email = Console.ReadLine(); }
                        int idClient = RecupIdMail(conn, Console.ReadLine());
                        bool clientExiste = Existe(conn, "Client", "IDClient", idClient);

                        while (clientExiste == true)
                        {
                            Console.Write("Entrer le mail du compte Tiers : "); email = Console.ReadLine();
                            while (!Existe(conn, "Tiers", "Email", email)) { Console.Write("\nEntrer le mail du compte Tiers : "); email = Console.ReadLine(); }
                            idClient = RecupIdMail(conn, Console.ReadLine());
                            clientExiste = Existe(conn, "Client", "IDClient", idClient);
                        }
                        AjouterClient(true, idClient, conn);
                    }

                    else if (choix == "4")
                    {
                        Console.WriteLine("\nSuppression d'un client : ");
                        Console.Write("Entrer l'id du compte Client � supprimer : "); int idClient = Convert.ToInt32(Console.ReadLine());

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
                    Console.Write("\nChoix : ");
                    choix = Console.ReadLine();
                    while (choix != "1" && choix != "2" && choix != "3" && choix != "4" && choix != "r") { Console.Write("\nRentrer un choix valide : "); choix = Console.ReadLine(); }
                }
                RetourMenu(conn, idCompteActif);
            }
            
        }

		static void ModuleClient(MySqlConnection conn, int idCompteActif)
		{
			Console.Clear(); Console.WriteLine("Bienvenue sur le module client\n");

			Console.WriteLine("Que voulez-faire ? Selectionner le num�ro assoic� � l'option ou entrer 'r' pour retourner en arri�re: ");
			Console.WriteLine("1 - Devenir client");
            Console.WriteLine("2 - Supprimer mon compte client");
            Console.WriteLine("3 - Afficher les autres clients");
            Console.WriteLine("4 - Modifier mon compte client");
            Console.Write("\nChoix : ");
			string choix = Console.ReadLine();
			while (choix != "1" && choix != "2" && choix != "3" && choix != "4" && choix != "r") { Console.Write("\nRentre un choix valide : "); choix = Console.ReadLine(); }

            while (choix != "r")
            {
                if (choix == "1")
                {
                    Console.WriteLine("\nCr�ation d'un client : ");
                    bool clientExiste = Existe(conn, "Client", "IDClient", idCompteActif);

                    if (clientExiste) { Console.WriteLine("Vous �tes d�j� client !"); }
                    else { AjouterClient(true, idCompteActif, conn); }
                }

                if (choix == "2")
                {
                    Console.WriteLine("\nSuppression d'un client : ");
                    bool clientExiste = Existe(conn, "Client", "IDClient", idCompteActif);
                    
                    if (clientExiste) { SupprimerClient(conn, "Client", idCompteActif); }
                    else { Console.WriteLine("Vous n'�tes pas client, veuillez cr��r un compte client"); }
                }

                if (choix == "3")
                {
                    Console.WriteLine(""); AffichageClients(conn);
                }

                if (choix == "4")
                {
                    if (Existe(conn, "Client", "IDClient", idCompteActif))
                    {
                        InterfaceModifierCompte(conn, "Client", idCompteActif);
                    }
                    else { Console.WriteLine("Vous n'�tes pas client. Veuillez � cr��r un compte client"); }
                }

                Console.WriteLine("\nQue voulez-faire ? Selectionner le num�ro assoic� � l'option ou entrer 'r' pour retourner en arri�re: ");
                Console.WriteLine("1 - Devenir client\n2 - Supprimer mon compte client\n3 - Afficher les autres clients\n4 - Modifier mon compte client");
                Console.Write("Choix : ");
                choix = Console.ReadLine();
                while (choix != "1" && choix != "2" && choix != "3" && choix != "4" && choix != "r") { Console.Write("\nRentre un choix valide : "); choix = Console.ReadLine(); }
            }
            RetourMenu(conn, idCompteActif);
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


        static void ModuleCuisinier(MySqlConnection conn, int idCompteActif)
		{
            Console.Clear(); Console.WriteLine("Bienvenue sur le module cuisinier\n");

            Console.WriteLine("Que voulez-faire ? Selectionner le num�ro assoic� � l'option ou entrer 'r' pour retourner en arri�re: ");
            Console.WriteLine("1 - Devenir cuisinier");
            Console.WriteLine("2 - Supprimer un cuisinier");
			Console.WriteLine("3 - Proposer un nouveau plat");
            Console.WriteLine("4 - Afficher mes plats propos�s ");
            Console.WriteLine("5 - Modifier mon compte cuisinier");
            Console.WriteLine("6 - Afficher les donn�es d'un cuisinier");
            Console.Write("\nChoix : ");
            string choix = Console.ReadLine();
            while (choix != "1" && choix != "2" && choix != "3" && choix != "4" && 
                choix != "5" && choix != "6" && choix != "r") { Console.Write("\nRentre un choix valide : "); choix = Console.ReadLine(); }

            while (choix != "r")
            {
                if (choix == "1")
                {
                    Console.WriteLine("\nActivation de votre compte cuisinier : ");
                    bool clientExiste = Existe(conn, "Cuisinier", "IDCuisinier", idCompteActif);

                    if (clientExiste) { Console.WriteLine("Vous �tes d�j� cuisinier !"); }
                    else { AjouterCuisinier(true, idCompteActif, conn); }

                }

                if (choix == "2")
                {
                    Console.WriteLine("\nSuppression de votre compte cuisinier : ");
                    bool clientExiste = Existe(conn, "Cuisinier", "IDCuisinier", idCompteActif);

                    if (clientExiste) { SupprimerCuisinier(conn, "Cuisinier", idCompteActif); }
                    else { Console.WriteLine("Vous n'�tes pas cuisinier, veuillez cr��r un compte cuisinier"); }            
                }

                if (choix == "3")
                {
                    if (!Existe(conn, "Cuisinier", "IDCuisinier", idCompteActif))
                    {
                        Console.WriteLine("Erreur : cet utilisateur n'est pas enregistr� comme cuisinier.");
                    }
                    else
                    {
                        AjoutPlat(conn, idCompteActif);

                    }
                }

                if (choix == "4")
                {
                    PlatsAffichage(conn, idCompteActif, "PlatDuJour");
                }

                if (choix == "5")
                {
                    InterfaceModifierCompte(conn, "Cuisinier", idCompteActif);
                }

                if (choix == "6")
                {
                    Console.Write("Afficher mes infos ? (1) ou celles d'un autre cuisinier (2) : ");
                    string choixInfos = Console.ReadLine();
                    if (choixInfos == "1")
                    {
                        bool cuisinierExiste = Existe(conn, "Cuisinier", "IDCuisinier", idCompteActif);

                        if (cuisinierExiste) { ChoixAffichageCuisinier(conn, idCompteActif); }
                        else { Console.WriteLine("Vous n'�tes pas cuisinier, veuillez cr�er un compte cuisinier"); }
                    }
                    else if (choix == "2")
                    {
                        Console.Write("Entrer l'id du cuisinier dont vous voulez afficher les infos : ");
                        int idCuisinier = Convert.ToInt32(Console.ReadLine());
                        while (!Existe(conn, "Cuisinier", "IDCuisinier", idCompteActif))
                        {
                            Console.Write("Entrer un id valide : ");
                            idCuisinier = Convert.ToInt32(Console.ReadLine());
                        }
                        ChoixAffichageCuisinier(conn, idCuisinier);
                    }
                    else { Console.WriteLine("Choi invalide"); }

                }

                Console.WriteLine("\nQue voulez-faire ? Selectionner le num�ro assoic� � l'option ou entrer 'r' pour retourner en arri�re: ");
                Console.WriteLine("1 - Devenir cuisinier \n2 - Supprimer un cuisinier \n3 - Proposer un nouveau plat \n4 - Afficher mes plats propos�s \n5 - Modifier mon compte cuisinier \n6 - Afficher les donn�es d'un cuisinier");
                Console.Write("Choix : ");
                choix = Console.ReadLine();
                while (choix != "1" && choix != "2" && choix != "3" && choix != "4" && choix != "5" && choix != "6" && choix != "r") { Console.Write("\nRentre un choix valide : "); choix = Console.ReadLine(); }
            }
            RetourMenu(conn, idCompteActif);
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
        static void AjoutPlat(MySqlConnection conn, int idCuisinier)
        {
            Console.WriteLine("\nAjout d'un plat : ");
            Random random = new Random();
            int idplat = new Random().Next(1, 1001);
            while (Existe(conn, "Plat", "IDPlat", idplat)) { idplat = new Random().Next(1, 1001); }
            Console.Write("Nom du plat : "); string nomPlat = Console.ReadLine();
            Console.Write("Type du plat : "); string type = Console.ReadLine();
            Console.Write("Date de fabrication du plat (JJ-MM-AAAA) : "); DateTime fabrication = Convert.ToDateTime(Console.ReadLine());
            Console.Write("Date de peremption du plat (JJ-MM-AAAA) : "); DateTime peremption = Convert.ToDateTime(Console.ReadLine());
            Console.Write("Nationalit� du plat : "); string natio = Console.ReadLine();
            Console.Write("Ingredients du plat : "); string ingredients = Console.ReadLine();
            Console.Write("R�gime du plat : "); string regime = Console.ReadLine();
            Console.Write("Prix du plat (avec une virgule et pas un point) : "); string prixLu = Console.ReadLine(); prixLu.Replace(".", ","); double prix = Convert.ToDouble(prixLu);
            Console.Write("Nombre de personnes pour le plat : "); int nbPersonnes = Convert.ToInt32(Console.ReadLine());

            FairePlat(true, conn, idplat, type, nomPlat, fabrication, peremption, natio, regime, ingredients, prix, nbPersonnes, idCuisinier);
        }

        static void ModuleCommande(MySqlConnection conn, int idCompteActif)
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
                    if(!Existe(conn, "Client", "IDClient", idCompteActif))
                    {
                        Console.WriteLine("Erreur : vous n'�tes pas un client. Veuillez cr��r un compte client dans l'interface 'Client'");
                    }
                    else
                    {
                        Console.WriteLine("\nPlats disponibles : "); /// affichage des plats pour faciliter le choix du client
                        PlatsDispos(conn);

                        Console.WriteLine("Ajout d'une commande : ");
                        Random random = new Random();
                        int idCommande = random.Next(1, 1001);
                        while (Existe(conn, "Commande", "IDCommande", idCommande))
                        {
                            idCommande = random.Next(1, 1001);
                        }
                        Console.Write("Entrer une date de livraison : "); DateTime date = Convert.ToDateTime(Console.ReadLine());
                        Console.Write("Entrer une heure de livraison : "); DateTime heure = Convert.ToDateTime(Console.ReadLine());
                        Console.Write("Entrer un id de cuisinier : "); int idCuisinier = Convert.ToInt32(Console.ReadLine());
                        Console.Write("Entrer un id de plat : "); int idPlat = Convert.ToInt32(Console.ReadLine());
                        bool valide = PlatCuisinierValide(conn, idCuisinier, idPlat);

                        if (valide == false) { Console.WriteLine("Ce plat n'est pas disponible pour ce cuisinier, il n'est donc pas ajout� � la commande"); }
                        else
                        {
                            CommandePlat(conn, idCommande, idPlat);
                        }

                     
                        Console.Write("Voulez-vous ajouter d'autres plats � la commande (oui 'O' ou non 'N') ? "); string rep = Console.ReadLine().ToUpper();
                        while (rep == "O")
                        {
                            Console.Write("Entrer un id de cuisinier : "); idCuisinier = Convert.ToInt32(Console.ReadLine());
                            Console.Write("\nEntrer un id de plat : "); idPlat = Convert.ToInt32(Console.ReadLine());

                            valide = PlatCuisinierValide(conn, idCuisinier, idPlat);

                            if (valide == false) { Console.WriteLine("Ce plat n'est pas disponible pour ce cuisinier, il n'est donc pas ajout� � la commande"); }
                            else
                            {
                                CommandePlat(conn, idCommande, idPlat);
                            }

                            Console.Write("Voulez-vous ajouter d'autres plats � la commande (oui 'O' ou non 'N') ? "); rep = Console.ReadLine().ToUpper();
                        }
                        FaireUneCommande(true, conn, idCommande, date, heure, idCompteActif, idCuisinier);
                        Console.WriteLine("Commande termin�e, on peut passer � la transaction : ");
                        Console.WriteLine("Prix de la commande : " + CalculerPrixCommande(idCommande, conn) + " euros");
                        Console.WriteLine("\nPour faciliter la livraison, donner les stations de m�tro (attention � l'orthographe et mettre des espaces entre les tirets) les plus proches du : ");
                        Console.Write("Client : "); string station1 = Console.ReadLine();
                        Console.Write("Cuisinier : "); string station2 = Console.ReadLine();
                        MetroParis(station1, station2);
                    }
                    
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
			RetourMenu(conn, idCompteActif); 
		}

		static void ModuleStatistiques(MySqlConnection conn, int idCompteActif)
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

                Console.Write("\nChoix : ");
                choix = Console.ReadLine();

                while (choix != "1" && choix != "2" && choix != "3" && choix != "4" && choix != "5" && choix != "r")
                {
                    Console.Write("\nChoix invalide, rentrer un nouveau choix : ");
                    choix = Console.ReadLine();
                }

            }
            RetourMenu(conn, idCompteActif);
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
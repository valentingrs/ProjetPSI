using System.Diagnostics;
using static Association.GrapheStation;

namespace Association
{
	internal class PlusCourtChemin
	{
		/// Ficher contenant des algorithmes de recherche de plus court chemin
		/// Comparaison des algorithmes
        public static void FloydWarshall<T>(Graphe<T> graphe, Noeud<T> dep, Noeud<T> arr)
        {
            if (!graphe.Noeuds.Contains(dep)) return; // Vérifie que le départ est dans le graphe
            if (!graphe.Noeuds.Contains(arr)) return;
            const int inf = int.MaxValue;

            // Initialisation matrice des plus courts chemins
            // Initialisation matrice des prédecesseurs
            double?[,] plusCourts = new double?[graphe.Taille(), graphe.Taille()];
            T[,] predecesseurs = new T[graphe.Taille(), graphe.Taille()];

            for (int i = 0; i < graphe.Taille(); i++)
            {
                for (int j = 0; j < graphe.Taille(); j++)
                {
                    Noeud<T> noeud1 = graphe.Noeuds[i];
                    Noeud<T> noeud2 = graphe.Noeuds[j];
                    
                    Lien<T> lien = graphe.IdentifierLien(noeud1, noeud2);
                    if (i == j)
                    {
                        plusCourts[i, j] = 0;
                        predecesseurs[i, j] = default(T); // sorte de "null" général pour tout type T
                    }
                    else if (lien == null)
                    {
                        plusCourts[i, j] = inf;
                        predecesseurs[i, j] = default(T);
                    }
                    else
                    {
                        plusCourts[i, j] = lien.Poids;
                        predecesseurs[i, j] = lien.Noeud1.Nom;
                    }

                }
            }


            // Itérations
            for (int k = 1; k <= graphe.Taille(); k++)
            {
                for (int i = 0; i < graphe.Taille(); i++)
                {
                    for (int j = 0; j < graphe.Taille(); j++)
                    {
                        // on réalise ces conversions car Math.Min ne prend pas des double? mais que des double
                        // le ?? permet de convertir un double? en double 
                        double? a = plusCourts[i, j]; double valeurA = a ?? double.MaxValue;
                        double? b = plusCourts[i, k - 1]; double valeurB = b ?? double.MaxValue;
                        double? c = plusCourts[k - 1, j]; double valeurC = c ?? double.MaxValue;

                        double? coeff = Math.Min(valeurA, valeurB + valeurC);

                        plusCourts[i, j] = coeff;

                        if (coeff != a) { predecesseurs[i, j] = predecesseurs[k - 1, j]; }
                    }
                }

            }

            Dictionary<T, int> dictionnaireIndices = new Dictionary<T, int>();
            for (int i = 0; i < graphe.Noeuds.Count(); i++)
            {
                dictionnaireIndices[graphe.Noeuds[i].Nom] = i;
            }
            List<T> chemin = new List<T>();
            int depInt = dictionnaireIndices[dep.Nom];
            int arrInt = dictionnaireIndices[arr.Nom];
            if (predecesseurs[depInt, arrInt] == null)
            {
                Console.WriteLine("Aucun chemin trouvé !");
            }
            else
            {
                int current = arrInt;
                while (current != depInt)
                {
                    chemin.Insert(0, graphe.Noeuds[current].Nom);
                    current = dictionnaireIndices[predecesseurs[depInt, current]];
                }
                chemin.Insert(0, graphe.Noeuds[depInt].Nom);
            }

            Console.WriteLine("Itinéraire le plus court entre " + dep + " et " + arr + " : ");
            foreach (T elem in chemin) { Console.WriteLine(elem); }
            Console.WriteLine("Temps total : " + plusCourts[depInt, arrInt] + " minutes");
        }

        public static void Dijkstra<T>(Graphe<T> G, Noeud<T> depart, Noeud<T> arrivee)
        {
            if (!G.Noeuds.Contains(depart)) return; // Vérifie que le départ est dans le graphe
            if (!G.Noeuds.Contains(arrivee)) return;

            int V = G.Noeuds.Count;
            Dictionary<Noeud<T>, double?> Dist = new Dictionary<Noeud<T>, double?>(); // Stocke les distances
            Dictionary<Noeud<T>, Noeud<T>> Pred = new Dictionary<Noeud<T>, Noeud<T>>(); // Stocke les prédécesseurs
            List<Noeud<T>> non_visités = new List<Noeud<T>>(G.Noeuds); // Liste des noeuds non visités

            foreach (var noeud in G.Noeuds)
            {
                Dist[noeud] = double.PositiveInfinity; // Initialise toutes les distances à l'infini
                Pred[noeud] = null; // Aucun prédécesseur au début
            }

            Dist[depart] = 0; // La distance du départ à lui-même est 0

            while (non_visités.Count > 0)
            {
                // Trouver le nœud avec la plus petite distance non visitée
                Noeud<T> noeud_plus_proche = null;
                double? distance_minimale = double.PositiveInfinity;

                foreach (Noeud<T> noeud in non_visités)
                {
                    if (Dist[noeud] < distance_minimale)
                    {
                        distance_minimale = Dist[noeud];
                        noeud_plus_proche = noeud;
                    }
                }

                if (noeud_plus_proche == null) break; // Sécurité si aucun nœud atteignable

                non_visités.Remove(noeud_plus_proche); // On le marque comme visité **ici seulement**

                // Mettre à jour les distances des voisins
                foreach (Noeud<T> voisin in noeud_plus_proche.Voisins)
                {
                    if (non_visités.Contains(voisin))
                    {
                        double? poids_lien = G.IdentifierLien(noeud_plus_proche, voisin).Poids;
                        double? nouvelle_dist = Dist[noeud_plus_proche] + poids_lien;

                        if (nouvelle_dist < Dist[voisin]) // Mise à jour si une meilleure distance est trouvée
                        {
                            Dist[voisin] = nouvelle_dist;
                            Pred[voisin] = noeud_plus_proche;
                        }
                    }
                }
            }
            // Affichage du chemin le plus court
            if (Dist[arrivee] == double.PositiveInfinity)
            {
                Console.WriteLine($"Aucun chemin trouvé entre {depart} et {arrivee}.");
                return;
            }

            List<Noeud<T>> chemin = new List<Noeud<T>>();
            Noeud<T> actuel = arrivee;

            while (!(actuel is null))
            {
                chemin.Insert(0, actuel);
                actuel = Pred[actuel];
            }

            Console.WriteLine("\nChemin le plus court :");
            
            foreach (Noeud<T> noeud in chemin)
            {
                Console.WriteLine(noeud);
            }

            Console.WriteLine($"Distance totale: {Dist[arrivee]} minutes");
        }

        public static void Bellman_Ford<T>(Graphe<T> G, Noeud<T> depart, Noeud<T> arrivee)
        {
            if (!G.Noeuds.Contains(depart)) return; // Vérifie que le noeud de départ est dans le graphe
            if (!G.Noeuds.Contains(arrivee)) return;

            int V = G.Noeuds.Count;
            Dictionary<Noeud<T>, double?> Dist = new Dictionary<Noeud<T>, double?>(); // Stocke les distances
            Dictionary<Noeud<T>, Noeud<T>> Pred = new Dictionary<Noeud<T>, Noeud<T>>(); // Stocke les prédécesseurs

            // Initialisation des distances
            foreach (var noeud in G.Noeuds)
            {
                Dist[noeud] = double.PositiveInfinity;
                Pred[noeud] = default;
            }

            Dist[depart] = 0; // La distance du départ à lui-même est 0

            // Phase de relaxation (V - 1 fois)
            for (int i = 0; i < V - 1; i++)
            {
                foreach (Lien<T> l in G.Liens)
                {
                    Noeud<T> u = l.Noeud1;
                    Noeud<T> v = l.Noeud2;
                    double? poids = l.Poids;

                    if (Dist[u] != double.PositiveInfinity && Dist[u] + poids < Dist[v])
                    {
                        Dist[v] = Dist[u] + poids;
                        Pred[v] = u;
                    }
                }
            }

            // Détection des cycles de poids négatif
            foreach (Lien<T> l in G.Liens)
            {
                Noeud<T> u = l.Noeud1;
                Noeud<T> v = l.Noeud2;
                double? poids = l.Poids;

                if (Dist[u] != double.PositiveInfinity && Dist[u] + poids < Dist[v])
                {
                    throw new Exception("Cycle de poids négatif détecté !");
                }
            }

            // Affichage du chemin le plus court
            if (Dist[arrivee] == double.PositiveInfinity)
            {
                Console.WriteLine($"Aucun chemin trouvé entre {depart} et {arrivee}.");
                return;
            }

            List<Noeud<T>> chemin = new List<Noeud<T>>();
            Noeud<T> actuel = arrivee;

            while (!(actuel is null))
            {
                chemin.Insert(0, actuel);
                actuel = Pred[actuel];
            }

            Console.WriteLine("\nChemin le plus court :");

            foreach (Noeud<T> noeud in chemin)
            {
                Console.WriteLine(noeud);
            }

            Console.WriteLine($"Distance totale: {Dist[arrivee]}");
        }

        public static void TestAlgoGraphes()
        {
            // on teste l'efficacité en temps des algorithmes avec le graphe des stations de métro
            Graphe<Station> graphe = LireStationMetro("MetroParis.xlsx");
            // Pour le test on prendra deux stations sufisamment éloignées et demandant plusieurs correspondances
            Station dep = TrouverStationParNom(graphe, "Eglise d'Auteuil");
            Station arr = TrouverStationParNom(graphe, "Saint-Fargeau");

            Noeud<Station> noeudDep = graphe.IdentifierNoeud(dep);
            Noeud<Station> noeudArr = graphe.IdentifierNoeud(arr);

            Console.WriteLine("Chemin le plus court entre Porte de Champerret et Quai de la Gare");
            Console.WriteLine("On a choisi ces deux stations car elles sont suffisamment éloignées et demandent plusieurs correspondances ");
            
            Console.WriteLine("\nAvec Dijkstra : ");
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            Dijkstra(graphe, noeudDep, noeudArr);
            stopwatch.Stop();
            Console.WriteLine("Temps d'execution avec l'algorithme de Dijkstra : " + stopwatch.ElapsedMilliseconds + " ms");

            Console.WriteLine("\nAvec Bellman Ford : ");
            stopwatch = new Stopwatch();
            stopwatch.Start();
            Dijkstra(graphe, noeudDep, noeudArr);
            stopwatch.Stop();
            Console.WriteLine("Temps d'execution avec l'algorithme de Bellman Ford : " + stopwatch.ElapsedMilliseconds + " ms");

            Console.WriteLine("\nAvec Floyd-Warhsall : ");
            stopwatch = new Stopwatch();
            stopwatch.Start();
            Dijkstra(graphe, noeudDep, noeudArr);
            stopwatch.Stop();
            Console.WriteLine("Temps d'execution avec l'algorithme de Floyd-Warshall : " + stopwatch.ElapsedMilliseconds + " ms");

        }
    }
}
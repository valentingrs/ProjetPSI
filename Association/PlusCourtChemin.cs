namespace Association
{
	internal class PlusCourtChemin
	{
		/// Ficher contenant des algorithmes de recherche de plus court chemin
		/// Comparaison des algorithmes
        public static void FloydWarshall<T>(Graphe<T> graphe, T dep, T arr)
        {
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
            int depInt = dictionnaireIndices[dep];
            int arrInt = dictionnaireIndices[arr];
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

            Console.WriteLine("Chemin le plus court entre " + dep + " et " + arr + " : ");
            foreach (T elem in chemin) { Console.Write(elem + ", "); }
        }


    }
}
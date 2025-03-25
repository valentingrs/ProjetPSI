using System;

namespace Association
{
	public class Noeud<T>
	{
		private T nom;
		private List<Noeud<T>> voisins;

		public Noeud(T nom)
		{
			voisins = new List<Noeud<T>>();
			this.nom = nom;
		}

		public T Nom
		{
			get { return nom; }
			set { nom = value; }
		}

		public List<Noeud<T>> Voisins
		{
			get { return voisins; }
			set { voisins = value; }
		}

		public void AjouterVoisin(Noeud<T> s)
		{
			voisins.Add(s);
		}

		public override string ToString()
		{
			string s = "" + nom;
			return s;
		}

		public int Degre()
		{
			return voisins.Count;
		}

		public bool Equals(Noeud<T> sommet2)
		{
			return (nom != null && nom.Equals(sommet2.nom));
		}

		public static bool operator ==(Noeud<T> s1, Noeud<T> s2)
		{
			return s1.Equals(s2);
		}

		public static bool operator !=(Noeud<T> s1, Noeud<T> s2)
		{
			return !(s1 == s2);
		}

		public void AfficherVoisins()
		{
			if (voisins.Count == 0)
			{
				Console.WriteLine($"{nom} n'a pas de voisins.");
			}
			else
			{
				Console.WriteLine($"Les voisins de {nom} sont :");
				foreach (Noeud<T> voisin in voisins)
				{
					Console.Write(voisin.Nom  + " ");
				}
				Console.WriteLine("");
			}
		}

	}

}


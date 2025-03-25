using System;

namespace Association
{
	public class Lien<T>
	{
		private Noeud<T> noeud1;
		private Noeud<T> noeud2;

		public Lien(Noeud<T> noeud1, Noeud<T> noeud2)
		{
			this.noeud1 = noeud1;
			this.noeud2 = noeud2;
		}

		public Noeud<T> Noeud1
		{
			get { return noeud1; }
			set { noeud1 = value; }
		}

		public Noeud<T> Noeud2
        {
			get { return noeud2; }
			set { noeud2 = value; }
		}

		public override string ToString()
		{
			string s = noeud1.Nom + " - " + noeud2.Nom;
			return s;
		}
	}

}
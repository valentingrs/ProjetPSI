using System;

namespace Association
{
	public class Lien
	{
		private Noeud noeud1;
		private Noeud noeud2;

		public Lien(Noeud noeud1, Noeud noeud2)
		{
			this.noeud1 = noeud1;
			this.noeud2 = noeud2;
		}

		public Noeud Noeud1
		{
			get { return noeud1; }
			set { noeud1 = value; }
		}

		public Noeud Noeud2
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
using System;
using System.Collections.Generic;
using System.Text;

namespace Pra.Bieren.CORE.Entities
{
    public class BierSoort
    {
		private int id;
		private string soort;

		public int ID
		{
			get { return id; }
			set { id = value; }
		}
		public string Soort
		{
			get { return soort; }
			set
			{
				value = value.Trim();
				if (value.Length == 0)
					throw new Exception("Waarde biersoort kan niet leeg zijn");
				if (value.Length > 50)
					value = value.Substring(0, 50);
				soort = value;
			}
		}

		public BierSoort()
        {

        }
		public BierSoort(int id, string soort)
		{
			this.id = id;
			this.soort = soort;
		}

		public override string ToString()
		{
			return soort;	
		}

	}
}

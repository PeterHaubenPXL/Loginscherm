using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NUnitTest
{
    public class Department
    {
		private string naam;

		public string Naam
		{
			get { return naam; }
			set {
				naam = value.ToUpper();
			}
		}

	}
}

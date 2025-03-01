using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAppWPL2DemoTesting
{
    public class Student
    {
		private string _firstName;

		public string FirstName
		{
			get { return _firstName; }
			set { _firstName = value; }
		}


		private string _lastName;

		public string LastName
		{
			get { return _lastName; }
			set { _lastName = value; }
		}

		private static int nextID;

		private int _id;

		public int ID
		{
			get { return _id; }
			set 
			{
				_id = value;
			}
		}

        public Student()
        {
			FirstName = "John";
			LastName = "Doe";
			ID = nextID++;
        }

        public Student(string firstName,string lastName)
        {
			FirstName = firstName;
			LastName = lastName;
			ID = nextID++;
        }

		private int GetId()
		{
			return _id + 1;
		}

    }
}

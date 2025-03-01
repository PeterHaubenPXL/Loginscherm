using ClassLibTeam04.Business.Entity;
using ClassLibTeam04.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibTeam04.Business
{
    public static class Students
    {
        public static IEnumerable<Student> List()
        {
            return StudentRepository.StudentList;
        }

        public static void Add(string firstName, string lastName)
        {
            StudentRepository.Add(firstName, lastName);
        }

    }
}

using ClassLibTeam04.Business.Entities;
using ClassLibTeam04.Data.Framework;
using ClassLibTeam04.Data;
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

        //public static void Add(string firstName, string lastName)
        //{
        //    StudentRepository.Add(firstName, lastName);
        //}


        public static SelectResult GetStudents()
        {
            StudentData studentData = new StudentData();
            SelectResult result = studentData.Select();
            return result;
        }

        public static InsertResult Add(string firstName, string lastName)
        {
            Student student = new Student();
            student.FirstName = firstName;
            student.LastName = lastName;
            StudentData studentData = new StudentData();
            InsertResult result = studentData.Insert(student);
            return result;
        }

    }
}

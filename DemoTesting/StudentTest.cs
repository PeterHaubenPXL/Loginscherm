using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPL2Testing
{
    // TestFixture: Geeft aan dat deze class testen bevat
    [TestFixture] 
    class StudentTest
    {
        Student naamlozeStudent;
        Student janSchippers;

        // SetUp: Zet alles klaar om te starten met de testen
        // Alle code die hergebruikt wordt doorheen de testen
        // wordt hier geinitialiseerd
        [SetUp] 
        public void Init()
        {
            naamlozeStudent = new Student();
            naamlozeStudent.FirstName = "";
            naamlozeStudent.LastName = "";

            janSchippers = new Student("Jan", "Schippers");
        }


        [Test]
        public void IsLegeStudentVoornaamJohn()
        {
            Assert.That(naamlozeStudent.FirstName, Is.EqualTo("John"));
        }

        [Test]
        public void IsLegeStudentAchternaamDoe()
        {
            Assert.That(naamlozeStudent.LastName, Is.EqualTo("Doe"));
        }

        [Test]
        public void IsNaamJanSchippers()
        {
            Assert.That(janSchippers.FirstName, Is.EqualTo("Jan"));
            Assert.That(janSchippers.LastName, Is.EqualTo("Schippers"));
        }

        [Test]
        public void IsIDNietGelijk()
        {
            Assert.That(janSchippers.ID, 
                Is.Not.EqualTo(naamlozeStudent.ID));
        }
    }
}

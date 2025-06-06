using System;
using NUnit.Framework;
using _3aWI_Projekt.Models;

namespace ModelsUnitest
{
    public class StudentUnitest
    {
        [Test]
        public void Constructor_SetsProperties()
        {
            var birthdate = new DateTime(2005, 1, 1);
            var student = new Student("John", "Doe", Person.Genders.m, birthdate, Student.SchoolClasses.Class1a, Student.Tracks.WI);

            Assert.That(student.Firstname, Is.EqualTo("John"));
            Assert.That(student.Lastname, Is.EqualTo("Doe"));
            Assert.That(student.Gender, Is.EqualTo(Person.Genders.m));
            Assert.That(student.SchoolClass, Is.EqualTo(Student.SchoolClasses.Class1a));
            Assert.That(student.Track, Is.EqualTo(Student.Tracks.WI));
            Assert.That(student.Age, Is.EqualTo(DateTime.Now.Year - birthdate.Year));
        }
    }
}

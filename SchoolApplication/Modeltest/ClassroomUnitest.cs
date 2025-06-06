using System;
using NUnit.Framework;
using _3aWI_Projekt.Models;

namespace ModelsUnitest
{
    public class ClassroomUnitest
    {
        private Classroom _room;
        private Student _student1;
        private Student _student2;

        [SetUp]
        public void Setup()
        {
            _room = new Classroom("R1", "large", 1, true);
            _student1 = new Student("John", "Doe", Person.Genders.m, new DateTime(2005,1,1), Student.SchoolClasses.Class1a, Student.Tracks.WI);
            _student2 = new Student("Jane", "Doe", Person.Genders.w, new DateTime(2005,1,1), Student.SchoolClasses.Class1a, Student.Tracks.WI);
        }

        [Test]
        public void AddStudent_WhenSpace_AddsStudent()
        {
            _room.AddStudent(_student1);
            Assert.That(_room.Students.Count, Is.EqualTo(1));
        }

        [Test]
        public void AddStudent_WhenNoSpace_DoesNotAddStudent()
        {
            _room.AddStudent(_student1);
            _room.AddStudent(_student2);
            Assert.That(_room.Students.Count, Is.EqualTo(1));
        }

        [Test]
        public void RemoveStudent_WhenExisting_RemovesStudent()
        {
            _room.AddStudent(_student1);
            _room.RemoveStudent(_student1);
            Assert.That(_room.Students, Does.Not.Contain(_student1));
        }

        [Test]
        public void RemoveStudent_WhenNotExisting_DoesNothing()
        {
            _room.AddStudent(_student1);
            _room.RemoveStudent(_student2);
            Assert.That(_room.Students.Count, Is.EqualTo(1));
        }

        [Test]
        public void ClearStudents_RemovesAll()
        {
            _room.AddStudent(_student1);
            _room.ClearStudents();
            Assert.That(_room.Students.Count, Is.EqualTo(0));
        }

        [Test]
        public void ChangeSeats_ValidValue_UpdatesProperty()
        {
            _room.ChangeSeats(5);
            Assert.That(_room.Seats, Is.EqualTo(5));
        }

        [Test]
        public void ChangeSeats_InvalidValue_DoesNotChange()
        {
            _room.ChangeSeats(-1);
            Assert.That(_room.Seats, Is.EqualTo(1));
        }

        [Test]
        public void ChangeCynap_SetsValue()
        {
            _room.ChangeCynap(false);
            Assert.That(_room.Cynap, Is.False);
        }
    }
}

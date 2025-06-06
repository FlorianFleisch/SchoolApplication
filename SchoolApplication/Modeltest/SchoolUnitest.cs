using System;
using System.Collections.Generic;
using NUnit.Framework;
using _3aWI_Projekt.Models;

namespace ModelsUnitest
{
    public class SchoolUnitest
    {
        private School _school;
        private Student _studentM;
        private Student _studentW;
        private Classroom _room1;
        private Classroom _room2;

        [SetUp]
        public void Setup()
        {
            _school = new School("HTL");
            _studentM = new Student("John", "Doe", Person.Genders.m, new DateTime(2000,1,1), Student.SchoolClasses.Class1a, Student.Tracks.WI);
            _studentW = new Student("Jane", "Doe", Person.Genders.w, new DateTime(2001,1,1), Student.SchoolClasses.Class1b, Student.Tracks.WI);
            _room1 = new Classroom("R1", "large", 30, true);
            _room2 = new Classroom("R2", "small", 1, false);
        }

        [Test]
        public void AddStudent_AddsToList()
        {
            _school.AddStudent(_studentM);
            Assert.That(_school.Students, Contains.Item(_studentM));
        }

        [Test]
        public void AddClassroom_AddsToList()
        {
            _school.AddClassroom(_room1);
            Assert.That(_school.Classrooms, Contains.Item(_room1));
        }

        [Test]
        public void GetNumberOfStudents_ReturnsCount()
        {
            _school.AddStudent(_studentM);
            _school.AddStudent(_studentW);
            Assert.That(_school.GetNumberOfStudents(), Is.EqualTo(2));
        }

        [Test]
        public void GetMaleAndFemaleStudentCount_ReturnsCounts()
        {
            _school.AddStudent(_studentM);
            _school.AddStudent(_studentW);
            var (male, female) = _school.GetMaleAndFemaleStudentCount();
            Assert.Multiple(() =>
            {
                Assert.That(male, Is.EqualTo(1));
                Assert.That(female, Is.EqualTo(1));
            });
        }

        [Test]
        public void GetNumberOfClassrooms_ReturnsCount()
        {
            _school.AddClassroom(_room1);
            _school.AddClassroom(_room2);
            Assert.That(_school.GetNumberOfClassrooms(), Is.EqualTo(2));
        }

        [Test]
        public void GetAverageAge_ReturnsAverage()
        {
            _school.AddStudent(_studentM);
            _school.AddStudent(_studentW);
            double expected = ((double)_studentM.Age + _studentW.Age) / 2;
            Assert.That(_school.GetAverageAge(), Is.EqualTo(expected));
        }

        [Test]
        public void GetClassroomsWithCynap_ReturnsOnlyWithCynap()
        {
            _school.AddClassroom(_room1);
            _school.AddClassroom(_room2);
            var result = _school.GetClassroomsWithCynap();
            Assert.That(result, Has.Count.EqualTo(1));
            Assert.That(result[0], Is.EqualTo(_room1));
        }

        [Test]
        public void GetNumberOfClasses_ReturnsDistinctCount()
        {
            _school.AddStudent(_studentM);
            _school.AddStudent(_studentW);
            Assert.That(_school.GetNumberOfClasses(), Is.EqualTo(2));
        }

        [Test]
        public void GetClassStudentCounts_ReturnsDictionary()
        {
            _school.AddStudent(_studentM);
            _school.AddStudent(_studentW);
            Dictionary<string, int> counts = _school.GetClassStudentCounts();
            Assert.That(counts[Student.SchoolClasses.Class1a.ToString()], Is.EqualTo(1));
            Assert.That(counts[Student.SchoolClasses.Class1b.ToString()], Is.EqualTo(1));
        }

        [Test]
        public void GetFemalePercentageInClass_ReturnsCorrectPercentage()
        {
            _school.AddStudent(_studentM);
            _school.AddStudent(_studentW);
            double percentage = _school.GetFemalePercentageInClass(Student.SchoolClasses.Class1a.ToString());
            Assert.That(percentage, Is.EqualTo(0));
            double percentage2 = _school.GetFemalePercentageInClass(Student.SchoolClasses.Class1b.ToString());
            Assert.That(percentage2, Is.EqualTo(100));
        }

        [Test]
        public void CanClassFitInRoom_ReturnsTrueOrFalse()
        {
            _school.AddStudent(_studentM);
            _school.AddStudent(_studentW);
            // add another student from the same class as _studentM so the
            // smaller room is insufficient
            var studentM2 = new Student("Jim", "Beam", Person.Genders.m,
                new DateTime(2002, 1, 1), Student.SchoolClasses.Class1a, Student.Tracks.WI);
            _school.AddStudent(studentM2);
            _school.AddClassroom(_room1);
            _school.AddClassroom(_room2);
            Assert.That(_school.CanClassFitInRoom(Student.SchoolClasses.Class1a.ToString(), _room1), Is.True);
            Assert.That(_school.CanClassFitInRoom(Student.SchoolClasses.Class1a.ToString(), _room2), Is.False);
        }
    }
}

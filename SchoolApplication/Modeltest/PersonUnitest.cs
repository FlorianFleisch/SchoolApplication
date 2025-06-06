using NUnit.Framework;
namespace ModelsUnitest
{
    public class Tests
    {
        private Person _Person;
        [SetUp]
        public void Setup()
        {
            _Person = new Person("Max", "Mustermann", 0, new DateTime(2000, 1, 1));
        }

        [Test]
        [TestCase("Max", "Mustermann", 0, "1/1/2000")]
        public void CreateObjektPerson_ReturnsObjektPerson(string firstname, string lastname,  Person.Genders gender, string birthdatetring)
        {
            DateTime birthdate = DateTime.Parse(birthdatetring);
            _Person = new Person(firstname, lastname, gender, birthdate);
            Assert.That(_Person.Firstname, Is.EqualTo(firstname));
            Assert.That(_Person.Lastname, Is.EqualTo(lastname));
            Assert.That(_Person.Gender, Is.EqualTo(gender));
            Assert.That(_Person.Age, Is.EqualTo(DateTime.Now.Year - birthdate.Year));
        }

        [Test]
        public void ChangeBirthday_ReturnsNewAge()
        {
            DateTime newBirthdate = new DateTime(1990, 1, 1);
            _Person.changeBirthday(newBirthdate);
            Assert.That(_Person.Age, Is.EqualTo(DateTime.Now.Year - newBirthdate.Year));
        }

        [Test]
        public void ChangeFirstname_UpdatesFirstname()
        {
            string newName = "John";
            _Person.changeFirstname(newName);
            Assert.That(_Person.Firstname, Is.EqualTo(newName));
        }

        [Test]
        public void ChangeLastname_UpdatesLastname()
        {
            string newName = "Doe";
            _Person.changeLastname(newName);
            Assert.That(_Person.Lastname, Is.EqualTo(newName));
        }
    }
}

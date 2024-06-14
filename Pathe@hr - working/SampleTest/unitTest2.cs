using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace UnitTestProject
{
    [TestClass]
    public class UnitTest2
    {
        [TestMethod]
        public void TestVoegFilmToe_GeenGenres()
        {
            string naam = "Film Zonder Genres";
            string genresInput = " ";
            string acteursInput = "Brad Pitt, Angelina Jolie";
            string omschrijving = "Een film zonder specifieke genres.";
            string inputDuur = "90";

            Assert.ThrowsException<ArgumentException>(() =>
            {
                AdminSystem.VoegFilmToe(naam, genresInput, acteursInput, omschrijving, inputDuur);
            });
        }
        [TestMethod]
        public void TestVoegFilmToe_GeenActeurs()
        {

            string naam = "Film Zonder Acteurs";
            string genresInput = "Komedie, Arthouse";
            string acteursInput = "";
            string omschrijving = "Een komische film zonder bekende acteurs.";
            string inputDuur = "105";

            Assert.ThrowsException<ArgumentException>(() =>
            {
                AdminSystem.VoegFilmToe(naam, genresInput, acteursInput, omschrijving, inputDuur);
            });
        }
        [TestMethod]
        public void TestVoegFilmToe()
        {
            string naam = "Test Film";
            string genresInput = "Drama, Actie";
            string acteursInput = "Dwayne Johnson, Sylvester Stallone";
            string omschrijving = "Een spannende actie-drama film.";
            string inputDuur = "120";

            Film result = AdminSystem.VoegFilmToe(naam, genresInput, acteursInput, omschrijving, inputDuur);

            Assert.IsNotNull(result);
            Assert.AreEqual(naam, result.Titel);
            CollectionAssert.AreEqual(new List<string> { "Drama", "Actie" }, result.Genres);
            CollectionAssert.AreEqual(new List<string> { "Dwayne Johnson", "Sylvester Stallone" }, result.Acteurs);
            Assert.AreEqual(omschrijving, result.Omschrijving);
            Assert.AreEqual(120, result.Duur);
        }

        [TestMethod]
        public void TestLogin()
        {
            // Simulate correct login
            string email = "admin12@gmail.com";
            string password = "admin12";
            bool result = AdminSystem.Login(email, password);
            Assert.IsTrue(result, "Login with correct credentials should succeed.");

            // Simulate incorrect email
            email = "wrong@gmail.com";
            password = "admin12";
            result = AdminSystem.Login(email, password);
            Assert.IsFalse(result, "Login with incorrect email should fail.");
        }

    }
}

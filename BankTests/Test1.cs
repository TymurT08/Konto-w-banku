using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Bank;

namespace BankTests
{
    [TestClass]
    public class KontoTests
    {
        [TestMethod]
        public void Constructor_ValidData_CreatesAccount()
        {
            var konto = new Konto("Jan Kowalski", 100m);
            Assert.AreEqual("Jan Kowalski", konto.Nazwa);
            Assert.AreEqual(100m, konto.Bilans);
            Assert.IsFalse(konto.CzyZablokowane);
        }

        [TestMethod]
        public void Constructor_EmptyName_ThrowsArgumentException()
        {
            try { new Konto(""); Assert.Fail(); } catch (ArgumentException) { }
            try { new Konto(null); Assert.Fail(); } catch (ArgumentException) { }
        }

        [TestMethod]
        public void Constructor_NegativeBalance_ThrowsArgumentOutOfRangeException()
        {
            try { new Konto("Jan", -10m); Assert.Fail(); } catch (ArgumentOutOfRangeException) { }
        }

        [TestMethod]
        public void Wplata_ValidAmount_IncreasesBalance()
        {
            var konto = new Konto("Jan", 100m);
            konto.Wplata(50m);
            Assert.AreEqual(150m, konto.Bilans);
        }

        [TestMethod]
        public void Wplata_NegativeOrZeroAmount_ThrowsArgumentOutOfRangeException()
        {
            var konto = new Konto("Jan", 100m);
            try { konto.Wplata(-10m); Assert.Fail(); } catch (ArgumentOutOfRangeException) { }
            try { konto.Wplata(0m); Assert.Fail(); } catch (ArgumentOutOfRangeException) { }
        }

        [TestMethod]
        public void Wyplata_ValidAmount_DecreasesBalance()
        {
            var konto = new Konto("Jan", 100m);
            konto.Wyplata(40m);
            Assert.AreEqual(60m, konto.Bilans);
        }

        [TestMethod]
        public void Wyplata_MoreThanBalance_ThrowsInvalidOperationException()
        {
            var konto = new Konto("Jan", 100m);
            try { konto.Wyplata(150m); Assert.Fail(); } catch (InvalidOperationException) { }
        }

        [TestMethod]
        public void Wyplata_NegativeOrZeroAmount_ThrowsArgumentOutOfRangeException()
        {
            var konto = new Konto("Jan", 100m);
            try { konto.Wyplata(-10m); Assert.Fail(); } catch (ArgumentOutOfRangeException) { }
            try { konto.Wyplata(0m); Assert.Fail(); } catch (ArgumentOutOfRangeException) { }
        }

        [TestMethod]
        public void Blokuj_Odblokuj_ChangesState()
        {
            var konto = new Konto("Jan");

            konto.BlokujKonto();
            Assert.IsTrue(konto.CzyZablokowane);

            konto.OdblokujKonto();
            Assert.IsFalse(konto.CzyZablokowane);
        }

        [TestMethod]
        public void BlockedAccount_Wplata_ThrowsInvalidOperationException()
        {
            var konto = new Konto("Jan", 100m);
            konto.BlokujKonto();
            try { konto.Wplata(50m); Assert.Fail(); } catch (InvalidOperationException) { }
        }

        [TestMethod]
        public void BlockedAccount_Wyplata_ThrowsInvalidOperationException()
        {
            var konto = new Konto("Jan", 100m);
            konto.BlokujKonto();
            try { konto.Wyplata(50m); Assert.Fail(); } catch (InvalidOperationException) { }
        }
    }
}
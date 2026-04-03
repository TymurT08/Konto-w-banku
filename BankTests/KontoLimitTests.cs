using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Bank;

namespace BankTests
{
    [TestClass]
    public class KontoLimitTests
    {
        [TestMethod]
        public void Constructor_ValidData_CreatesAccount()
        {
            var konto = new KontoLimit("Anna", 100m, 50m);
            Assert.AreEqual("Anna", konto.Nazwa);
            Assert.AreEqual(150m, konto.Bilans);
            Assert.AreEqual(50m, konto.LimitDebetowy);
        }

        [TestMethod]
        public void Constructor_NegativeLimit_ThrowsException()
        {
            try { new KontoLimit("Anna", 100m, -50m); Assert.Fail(); } catch (ArgumentOutOfRangeException) { }
        }

        [TestMethod]
        public void Wyplata_WithinBalance_DoesNotUseLimit()
        {
            var konto = new KontoLimit("Anna", 100m, 50m);
            konto.Wyplata(40m);
            Assert.AreEqual(110m, konto.Bilans);
            Assert.IsFalse(konto.CzyZablokowane);
        }

        [TestMethod]
        public void Wyplata_ExceedsBalanceButWithinLimit_UsesLimitAndBlocks()
        {
            var konto = new KontoLimit("Anna", 100m, 50m);
            konto.Wyplata(120m);
            Assert.AreEqual(-20m, konto.Bilans);
            Assert.IsTrue(konto.CzyZablokowane);
        }

        [TestMethod]
        public void Wyplata_ExceedsTotalFunds_ThrowsException()
        {
            var konto = new KontoLimit("Anna", 100m, 50m);
            try { konto.Wyplata(160m); Assert.Fail(); } catch (InvalidOperationException) { }
        }

        [TestMethod]
        public void Wplata_PartialDebtRepayment_RemainsBlocked()
        {
            var konto = new KontoLimit("Anna", 100m, 50m);
            konto.Wyplata(120m);
            konto.Wplata(10m);
            Assert.AreEqual(-10m, konto.Bilans);
            Assert.IsTrue(konto.CzyZablokowane);
        }

        [TestMethod]
        public void Wplata_FullDebtRepayment_UnblocksAndRestoresLimit()
        {
            var konto = new KontoLimit("Anna", 100m, 50m);
            konto.Wyplata(120m);
            konto.Wplata(20m);
            Assert.AreEqual(50m, konto.Bilans);
            Assert.IsFalse(konto.CzyZablokowane);
        }

        [TestMethod]
        public void Wplata_Overpayment_UnblocksAndIncreasesBalance()
        {
            var konto = new KontoLimit("Anna", 100m, 50m);
            konto.Wyplata(120m);
            konto.Wplata(30m);
            Assert.AreEqual(60m, konto.Bilans);
            Assert.IsFalse(konto.CzyZablokowane);
        }
    }
}
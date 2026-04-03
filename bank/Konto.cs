using System;

namespace Bank
{
    public class Konto
    {
        private string klient;
        private decimal bilans;
        private bool zablokowane = false;

        public Konto(string klient, decimal bilansNaStart = 0)
        {
            if (string.IsNullOrWhiteSpace(klient))
                throw new ArgumentException("Nazwa klienta nie może być pusta.");

            if (bilansNaStart < 0)
                throw new ArgumentOutOfRangeException(nameof(bilansNaStart), "Bilans początkowy nie może być ujemny.");

            this.klient = klient;
            this.bilans = bilansNaStart;
        }

        public string Nazwa => klient;
        public decimal Bilans => bilans;
        public bool CzyZablokowane => zablokowane;

        public void Wplata(decimal kwota)
        {
            if (zablokowane)
                throw new InvalidOperationException("Konto jest zablokowane. Operacja niemożliwa.");

            if (kwota <= 0)
                throw new ArgumentOutOfRangeException(nameof(kwota), "Kwota wpłaty musi być dodatnia.");

            bilans += kwota;
        }

        public void Wyplata(decimal kwota)
        {
            if (zablokowane)
                throw new InvalidOperationException("Konto jest zablokowane. Operacja niemożliwa.");

            if (kwota <= 0)
                throw new ArgumentOutOfRangeException(nameof(kwota), "Kwota wypłaty musi być dodatnia.");

            if (kwota > bilans)
                throw new InvalidOperationException("Brak wystarczających środków na koncie.");

            bilans -= kwota;
        }

        public void BlokujKonto()
        {
            zablokowane = true;
        }

        public void OdblokujKonto()
        {
            zablokowane = false;
        }
    }
}
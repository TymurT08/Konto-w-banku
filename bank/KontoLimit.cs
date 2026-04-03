using System;

namespace Bank
{
    public class KontoLimit
    {
        private Konto konto;
        private decimal limitDebetowy;
        private decimal wykorzystanyDebet = 0;

        public KontoLimit(string klient, decimal bilansNaStart = 0, decimal limitDebetowy = 0)
        {
            this.konto = new Konto(klient, bilansNaStart);

            if (limitDebetowy < 0)
                throw new ArgumentOutOfRangeException(nameof(limitDebetowy), "Limit debetowy nie może być ujemny.");

            this.limitDebetowy = limitDebetowy;
        }

        public string Nazwa => konto.Nazwa;

        public bool CzyZablokowane => konto.CzyZablokowane;

        public decimal LimitDebetowy
        {
            get => limitDebetowy;
            set
            {
                if (value < 0)
                    throw new ArgumentOutOfRangeException(nameof(value), "Limit nie może być ujemny.");
                limitDebetowy = value;
            }
        }

        public decimal Bilans => konto.Bilans - wykorzystanyDebet + (wykorzystanyDebet == 0 ? limitDebetowy : 0);

        public void Wyplata(decimal kwota)
        {
            if (konto.CzyZablokowane)
                throw new InvalidOperationException("Konto jest zablokowane.");
            if (kwota <= 0)
                throw new ArgumentOutOfRangeException(nameof(kwota), "Kwota musi być dodatnia.");

            if (kwota <= konto.Bilans)
            {
                konto.Wyplata(kwota);
            }
            else
            {
                if (wykorzystanyDebet == 0 && kwota <= konto.Bilans + limitDebetowy)
                {
                    decimal brakujacaKwota = kwota - konto.Bilans;

                    if (konto.Bilans > 0)
                    {
                        konto.Wyplata(konto.Bilans);
                    }

                    wykorzystanyDebet = brakujacaKwota;
                    konto.BlokujKonto();
                }
                else
                {
                    throw new InvalidOperationException("Brak środków lub jednorazowy limit został już wykorzystany.");
                }
            }
        }

        public void Wplata(decimal kwota)
        {
            if (kwota <= 0)
                throw new ArgumentOutOfRangeException(nameof(kwota), "Kwota musi być dodatnia.");

            if (wykorzystanyDebet > 0)
            {
                if (kwota > wykorzystanyDebet)
                {
                    decimal reszta = kwota - wykorzystanyDebet;
                    wykorzystanyDebet = 0;
                    konto.OdblokujKonto();
                    konto.Wplata(reszta);
                }
                else if (kwota == wykorzystanyDebet)
                {
                    wykorzystanyDebet = 0;
                    konto.OdblokujKonto();
                }
                else
                {
                    wykorzystanyDebet -= kwota;
                }
            }
            else
            {
                konto.Wplata(kwota);
            }
        }

        public void BlokujKonto() => konto.BlokujKonto();

        public void OdblokujKonto() => konto.OdblokujKonto();
    }
}
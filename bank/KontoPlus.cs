using System;

namespace Bank
{
    public class KontoPlus : Konto
    {
        private decimal limitDebetowy;
        private decimal wykorzystanyDebet = 0;

        public KontoPlus(string klient, decimal bilansNaStart = 0, decimal limitDebetowy = 0)
            : base(klient, bilansNaStart)
        {
            if (limitDebetowy < 0)
                throw new ArgumentOutOfRangeException(nameof(limitDebetowy), "Limit debetowy nie może być ujemny.");

            this.limitDebetowy = limitDebetowy;
        }

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

        public new decimal Bilans => base.Bilans - wykorzystanyDebet + (wykorzystanyDebet == 0 ? limitDebetowy : 0);

        public new void Wyplata(decimal kwota)
        {
            if (CzyZablokowane)
                throw new InvalidOperationException("Konto jest zablokowane.");
            if (kwota <= 0)
                throw new ArgumentOutOfRangeException(nameof(kwota), "Kwota musi być dodatnia.");

            if (kwota <= base.Bilans)
            {
                base.Wyplata(kwota);
            }
            else
            {
                if (wykorzystanyDebet == 0 && kwota <= base.Bilans + limitDebetowy)
                {
                    decimal brakujacaKwota = kwota - base.Bilans;

                    if (base.Bilans > 0)
                    {
                        base.Wyplata(base.Bilans);
                    }

                    wykorzystanyDebet = brakujacaKwota;
                    BlokujKonto();
                }
                else
                {
                    throw new InvalidOperationException("Brak środków lub jednorazowy limit został już wykorzystany.");
                }
            }
        }

        public new void Wplata(decimal kwota)
        {
            if (kwota <= 0)
                throw new ArgumentOutOfRangeException(nameof(kwota), "Kwota musi być dodatnia.");

            if (wykorzystanyDebet > 0)
            {
                if (kwota > wykorzystanyDebet)
                {
                    decimal reszta = kwota - wykorzystanyDebet;
                    wykorzystanyDebet = 0;
                    OdblokujKonto();
                    base.Wplata(reszta);
                }
                else if (kwota == wykorzystanyDebet)
                {
                    wykorzystanyDebet = 0;
                    OdblokujKonto();
                }
                else
                {
                    wykorzystanyDebet -= kwota;
                }
            }
            else
            {
                base.Wplata(kwota);
            }
        }
    }
}
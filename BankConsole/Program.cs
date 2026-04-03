using System;
using Bank;

namespace BankConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("--- Symulacja: Konto ---");
            Konto konto = new Konto("Jan", 100m);
            Console.WriteLine($"Konto: {konto.Nazwa}, Bilans: {konto.Bilans}");
            konto.Wplata(50m);
            Console.WriteLine($"Po wplacie 50: {konto.Bilans}");
            konto.Wyplata(30m);
            Console.WriteLine($"Po wyplacie 30: {konto.Bilans}");

            Console.WriteLine("\n--- Symulacja: KontoPlus ---");
            KontoPlus kontoPlus = new KontoPlus("Anna", 100m, 50m);
            Console.WriteLine($"KontoPlus: Bilans: {kontoPlus.Bilans}");
            kontoPlus.Wyplata(120m);
            Console.WriteLine($"Po wyplacie 120. Bilans: {kontoPlus.Bilans}. Zablokowane: {kontoPlus.CzyZablokowane}");
            kontoPlus.Wplata(20m);
            Console.WriteLine($"Po wplacie 20. Bilans: {kontoPlus.Bilans}. Zablokowane: {kontoPlus.CzyZablokowane}");

            Console.WriteLine("\n--- Symulacja: KontoLimit ---");
            KontoLimit kontoLimit = new KontoLimit("Piotr", 100m, 50m);
            Console.WriteLine($"KontoLimit: Bilans: {kontoLimit.Bilans}");
            kontoLimit.Wyplata(120m);
            Console.WriteLine($"Po wyplacie 120. Bilans: {kontoLimit.Bilans}. Zablokowane: {kontoLimit.CzyZablokowane}");

            Console.ReadLine();
        }
    }
}
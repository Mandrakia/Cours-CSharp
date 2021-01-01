# Correction des exercices



### Exercice 1 : 

```c#
using System;

namespace Exercice1{
    class Program{
        public static void Main(){
            var randomNumber = new Random().Next(1,500);
            var counter = 0;
            while(true)
            {
                Console.WriteLine("Devinez le nombre :");
                var text = Console.ReadLine();
                if(int.TryParse(text,out var number))
                {
                    counter++;
                    if(number > randomNumber)
                        Console.WriteLine("Trop grand");
                    else if(number < randomNumber)
                        Console.WriteLine("Trop petit");
                    else
                    {
                        Console.WriteLine("Vous avez trouvé !");
                        Console.WriteLine($"Cela vous a pris : {counter} essais");
                        break;
                    }
                }
                else
                    Console.WriteLine("Tapez un nombre...");
            }
        }
    }
}
```

### Exercice 2 :

```c#
using System;
using System.IO;

namespace Exercice1{
    class Program{
        public static void Main(){
            var lines = File.ReadAllLines("exercice2.txt");
            var result = 0;
            for(var i=0;i<lines.Length;i++)
            {
                var nb = int.Parse(lines[i]);
                result += nb;
            }
            Console.WriteLine($"Somme : {result}");
        }
    }
}
```

### Exercice 3 :

```c#
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace cours_csharp
{
    class FileIO{
        static public IEnumerable<string> GetContent(string path)
        {
            using(var fs = File.OpenRead(path))
            {
                StreamReader str = new StreamReader(fs);
                var line = string.Empty;
                while((line = str.ReadLine())!= null)
                {
                    yield return line;
                }
            }
        }
    }
    class SaleRecord{
        public string Region { get; set; }
        public string Country { get; set; }
        public string ItemType { get; set; }
        public decimal TotalRevenue  { get; set; }

        public SaleRecord(string line)
        {
            var sp = line.Split(',');
            Region = sp[0];
            Country = sp[1];
            ItemType = sp[2];
            TotalRevenue = Decimal.Parse(sp[11],System.Globalization.CultureInfo.InvariantCulture);
        }

        static public IEnumerable<SaleRecord> GetSales(string path)
        {
            return FileIO.GetContent(path).Skip(1).Select(a=> new SaleRecord(a));
        }
    }
    class Program
    {       
        static void Main()
        {
            var sales = SaleRecord.GetSales("sales.csv");
            var grps = sales
                .Where(a=> a.Country == "France")
                .GroupBy(a=> a.ItemType)
                .ToDictionary(a=> a.Key,a=> a.Sum(b=> b.TotalRevenue));
            foreach(var kv in grps.OrderByDescending(a=> a.Value))
            {
                Console.WriteLine($"Type {kv.Key} : revenue {kv.Value}$");
            }
        }
    }
}

```


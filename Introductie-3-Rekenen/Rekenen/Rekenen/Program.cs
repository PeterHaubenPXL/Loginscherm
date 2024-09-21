// See https://aka.ms/new-console-template for more information

Console.Write("Geef een getal in: ");
double getal1 = Convert.ToDouble(Console.ReadLine());

Console.Write("Geef nog een getal in: ");
double getal2 = Convert.ToDouble(Console.ReadLine());

Console.WriteLine();
Console.Write("De som = ");
Double som = getal1 + getal2;
Console.WriteLine(som);

Console.WriteLine();
Console.Write("Het verschil = ");
Double verschil = getal1 - getal2;
Console.WriteLine(verschil);

Console.WriteLine();
Console.Write("Het product = ");
Double product = getal1 * getal2;
Console.WriteLine(product);

Console.WriteLine();
Console.Write("Het quotient = ");
Double quotient = getal1 / getal2;
Console.WriteLine(quotient);

Console.WriteLine();
Console.WriteLine("------------------------");
Console.Read();
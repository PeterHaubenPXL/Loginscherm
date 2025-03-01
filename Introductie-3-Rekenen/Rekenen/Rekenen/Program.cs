// See https://aka.ms/new-console-template for more information

Console.Write("Geef een getal in: ");
double number1 = Convert.ToDouble(Console.ReadLine());

Console.Write("Geef nog een getal in: ");
double number2 = Convert.ToDouble(Console.ReadLine());

Console.WriteLine();
Console.Write("De som = ");
Double sum = number1 + number2;
Console.WriteLine(sum);

Console.WriteLine();
Console.Write("Het verschil = ");
Double subtract = number1 - number2;
Console.WriteLine(subtract);

Console.WriteLine();
Double multiplication = number1 * number2;
Console.Write($"Het product = {multiplication}");

Console.WriteLine();
Console.Write("Het quotient = ");
Double division = number1 / number2;
Console.WriteLine(division);

Console.WriteLine();
Console.WriteLine("------------------------");

Console.WriteLine("\nDruk op enter om af te sluiten");
Console.Read();
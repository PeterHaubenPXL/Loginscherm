// See https://aka.ms/new-console-template for more information

string groet = @"-----------------------------------
|                                 |
| Welkom bij Ethias Administratie |
|                                 |
-----------------------------------";

Console.WriteLine(groet);
Console.WriteLine();
Console.WriteLine("Geef de achternaam in van de klant: ");
string Achternaam = Console.ReadLine();
Console.WriteLine("Geef de voornaam in van de klant: ");
string Voornaam = Console.ReadLine();
Console.WriteLine("Geef de leeftijd in van de klant: ");
bool resultaat = int.TryParse(Console.ReadLine(), out int cijfer);

Console.WriteLine("Geef aan of de klant een man is(true /false): ");
resultaat = Boolean.TryParse(Console.ReadLine(), out bool isMan);

Console.WriteLine();
Console.WriteLine("----------------------------------");
Console.WriteLine("Ingevulde gegevens");
Console.WriteLine($"Achternaam: {Achternaam} - Voornaam: {Voornaam} - Leeftijd: {cijfer} - Man: {isMan}");



// See https://aka.ms/new-console-template for more information

string groet = @"  -----------------------------------
  |                                 |
  | Welkom bij Ethias Administratie |
  |                                 |
  -----------------------------------

       |\_/|
       | @ @  Welkom bij Ethias regestratie
       |   <>              _
       |  -/\-----____  ((| |))
       |              `\__| |
   ____|_       ___|   |____|
  /_/_____/____/_______|
 
";

Console.WriteLine(groet);
Console.WriteLine();

Console.WriteLine("Geef de achternaam in van de klant: ");
string surname = Console.ReadLine();

Console.WriteLine("Geef de voornaam in van de klant: ");
string name = Console.ReadLine();

Console.WriteLine("Geef de leeftijd in van de klant: ");
bool result = int.TryParse(Console.ReadLine(), out int age);

Console.WriteLine("Geef aan of de klant een man is(true /false): ");
result = Boolean.TryParse(Console.ReadLine(), out bool isMan);

Console.WriteLine();
Console.WriteLine("----------------------------------");
Console.WriteLine("Ingevulde gegevens");
Console.WriteLine($"Achternaam: {surname} - Voornaam: {name} - Leeftijd: {age} - Man: {isMan}");



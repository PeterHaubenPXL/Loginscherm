// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

Person p = new Person();
p.Naam = "Hauben";
p.Voornaam = "Peter";

Console.WriteLine(p.ToonInfo());

Console.ReadKey();

class Person()
{
    public string Naam { get; set; }
    public string Voornaam { get; set; }

    public string ToonInfo()
    {
        return Voornaam + " " + Naam;
    }

    public static DateTime ToonDatum()
    {
        return DateTime.Now;
    }
}

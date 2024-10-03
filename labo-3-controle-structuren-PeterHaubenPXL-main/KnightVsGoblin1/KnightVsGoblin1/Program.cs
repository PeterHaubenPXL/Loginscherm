﻿
int goblinHealth = 20;
int attackKnight = 10;
int attackGoblin = 5;



Console.Write("Hoeveeel levenspunten heeft de ridder: ");

bool isResult = int.TryParse(Console.ReadLine(), out int knightHealth);

if (!(knightHealth >= 0 && knightHealth <= 100))
{
    knightHealth = 100;
}

for (int i = 1; i < 5; i++)
{
    Console.Write("Geef een geldige actie in (1/2): Actie ",i);

    isResult = int.TryParse(Console.ReadLine(), out int actionPlayer);

    switch (actionPlayer)
    {
        case 1:
            knightHealth = knightHealth - attackGoblin;
            goblinHealth = goblinHealth + 1;
            Console.WriteLine("Goblin attacked Knight");
            Console.WriteLine();
            break;
        case 2:
            knightHealth = knightHealth + 2;
            goblinHealth = goblinHealth - attackKnight;
            Console.WriteLine("Knight attacked goblin");
            Console.WriteLine();
            break;
        default:
            Console.WriteLine("Geef een geldige actie in (1 / 2)");
            Console.ReadLine();
            break;
    }

    string result = (knightHealth <= 0) ? $"De ridder is dood" :
                      $"de ridder heeft {knightHealth} levenspunten";

    Console.WriteLine(result);

    result = (goblinHealth <= 0) ? $"De goblin is dood" :
                          $"de goblin heeft {goblinHealth} levenspunten";

    Console.WriteLine(result);

    if(goblinHealth<=0)
    {
        Console.WriteLine("Je bent gewonnen - De Goblin is dood -");
        break;
    }
    else if(knightHealth<=0)
    {
        Console.WriteLine("Game over, je ridder is dood");
        break;
    }

}



Console.ReadLine();

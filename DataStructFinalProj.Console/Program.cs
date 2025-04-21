using DataStructFinalProj.Logic;

Console.Clear();
//Initializing Dungeon and Welcome
GameRunner game = new GameRunner();

Console.WriteLine("Welcome to the Dungeon! We got fun and games!");
Console.WriteLine();

Console.WriteLine("Creating Dungeon...");
game.dungeon.SetupDungeonRandomized();
game.BuildChallengeBST(15);
game.PressAnyKeyToContinue();

game.player.DisplayPlayerStats();
Console.WriteLine();
game.inventory.DisplayInventory();
Console.WriteLine();
game.PressAnyKeyToContinue();

//Dungeon Loop
game.challengeTree.TraverseInteractive(game.dungeon, "1", "15", game.player, game.inventory, game);


//Post game summary
Console.WriteLine("\nGame Over!");
Console.WriteLine($"Final Stats - Health: {game.player.Health}, Strength: {game.player.Strength}, Agility: {game.player.Agility}, Intelligence: {game.player.Intelligence}");

if (game.treasureStack.Count > 0)
{
   Console.WriteLine($"\nYou collected {game.treasureStack.Count} treasures:");
   while (game.treasureStack.Count > 0)
   {
      var treasure = game.treasureStack.Pop();
      Console.WriteLine($"- {treasure.Name}");
   }
}
else
{
   Console.WriteLine("\nYou didn't find any treasure.");
}

Console.WriteLine("\nThanks for playing!");
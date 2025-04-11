namespace DataStructFinalProj.Logic;

public class GameRunner
{
   public Player player;
   public Inventory inventory;
   public DungeonGraph dungeon;
   private InventoryItem Sword = new InventoryItem("Sword", "Combat", 1, 0, 0);
   private InventoryItem HealthPotion = new InventoryItem("Health Potion", "Potion", 0, 0, 0);
   public bool Running
   {
      get
      {
         if (player.Health <= 0)
         {
            return false;
         }
         else
         {
            return true;
         }
      }
   }

   public GameRunner()
   {
      player = new Player(1, 1, 1, 20);
      inventory = new Inventory(player);
      dungeon = new DungeonGraph();

      //Always start with these items
      inventory.AddItem(Sword, player);
      inventory.AddItem(HealthPotion, player);
   }

   //TODO: Add dungeon making methods to control creation of the map
   //TODO: Add Challenge BST methods to control the challenges inside each room of the map

   public void DisplayPlayerStats(Player player)
   {
      Console.WriteLine("--Player Stats--");
      Console.WriteLine($"Health: {player.Health}");
      Console.WriteLine($"Strength: {player.Strength}");
      Console.WriteLine($"Agility: {player.Agility}");
      Console.WriteLine($"Intelligence: {player.Intelligence}");
   }

   public void PressAnyKeyToContinue()
   {
      Console.WriteLine();
      Console.WriteLine("Press any key to continue.");
      Console.ReadKey(true);
      Console.Clear();
   }
}
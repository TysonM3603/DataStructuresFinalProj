namespace DataStructFinalProj.Logic;

public class GameRunner
{
   public Player player;
   public Inventory inventory;
   public DungeonGraph dungeon;
   public BinarySearchTree challengeTree;
   public Stack<Treasure> treasureStack;
   private Random treasureRng;
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
      player = new Player(5, 5, 5, 20);
      inventory = new Inventory(player);
      dungeon = new DungeonGraph();
      challengeTree = new BinarySearchTree();
      treasureStack = new Stack<Treasure>();
      treasureRng = new Random();

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

   public Treasure GenerateRandomTreasure()
   {
      string[] names = { "Gold Coin", "Ruby Gem", "Ancient Scroll", "Mystic Orb" };
      int roll = new Random().Next(names.Length);

      return names[roll] switch
      {
         "Gold Coin" => new Treasure("Gold Coin", p => p.Health += 5),
         "Ruby Gem" => new Treasure("Ruby Gem", p => p.Strength += 1),
         "Ancient Scroll" => new Treasure("Ancient Scroll", p => p.Intelligence += 1),
         "Mystic Orb" => new Treasure("Mystic Orb", p => p.Agility += 1),
         _ => new Treasure("Dust", _ => Console.WriteLine("It's worthless..."))
      };
   }

   public void PressAnyKeyToContinue()
   {
      Console.WriteLine();
      Console.WriteLine("Press any key to continue.");
      Console.ReadKey(true);
      Console.Clear();
   }
}
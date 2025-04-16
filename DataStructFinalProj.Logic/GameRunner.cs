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
      inventory.AddNewItem("Lockpick", "Utility", 0, 0, 0, player);

      //Generates randomized dungeon and creates challenge BST
      StartGame();
   }

   public void StartGame()
   {
      dungeon.SetupDungeonRandomized();
      BuildChallengeBST(15); //Number can be changed for more or less challenges
   }


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

   public List<Challenge> GenerateChallenges(int count)
   {
      Random rng = new Random();
      List<Challenge> challenges = new();

      string[] types = { "Combat", "Puzzle", "Trap", "Magic" };
      string[] stats = { "Strength", "Agility", "Intelligence" };
      string[] items = { null, "Lockpick", "MagicKey", "Torch", "GrapplingHook" };

      for (int i = 0; i < count; i++)
      {
         string type = types[rng.Next(types.Length)];
         string requiredStat = stats[rng.Next(stats.Length)];
         int difficulty = rng.Next(1, 100);
         int requiredStatValue = rng.Next(1, 11);
         string? requiredItem = items[rng.Next(items.Length)];

         challenges.Add(new Challenge(type, difficulty, requiredStat, requiredStatValue, requiredItem));
      }
      return challenges;
   }

   public void BuildChallengeBST(int challengeCount)
   {
      var challenges = GenerateChallenges(challengeCount);

      foreach (var challenge in challenges)
      {
         challengeTree.Insert(challenge);
      }
      Console.WriteLine($"Challenge BST built with {challengeCount} challenges.");
   }



   public void PressAnyKeyToContinue()
   {
      Console.WriteLine();
      Console.WriteLine("Press any key to continue.");
      Console.ReadKey(true);
      Console.Clear();
   }
}
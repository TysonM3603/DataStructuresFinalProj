namespace DataStructFinalProj.Logic;

public class GameRunner
{
   public Player player;
   public Inventory inventory;
   public DungeonGraph dungeon;
   public BinarySearchTree challengeTree;
   public Stack<InventoryItem> treasureStack;
   private Random treasureRng;
   private InventoryItem Sword = new InventoryItem("Sword", "Combat", 1, 0, 0);
   private InventoryItem HealthPotion = new InventoryItem("Health Potion", "Potion", 0, 0, 0);
   public bool Running
   {
      get
      {
         if (player.Health <= 0)
         {
            player.Health = 0;
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
      treasureStack = new Stack<InventoryItem>();
      treasureRng = new Random();

      //Always start with these items
      inventory.AddItem(HealthPotion, player);
      inventory.AddNewItem("Lockpick", "Utility", 0, 0, 0, player);
      inventory.AddItem(Sword, player);

      //Generates randomized dungeon and creates challenge BST
      StartGame();
   }

   public void StartGame()
   {
      Console.WriteLine("Creating Dungeon...");
      dungeon.SetupDungeonRandomized();
      BuildChallengeBST(15); //Number can be changed for more or less challenges
   }

   public InventoryItem GenerateRandomTreasure()
   {
      string[] names = { "Gold Coin", "Ruby Gem", "Ancient Scroll", "Mystic Orb" };
      int roll = new Random().Next(names.Length);

      return names[roll] switch
      {
         "Gold Coin" => new InventoryItem("Gold Coin", "Health Boost", 0, 0, 0),
         "Ruby Gem" => new InventoryItem("Ruby Gem", "Strength", 2, 0, 0),
         "Ancient Scroll" => new InventoryItem("Ancient Scroll", "Intelligence", 0, 0, 2),
         "Swiftness Boots" => new InventoryItem("Swiftness Boots", "Agility", 0, 2, 0),
         _ => new InventoryItem("Dust", "Useless", 0, 0, 0)
      };
   }

   public List<Challenge> GenerateChallenges(int count)
   {
      Random rng = new Random();
      List<Challenge> challenges = new();

      string[] types = { "Combat", "Puzzle", "Trap", "Magic" };
      string[] stats = { "Strength", "Agility", "Intelligence" };
      string[] items = { "Lockpick", "MagicKey", "Torch", "GrapplingHook" };

      for (int i = 0; i < count; i++)
      {
         string type = types[rng.Next(types.Length)];
         string requiredStat = stats[rng.Next(stats.Length)];
         int difficulty = rng.Next(1, 100);
         int requiredValue = rng.Next(3, 11);

         // 80% stat-based, 20% item-based
         bool useItem = rng.Next(100) < 20;
         string? requiredItem = useItem ? items[rng.Next(items.Length)] : null;

         challenges.Add(new Challenge(type, difficulty, requiredStat, requiredValue, requiredItem));
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
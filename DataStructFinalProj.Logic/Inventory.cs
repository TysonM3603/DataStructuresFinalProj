namespace DataStructFinalProj.Logic;

public class Inventory
{
   private Queue<InventoryItem> inventory = new Queue<InventoryItem>();
   private Player player;

   public Inventory(Player player)
   {
      this.player = player;
   }


   public void AddItem(InventoryItem item, Player player)
   {
      if (inventory.Count >= 5)
      {
         ProcessNextItem(player);
      }
      inventory.Enqueue(item);
      InventoryStatCheck(player); // update stats after adding
   }

   public void AddNewItem(string name, string type, int strength, int agility, int intelligence, Player player)
   {
      if (inventory.Count >= 5)
      {
         ProcessNextItem(player);
      }
      InventoryItem item = new InventoryItem(name, type, strength, agility, intelligence);
      inventory.Enqueue(item);
      InventoryStatCheck(player);
   }

   public void ProcessNextItem(Player player)
   {
      if (inventory.Count == 0)
      {
         Console.WriteLine("Your inventory is empty.");
         return;
      }

      InventoryItem oldestItem = inventory.Dequeue();
      Console.WriteLine($"{oldestItem.Name} was removed.");

      // Update player stats after the item is removed
      InventoryStatCheck(player);
   }

   public void DisplayInventory()
   {
      if (inventory.Count == 0)
      {
         Console.WriteLine("Your inventory is empty.");
         return;
      }

      Console.WriteLine("Inventory:");
      foreach (var item in inventory)
      {
         Console.WriteLine($"- {item.Name}, Type: {item.Type}, Strength: {item.Strength}, Agility: {item.Agility}, Intelligence: {item.Intelligence}");
      }
   }

   public void InventoryStatCheck(Player player)
   {
      player.Strength = 1;
      player.Agility = 1;
      player.Intelligence = 1;

      foreach (var i in inventory)
      {
         if (i.Strength > 0)
         {
            player.Strength += i.Strength;
         }

         if (i.Agility > 0)
         {
            player.Agility += i.Agility;
         }

         if (i.Intelligence > 0)
         {
            player.Intelligence += i.Intelligence;
         }
      }
   }
}

public class InventoryItem
{
   public string Name { get; set; }
   public string Type { get; set; }
   public int Strength { get; set; }
   public int Agility { get; set; }
   public int Intelligence { get; set; }

   public InventoryItem(string name, string type, int strength, int agility, int intelligence)
   {
      this.Name = name;
      this.Type = type;
      this.Strength = strength;
      this.Agility = agility;
      this.Intelligence = intelligence;
   }
}
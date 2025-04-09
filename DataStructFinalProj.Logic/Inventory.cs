namespace DataStructFinalProj.Logic;

public class Inventory
{
   private Queue<InventoryItem> inventory = new Queue<InventoryItem>();

   public void AddItem(InventoryItem item)
   {
      if (inventory.Count >= 5)
      {
         ProcessNextItem();
      }
      inventory.Enqueue(item);
   }

   public void AddNewItem(string name, string type, int strength, int agility, int intelligence, int quantity)
   {
      if (inventory.Count >= 5)
      {
         ProcessNextItem();
      }
      InventoryItem item = new InventoryItem(name, type, strength, agility, intelligence, quantity);
      inventory.Enqueue(item);
      // Console.WriteLine($"Added {item.Quantity} {item.Name} to inventory."); //This is used to test to ensure Items are added to the inventory
   }

   public void ProcessNextItem()
   {
      if (inventory.Count == 0)
      {
         Console.WriteLine("Your inventory is empty.");
         return;
      }

      // var ItemToBeRemoved = inventory.Peek();
      InventoryItem oldestItem = inventory.Dequeue();
      Console.WriteLine($"{oldestItem.Name} was removed to make space for a new item.");
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
         Console.WriteLine($"- {item.Name}, Quantity: {item.Quantity}, Type: {item.Type}, Strength: {item.Strength}, Agility: {item.Agility}, Intelligence: {item.Intelligence}");
      }
   }

   public void InventoryStatCheck(Player player, InventoryItem item)
   {
      if (inventory.Contains(item))
      {
         if (item.Strength > 0)
         {
            player.Strength += item.Strength;
         }

         if (item.Agility > 0)
         {
            player.Agility += item.Agility;
         }

         if (item.Intelligence > 0)
         {
            player.Intelligence += item.Intelligence;
         }
      }
      else
      {
         player.Strength -= item.Strength;
         if (player.Strength < 1)
         {
            player.Strength = 1;
         }

         player.Agility -= item.Agility;
         if (player.Agility < 1)
         {
            player.Agility = 1;
         }

         player.Intelligence -= item.Intelligence;
         if (player.Intelligence < 1)
         {
            player.Intelligence = 1;
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
   public int Quantity { get; set; }

   public InventoryItem(string name, string type, int strength, int agility, int intelligence, int quantity)
   {
      this.Name = name;
      this.Type = type;
      this.Strength = strength;
      this.Agility = agility;
      this.Intelligence = intelligence;
      this.Quantity = quantity;
   }
}
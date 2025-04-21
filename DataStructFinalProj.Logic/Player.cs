namespace DataStructFinalProj.Logic;

public class Player
{
   public int Strength { get; set; }
   public int Agility { get; set; }
   public int Intelligence { get; set; }
   public int Health { get; set; }

   public Player(int strength, int agility, int intelligence, int health)
   {
      this.Strength = strength;
      this.Agility = agility;
      this.Intelligence = intelligence;
      this.Health = health;
   }

   public void DisplayPlayerStats()
   {
      Console.WriteLine("--Player Stats--");
      Console.WriteLine($"Health: {Health}");
      Console.WriteLine($"Strength: {Strength}");
      Console.WriteLine($"Agility: {Agility}");
      Console.WriteLine($"Intelligence: {Intelligence}");
   }
}
namespace DataStructFinalProj.Logic;

public class Treasure
{
   public string Name { get; }
   public Action<Player> ApplyEffect { get; }

   public Treasure(string name, Action<Player> effect)
   {
      Name = name;
      ApplyEffect = effect;
   }

   public void Use(Player player)
   {
      ApplyEffect(player);
      Console.WriteLine($"Used treasure: {Name}!");
   }
}
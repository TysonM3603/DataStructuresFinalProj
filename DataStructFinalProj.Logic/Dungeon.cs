namespace DataStructFinalProj.Logic;

public class DungeonGraph
{
   public Dictionary<string, List<DungeonEdge>> Graph { get; set; }

   public DungeonGraph()
   {
      Graph = new Dictionary<string, List<DungeonEdge>>();
   }

   public void AddRoom(string name)
   {
      if (Graph.ContainsKey(name))
      {
         Console.WriteLine($"{name} already exists.");
         return;
      }

      Graph[name] = new List<DungeonEdge>();
      Console.WriteLine($"{name} has been added.");
   }

   public void RemoveRoom(string name)
   {
      if (!Graph.ContainsKey(name))
      {
         Console.WriteLine($"{name} does not exist.");
         return;
      }

      foreach (var room in Graph.Keys.ToList())
      {
         Graph[room].RemoveAll(e => e.Destination == name);
      }

      Graph.Remove(name);
      Console.WriteLine($"{name} has been removed from the Graph.");
   }

   public void AddPath(string fromRoom, DungeonEdge toRoom)
   {
      if (!DoesRoomsExist(fromRoom))
      {
         Console.WriteLine($"Room {fromRoom} doesn't exist.");
         return;
      }

      if (Graph[fromRoom].Any(e => e.Destination == toRoom.Destination))
      {
         Console.WriteLine($"Path from {fromRoom} to {toRoom.Destination} already exists.");
         return;
      }

      Graph[fromRoom].Add(toRoom);
      Console.WriteLine($"Path from {fromRoom} to {toRoom.Destination} added.");
   }

   public void RemovePath(string fromRoom, string toRoom)
   {
      if (!DoesRoomsExist(fromRoom) || !DoesRoomsExist(toRoom))
      {
         Console.WriteLine($"One or both rooms do not exist.");
         return;
      }

      if (!Graph[fromRoom].Any(e => e.Destination == toRoom))
      {
         Console.WriteLine($"No paths exist there.");
         return;
      }

      Graph[fromRoom].RemoveAll(e => e.Destination == toRoom);
      Console.WriteLine($"Removed {fromRoom} to {toRoom}");
   }

   private bool DoesRoomsExist(string room) => Graph.ContainsKey(room);

   public bool DepthFirstSearchRecursive(string current, string target, HashSet<string> visited, List<string> path)
   {
      path.Add(current);
      visited.Add(current);

      // Base case
      if (current == target)
      {
         return true;
      }

      path.Remove(current);
      return false;
   }

   public void SetupDungeon()
   {
      string[] rooms = { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K" };
      foreach (string room in rooms)
      {
         AddRoom(room);
      }

      AddPath("A", new DungeonEdge("B", 2));
      AddPath("A", new DungeonEdge("C", 4));
      AddPath("B", new DungeonEdge("D", 1));
      AddPath("B", new DungeonEdge("E", 3));
      AddPath("C", new DungeonEdge("D", 2));
      AddPath("C", new DungeonEdge("F", 3));
      AddPath("D", new DungeonEdge("G", 4));
      AddPath("D", new DungeonEdge("H", 5));
      AddPath("E", new DungeonEdge("H", 2));
      AddPath("E", new DungeonEdge("B", 3)); // Cycle
      AddPath("F", new DungeonEdge("I", 2));
      AddPath("G", new DungeonEdge("J", 6));
      AddPath("H", new DungeonEdge("J", 3));
      AddPath("I", new DungeonEdge("J", 1));
      AddPath("F", new DungeonEdge("K", 3));
   }
}

public class DungeonEdge
{
   public string Destination { get; }
   public int Distance { get; }

   public DungeonEdge(string dest, int distance)
   {
      Destination = dest;
      Distance = distance;
   }
}
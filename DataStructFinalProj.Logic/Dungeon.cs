namespace DataStructFinalProj.Logic;

public class DungeonEdge
{
   public string Destination { get; }
   public int? RequiredStrength { get; set; }
   public int? RequiredAgility { get; set; }
   public int? RequiredIntelligence { get; set; }
   public string? RequiredItem { get; set; }

   public DungeonEdge(string dest, int? requiredStrength = null, int? requiredAgility = null, int? requiredIntelligence = null, string? requiredItem = null)
   {
      Destination = dest;
      RequiredStrength = requiredStrength;
      RequiredAgility = requiredAgility;
      RequiredIntelligence = requiredIntelligence;
      RequiredItem = requiredItem;
   }
}

public class DungeonGraph
{
   public Dictionary<string, List<DungeonEdge>> Graph { get; set; }

   public DungeonGraph()
   {
      Graph = new Dictionary<string, List<DungeonEdge>>();
   }

   public void AddRoom(string name)
   {
      if (!Graph.ContainsKey(name))
      {
         Graph[name] = new List<DungeonEdge>();
      }
   }

   public void AddPath(string fromRoom, DungeonEdge toRoom)
   {
      if (Graph.ContainsKey(fromRoom) && Graph.ContainsKey(toRoom.Destination))
      {
         if (!Graph[fromRoom].Any(e => e.Destination == toRoom.Destination))
         {
            Graph[fromRoom].Add(toRoom);
         }
      }
   }

   public bool DepthFirstSearchRecursive(string current, string target, HashSet<string> visited, List<string> path)
   {
      visited.Add(current);
      path.Add(current);

      if (current == target)
      {
         return true;
      }

      foreach (var edge in Graph[current])
      {
         if (!visited.Contains(edge.Destination))
         {
            if (DepthFirstSearchRecursive(edge.Destination, target, visited, path))
            {
               return true;
            }
         }
      }

      path.RemoveAt(path.Count - 1); // Backtrack
      return false;
   }

   public void SetupDungeonRandomized()
   {
      Random rng = new Random();

      // Create 15 rooms (can be changed for larger dungeon)
      for (int i = 1; i <= 15; i++)
      {
         AddRoom(i.ToString());
      }

      // Generate one guaranteed path (10–15 rooms long) to exit
      int pathLength = rng.Next(10, 16); // 10 to 15 rooms
      List<string> path = Enumerable.Range(1, 15).Select(i => i.ToString()).OrderBy(x => rng.Next()).Take(pathLength).ToList();

      for (int i = 0; i < path.Count - 1; i++)
      {
         AddPath(path[i], GenerateRandomEdge(path[i + 1], rng));
      }

      // Add extra random paths (optional edges)
      int extraEdges = rng.Next(10, 20);
      List<string> roomList = Graph.Keys.ToList();

      for (int i = 0; i < extraEdges; i++)
      {
         string from = roomList[rng.Next(roomList.Count)];
         string to = roomList[rng.Next(roomList.Count)];
         if (from != to && !Graph[from].Any(e => e.Destination == to))
         {
            AddPath(from, GenerateRandomEdge(to, rng));
         }
      }
      Console.WriteLine($"Random dungeon generated with a guaranteed path of length {path.Count}.");
   }

   private DungeonEdge GenerateRandomEdge(string destination, Random rng)
   {
      // 30% chance to require a stat, 30% chance to require an item, 40% chance for no requirement
      int roll = rng.Next(100);
      if (roll < 30)
      {
         // 30% chance to require Strength stat, 30% chance to require Agility stat, 30% chance to require Intelligence stat, 10% chance to require all three stats
         roll = rng.Next(100);
         if (roll < 30)
         {
            int requiredStrength = rng.Next(5, 11); // Strength 5–10
            return new DungeonEdge(destination, requiredStrength: requiredStrength);
         }
         else if (roll >= 30 && roll < 60)
         {
            int requiredAgility = rng.Next(5, 11); // Agility 5-10
            return new DungeonEdge(destination, requiredAgility: requiredAgility);
         }
         else if (roll >= 60 && roll < 90)
         {
            int requiredIntelligence = rng.Next(5, 11); // Intelligence 5-10
            return new DungeonEdge(destination, requiredIntelligence: requiredIntelligence);
         }
         else
         {
            int requiredStrength = rng.Next(5, 11); // Strength 5–10
            int requiredAgility = rng.Next(5, 11); // Agility 5-10
            int requiredIntelligence = rng.Next(5, 11); // Intelligence 5-10
            return new DungeonEdge(destination, requiredStrength: requiredStrength, requiredAgility: requiredAgility, requiredIntelligence: requiredIntelligence);
         }
      }
      else if (roll < 60)
      {
         string[] items = { "Lockpick", "MagicKey", "Torch", "GrapplingHook" };
         string item = items[rng.Next(items.Length)];
         return new DungeonEdge(destination, requiredItem: item);
      }
      else
      {
         return new DungeonEdge(destination); // No requirement
      }
   }

   // A utility to verify path exists from start to exit
   public bool IsPathToExit(string start, string exit)
   {
      var visited = new HashSet<string>();
      var path = new List<string>();
      return DepthFirstSearchRecursive(start, exit, visited, path);
   }

   public void PrintPathToExit(string start, string exit)
   {
      var visited = new HashSet<string>();
      var path = new List<string>();
      if (DepthFirstSearchRecursive(start, exit, visited, path))
      {
         Console.WriteLine($"Path from {start} to {exit}: {string.Join(" -> ", path)}");
      }
      else
      {
         Console.WriteLine($"No path found from {start} to {exit}.");
      }
   }
}
using System;
using System.Collections.Generic;

namespace DataStructFinalProj.Logic
{
   public class Challenge
   {
      public string Type { get; set; }
      public int Difficulty { get; set; }
      public string RequiredStat { get; set; }
      public int RequiredStatValue { get; set; }
      public string? RequiredItem { get; set; }

      public Challenge(string type, int difficulty, string requiredStat, int requiredStatValue, string? requiredItem = null)
      {
         Type = type;
         Difficulty = difficulty;
         RequiredStat = requiredStat;
         RequiredStatValue = requiredStatValue;
         RequiredItem = requiredItem;
      }
   }

   public class Node
   {
      public Challenge Challenge { get; set; }
      public Node? Left { get; set; }
      public Node? Right { get; set; }

      public Node(Challenge challenge)
      {
         Challenge = challenge;
         Left = null;
         Right = null;
      }
   }

   public class BinarySearchTree
   {
      public Node? RootNode { get; set; }
      public Stack<int> RoomStack = new();
      public Dictionary<int, Challenge> RoomChallengeMap = new();

      public void Insert(Challenge challenge)
      {
         RootNode = InsertNode(RootNode, challenge);
         RoomChallengeMap[challenge.Difficulty] = challenge;
      }

      private Node InsertNode(Node? node, Challenge challenge)
      {
         if (node == null)
         {
            return new Node(challenge);
         }

         if (challenge.Difficulty < node.Challenge.Difficulty)
         {
            node.Left = InsertNode(node.Left, challenge);
         }
         else if (challenge.Difficulty > node.Challenge.Difficulty)
         {
            node.Right = InsertNode(node.Right, challenge);
         }
         return node;
      }

      public Challenge? FindClosestChallenge(int roomNumber)
      {
         return FindClosestChallenge(RootNode, roomNumber, null);
      }

      private Challenge? FindClosestChallenge(Node? node, int target, Challenge? closest)
      {
         if (node == null)
         {
            return closest;
         }

         if (closest == null || Math.Abs(node.Challenge.Difficulty - target) < Math.Abs(closest.Difficulty - target))
         {
            closest = node.Challenge;
         }

         if (target < node.Challenge.Difficulty)
         {
            return FindClosestChallenge(node.Left, target, closest);
         }
         else if (target > node.Challenge.Difficulty)
         {
            return FindClosestChallenge(node.Right, target, closest);
         }
         else
         {
            return node.Challenge;
         }
      }

      public bool AttemptChallenge(Challenge challenge, Player player, Inventory playerItems)
      {
         if (challenge.RequiredItem != null && !playerItems.Contains(challenge.RequiredItem))
         {
            Console.WriteLine("Missing required item!");
            player.Health -= challenge.RequiredStatValue;
            return false;
         }

         int playerStatValue = challenge.RequiredStat
         switch
         {
            "Strength" => player.Strength,
            "Agility" => player.Agility,
            "Intelligence" => player.Intelligence,
            _ => 0
         };

         if (playerStatValue >= challenge.RequiredStatValue)
         {
            Console.WriteLine("Challenge succeeded!");
            DeleteNode(challenge.Difficulty);
            RoomChallengeMap.Remove(challenge.Difficulty);
            RebalanceTree();
            return true;
         }
         else
         {
            int damage = challenge.RequiredStatValue - playerStatValue;
            Console.WriteLine($"Challenge failed! Took {damage} damage.");
            player.Health -= damage;
            return false;
         }
      }

      public void DeleteNode(int difficulty)
      {
         RootNode = DeleteNode(RootNode, difficulty);
      }

      private Node? DeleteNode(Node? node, int target)
      {
         if (node == null)
         {
            return null;
         }

         if (target < node.Challenge.Difficulty)
         {
            node.Left = DeleteNode(node.Left, target);
         }
         else if (target > node.Challenge.Difficulty)
         {
            node.Right = DeleteNode(node.Right, target);
         }
         else
         {
            if (node.Left == null && node.Right == null)
            {
               return null;
            }
            if (node.Left == null || node.Right == null)
            {
               return node.Left ?? node.Right;
            }
            Challenge successor = GetMinValue(node.Right);
            node.Challenge = successor;
            node.Right = DeleteNode(node.Right, successor.Difficulty);
         }
         return node;
      }

      private Challenge GetMinValue(Node node)
      {
         while (node.Left != null)
         {
            node = node.Left;
         }
         return node.Challenge;
      }

      public void RebalanceTree()
      {
         List<Challenge> sorted = new();
         InOrderTraversal(RootNode, sorted);
         RootNode = BuildBalancedTree(sorted, 0, sorted.Count - 1);
      }

      private void InOrderTraversal(Node? node, List<Challenge> list)
      {
         if (node == null)
         {
            return;
         }
         InOrderTraversal(node.Left, list);
         list.Add(node.Challenge);
         InOrderTraversal(node.Right, list);
      }

      private Node? BuildBalancedTree(List<Challenge> list, int start, int end)
      {
         if (start > end)
         {
            return null;
         }
         int mid = (start + end) / 2;
         Node node = new(new Challenge(
             list[mid].Type,
             list[mid].Difficulty,
             list[mid].RequiredStat,
             list[mid].RequiredStatValue,
             list[mid].RequiredItem));
         node.Left = BuildBalancedTree(list, start, mid - 1);
         node.Right = BuildBalancedTree(list, mid + 1, end);
         return node;
      }

      public void TraverseDungeonWithChallenges(DungeonGraph dungeon, string startRoom, string exitRoom, Player player, Inventory playerItems, GameRunner runner)
      {
         Stack<string> pathStack = new Stack<string>();
         HashSet<string> visited = new HashSet<string>();

         pathStack.Push(startRoom);

         while (pathStack.Count > 0)
         {
            string currentRoom = pathStack.Peek();
            if (!visited.Contains(currentRoom))
            {
               Console.WriteLine($"\nEntered Room {currentRoom}");
               visited.Add(currentRoom);

               if (int.TryParse(currentRoom, out int roomNumber))
               {
                  var challenge = FindClosestChallenge(roomNumber);
                  if (challenge != null)
                  {
                     Console.WriteLine($"Facing Challenge: {challenge.Type} | Difficulty {challenge.Difficulty}");
                     AttemptChallenge(challenge, player, playerItems);
                  }
               }

               // 10% chance to find treasure
               if (new Random().Next(100) < 10)
               {
                  InventoryItem treasure = runner.GenerateRandomTreasure();
                  Console.WriteLine($"You found a treasure: {treasure.Name}!");
                  runner.treasureStack.Push(treasure);
               }

               if (currentRoom == exitRoom)
               {
                  Console.WriteLine("Exit found! Dungeon traversal complete.");
                  return;
               }
            }

            bool moved = false;
            foreach (var edge in dungeon.Graph[currentRoom])
            {
               if (!visited.Contains(edge.Destination) && CanTraverseEdge(edge, player, playerItems))
               {
                  pathStack.Push(edge.Destination);
                  moved = true;
                  break;
               }
            }

            if (!moved)
            {
               Console.WriteLine($"Dead end at Room {currentRoom}. Backtracking...");
               pathStack.Pop();
            }

            if (player.Health <= 0 || RoomChallengeMap.Count == 0)
            {
               Console.WriteLine("Game Over!");
               Console.WriteLine("Displaying path to the exit...");
               dungeon.PrintPathToExit(startRoom, exitRoom);
               return;
            }
         }
         Console.WriteLine("No path to exit found.");
      }

      private bool CanTraverseEdge(DungeonEdge edge, Player player, Inventory items)
      {
         if (edge.RequiredItem != null && !items.Contains(edge.RequiredItem))
         {
            return false;
         }

         if (edge.RequiredStrength.HasValue && player.Strength < edge.RequiredStrength.Value)
         {
            return false;
         }

         if (edge.RequiredAgility.HasValue && player.Agility < edge.RequiredAgility.Value)
         {
            return false;
         }

         if (edge.RequiredIntelligence.HasValue && player.Intelligence < edge.RequiredIntelligence.Value)
         {
            return false;
         }
         return true;
      }
   }
}
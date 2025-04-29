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
      public bool Seen { get; set; } = false;
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

         if (!node.Challenge.Seen && (closest == null || Math.Abs(node.Challenge.Difficulty - target) < Math.Abs(closest.Difficulty - target)))
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
            Console.WriteLine($"Challenge failed! Took {challenge.RequiredStatValue} damage.");
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

      public void TraverseInteractive(DungeonGraph dungeon, string startRoom, string exitRoom, Player player, Inventory inventory, GameRunner runner)
      {
         Stack<string> movementHistory = new();
         string currentRoom = startRoom;
         movementHistory.Push(currentRoom);
         HashSet<string> visited = [currentRoom];

         while (true)
         {
            Console.Clear();
            Console.WriteLine($"You are in Room {currentRoom}.");

            // Challenge phase
            if (int.TryParse(currentRoom, out int roomNumber))
            {
               var challenge = FindClosestChallenge(roomNumber);
               if (new Random().Next(100) < 50 && challenge != null) // There is a 50% chance of a challenge happening when entering a room
               {
                  Console.WriteLine($"Facing Challenge: {challenge.Type} (Difficulty {challenge.Difficulty})");
                  challenge.Seen = true;

                  if (challenge.RequiredItem != null)
                  {
                     bool hasItem = inventory.Contains(challenge.RequiredItem);
                     Console.WriteLine($"- Requires item: {challenge.RequiredItem} | You {(hasItem ? "have" : "don't have")} it.");
                  }
                  else
                  {
                     int playerStatValue = challenge.RequiredStat
                     switch
                     {
                        "Strength" => player.Strength,
                        "Agility" => player.Agility,
                        "Intelligence" => player.Intelligence,
                        _ => 0
                     };
                     Console.WriteLine($"- Requires {challenge.RequiredStat} ≥ {challenge.RequiredStatValue} | You have: {playerStatValue}");
                  }
                  runner.PressAnyKeyToContinue();
                  AttemptChallenge(challenge, player, inventory);

                  // Early end if health or challenges run out
                  if (player.Health <= 0 || RoomChallengeMap.Count == 0)
                  {
                     Console.Clear();
                     Console.WriteLine("Game over! Player died or no more challenges remain.");
                     dungeon.PrintPathToExit(currentRoom, exitRoom);
                     return;
                  }
               }
               else
               {
                  Console.WriteLine("There is no challenge in this room.");
               }
            }

            // 10% chance to find treasure
            if (new Random().Next(100) < 10)
            {
               var treasure = runner.GenerateRandomTreasure();
               Console.WriteLine($"You found a treasure: {treasure.Name}!");
               runner.treasureStack.Push(treasure);
            }

            // Reached exit
            if (currentRoom == exitRoom && player.Health > 0)
            {
               Console.WriteLine("Congratulations! You've reached the exit!");
               return;
            }

            Console.WriteLine("\nWould you like to view your inventory and stats or use an item? (type 'view', 'use', or 'skip')");
            string? action = Console.ReadLine()?.ToLower();

            if (action == "view")
            {
               player.DisplayPlayerStats();
               inventory.DisplayInventory();
            }
            else if (action == "use")
            {
               inventory.DisplayInventory();
               Console.WriteLine("Enter the name of the item you want to use:");

               string? itemName = Console.ReadLine();
               if (string.IsNullOrWhiteSpace(itemName))
               {
                  Console.WriteLine("No item used.");
               }
               else
               {
                  var used = UseInventoryItem(itemName.Trim(), inventory, player);
                  if (!used)
                  {
                     Console.WriteLine("Could not use item.");
                  }
               }
            }
            runner.PressAnyKeyToContinue();
            Console.Clear();

            // Show path options
            Console.WriteLine("\nAvailable paths from this room:");

            var availableEdges = dungeon.Graph[currentRoom];
            for (int i = 0; i < availableEdges.Count; i++)
            {
               var edge = availableEdges[i];
               string extra = "";

               if (edge.RequiredItem != null)
                  extra = $"Requires item: {edge.RequiredItem}";
               else if (edge.RequiredStrength.HasValue)
                  extra = $"Requires Strength ≥ {edge.RequiredStrength}";
               else if (edge.RequiredAgility.HasValue)
                  extra = $"Requires Agility ≥ {edge.RequiredAgility}";
               else if (edge.RequiredIntelligence.HasValue)
                  extra = $"Requires Intelligence ≥ {edge.RequiredIntelligence}";

               Console.WriteLine($"{i + 1}: Room {edge.Destination} {(!string.IsNullOrEmpty(extra) ? $"| {extra}" : "")}");
            }

            // Prompt
            Console.WriteLine("Enter the number of the room you want to move to, or type 'back' to return to the previous room:");

            string? choice = Console.ReadLine()?.ToLower();

            if (choice == "back")
            {
               if (movementHistory.Count > 1)
               {
                  movementHistory.Pop(); // remove current room
                  currentRoom = movementHistory.Peek(); // go back
                  Console.WriteLine($"You return to Room {currentRoom}.");
                  runner.PressAnyKeyToContinue();
                  continue;
               }
               else
               {
                  Console.WriteLine("You're at the starting room. You can't go back further.");
                  runner.PressAnyKeyToContinue();
                  continue;
               }
            }
            else if (int.TryParse(choice, out int selected) && selected >= 1 && selected <= availableEdges.Count)
            {
               var chosenEdge = availableEdges[selected - 1];
               if (chosenEdge.HasBeenSeen == false)
               {
                  chosenEdge.HasBeenSeen = true;

                  if (!CanTraverseEdge(chosenEdge, player, inventory))
                  {
                     Console.WriteLine("\nYou fail to meet the path challenge!");

                     if (chosenEdge.RequiredItem != null && !inventory.Contains(chosenEdge.RequiredItem))
                     {
                        Console.WriteLine($"You are missing required item: {chosenEdge.RequiredItem}");
                     }
                     else
                     {
                        if (chosenEdge.RequiredStrength.HasValue && player.Strength < chosenEdge.RequiredStrength)
                        {
                           Console.WriteLine($"Strength too low! Need {chosenEdge.RequiredStrength}, you have {player.Strength}");
                        }
                        if (chosenEdge.RequiredAgility.HasValue && player.Agility < chosenEdge.RequiredAgility)
                        {
                           Console.WriteLine($"Agility too low! Need {chosenEdge.RequiredAgility}, you have {player.Agility}");
                        }
                        if (chosenEdge.RequiredIntelligence.HasValue && player.Intelligence < chosenEdge.RequiredIntelligence)
                        {
                           Console.WriteLine($"Intelligence too low! Need {chosenEdge.RequiredIntelligence}, you have {player.Intelligence}");
                        }
                     }

                     Console.WriteLine("You force your way through the path and take 3 damage!");
                     player.Health -= 3;

                     if (player.Health <= 0)
                     {
                        Console.WriteLine("You died while trying to force your way through...");
                        dungeon.PrintPathToExit(currentRoom, exitRoom);
                        return;
                     }
                     currentRoom = chosenEdge.Destination;
                     movementHistory.Push(currentRoom);
                     visited.Add(currentRoom);
                     runner.PressAnyKeyToContinue();
                  }
                  else
                  {
                     currentRoom = chosenEdge.Destination;
                     movementHistory.Push(currentRoom);
                     visited.Add(currentRoom);
                  }
               }
               else
               {
                  Console.WriteLine("You already used this path. Proceeding...");
               }
            }
            else
            {
               Console.WriteLine("Invalid input.");
               runner.PressAnyKeyToContinue();
            }
         }
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

      private bool UseInventoryItem(string name, Inventory inventory, Player player)
      {
         //TODO: add usability for other items than just a health potion
         if (name.ToLower() == "health potion" && inventory.Contains("Health Potion"))
         {
            player.Health += 10;
            if (player.Health > 20)
            {
               player.Health = 20;
            }
            Console.WriteLine("You used a Health Potion and restored 10 health!");
            inventory.ProcessNextItem(player);
            return true;
         }
         Console.WriteLine("You can't use that item here.");
         return false;
      }

   }
}
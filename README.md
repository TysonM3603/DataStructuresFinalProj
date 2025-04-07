# DataStructuresFinalProj
Repo for my DataStructures final project.

Instructions:
1. Character Creation:
   * Define a hero with:
      - Strength (affects combat),
      - Agility (affects traps),
      - Intelligence (affects puzzles),
      - Health (starts at 20; game ends if it reaches 0).
   * Implement an inventory (max 5 items) using a queue (FIFO: discard oldest item when full). Start with 2 items (e.g., Sword, Health Potion).

2. Map Generation:
   * Represent the map as a graph with at least 15 nodes (rooms) and edges (paths).
   * Each room has a unique number (1–15+). Some edges require specific stats or items (e.g., Strength > 8 or a Lockpick).
   * Designate one node as the “exit” (e.g., a boss room or portal). Ensure a valid path from start to exit exists, spanning 10–15 nodes, verifiable via BFS or DFS.

3. Challenge System:
   * Store challenges in a binary search tree (BST), where each node represents a challenge (e.g., Puzzle, Combat, Trap) with a difficulty number (1–20).
   * When entering a room, use the room’s number to search the BST for the closest difficulty node (e.g., Room 7 finds the nearest challenge like 6 or 8).
   * Challenges require specific stats or items to succeed (e.g., Combat: Strength > 5 or Sword). On failure, lose health equal to the stat difference (e.g., Strength 4 vs. 6 = -2 health).
   * After completing a challenge, remove its node and rebalance the BST (self-balancing like AVL is optional but justify your approach).

4. Exploration Mechanics:
   * Use a stack to track visited rooms and avoid dead-end paths (pop dead ends from future options).
   * Store room data (e.g., number to challenge mapping) in a dictionary for O(1) lookups.

5. Treasure and Winning:
   * Each room has a low chance (e.g., 10%) to yield treasure, stored in a stack (LIFO: last found, first used). Treasures (e.g., Gold, Gems) can boost stats or earn bonus points.
   * Win by reaching the exit node with health > 0. If health hits 0 or the BST empties, print a valid path to the exit and end.

6. Big O Analysis:
   * Document the time complexity of at least 3 operations (e.g., BST search, graph traversal, inventory management) in comments or a README.

Big O Analysis: 

Notes on Implementation: 
- Map: Graph with at least 15 nodes (rooms) and edges (paths)
- Challenge System: BST where each node represents a challenge with a difficulty number (1-20). Challenges completed are removed from the BST and then is rebalanced. Challenges require specific stats or items to succeed.
- Exploration Mechanics: Stack to track visited rooms and avoid dead-end paths. Store room data in a dictionary for O(1) lookups.
- Treasure and Winning: Each room has a low chance to yield treasure, stored in a stack (LIFO). Treasures can boost stats or earn bonus points. Win by reaching the exit node with health > 0. If health hits 0 or BST empties, print a valid path to the exit and end.
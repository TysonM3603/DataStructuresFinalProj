namespace DataStructFinalProj.Logic;

public class BinarySearchTree
{
   public Node? RootNode { get; set; }
   public int DepthCounter { get; set; }

   public BinarySearchTree()
   {
      RootNode = null;
   }

   public void Insert(int data)
   {
      RootNode = InsertNode(RootNode, data);
   }

   public Node InsertNode(Node node, int data)
   {
      if (node == null)
      {
         return new Node(data);
      }

      if (data < node.Data)
      {
         node.Left = InsertNode(node.Left, data);
      }
      else if (data > node.Data)
      {
         node.Right = InsertNode(node.Right, data);
      }

      return node;
   }

   public void InsertIteratively(int data)
   {
      if (RootNode == null)
      {
         RootNode = new Node(data);
         return;
      }

      Node current = RootNode;
      while (true)
      {
         if (data < current.Data)
         {
            if (current.Left == null)
            {
               current.Left = new Node(data);
               return;
            }
            current = current.Left;
         }
         else if (data > current.Data)
         {
            if (current.Right == null)
            {
               current.Right = new Node(data);
               return;
            }
            current = current.Right;
         }
         else
         {
            return;
         }
      }
   }

   public void DeleteNode(Node target)
   {
      DeleteNode(target.Data);
   }

   public void DeleteNode(int target)
   {
      RootNode = DeleteNode(RootNode, target);
   }

   public Node? DeleteNode(Node currentNode, int target)
   {
      if (currentNode == null)
      {
         return currentNode;
      }

      if (target < currentNode.Data)
      {
         currentNode.Left = DeleteNode(currentNode.Left, target); //search left
      }
      else if (target > currentNode.Data)
      {
         currentNode.Right = DeleteNode(currentNode.Right, target); //search right
      }
      else
      {
         //Found the number

         //Leaf
         if (currentNode.Left == null && currentNode.Right == null)
         {
            return null;
         }

         // 1 Child
         if (currentNode.Left == null || currentNode.Right == null)
         {
            //Node? result = currentNode.Left == null ? currentNode.Right : currentNode.Left;
            return currentNode.Left == null ? currentNode.Right : currentNode.Left;
         }

         // 2 Children
         currentNode.Data = GetMinValue(currentNode.Right);
         currentNode.Right = DeleteNode(currentNode.Right, currentNode.Data);
      }
      return currentNode;
   }

   public void Balanced()
   {
      if (IsBalanced() == true)
      {
         Console.WriteLine("The tree is balanced.");
      }
      else
      {
         Console.WriteLine("The tree is not balanced.");
         Console.WriteLine("Rebalancing inventory tree...");
         RebalanceTree();
         Balanced();
      }
   }

   public bool IsBalanced()
   {
      return CheckHeight(RootNode) != -1;
   }

   public int CheckHeight(Node? node)
   {
      if (node == null)
      {
         return 0;
      }

      int leftHeight = CheckHeight(node.Left);
      if (leftHeight == -1)
      {
         return -1;
      }

      int rightHeight = CheckHeight(node.Right);
      if (rightHeight == -1)
      {
         return -1;
      }

      if (Math.Abs(leftHeight - rightHeight) > 1)
      {
         return -1;
      }

      return Math.Max(leftHeight, rightHeight) + 1;
   }

   public void InOrderTraversal(List<InventoryItem> inventoryItems)
   {
      InOrderTraversal(RootNode, inventoryItems);
   }

   public void InOrderTraversal(Node? node, List<InventoryItem> Items)
   {
      if (node == null)
      {
         return;
      }

      InOrderTraversal(node.Left, Items);
      for (int i = 0; i < Items.Count; i++)
      {
         if (Items[i].ID == node.Data)
         {
            Console.WriteLine($"Item ID: {Items[i].ID}; Item Name: {Items[i].Name}; Item Type: {Items[i].Type}; Item Rarity: {Items[i].Rarity}; Item Strength: {Items[i].Strength};");
         }
      }
      InOrderTraversal(node.Right, Items);
   }

   public void DescendingOrder()
   {
      DescendingOrder(RootNode);
   }
   public void DescendingOrder(Node? node)
   {
      if (node == null)
      {
         return;
      }

      DescendingOrder(node.Right);
      Console.WriteLine(node.Data);
      DescendingOrder(node.Left);
   }

   public void PostOrderTraversal()
   {
      PostOrderTraversal(RootNode);
   }
   public void PostOrderTraversal(Node? node)
   {
      if (node == null)
      {
         return;
      }

      PostOrderTraversal(node.Left);
      PostOrderTraversal(node.Right);
      Console.WriteLine(node.Data);
   }

   public void PreOrderTraversal()
   {
      PreOrderTraversal(RootNode);
   }
   public void PreOrderTraversal(Node? node)
   {
      if (node == null)
      {
         return;
      }

      Console.WriteLine(node.Data);
      PreOrderTraversal(node.Left);
      PreOrderTraversal(node.Right);
   }

   public void LevelOrderTraversal()
   {
      if (RootNode == null)
      {
         return;
      }

      Queue<Node> queue = new Queue<Node>();
      queue.Enqueue(RootNode);

      while (queue.Count > 0)
      {
         Node current = queue.Dequeue();
         Console.WriteLine(current.Data);

         if (current.Left != null)
         {
            queue.Enqueue(current.Left);
         }
         if (current.Right != null)
         {
            queue.Enqueue(current.Right);
         }
      }
   }

   public bool Search(int target)
   {
      int closest = int.MaxValue;
      bool found = SearchRecursiveWithClosest(RootNode, target, ref closest);

      if (!found)
      {
         Console.WriteLine($"Item with ID {target} not found.");
         if (closest != int.MaxValue)
         {
            Console.WriteLine($"Did you mean ID {closest}?");
         }
      }
      return found;
   }

   private bool SearchRecursiveWithClosest(Node? node, int target, ref int closest)
   {
      if (node == null)
      {
         return false;
      }

      DepthCounter++;

      if (Math.Abs(node.Data - target) < Math.Abs(closest - target))
      {
         closest = node.Data;
      }

      if (target == node.Data)
      {
         return true;
      }
      else if (target < node.Data)
      {
         return SearchRecursiveWithClosest(node.Left, target, ref closest);
      }
      else
      {
         return SearchRecursiveWithClosest(node.Right, target, ref closest);
      }
   }


   public int GetMinValue(Node? currentNode)
   {
      if (currentNode == null)
      {
         throw new ArgumentNullException(nameof(currentNode), "Tree is empty.");
      }

      while (currentNode.Left != null)
      {
         currentNode = currentNode.Left;
      }

      return currentNode.Data;
   }

   public int GetMaxValue()
   {
      Node result = RootNode;

      while (result.Right != null)
      {
         result = result.Right;
      }
      return result.Data;
   }

   public void RebalanceTree()
   {
      List<int> sortedNodes = new List<int>();
      StoreInOrder(RootNode, sortedNodes);
      RootNode = BuildAVLTree(sortedNodes, 0, sortedNodes.Count - 1);
   }

   private void StoreInOrder(Node? node, List<int> nodes)
   {
      if (node == null)
         return;
      StoreInOrder(node.Left, nodes);
      nodes.Add(node.Data);
      StoreInOrder(node.Right, nodes);
   }

   private Node? BuildAVLTree(List<int> nodes, int start, int end)
   {
      if (start > end)
         return null;

      int mid = (start + end) / 2;
      Node newNode = new Node(nodes[mid]);

      newNode.Left = BuildAVLTree(nodes, start, mid - 1);
      newNode.Right = BuildAVLTree(nodes, mid + 1, end);

      return newNode;
   }

   public void OptimizeTree()
   {
      List<int> sortedNodes = new List<int>();
      StoreInOrder(RootNode, sortedNodes);
      RootNode = BuildBalancedTree(sortedNodes, 0, sortedNodes.Count - 1);
   }

   private Node? BuildBalancedTree(List<int> nodes, int start, int end)
   {
      if (start > end)
         return null;

      int mid = (start + end) / 2;
      Node newNode = new Node(nodes[mid]);

      newNode.Left = BuildBalancedTree(nodes, start, mid - 1);
      newNode.Right = BuildBalancedTree(nodes, mid + 1, end);

      return newNode;
   }
}

public class Node
{
   public int Data { get; set; }
   public Node? Left { get; set; }
   public Node? Right { get; set; }

   public Node(int data)
   {
      Data = data;
      Left = null;
      Right = null;
   }
}
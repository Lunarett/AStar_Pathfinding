using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinder
{
	private const int MOVE_STRAIGHT_COST = 10;
	private const int MOVE_DIAGONAL_COST = 14;

	private Grid<Node> m_grid;

	private List<Node> m_openList;
	private List<Node> m_closedList;

	public Grid<Node> Grid { get { return m_grid; } }

	public static Pathfinder Instance { get; private set; }

	public Pathfinder(int width, int height, float cellSize, Vector3 offset)
	{
		Instance = this;
		m_grid = new Grid<Node>(width, height, cellSize, offset, (Grid<Node> g, int x, int y) => new Node(g, x, y));
	}

	public List<Vector3> FindPath(Vector3 startPosition, Vector3 endPosition)
	{
		m_grid.GetGridPosition(startPosition, out int startX, out int startY);
		m_grid.GetGridPosition(endPosition, out int endX, out int endY);

		List<Node> path = FindPath(startX, startY, endX, endY);

		if(path != null)
		{
			List<Vector3> vectorPath = new List<Vector3>();
			
			foreach (Node node in path)
			{
				vectorPath.Add(new Vector3(node.x, node.y) * m_grid.CellSize + Vector3.one * 5);
			}

			return vectorPath;
		}
		else
		{
			return null;
		}

	}

	public List<Node> FindPath(int startX, int startY, int endX, int endY)
	{
		Node startNode = m_grid.GetObject(startX, startY);
		Node endNode = m_grid.GetObject(endX, endY);

		m_openList = new List<Node>() { startNode };
		m_closedList = new List<Node>();

		for (int x = 0; x < m_grid.Width; x++)
			for (int y = 0; y < m_grid.Height; y++)
			{
				Node node = m_grid.GetObject(x, y);
				node.gCost = int.MaxValue;
				node.CalculateFCost();
				node.PreviousNode = null;
			}

		startNode.gCost = 0;
		startNode.hCost = CalculateDistance(startNode, endNode);
		startNode.CalculateFCost();


		while(m_openList.Count > 0)
		{
			Node currentNode = GetLowestFCostNode(m_openList);

			if(currentNode == endNode)
			{
				return CalculatePath(endNode);
			}

			m_openList.Remove(currentNode);
			m_closedList.Add(currentNode);

			foreach (Node node in GetNeighbourList(currentNode))
			{
				if(m_closedList.Contains(node)) continue;

				if(!node.IsWalkable)
				{
					m_closedList.Add(node);
					continue;
				}

				int tentativeGCost = currentNode.gCost + CalculateDistance(currentNode, node);

				if(tentativeGCost < node.gCost)
				{
					node.PreviousNode = currentNode;
					node.gCost = tentativeGCost;
					node.hCost = CalculateDistance(node, endNode);
					node.CalculateFCost();

					if (!m_openList.Contains(node))
					{
						m_openList.Add(node);
					}
				}
			}
		}

		return null;
	}

	private List<Node> GetNeighbourList(Node currentNode)
	{
		List<Node> neighbours = new List<Node>();

		if(currentNode.x - 1 >= 0)
		{
			//Left
			neighbours.Add(GetNode(currentNode.x - 1, currentNode.y));

			// Bottom Left
			if(currentNode.y - 1 >= 0) neighbours.Add(GetNode(currentNode.x - 1, currentNode.y - 1));

			// Upper Left
			if (currentNode.y + 1 < m_grid.Height) neighbours.Add(GetNode(currentNode.x - 1, currentNode.y + 1));
		}

		if(currentNode.x + 1 < m_grid.Width)
		{
			//Right
			neighbours.Add(GetNode(currentNode.x + 1, currentNode.y));

			// Bottom Right
			if (currentNode.y - 1 >= 0) neighbours.Add(GetNode(currentNode.x + 1, currentNode.y - 1));

			// Upper Right
			if (currentNode.y + 1 < m_grid.Height) neighbours.Add(GetNode(currentNode.x + 1, currentNode.y + 1));
		}

		// Up
		if (currentNode.y + 1 < m_grid.Height) neighbours.Add(GetNode(currentNode.x, currentNode.y + 1));

		// Down
		if (currentNode.y - 1 >= 0) neighbours.Add(GetNode(currentNode.x, currentNode.y - 1));

		return neighbours;
	}

	public Node GetNode(int x, int y)
	{
		return m_grid.GetObject(x, y);
	}

	private List<Node> CalculatePath(Node endNode)
	{
		List<Node> path = new List<Node>();
		path.Add(endNode);
		Node currentNode = endNode;

		while(currentNode != null)
		{
			path.Add(currentNode);
			currentNode = currentNode.PreviousNode;
		}

		path.Reverse();
		return path;
	}

	private int CalculateDistance(Node a, Node b)
	{
		int xDistance = Mathf.Abs(a.x - b.x);
		int yDistance = Mathf.Abs(a.y - b.y);
		int remaining = Mathf.Abs(xDistance - yDistance);

		return MOVE_DIAGONAL_COST * Mathf.Min(xDistance, yDistance) + MOVE_STRAIGHT_COST * remaining;
	}

	private Node GetLowestFCostNode(List<Node> nodeList)
	{
		Node lowestFCostNode = nodeList[0];

		for (int i = 0; i < nodeList.Count; i++)
		{
			if(nodeList[i].fCost < lowestFCostNode.fCost)
			{
				lowestFCostNode = nodeList[i];
			}
		}

		return lowestFCostNode;
	}
}

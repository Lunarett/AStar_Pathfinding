using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
	private Grid<Node> m_grid;
	private int m_x;
	private int m_y;

	private int m_gCost;
	private int m_hCost;
	private int m_fCost;

	private bool m_isWalkable;

	public int x { get { return m_x; } set { m_x = value; } }
	public int y { get { return m_y; } set { m_y = value;} }

	public int gCost { get { return m_gCost; } set { m_gCost = value; } }
	public int hCost { get { return m_hCost; } set { m_hCost = value; } }
	public int fCost { get { return m_fCost; } set { m_fCost = value; } }

	public bool IsWalkable { get { return m_isWalkable; } set { m_isWalkable = value; } }

	public Node PreviousNode;

	public Node(Grid<Node> grid, int x, int y)
	{
		m_grid = grid;
		m_x = x;
		m_y = y;

		m_isWalkable = true;
	}

	public void CalculateFCost()
	{
		m_fCost = m_gCost + m_hCost;
	}

	public override string ToString()
	{
		return m_x + ", " + m_y;
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavGrid : MonoBehaviour
{
	[SerializeField] private GameObject m_cellPrefab;
	[Space]
	[SerializeField] private Vector2 m_gridSize = new Vector2(10, 10);
	[SerializeField] private float m_cellSize = 10;
	[SerializeField] private Vector3 m_offset = Vector3.zero;
	[SerializeField] private LayerMask m_mask;

	private Pathfinder m_pathfinder;

	private void Start()
	{
		m_pathfinder = new Pathfinder((int)m_gridSize.x, (int)m_gridSize.y, m_cellSize, m_offset);
		SpawnCells();
	}

	private void SpawnCells()
	{
		for (int x = 0; x < m_pathfinder.Grid.Width; x++)
		{
			for (int y = 0; y < m_pathfinder.Grid.Height; y++)
			{
				GameObject cellObject = Instantiate(m_cellPrefab, new Vector3(m_pathfinder.Grid.GetWorldPosition(x, y).x + (m_pathfinder.Grid.CellSize * 0.5f), m_pathfinder.Grid.GetWorldPosition(x, y).y + (m_pathfinder.Grid.CellSize * 0.5f)), Quaternion.identity);
				cellObject.transform.localScale = Vector3.one * m_cellSize;
				Cell cell = cellObject.GetComponent<Cell>();
				cell.CheckCollision(m_mask);
				m_pathfinder.GetNode(x,y).IsWalkable = cell.IsWalkable;
			}
		}
	}
}

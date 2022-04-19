using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnChangedEventArgs : EventArgs
{
	public int x;
	public int y;
}

public class Grid<T>
{
	private int m_width;
	private int m_height;

	private float m_cellSize;

	private Vector3 m_offsetPosition;
	private T[,] m_grid;

	public int Width { get { return m_width; } }
	public int Height { get { return m_height; } }
	public float CellSize { get { return m_cellSize; } }

	public event EventHandler<OnChangedEventArgs> OnChanged;

	public Grid(int width, int height, float cellSize, Vector3 offset, Func<Grid<T>, int, int, T> gridObject)
	{
		m_width = width;
		m_height = height;
		m_cellSize = cellSize;
		m_offsetPosition = offset;

		m_grid = new T[width, height];
		TextMesh[,] debugTextArray = new TextMesh[width, height];

		for (int x = 0; x < m_grid.GetLength(0); x++)
		{
			for (int y = 0; y < m_grid.GetLength(1); y++)
			{
				m_grid[x, y] = gridObject(this, x, y);
			}
		}

		OnChanged += (object sender, OnChangedEventArgs eventArgs) =>
		{
			debugTextArray[eventArgs.x, eventArgs.y].text = m_grid[eventArgs.x, eventArgs.y]?.ToString();
		};
	}

	public Vector3 GetWorldPosition(int x, int y)
	{
		return new Vector3(x, y) * m_cellSize + m_offsetPosition;
	}

	public void GetGridPosition(Vector3 worldPosition, out int x, out int y)
	{
		x = Mathf.FloorToInt((worldPosition - m_offsetPosition).x / m_cellSize);
		y = Mathf.FloorToInt((worldPosition - m_offsetPosition).y / m_cellSize);
	}

	public void SetObject(int x, int y, T obj)
	{
		if (x >= 0 && y >= 0 && x < m_width && y < m_height)
		{
			m_grid[x, y] = obj;
			if (OnChanged != null) OnChanged(this, new OnChangedEventArgs { x = x, y = y });
		}
	}

	public void SetObject(Vector3 worldPosition, T obj)
	{
		int x;
		int y;

		GetGridPosition(worldPosition, out x, out y);
		SetObject(x, y, obj);
	}

	public T GetObject(int x, int y)
	{
		if (x >= 0 && y >= 0 && x < m_width && y < m_height)
			return m_grid[x, y];
		else
			return default(T);
	}

	public T GetObject(Vector3 worldPosition)
	{
		int x;
		int y;

		GetGridPosition(worldPosition, out x, out y);
		return GetObject(x, y);
	}

	public void CallOnChanged(int x, int y)
	{
		if (OnChanged != null)
		{
			OnChanged(this, new OnChangedEventArgs { x = x, y = y });
		}
	}
}

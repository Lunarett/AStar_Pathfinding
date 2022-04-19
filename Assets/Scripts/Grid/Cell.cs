using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
	[SerializeField] private SpriteRenderer m_cellSprite;
	[SerializeField] private SpriteRenderer[] m_borderSprite;
	[SerializeField] private LayerMask m_layerMask;

	public bool IsWalkable = true;

	private void Start()
	{
		CheckCollision(m_layerMask);

		Debug.Log(IsWalkable);
	}

	public void SetCellColor(Color color)
	{
		m_cellSprite.color = color;
	}

	public void SetBorderColor(Color color)
	{
		foreach (SpriteRenderer border in m_borderSprite)
		{
			border.color = color;
		}
	}

	public void SetColor(Color cellColor, Color borderColor)
	{
		m_cellSprite.color = cellColor;

		foreach (SpriteRenderer border in m_borderSprite)
		{
			border.color = borderColor;
		}
	}

	public void CheckCollision(LayerMask mask)
	{
		Collider2D collider = Physics2D.OverlapBox(m_cellSprite.transform.position, m_cellSprite.transform.localScale, transform.position.z);

		if(collider == null)
		{
			Debug.Log("null");
			return;
		}

		Debug.Log(collider.gameObject.name);

		int layer = 1 << collider.gameObject.layer;
		
		IsWalkable = mask == layer ? false : true;
	}
}

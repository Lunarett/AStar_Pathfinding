using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
	[SerializeField] private float m_Speed = 3.0f;

	private List<Vector3> m_pathList;
	private int m_pathIndex;
	private Transform m_targetTransform;

	private void Awake()
	{
		m_targetTransform = GameObject.FindGameObjectWithTag("Player").transform;
	}

	private void Update()
	{
		HandleMovement();
		MoveTo(m_targetTransform.position);
	}

	private void HandleMovement()
	{
		if (m_pathList == null) return;

		Vector3 targetPosition = m_pathList[m_pathIndex];

		if(Vector3.Distance(transform.position, targetPosition) > 1)
		{
			Vector3 moveDirection = (targetPosition - transform.position).normalized;
			float previousDistance = Vector3.Distance(transform.position, targetPosition);
			transform.position = transform.position + moveDirection * m_Speed * Time.deltaTime;
		}
		else
		{
			m_pathIndex++;

			if(m_pathIndex >= m_pathList.Count)
			{
				m_pathList = null;
			}
		}
	}

	public void MoveTo(Vector3 targetDestination)
	{
		m_pathIndex = 0;
		m_pathList = Pathfinder.Instance.FindPath(transform.position, targetDestination);
	
		if(m_pathList != null && m_pathList.Count > 1)
		{
			m_pathList.RemoveAt(0);
		}
	}
}

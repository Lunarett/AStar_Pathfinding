using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
	[SerializeField] private float m_speed;
	private Rigidbody2D m_Rigidbody;

	private void Awake()
	{
		m_Rigidbody = GetComponent<Rigidbody2D>();
	}

	private void Update()
	{
		Move();
	}

	private void Move()
	{
		float x = Input.GetAxisRaw("Horizontal") * m_speed;
		float y = Input.GetAxisRaw("Vertical") * m_speed;

		Vector2 direction = new Vector2(x, y);

		m_Rigidbody.velocity = direction;
	}
}

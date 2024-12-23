using JetBrains.Annotations;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

public class TrapController : MonoBehaviour
{

	public enum TrapType {Move, Explote, Controls}
	
	[Header("Trap Settings")]
	public TrapType trapType;
	public float speed;
	public string direction;
	public float distance;
	public float finalPosition;
	private Rigidbody2D rb2d;
	public bool trapActivated;
	public bool destroy;


	private void Start()
	{
		// Suscribirse al evento
		TrapEventManager.OnTrapTriggered += CheckTrap;

		// Obtener el Rigidbody si es necesario
		rb2d = GetComponent<Rigidbody2D>();
	}

	private void FixedUpdate()
	{
		if (trapActivated)
		{
			Move(direction, speed, distance);
		}
	}

	private void OnDestroy()
	{
		// Desuscribirse al evento al destruir el objeto
		TrapEventManager.OnTrapTriggered -= CheckTrap;
	}

	private void CheckTrap(TrapController trap)
	{
		if (trap == this) // Verificar si esta es la trampa que debe activarse
		{
			ActivateTrap(); // Activar esta trampa
		}
	}
	private void ActivateTrap()
	{
		switch (trapType)
		{
			case TrapType.Move:
				trapActivated = true;
				SoundController.soundManager.PlaySound("TrapMove");
				if (direction == "horizontal")
				{
					finalPosition = distance + transform.position.x;
				}
				else
				{
					finalPosition = distance + transform.position.y;
				}
				break;

			case TrapType.Explote:
				RestartLevel();
				break;
			
			case TrapType.Controls:
				SoundController.soundManager.PlaySound("Controls");
				PlayerController player = Object.FindAnyObjectByType<PlayerController>();
				player.controlsInverted = !player.controlsInverted;
				break;
		}
	}

	public void Move(string direction, float speed, float distance)
	{
		Vector3 position = transform.position;
		float currentPos, targetPos;

		if (direction == "horizontal")
		{
			currentPos = position.x;
			targetPos = finalPosition;
		}
		else
		{
			currentPos = position.y;
			targetPos = finalPosition;
		}

		currentPos += speed * Time.deltaTime;

		// Verificar si se pasó del punto objetivo
		if ((speed > 0 && currentPos >= targetPos) || (speed < 0 && currentPos <= targetPos))
		{
			currentPos = targetPos; // Ajustar al punto final
			trapActivated = false; // Desactivar la trampa
			if (destroy)
			{
				Destroy(gameObject);
			}
		}

		// Actualizar la posición según la dirección
		if (direction == "horizontal")
			position.x = currentPos;
		else
			position.y = currentPos;

		transform.position = position; // Aplicar la nueva posición
	}

	
	private void RestartLevel()
	{
		SoundController.soundManager.PlaySound("Die");
		LevelController level = Object.FindAnyObjectByType<LevelController>();
		level.RestartLevel();
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if(collision.CompareTag("Player") && this.CompareTag("Trap"))
		{
			PlayerController player = Object.FindAnyObjectByType<PlayerController>();
			player.GetComponent<PlayerController>().animator.SetTrigger("Die");
			RestartLevel();
		}
		if (collision.CompareTag("Player") && this.CompareTag("Coin"))
		{
			SoundController.soundManager.PlaySound("Coin1");
			Destroy(gameObject);
		}
	}

}

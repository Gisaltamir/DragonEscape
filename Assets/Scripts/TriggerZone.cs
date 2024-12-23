using UnityEngine;

public class TriggerZone : MonoBehaviour
{
	public TrapController Trap;
	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag("Player"))
		{
			TrapEventManager.TriggerTrap(Trap);
		}
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		if (collision.CompareTag("Player"))
		{
			TriggerZone.Destroy(this);
		}
	}
}

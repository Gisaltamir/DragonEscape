using System;
using UnityEngine;

public class TrapEventManager : MonoBehaviour
{
	public static event Action<TrapController> OnTrapTriggered;

	public static void TriggerTrap(TrapController trap)
	{
		if (OnTrapTriggered != null)
		{
			OnTrapTriggered?.Invoke(trap); // Envía la señal
		}
	}
}

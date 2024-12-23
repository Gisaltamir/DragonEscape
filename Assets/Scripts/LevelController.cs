using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelController : MonoBehaviour
{
	//LevelController levelController;
	public GameManager manager;

	private void Start()
	{
		manager = GameManager.manager;
	}

	public void RestartLevel()
	{
		if (manager.nextLevel == 0)
		{
			SceneManager.LoadScene(manager.currentLevel);
		}
		else
		{
			SceneManager.LoadScene(manager.currentLevel + manager.nextLevel);
		}
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag("Player"))
		{
			
			if (gameObject.name == "FinalDoor")
			{
				SoundController.soundManager.PlaySound("DoorReach");
				manager.nextLevel = 0;
				manager.levelCleared = true;
				SceneManager.LoadScene("Map");
			}
			else //Es la otra puerta
			{
				SoundController.soundManager.PlaySound("DoorReach1");
				manager.nextLevel++;
				SceneManager.LoadScene(manager.currentLevel + manager.nextLevel);
			}
		}
	}

}

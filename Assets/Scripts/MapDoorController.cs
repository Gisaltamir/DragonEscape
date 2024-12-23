using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MapDoorController : MonoBehaviour
{
    public Image celds;
    public GameObject doorOpen;

    public bool openDoor; // Efecto abrir puertas
    public bool isDoorActive; // Se puede acceder al nivel
	public bool cleared;

	public string levelToLoad; 
    

	public bool isInTrigger;
	private GameObject currentTrigger;
	private float doorTimer = 0f;

	private GameManager manager;

	void Start()
    {
		manager = GameManager.manager;
		set_doors_active();
		Cursor.visible = false;
		
	}

    void Update()
    {
		if (isDoorActive)
		{
			celds.fillAmount = 0;
		}
		if (openDoor & !isDoorActive)
		{
			OpenDoor();
		}

		if (isInTrigger && Input.GetKeyDown(KeyCode.Return) && isDoorActive)
		{
            manager.currentLevel = levelToLoad;
			LoadLevel();
		}
	}


    public void OpenDoor()
    {
        celds.fillAmount -= 1 * Time.deltaTime;
		doorTimer += Time.deltaTime;

		if (doorTimer >= 2f)
		{
			isDoorActive = true;
			doorTimer = 0f;
		}
	}
	public void LoadLevel()
	{
		SoundController audio = SoundController.soundManager;
		audio.PlaySound("ButtonSelected");
		if(manager.currentLevel == "Credits")
		{
			Cursor.visible = true;
			audio.PlaySound("Win");
		}
		SceneManager.LoadScene(manager.currentLevel);
	}

	public void set_doors_active()
	{
		string[] levels = manager.levels;
		
		for (int i = 0; i < levels.Length; i++)
		{
			cleared = manager.get_level_clear(levels[i]);
			if (cleared)
			{
				set_door_clear(levels[i]);
			}
		}
		manager.set_level_active(manager.get_last_level());
	}

	public void set_door_clear(string level)
	{
		GameObject door = GameObject.Find(level);
		door.transform.GetChild(2).gameObject.SetActive(true);
		door.GetComponent<MapDoorController>().isDoorActive = true;
	}


	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag("Player"))
		{
			isInTrigger = true;
			currentTrigger = collision.gameObject;
		}
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		if (collision.CompareTag("Player"))
		{
			isInTrigger = false; // Marca que salió del trigger
			currentTrigger = null; // Limpia la referencia
		}
	}
}

using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public bool showMenu;
	public static PauseMenu menu;
	public GameObject canvas;

	private void Awake()
	{
		if (menu == null)
		{
			DontDestroyOnLoad(gameObject);
			menu = this;
		}
		else
		{
			Destroy(gameObject);
		}
	}


    // Update is called once per frame
    void Update()
    {
		if (Input.GetKeyDown(KeyCode.Escape) & (SceneManager.GetActiveScene().name != "Menu" & SceneManager.GetActiveScene().name != "Settings"))
		{
			Cursor.visible = !showMenu;
			showMenu = !showMenu;
		}
		canvas.SetActive(showMenu);

		if (showMenu)
		{
			Time.timeScale = 0f;
		}
		else
		{
			Time.timeScale = 1f;
		}
	}

	public void Restart()
	{
		HideMenu();
		if (SceneManager.GetActiveScene().name == "Map") {
			GameManager.manager.ResetGame();
			SceneManager.LoadScene("Map");
		}
		else
		{
			GameManager.manager.nextLevel = 0;
			SceneManager.LoadScene(GameManager.manager.currentLevel);
		}
	}

	public void HideMenu()
	{
		canvas.gameObject.SetActive(false);
		showMenu = false;
		Cursor.visible = showMenu;
	}

	public void Exit()
	{
		HideMenu();
		GameManager.manager.nextLevel = 0;
		SceneManager.LoadScene("Menu");
	}
}

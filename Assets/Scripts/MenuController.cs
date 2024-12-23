using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
	public Slider musicVolume;
	public AudioSource musicSource;

	public void Start()
	{
		Cursor.visible = true;
	}

	public void Update()
	{
		if (musicVolume != null)
		{
			musicVolume.value = SoundController.soundManager.volume;
		}
	}

	public void LoadMap()
	{
		get_button_sound();
		SceneManager.LoadScene("Map");
	}
	
	public void LoadSettings()
	{
		get_button_sound();
		PlaySound("BackgroundSettings");
		SceneManager.LoadScene("Settings");
	}

	public void setVolume()
	{
		PlaySound("BackgroundSettings");
		SoundController.soundManager.volume = musicVolume.value;
	}

	public void ReturnMenu()
	{
		get_button_sound();
		SceneManager.LoadScene("Menu");
	}

	public void ExitGame()
	{
		get_button_sound();
		Application.Quit();
	}

	public void ResetGame()
	{
		get_button_sound();
		GameManager.manager.ResetGame();
	}

	public void PlaySound(string sound)
	{
		if (!SoundController.soundManager.audioSource.isPlaying) 
		{ 
			SoundController.soundManager.PlaySound(sound);
		}
	}
	public void get_button_sound()
	{
		SoundController.soundManager.PlaySound("ButtonSelected");
	}
}

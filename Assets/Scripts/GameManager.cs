using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
	
	public string currentLevel;
	public static GameManager manager;
    public bool levelCleared;
    public string[] levels = { "Pits", "Spikes", "Coins", "Controls", "Doors", "Credits"};
	public bool[] doorStates = { false, false, false, false, false, false};
	public int nextLevel;

	private void Awake()
    {
		if (manager == null)
        {
            DontDestroyOnLoad(gameObject);
			manager = this;
			Load();
		}
        else
        {
            Destroy(gameObject);
        }

		nextLevel = 0;
	}

	private void Start()
	{
		SoundController.soundManager.PlaySound("MenuIntro");
	}

	private void Update()
    {
		if (levelCleared) 
		{
			try { set_level_clear(); }
			catch { }
        }
	}

	public void set_level_active(string level)
	{
		GameObject nextDoor = GameObject.Find(level);
		if (!string.IsNullOrEmpty(level))
		{
			nextDoor.GetComponent<MapDoorController>().openDoor = true;
			SoundController.soundManager.PlaySound("OpenDoor");
			try
			{
				GameObject.Find("Character").GetComponent<PlayerController>().Respawn();
			}
			catch { }
		}
	}

	public bool get_level_clear(string level)
	{
		int index = get_index_level(level);
		if (index == -1)
		{
			return false;
		}
		return doorStates[index];
	}

	private void set_level_clear()
	{
		int i = System.Array.IndexOf(levels, currentLevel);
		doorStates[i] = true;
		Save();
		string nextLevel = get_next_level();
		set_level_active(nextLevel);
		levelCleared = false;
	}

	public string get_next_level() //This is in case the player decides replay a cleared level, so I set active the next level and not the last to open
	{
		string nextLevel = "";
		for (int i = 0; i < levels.Length; i++)
		{
			if (levels[i] == currentLevel)
			{
				return levels[i+1];
			}
		}
		return nextLevel;
	}

	public string get_last_level()
	{
		string nextLevel = "";
		for (int i = 0; i < doorStates.Length; i++)
		{
			if (doorStates[i] == false)
			{
				return levels[i];
			}
		}
		return nextLevel;
	}

	public int get_index_level(string level)
	{
		int index = System.Array.IndexOf(levels, level);
		return index;
	}

	public void Save()
	{
		BinaryFormatter bf = new BinaryFormatter();
		FileStream file = File.Create(Application.persistentDataPath + "/gameInfo.dat");
		ManagerData data = new ManagerData();
		data.doorStates = doorStates;
		data.currentLevel = currentLevel;

		bf.Serialize(file, data);
		file.Close();
	}

	public void ResetGame()
	{
		BinaryFormatter bf = new BinaryFormatter();
		FileStream file = File.Create(Application.persistentDataPath + "/gameInfo.dat");
		ManagerData data = new ManagerData();
		doorStates = data.doorStates;
		currentLevel = data.currentLevel;
		nextLevel = 0;
		bf.Serialize(file, data);
		file.Close();
	}

	public void Load()
	{
		string filePath = Application.persistentDataPath + "/gameInfo.dat";
		if (File.Exists(filePath))
		{
			BinaryFormatter bf = new BinaryFormatter();
			FileStream file = File.Open(filePath, FileMode.Open);
			ManagerData data = (ManagerData)bf.Deserialize(file);
			file.Close();
			doorStates = data.doorStates;
			currentLevel = data.currentLevel;
		}
	}

	[Serializable]
	class ManagerData
	{
		public string currentLevel = "";
		public bool[] doorStates = { false, false, false, false, false, false };
	}

}

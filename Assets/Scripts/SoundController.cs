using UnityEngine;

public class SoundController: MonoBehaviour
{
	// Start is called once before the first execution of Update after the MonoBehaviour is created

	public static SoundController soundManager;
	public AudioSource audioSource;
	public AudioClip[] audioClips; // Asocia sonidos en el Inspector
	public float volume;

	private void Awake()
	{
		volume = 0.3f;

		if (soundManager == null)
		{
			soundManager = this;

		}
		else
		{
			Destroy(gameObject);
		}
	}

	private void Update()
	{
		audioSource.volume = volume;
	}
	public void PlaySound(string clipName, bool loop = false)
	{
		AudioClip clip = System.Array.Find(audioClips, clip => clip.name == clipName);
		if (clip != null) { 
			audioSource.PlayOneShot(clip);
		}
	}

}

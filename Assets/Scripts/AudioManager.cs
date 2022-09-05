using System;
using UnityEngine.Audio;
using UnityEngine;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour
{
	public static AudioManager manager;
	public AudioMixerGroup audioMxrGroup;
	public List<Sound> soundsList;

	void Awake()
	{
		// Ensure only one manager exists
		if (manager == null)
        {
			manager = this;
			DontDestroyOnLoad(gameObject);
		}
		else Destroy(gameObject);

		foreach (Sound sound in soundsList)
		{
			sound.source = gameObject.AddComponent<AudioSource>();
			sound.source.clip = sound.clip;
			sound.source.loop = sound.loop;

			sound.source.outputAudioMixerGroup = audioMxrGroup;
		}
	}

	public void Play(string sound)
	{

		Sound mySound = soundsList.Find(item => item.name == sound);
		if (mySound == null)
		{
			Debug.LogError("Sound " + name + " not found");
			return;
		}

		float volumeVariance = UnityEngine.Random.Range(-mySound.volumeDeviation / 2f, mySound.volumeDeviation / 2f) + 1f;
		float pitchVariance = UnityEngine.Random.Range(-mySound.pitchDeviation / 2f, mySound.pitchDeviation / 2f) + 1f;
		mySound.source.volume = mySound.volume * volumeVariance;
		mySound.source.pitch = mySound.pitch * pitchVariance;

		mySound.source.Play();
	}

}

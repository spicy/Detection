using UnityEngine.Audio;
using UnityEngine;

[System.Serializable]
public class Sound 
{
	public string name;
	public AudioClip clip;
	public AudioMixerGroup audioMxrGroup;
	public bool loop = false;
	[HideInInspector] public AudioSource source;

	[Range(0f, 1f)]
	public float volume = .75f;
	[Range(0f, 1f)]
	public float volumeDeviation = .1f;
	[Range(.1f, 3f)]
	public float pitch = 1f;
	[Range(0f, 1f)]
	public float pitchDeviation = .1f;
}

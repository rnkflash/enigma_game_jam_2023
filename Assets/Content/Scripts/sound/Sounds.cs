using UnityEngine;
using System;

public class Sounds : Singleton<Sounds>
{
	public Sound[] sounds;
	
	protected override void Created()
	{
		base.Created();
		sounds = Resources.LoadAll<Sound>("Sounds");

	}

	public AudioClip GetAudioClip(string id)
	{
		return Array.Find(sounds, element => element.name == id).clip;
	}
}
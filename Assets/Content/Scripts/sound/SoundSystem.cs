using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Audio;

public static class SoundSystem
    {
        private static GameObject oneShotGameObject = null;
        private static AudioSource oneShotAudioSource = null;

        private static GameObject musicGameObject = null;
        private static AudioSource musicAudioSource = null;

		private static AudioMixer mixer = null;

		public static bool IsPlayingMusic() 
		{
			return musicGameObject != null;
		}

        public static void PlaySound(AudioClip[] sound, Vector3 position)
        {
            PlaySound(sound[UnityEngine.Random.Range(0, sound.Length)], position);
        }

        public static void PlaySound(AudioClip sound, Vector3 position)
        {
            GameObject soundObj = new GameObject("Sound");
            soundObj.transform.position = position;
            AudioSource audioSource = soundObj.AddComponent<AudioSource>();
            audioSource.clip = sound;
            audioSource.maxDistance = 100f;
            audioSource.spatialBlend = 1f;
            audioSource.rolloffMode = AudioRolloffMode.Linear;
            audioSource.dopplerLevel = 0f;
            audioSource.Play();
            UnityEngine.Object.Destroy(soundObj, audioSource.clip.length);
        }

        public static void PlaySound(AudioClip sound)
        {
            if (oneShotGameObject == null)
            {
                oneShotGameObject = new GameObject("Sound");
                oneShotAudioSource = oneShotGameObject.AddComponent<AudioSource>();
            }
            oneShotAudioSource.PlayOneShot(sound);
        }

        public static void PlayMusic(AudioClip music)
        {
            if (musicGameObject == null)
            {
                musicGameObject = new GameObject("Music");
				UnityEngine.Object.DontDestroyOnLoad(musicGameObject);
				musicAudioSource = musicGameObject.AddComponent<AudioSource>();

				if (mixer == null) {
					mixer = (Resources.Load("AudioMixer") as Mixer).mixer;
				}
				musicAudioSource.outputAudioMixerGroup = mixer.FindMatchingGroups("Music")[0];
            }
            musicAudioSource.clip = music;
            musicAudioSource.loop = true;
            musicAudioSource.Play();
        }

		public static void ChangeTrack(AudioClip clip)
		{
			if(musicGameObject == null)
			{
				PlayMusic(clip);
			}
			else
			{
				if(clip != musicAudioSource.clip)
				{
					Sounds.Instance.StartCoroutine(SwitchTrack(clip));
				}
			}
		}

		public static IEnumerator SwitchTrack(AudioClip nextTrack, float durationIn = .25f, float durationOut = .25f)
		{
			musicAudioSource.volume = 1f;
			var elapsedTime = 0f;
			var fadeStartTime = durationIn;
			if (fadeStartTime < 0f)
				fadeStartTime = 0f;
			while (elapsedTime < fadeStartTime)
			{
				elapsedTime += Time.deltaTime;
				yield return null;
			}
			while (musicAudioSource.volume > 0f && elapsedTime < musicAudioSource.clip.length)
			{
				musicAudioSource.volume -= Time.deltaTime * 1f / durationIn;
				yield return null;
			}
			PlayMusic(nextTrack);
			elapsedTime = 0f;
			while (elapsedTime < durationOut)
			{
				elapsedTime += Time.deltaTime;
				musicAudioSource.volume = Mathf.Lerp(0f, 1f, elapsedTime / durationOut);
				yield return null;
			}
		}

		public static IEnumerator Fade(float durationIn = .25f, float durationOut = .25f)
		{
			musicAudioSource.volume = 1f;
			var elapsedTime = 0f;
			var fadeStartTime = durationIn;
			if (fadeStartTime < 0f)
				fadeStartTime = 0f;
			while (elapsedTime < fadeStartTime)
			{
				elapsedTime += Time.deltaTime;
				yield return null;
			}
			while (musicAudioSource.volume > 0f && elapsedTime < musicAudioSource.clip.length)
			{
				musicAudioSource.volume -= Time.deltaTime * 1f / durationIn;
				yield return null;
			}
			elapsedTime = 0f;
			while (elapsedTime < durationOut)
			{
				elapsedTime += Time.deltaTime;
				musicAudioSource.volume = Mathf.Lerp(0f, 1f, elapsedTime / durationOut);
				yield return null;
			}
		}
	}

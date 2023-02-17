using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeMusicController : MonoBehaviour
{
    public SoundId track;
    void Start()
    {
        SoundSystem.ChangeTrack(Sounds.Instance.GetAudioClip(track));
    }
}

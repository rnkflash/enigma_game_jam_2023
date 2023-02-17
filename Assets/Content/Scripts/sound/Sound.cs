using UnityEngine;
[CreateAssetMenu(fileName = "Sound", menuName = "Static Data/Sound")]
public class Sound : ScriptableObject
{
  public SoundId id;
  public AudioClip clip;
}
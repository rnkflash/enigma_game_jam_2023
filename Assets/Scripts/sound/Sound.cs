using UnityEngine;
[CreateAssetMenu(fileName = "Sound", menuName = "Static Data/Sound")]
public class Sound : ScriptableObject
{
  public string id;
  public AudioClip clip;
}
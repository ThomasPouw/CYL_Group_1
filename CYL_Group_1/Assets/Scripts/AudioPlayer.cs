using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class AudioPlayer : MonoBehaviour
{
    [SerializeField] List<Audio> Audios;
    AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayByName(string name)
    {
        Audio record = Audios.SingleOrDefault(x => x.name == name);
        AudioClip clip = record.AudioClips[Random.Range(0, record.AudioClips.Count)];
        audioSource.PlayOneShot(clip);
    }
}

[System.Serializable]
internal class Audio
{
    public string name;
    public List<AudioClip> AudioClips;
}

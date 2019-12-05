using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Music : SingletonMonoBehaviour<Music> {

    [SerializeField] private AudioClip[] bgmLoopAudioClip = null;
    public List<AudioClip> audioClip = new List<AudioClip>();

    //BGM用
    [SerializeField] AudioSource BGM_AudioSource;
    //SE用
    [SerializeField] AudioSource SE_AudioSource;

    // Use this for initialization
    void Start () {            
        
    }
	
    public void Play_BGM(int ID)
    {
        BGM_AudioSource.clip = bgmLoopAudioClip[ID];
        BGM_AudioSource.loop = true;
        BGM_AudioSource.playOnAwake = false;

        if (BGM_AudioSource == null || BGM_AudioSource.isPlaying)
        {
            return;
        }
        BGM_AudioSource.Play();
    }
    public void Pause()
    {
        BGM_AudioSource.Pause();
    }

    public void UnPause()
    {
        BGM_AudioSource.UnPause();
    }

    public void SEsound(int ID)
    {
        SE_AudioSource.PlayOneShot(audioClip[ID]);
    }
}

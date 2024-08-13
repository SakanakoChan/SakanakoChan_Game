using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [SerializeField] private AudioSource[] sfx;
    [SerializeField] private AudioSource[] bgm;
    [Space]
    [SerializeField] private float sfxMinHearableDistance;

    public bool playingBGM;
    private int bgmIndex;

    private bool canPlaySFX = false;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(instance);
        }
        else
        {
            instance = this;
        }

        Invoke("AllowPlayingSFX", 1f);
    }

    private void Update()
    {
        if (!playingBGM)
        {
            StopAllBGM();
        }
        else
        {
            if (bgm[bgmIndex].isPlaying == false)
            {
                PlayBGM(bgmIndex);
            }
        }
    }

    public void PlaySFX(int _sfxIndex, Transform _sfxSourceTransform)
    {
        if (!canPlaySFX)
        {
            return;
        }

        //prevent from re-playing the same sfx
        if (sfx[_sfxIndex].isPlaying == true)
        {
            return;
        }

        //if the sfx source is too far from player, player won't hear it
        if (_sfxSourceTransform != null && Vector2.Distance(PlayerManager.instance.player.transform.position, _sfxSourceTransform.position) > sfxMinHearableDistance)
        {
            return;
        }


        if (_sfxIndex < sfx.Length)
        {
            sfx[_sfxIndex].pitch = Random.Range(0.85f, 1.15f);
            sfx[_sfxIndex].Play();
        }
    }

    public void StopSFX(int _sfxIndex)
    {
        if (_sfxIndex < sfx.Length)
        {
            sfx[_sfxIndex].Stop();
        }
    }

    public void PlayBGM(int _bgmIndex)
    {
        //stop all the bgms first
        StopAllBGM();

        //play the specified bgm
        if (_bgmIndex < bgm.Length)
        {
            bgmIndex = _bgmIndex;
            bgm[bgmIndex].Play();
        }
        else
        {
            Debug.Log("BGM Index out of range");
        }
    }

    public void PlayRandomBGM()
    {
        bgmIndex = Random.Range(0, bgm.Length);
        PlayBGM(bgmIndex);
    }

    public void StopAllBGM()
    {
        for (int i = 0; i < bgm.Length; i++)
        {
            bgm[i].Stop();
        }
    }

    public void AllowPlayingSFX()
    {
        canPlaySFX = true;
    }
}

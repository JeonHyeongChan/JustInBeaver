using UnityEngine;
using System.Collections;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [Header("Audio Sources")]
    public AudioSource bgmSource;
    public AudioSource sfxSource;

    [Header("Volume Setting")]
    [Range(0f, 1f)] public float bgmVolume = 0.6f;
    [Range(0f, 1f)] public float sfxVolume = 0.8f;

    //public AudioClip beaverHitSFX; <- 비버맞는효과음

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;

            //이벤트들 여기다
            //비버맞은거감지 += PlayBeaverHit;

            bgmSource.loop = true;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
        bgmSource.volume = bgmVolume;
        sfxSource.volume = sfxVolume;
    }

    private void OnDestroy()
    {
        //비버맞은거감지 -= PlayBeaverHit;
    }

    /// <summary>
    /// 배경음악
    /// </summary>
    /// <param name="clip"></param>
    /// <param name="loop"></param>
    public void PlayBGM(AudioClip clip, bool loop = true)
    {
        if (clip == null)
            return;

        bgmSource.clip = clip;
        bgmSource.loop = loop;
        bgmSource.Play();
    }

    /// <summary>
    /// 효과음
    /// </summary>
    /// <param name="clip"></param>
    public void PlaySFX(AudioClip clip)
    {
        if (clip == null)
            return;

        sfxSource.PlayOneShot(clip, sfxVolume);
    }

    //private void PlayBeaverHit()    //비버맞는소리
    //{
    //    if (beaverHitSFX != null)
    //        PlaySFX(beaverHitSFX);
    //}
}

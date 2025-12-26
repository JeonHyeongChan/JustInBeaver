using UnityEngine;
using System.Collections.Generic;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [Header("Audio Sources")]
    public AudioSource bgmSource;
    public AudioSource sfxSource;

    [Header("Volume Setting")]
    [Range(0f, 1f)] public float bgmVolume = 0.6f;
    [Range(0f, 1f)] public float sfxVolume = 0.8f;

    [Header("Scene BGM Setting")]
    public SceneBGM[] sceneBGMs;

    private Dictionary<SceneType, AudioClip> bgmMap;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            bgmSource.loop = true;

            bgmSource.volume = bgmVolume;
            sfxSource.volume = sfxVolume;

            BuildBGMMap();
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }   

    private void BuildBGMMap()
    {
        bgmMap = new Dictionary<SceneType, AudioClip>();

        foreach (var bgm in sceneBGMs)
        {
            if (!bgmMap.ContainsKey(bgm.sceneType))
            {
                bgmMap.Add(bgm.sceneType, bgm.clip);
            }
        }
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

        if (bgmSource.isPlaying && bgmSource.clip == clip) // 중복방지
            return;

        bgmSource.clip = clip;
        bgmSource.loop = loop;
        bgmSource.Play();
    }

    public void PlaySceneBGM(SceneType sceneType)
    {
        if (bgmMap == null)
            return;

        if (!bgmMap.TryGetValue(sceneType, out AudioClip clip))
            return;

        PlayBGM(clip);
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

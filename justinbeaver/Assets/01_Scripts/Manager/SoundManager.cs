using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [Header("Audio Sources")]
    public AudioSource bgmSource;
    public AudioSource sfxSource;

    [Header("Volume Setting")]
    [Range(0f, 1f)] public float bgmVolume;
    [Range(0f, 1f)] public float sfxVolume;

    [Header("Scene BGM Setting")]
    public SceneBGM[] sceneBGMs;

    private AudioClip currentSceneBGM;
    private AudioClip overrideBGM;

    [Header("State BGM")]
    public AudioClip chaseBGM;

    [Header("SFX Setting")]
    public SFXData[] sfxDatas;

    [Header("Gathering SFX")]
    public AudioSource loopSFXSource;

    [Header("Human Footstep SFX")]
    public AudioSource humanFootstepSource;

    private Dictionary<SceneType, AudioClip> bgmMap;
    private Dictionary<SFXType, AudioClip> sfxMap;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            bgmSource.loop = true;

            bgmSource.volume = bgmVolume;
            sfxSource.volume = sfxVolume;

            BuildBGMMap();
            BuildSFXMap();
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {        
        StopHumanFootstep(null); // 탈출할때 적 가까이 있었을때 발소리 재생 버그 방지
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

    private void BuildSFXMap()
    {
        sfxMap = new Dictionary<SFXType, AudioClip>();

        foreach (var sfx in sfxDatas)
        {
            if (!sfxMap.ContainsKey(sfx.type))
            {
                sfxMap.Add(sfx.type, sfx.clip);
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

        currentSceneBGM = clip;

        if (overrideBGM != null)
            return;

        PlayBGM(clip);
    }
    
    public void StopOverrideBGM()
    {
        if (overrideBGM == null)
            return;

        overrideBGM = null;

        if (currentSceneBGM != null)
            PlayBGM(currentSceneBGM);
    }

    public void PlayOverrideBGM(AudioClip clip)
    {
        if (clip == null)
            return;
        
        if (overrideBGM == clip && bgmSource.isPlaying)
            return;

        overrideBGM = clip;
        PlayBGM(clip);
    }

    /// <summary>
    /// 효과음
    /// </summary>
    /// <param name="clip"></param>
    public void PlaySFX(SFXType type)
    {
        if (sfxMap.TryGetValue(type, out var clip))
        {
            sfxSource.PlayOneShot(clip, sfxVolume);
        }
    }

    public void PlayLoopSFX(SFXType type)
    {
        if (!sfxMap.TryGetValue(type, out var clip))
            return;

        if (loopSFXSource.isPlaying && loopSFXSource.clip == clip)
            return;

        loopSFXSource.clip = clip;
        loopSFXSource.volume = sfxVolume;
        loopSFXSource.loop = true;
        loopSFXSource.Play();
    }

    public void StopLoopSFX(SFXType type)
    {
        if (!loopSFXSource.isPlaying)
            return;

        if (sfxMap.TryGetValue(type, out var clip) && loopSFXSource.clip != clip)
            return;

        loopSFXSource.Stop();
        loopSFXSource.clip = null;
    }

    public void PlayRandomSFX(params SFXType[] types)
    {
        if (types == null || types.Length == 0)
            return;

        int index = Random.Range(0, types.Length);
        PlaySFX(types[index]);
    }

    /// <summary>
    /// 인간 발소리
    /// </summary>
    /// <param name="type"></param>
    public void PlayHumanFootstep(Transform human, SFXType type)
    {
        if (humanFootstepSource == null)
            return;

        if (!sfxMap.TryGetValue(type, out var clip))
            return;

        if (humanFootstepSource.isPlaying && humanFootstepSource.clip == clip)
            return;

        humanFootstepSource.clip = clip;
        humanFootstepSource.volume = sfxVolume;
        humanFootstepSource.loop = true;
        humanFootstepSource.Play();
    }

    public void StopHumanFootstep(Transform human)
    {
        if (humanFootstepSource == null)
            return;

        if (humanFootstepSource.isPlaying)
            humanFootstepSource.Stop();
    }

    public void MuteBGM(bool mute)
    {
        if (bgmSource == null)
            return;

        bgmSource.mute = mute;
    }
}

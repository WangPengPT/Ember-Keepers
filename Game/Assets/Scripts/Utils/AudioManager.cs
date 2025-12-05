using UnityEngine;
using System.Collections.Generic;

namespace EmberKeepers.Utils
{
    /// <summary>
    /// 音效管理器 - 管理游戏中的音效和音乐
    /// </summary>
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager Instance { get; private set; }

        [Header("Audio Sources")]
        [SerializeField] private AudioSource musicSource;
        [SerializeField] private AudioSource sfxSource;

        [Header("Audio Clips")]
        [SerializeField] private AudioClip mainMenuMusic;
        [SerializeField] private AudioClip combatMusic;
        [SerializeField] private AudioClip victoryMusic;
        [SerializeField] private AudioClip defeatMusic;

        [Header("Settings")]
        [SerializeField] private float masterVolume = 1f;
        [SerializeField] private float musicVolume = 0.7f;
        [SerializeField] private float sfxVolume = 1f;

        private Dictionary<string, AudioClip> sfxClips = new Dictionary<string, AudioClip>();

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
                InitializeAudioSources();
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void InitializeAudioSources()
        {
            if (musicSource == null)
            {
                GameObject musicObj = new GameObject("MusicSource");
                musicObj.transform.SetParent(transform);
                musicSource = musicObj.AddComponent<AudioSource>();
                musicSource.loop = true;
                musicSource.playOnAwake = false;
            }

            if (sfxSource == null)
            {
                GameObject sfxObj = new GameObject("SFXSource");
                sfxObj.transform.SetParent(transform);
                sfxSource = sfxObj.AddComponent<AudioSource>();
                sfxSource.loop = false;
                sfxSource.playOnAwake = false;
            }

            UpdateVolumes();
        }

        /// <summary>
        /// 播放背景音乐
        /// </summary>
        public void PlayMusic(AudioClip clip, bool loop = true)
        {
            if (musicSource == null || clip == null) return;

            musicSource.clip = clip;
            musicSource.loop = loop;
            musicSource.Play();
        }

        /// <summary>
        /// 停止背景音乐
        /// </summary>
        public void StopMusic()
        {
            if (musicSource != null)
            {
                musicSource.Stop();
            }
        }

        /// <summary>
        /// 播放音效
        /// </summary>
        public void PlaySFX(AudioClip clip)
        {
            if (sfxSource == null || clip == null) return;
            sfxSource.PlayOneShot(clip, sfxVolume);
        }

        /// <summary>
        /// 播放音效（通过名称）
        /// </summary>
        public void PlaySFX(string sfxName)
        {
            if (sfxClips.ContainsKey(sfxName))
            {
                PlaySFX(sfxClips[sfxName]);
            }
        }

        /// <summary>
        /// 注册音效
        /// </summary>
        public void RegisterSFX(string name, AudioClip clip)
        {
            if (!sfxClips.ContainsKey(name))
            {
                sfxClips[name] = clip;
            }
        }

        /// <summary>
        /// 设置主音量
        /// </summary>
        public void SetMasterVolume(float volume)
        {
            masterVolume = Mathf.Clamp01(volume);
            UpdateVolumes();
        }

        /// <summary>
        /// 设置音乐音量
        /// </summary>
        public void SetMusicVolume(float volume)
        {
            musicVolume = Mathf.Clamp01(volume);
            UpdateVolumes();
        }

        /// <summary>
        /// 设置音效音量
        /// </summary>
        public void SetSFXVolume(float volume)
        {
            sfxVolume = Mathf.Clamp01(volume);
            UpdateVolumes();
        }

        private void UpdateVolumes()
        {
            if (musicSource != null)
            {
                musicSource.volume = masterVolume * musicVolume;
            }
            // SFX volume is applied in PlaySFX
        }

        /// <summary>
        /// 播放主菜单音乐
        /// </summary>
        public void PlayMainMenuMusic()
        {
            if (mainMenuMusic != null)
                PlayMusic(mainMenuMusic);
        }

        /// <summary>
        /// 播放战斗音乐
        /// </summary>
        public void PlayCombatMusic()
        {
            if (combatMusic != null)
                PlayMusic(combatMusic);
        }

        /// <summary>
        /// 播放胜利音乐
        /// </summary>
        public void PlayVictoryMusic()
        {
            if (victoryMusic != null)
                PlayMusic(victoryMusic, false);
        }

        /// <summary>
        /// 播放失败音乐
        /// </summary>
        public void PlayDefeatMusic()
        {
            if (defeatMusic != null)
                PlayMusic(defeatMusic, false);
        }
    }
}


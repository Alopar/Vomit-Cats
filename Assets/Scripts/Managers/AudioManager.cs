using System;
using UnityEngine;

namespace VomitCats
{
    public class AudioManager : MonoBehaviour
    {
        [SerializeField] private bool _soundOn;
        [SerializeField] private bool _musicOn;

        [SerializeField, Range(0f, 1f)] private float _soundVolume;
        [SerializeField, Range(0f, 1f)] private float _musicVolume;

        [SerializeField] private Sound[] sounds;

        private static AudioManager _self;

        void Awake()
        {
            _self = this;

            foreach (Sound sound in sounds)
            {
                sound.source = gameObject.AddComponent<AudioSource>();
                sound.source.clip = sound.clip;
                sound.source.volume = sound.volume;
                sound.source.loop = sound.loop;
            }

            if (GameSettings.Mute)
            {
                _soundOn = false;
                _musicOn = false;
            }
            else
            {
                _soundOn = true;
                _musicOn = true;
            }
        }

        private void ChangeSoundOn(bool value)
        {
            _soundOn = value;
            ChangeMute(SoundType.Sound, _soundOn);
        }

        private void ChangeMusicOn(bool value)
        {
            _musicOn = value;
            ChangeMute(SoundType.Music, _musicOn);
        }

        private void ChangeSoundVolume(float value)
        {
            _soundVolume = value;
            ChangeVolume(SoundType.Sound);
        }

        private void ChangeMusicVolume(float value)
        {
            _musicVolume = value;
            ChangeVolume(SoundType.Music);
        }

        private void ChangeVolume(SoundType soundType)
        {
            Sound[] _sounds = Array.FindAll(sounds, sound => sound.soundType == soundType);
            foreach (Sound _sound in _sounds)
            {
                switch (_sound.soundType)
                {
                    case SoundType.Sound:
                        _sound.source.volume = _sound.volume * _soundVolume;
                        break;
                    case SoundType.Music:
                        _sound.source.volume = _sound.volume * _musicVolume;
                        break;
                }
            }
        }

        public static void ChangeMute(SoundType soundType, bool mute)
        {
            Sound[] _sounds = Array.FindAll(_self.sounds, sound => sound.soundType == soundType);
            foreach (Sound sound in _sounds)
            {
                sound.source.mute = mute;
            }
        }

        public static void Play(string name)
        {
            Sound sound = Array.Find(_self.sounds, sound => sound.name == name);

            if (sound == null)
            {
                Debug.LogWarning("Sound: " + name + " not found!");
                return;
            }

            switch (sound.soundType)
            {
                case SoundType.Sound:
                    sound.source.Play();
                    sound.source.mute = !_self._soundOn;
                    sound.source.volume = _self._soundVolume;
                    break;
                case SoundType.Music:
                    sound.source.Play();
                    sound.source.mute = !_self._musicOn;
                    sound.source.volume = _self._musicVolume;
                    break;
            }
        }

        public static void Stop(string name)
        {
            Sound sound = Array.Find(_self.sounds, sound => sound.name == name);

            if (sound == null)
            {
                Debug.LogWarning("Sound: " + name + " not found!");
                return;
            }

            sound.source.Stop();
        }
    }

    [Serializable]
    public class Sound
    {
        public string name;
        public AudioClip clip;
        public bool loop;
        public SoundType soundType;
        [Range(0f, 1f)] public float volume;
        [HideInInspector] public AudioSource source;
    }

    public enum SoundType
    {
        Sound,
        Music
    }
}
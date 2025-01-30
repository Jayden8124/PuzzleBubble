using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using System.Collections.Generic;

namespace PuzzleBubble
{
    public class Audio
    {
        private Dictionary<string, SoundEffect> _sounds;
        private Dictionary<string, SoundEffectInstance> _soundInstances;

        public Audio()
        {
            _sounds = new Dictionary<string, SoundEffect>();
            _soundInstances = new Dictionary<string, SoundEffectInstance>();
        }

        // Load a sound into memory
        public void Load(ContentManager content, string soundName, string filePath)
        {
            // Load the sound effect
            SoundEffect sound = content.Load<SoundEffect>(filePath);
            _sounds[soundName] = sound;
        }

        // Play a sound effect once
        public void Play(string soundName)
        {
            if (_sounds.ContainsKey(soundName))
            {
                _sounds[soundName].Play();
            }
        }

        // Play a sound effect in a loop
        public void PlayLoop(string soundName)
        {
            if (_sounds.ContainsKey(soundName))
            {
                var soundInstance = _sounds[soundName].CreateInstance();
                soundInstance.IsLooped = true;
                soundInstance.Play();
                _soundInstances[soundName] = soundInstance;
            }
        }

        // Stop a looping sound effect
        public void StopLoop(string soundName)
        {
            if (_soundInstances.ContainsKey(soundName))
            {
                _soundInstances[soundName].Stop();
                _soundInstances.Remove(soundName);
            }
        }

        // Pause a looping sound effect
        public void PauseLoop(string soundName)
        {
            if (_soundInstances.ContainsKey(soundName))
            {
                _soundInstances[soundName].Pause();
            }
        }

        // Resume a paused sound effect
        public void ResumeLoop(string soundName)
        {
            if (_soundInstances.ContainsKey(soundName))
            {
                _soundInstances[soundName].Resume();
            }
        }

        // Stop all sounds
        public void StopAll()
        {
            foreach (var soundInstance in _soundInstances.Values)
            {
                soundInstance.Stop();
            }

            _soundInstances.Clear();
        }
    }
}
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

using System.Collections.Generic;

public class TDSound : TDComponent
{
    public SoundEffect SoundEffect;

    public float Volume = 1f;

    public bool SingleInstance = false;

    public bool IsPositional = false;

    private bool _isLooped = false;

    public bool IsLooped
    {
        get => _isLooped;
        set
        {
            _isLooped = value;
            foreach (SoundEffectInstance soundEffectInstance in _soundEffectInstances)
            {
                soundEffectInstance.IsLooped = _isLooped;
            }
        }
    }

    private float _time = 0f;
    private bool _isPlaying = false;

    private List<SoundEffectInstance> _soundEffectInstances = new List<SoundEffectInstance>();
    private List<float> _targetTimes = new List<float>();

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);

        if (_isPlaying) _time += (float)gameTime.ElapsedGameTime.TotalSeconds;

        if (_targetTimes.Count > 0 && _targetTimes[0] <= _time)
        {
            _soundEffectInstances.RemoveAt(0);
            _targetTimes.RemoveAt(0);
        }
    }

    public void Play()
    {

        if (!SingleInstance || !_isPlaying)
        {
            _isPlaying = true;

            SoundEffectInstance soundEffectInstance = SoundEffect.CreateInstance();
            soundEffectInstance.IsLooped = IsLooped;
            soundEffectInstance.Volume = Volume;
            soundEffectInstance.Play();

            _soundEffectInstances.Add(soundEffectInstance);
            _targetTimes.Add(_time + (float)SoundEffect.Duration.TotalSeconds);
        }
    }

    public void Pause()
    {
        _isPlaying = false;

        foreach (SoundEffectInstance soundEffectInstance in _soundEffectInstances)
        {
            soundEffectInstance.Pause();
        }
    }

    public void Stop()
    {
        foreach (SoundEffectInstance soundEffectInstance in _soundEffectInstances)
        {
            soundEffectInstance.Stop();
        }
    }
}
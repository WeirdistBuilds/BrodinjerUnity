using UnityEngine;

public class RandomSoundController : SoundController
{
    private int _clipLength;
    public AudioClip[] _mClips;
    
    
    public override void Play()
    {
        //Debug.Log("Play: " + gameObject.name);
        _clipLength = _mClips.Length;
        if (_mSource.isPlaying)
        {
            _mSource.Stop();
        }
        _mSource.clip = _mClips[Random.Range(0, _clipLength - 1)];
        _mSource.Play();
    }

}

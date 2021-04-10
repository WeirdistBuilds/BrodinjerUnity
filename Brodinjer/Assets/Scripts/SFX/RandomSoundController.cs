using UnityEngine;

public class RandomSoundController : SoundController
{
    private int _clipLength;
    public AudioClip[] _mClips;
    public int prevnum = -1;
    
    public override void Play()
    {
        //Debug.Log("Play: " + gameObject.name);
        _clipLength = _mClips.Length;
        if (_mSource.isPlaying)
        {
            _mSource.Stop();
        }
        int randomNum = Random.Range(0, _mClips.Length);
        if(randomNum == prevnum)
        {
            randomNum += 1;
            if(randomNum >= _mClips.Length)
            {
                randomNum -= 2;
            }
        }
        _mSource.clip = _mClips[randomNum];
        _mSource.Play();
    }

}

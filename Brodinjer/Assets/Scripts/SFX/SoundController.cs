using System.Collections;
using UnityEngine;

public class SoundController : MonoBehaviour
{
    protected AudioSource _mSource;
    private float initSound;
    void Start()
    {
        _mSource = GetComponent<AudioSource>();
        try
        {
            initSound = _mSource.volume;
        }
        catch { }

    }

    public virtual void Play()
    {
        if (_mSource.isPlaying)
        {
            _mSource.Stop();
        }
        _mSource.Play();
    }
    
    public virtual void Stop()
    {
        if (_mSource.isPlaying)
        {
            _mSource.Stop();
        }
    }

    public void FadeIn(float fadeTime)
    {
        if (_mSource == null)
            _mSource = GetComponent<AudioSource>();
        float endVolume = initSound;
        _mSource.volume = 0f;
        _mSource.Play();
        StartCoroutine(Fade(0, endVolume, fadeTime, false));
    }

    private IEnumerator Fade(float beginVolume, float endVolume, float fadeTime, bool stop)
    {
        float currentTime = 0;
        while(currentTime < fadeTime)
        {
            currentTime += Time.fixedDeltaTime;
            _mSource.volume = Mathf.Lerp(beginVolume, endVolume, GeneralFunctions.ConvertRange(0, fadeTime, 0, 1, currentTime));
            yield return new WaitForFixedUpdate();
        }
        if (stop)
            _mSource.Stop();
    }

    public void FadeOut(float fadeTime)
    {
        if (_mSource == null)
            _mSource = GetComponent<AudioSource>();
        StartCoroutine(Fade(_mSource.volume, 0, fadeTime, true));
    }
}

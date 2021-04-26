using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    public AudioMixer masterMixer;
    public FloatData masterFloatData, sfxFloatData, musicFloatData, ambienceFloatData;
    public Slider masterSlider, sfxSlider, musicSlider, ambienceSlider;
    public TMPro.TMP_Dropdown resolutionsDropdown, qualityDropdown;
    private Resolution[] resolutions;
    public IntData QualityIndex;
    public BoolData VsyncOn;
    public Toggle VsyncToggle;

    void Start()
    {
        resolutions = Screen.resolutions.Select(
            resolution => new Resolution {width = resolution.width, height = resolution.height}
            ).Distinct().ToArray();
        
        resolutionsDropdown.ClearOptions();
        List<string> options = new List<string>();
        int currentResolutionIndex = 0;
        
        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + "x" + resolutions[i].height;
            options.Add(option);

            if (resolutions[i].width == Screen.width &&
                resolutions[i].height == Screen.height)
            {
                currentResolutionIndex = i;
            }
        }
        
        resolutionsDropdown.AddOptions(options);
        resolutionsDropdown.value = currentResolutionIndex;
        resolutionsDropdown.RefreshShownValue();

        masterMixer.SetFloat("MasterVolume", masterFloatData.value);
        masterMixer.SetFloat("SFXVolume", sfxFloatData.value);
        masterMixer.SetFloat("MusicVolume", musicFloatData.value);
        masterMixer.SetFloat("AmbienceVolume", ambienceFloatData.value);

        masterSlider.value = masterFloatData.value;
        sfxSlider.value = sfxFloatData.value;
        musicSlider.value = musicFloatData.value;
        ambienceSlider.value = ambienceFloatData.value;

        Debug.Log("Set Quality: " + QualityIndex.value);
        qualityDropdown.SetValueWithoutNotify(QualityIndex.value);
        SetQuality(QualityIndex.value);
        VsyncToggle.isOn = VsyncOn.value;
        if (VsyncOn.value)
            QualitySettings.vSyncCount = 1;
        else
            QualitySettings.vSyncCount = 0;
    }
    
    public void SetMasterVolume(float volume)
    {
        masterMixer.SetFloat("MasterVolume", volume);
        masterFloatData.value = volume;
    }
    
    public void SetSFXVolume(float volume)
    {
        masterMixer.SetFloat("SFXVolume", volume);
        sfxFloatData.value = volume;
    }
    
    public void SetMusicVolume(float volume)
    {
        masterMixer.SetFloat("MusicVolume", volume);
        musicFloatData.value = volume;
    }
    
    public void SetAmbienceVolume(float volume)
    {
        masterMixer.SetFloat("AmbienceVolume", volume);
        ambienceFloatData.value = volume;
    }

    public void SetQuality(int qualityIndex)
    {
        QualityIndex.value = qualityIndex;
        QualitySettings.SetQualityLevel(qualityIndex);
    }

    public void SetFullScreen(bool isFullScreen)
    {
        Screen.fullScreen = isFullScreen;
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    public void SetVsync(bool vsync)
    {
        VsyncOn.value = vsync;
        if (vsync)
        {
            QualitySettings.vSyncCount = 1;
        }
        else
        {
            QualitySettings.vSyncCount = 0;
        }
    }
}
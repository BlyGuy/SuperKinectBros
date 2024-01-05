using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;

public class SettingsMenu : MonoBehaviour
{
    public AudioMixer audioMixer;
    public float mouseSensitivity = 100f;
    
    public bool isFullscreen = true;
    public GameObject fullscreenCheckmark;

    public bool KinectViewMoved = false;
    public Transform KinectViewTransform;

    //Resolution[] resolutions;
    //public TMP_Dropdown resolutionDropdown;

    void Start()
    {
        //resolutions = Screen.resolutions;

        //resolutionDropdown.ClearOptions();

        //List<string> options = new List<string>();
        //int currentResolutionIndex = 0;

        //for(int i = 0; i < resolutions.Length; i++)
        //{
        //    string option = resolutions[i].width + "x" + resolutions[i].height;
        //    options.Add(option);

        //    if (resolutions[i].width == Screen.currentResolution.width && 
        //        resolutions[i].height == Screen.currentResolution.height)
        //    {
        //        currentResolutionIndex = i;
        //    }
        //}

        //resolutionDropdown.AddOptions(options);
        //resolutionDropdown.value = currentResolutionIndex;
        //resolutionDropdown.RefreshShownValue();
    }


    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("volume", volume);
    }

    //public void SetQuality(int qualityIndex)
    //{
    //    QualitySettings.SetQualityLevel(qualityIndex);
    //}

    public void ToggleFullscreen()
    {

        isFullscreen = !isFullscreen; //Toggle bool
        Screen.fullScreen = isFullscreen; //Toggle screen
        fullscreenCheckmark.SetActive(isFullscreen);//Toggle checkmark
    }

    //public void SetResolution(int resolutionIndex)
    //{
    //    Resolution resolution = resolutions[resolutionIndex];
    //    Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    //}

    public void ToggleMoveKinectView()
    {
        //if (KinectViewMoved)
        //    KinectViewTransform.localPosition += 500.0f;
        //else
        //    KinectViewTransform.localPosition -= 500.0f;
        KinectViewMoved = !KinectViewMoved;
    }
}

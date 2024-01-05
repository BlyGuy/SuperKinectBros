using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;

public class SettingsMenu : MonoBehaviour
{
    public AudioMixer audioMixer;
    //public float mouseSensitivity = 100f;
    
    public bool isFullscreen = true;
    public GameObject fullscreenCheckmark;

    public bool KinectViewEnabled = true;
    public GameObject KinectView;
    public GameObject KinectViewPanel;
    public GameObject KinectViewCheckmark;

    //Resolution[] resolutions;
    //public TMP_Dropdown resolutionDropdown;

    void Start()
    {
        //ToggleHalfTime();
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

    public void ToggleFullscreen()
    {

        isFullscreen = !isFullscreen; //Toggle bool
        Screen.fullScreen = isFullscreen; //Toggle screen
        fullscreenCheckmark.SetActive(isFullscreen);//Toggle checkmark
    }

    public void ToggleHalfTime()
    {
        Time.timeScale = 0.5f;
    }

    public void ToggleMoveKinectView()
    {
        //Move the Kinect View off-screen ;)
        KinectViewEnabled = !KinectViewEnabled;
        if (KinectViewEnabled)
            KinectView.transform.position -= new Vector3(0.0f, 10000.0f, 0.0f);
        else
            KinectView.transform.position += new Vector3(0.0f, 10000.0f, 0.0f);
        KinectView.GetComponent<BodySourceView>().updateBodyPositions();
        KinectViewPanel.SetActive(KinectViewEnabled);
        KinectViewCheckmark.SetActive(KinectViewEnabled);
    }

    //public void SetQuality(int qualityIndex)
    //{
    //    QualitySettings.SetQualityLevel(qualityIndex);
    //}

    //public void SetResolution(int resolutionIndex)
    //{
    //    Resolution resolution = resolutions[resolutionIndex];
    //    Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    //}
}

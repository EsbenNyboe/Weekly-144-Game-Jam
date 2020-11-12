using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundObject : MonoBehaviour
{
    public bool debug;
    public AudioSource[] audioSources;

    public AudioClip singleFile;
    [Range(0, 1)]
    public float volumeStart;
    [Range(0, 2)]
    public float pitchStart;
    [Range(0, 1)]
    public float pitchRandomWidth;

    public AudioClip[] multiFile;
    public float[] volumeMulti;
    public float[] pitchMulti;

    public bool playOneShot;

    bool multiFileMode;
    bool multiVolMode;
    bool multiPitchMode;

    AudioClip selFile; // selected file
    int selFileIndex; // selected file index
    int voiceIndex;
    int voiceMax;

    float vol;
    float pitch;

    IEnumerator volFade;
    float volFadeStart;
    float volFadePath;
    float timeFadeDuration;
    float timeFadeStart;
    float timeProgress;

    public enum PlaybackMode
    {
        RandomNoRepeat,
        Random,
        Sequence
    }
    public PlaybackMode playbackMode;

    #region Initialization
    private void Awake()
    {
        PrepareVariables();
    }

    private void PrepareVariables()
    {
        multiFileMode = multiFile.Length > 0;
        multiVolMode = volumeMulti.Length > 0;
        multiPitchMode = pitchMulti.Length > 0;

        voiceMax = audioSources.Length - 1;
        vol = volumeStart;
        if (pitchStart == 0)
            pitchStart = 1;
        pitch = pitchStart;
        PrepareFile();
    }
    #endregion

    public void PlayInEditor()
    {
        PrepareVariables();
        PlayDefault();
    }
    public void PlayDefault()
    {
        if (debug)
            Debug.Log("sound:" + this.name, gameObject);
        switch (playbackMode)
        {
            case PlaybackMode.RandomNoRepeat:
                PlayRandomNoRepeat();
                break;
            case PlaybackMode.Random:
                PlayRandom();
                break;
            case PlaybackMode.Sequence:
                PlaySequence();
                break;
        }
    }

    #region File Sequence
    public void PlaySequence()
    {
        PlaySound();
        SelectNextFileInSequence();
        PrepareFile();
    }

    private void SelectNextFileInSequence()
    {
        selFileIndex++;
        if (selFileIndex >= multiFile.Length)
            selFileIndex = 0;
    }

    public void ResetSequence()
    {
        selFileIndex = 0;
    }
    #endregion


    #region File Random
    public void PlayRandom()
    {
        selFileIndex = Random.Range(0, multiFile.Length);
        PrepareFile();
        PlaySound();
    }

    private void PrepareFile()
    {
        if (multiFileMode)
        {
            selFile = multiFile[selFileIndex];
        }
        else
            selFile = singleFile;
    }

    #endregion


    #region File Random No Repeat

    public void PlayRandomNoRepeat()
    {
        if (multiFileMode)
            SelectRandomFileNoRepeat();
        PrepareFile();
        PlaySound();
    }

    private void SelectRandomFileNoRepeat()
    {
        int selFileIndexPrevious = selFileIndex;
        int randomDirection = Random.Range(0, 2);
        if (randomDirection != 1)
            randomDirection = -1;

        int count = 0;
        int countMax = 2;
        while (selFileIndex == selFileIndexPrevious)
        {
            if (count < countMax)
            {
                count++;
                selFileIndex = Random.Range(0, multiFile.Length);
            }
            else 
            {
                if (selFileIndex + randomDirection >= multiFile.Length)
                    selFileIndex = multiFile.Length - 2;
                else if (selFileIndex + randomDirection < 0)
                    selFileIndex = 1;
                else
                    selFileIndex += randomDirection;
            }
        }
    }


    #endregion


    #region Load and Play
    public void PlaySound()
    {
        SelectVoice();
        PrepareAudioSource(audioSources[voiceIndex]);
        if (playOneShot)
            audioSources[voiceIndex].PlayOneShot(selFile);
        else
            audioSources[voiceIndex].Play();
    }
    private void SelectVoice()
    {
        voiceIndex++;
        if (voiceIndex > voiceMax)
            voiceIndex = 0;
    }
    private void PrepareAudioSource(AudioSource audioSource)
    {
        audioSource.clip = selFile;
        LoadVolume(audioSource);
        LoadPitch(audioSource);

        //if (debug)
        //    print("v:" + audioSource.volume + " p:" + audioSource.pitch + " sel:" + selFileIndex);
    }
    private void LoadVolume(AudioSource audioSource)
    {
        if (multiVolMode)
            audioSource.volume = vol * volumeMulti[selFileIndex];
        else
            audioSource.volume = vol;
    }
    private void LoadPitch(AudioSource aSource)
    {
        if (multiPitchMode)
            aSource.pitch = pitch * pitchMulti[selFileIndex];
        else
            aSource.pitch = pitch;
        if (pitchRandomWidth > 0)
        {
            aSource.pitch *= Random.Range(aSource.pitch - pitchRandomWidth / 2, aSource.pitch + pitchRandomWidth / 2);
            if (aSource.pitch < 0)
                aSource.pitch = 0;
        }
    }
    #endregion


    #region VolumeFade

    AnimationCurve fadeCurve;
    public void VolumeFade(float targetVol, float fadeTime)
    {
        VolumeFade(targetVol, fadeTime, 0);
    }
    public void VolumeFade(float targetVol, float fadeTime, int straightCurvedSteepOrSteeplog) // configures and triggers the fade procedure
    {
        volFadeStart = vol;
        volFadePath = targetVol * volumeStart - volFadeStart; // targetVol=1, the target is not 1, but is equal to volumeStart

        timeFadeDuration = fadeTime;
        timeFadeStart = Time.time;

        switch (straightCurvedSteepOrSteeplog)
        {
            default:
                fadeCurve = AudioSystem.fadeCurvesStatic.straight;
                break;
            case 1:
                fadeCurve = AudioSystem.fadeCurvesStatic.curved;
                break;
            case 2:
                fadeCurve = AudioSystem.fadeCurvesStatic.steep;
                break;
            case 3:
                fadeCurve = AudioSystem.fadeCurvesStatic.steepLog;
                break;
        }

        if (volFade != null)
            StopCoroutine(volFade);

        volFade = VolFade();
        StartCoroutine(VolFade());
    }
    IEnumerator VolFade() // runs every frame, until fade procedure is done
    {
        bool stopFading = false;
        while (!stopFading)
        {
            timeProgress = (Time.time - timeFadeStart) / (timeFadeDuration);
            if (timeProgress > 1)
            {
                timeProgress = 1;
                stopFading = true;
            }
            if (timeProgress < 0)
            {
                timeProgress = 0;
                stopFading = true;
            }
            vol = volFadeStart + fadeCurve.Evaluate(timeProgress) * volFadePath;
            for (int i = 0; i < voiceMax; i++)
            {
                if (multiVolMode)
                    audioSources[i].volume = vol * volumeMulti[i];
                else
                    audioSources[i].volume = vol;
            }

            if (debug)
            {
                AudioSystem.fadeCurvesStatic.debugVolumeProgress = timeProgress;
                AudioSystem.fadeCurvesStatic.debugVolume = vol;
                AudioSystem.fadeCurvesStatic.debugDontEdit.AddKey(Time.realtimeSinceStartup, vol);
            }

            yield return null;
        }
    }
    #endregion

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSystem : MonoBehaviour
{
    public Soundbank soundbank;
    public static Soundbank sb;
    public FadeCurves fadeCurves;
    public static FadeCurves fadeCurvesStatic;

    private void Awake()
    {
        sb = soundbank;
        fadeCurvesStatic = fadeCurves;
    }
}
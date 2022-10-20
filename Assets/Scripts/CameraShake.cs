using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Events;
using DG.Tweening;

public class CameraShake : MonoBehaviour
{

    public enum Strength
    {
        VeryLow,
        Low,
        Medium,
        High,
        VeryHigh
    }

    Camera cam;

    private void Awake() => cam = Camera.main;


    private void OnEnable() => GameEvents.onScreenShakeEvent += Shake;

    private void OnDisable() => GameEvents.onScreenShakeEvent += Shake;


    void Shake(Strength str)
    {
        switch (str)
        {
            case Strength.VeryLow:
                cam.DOShakePosition(.1f, .25f, 5, 10, true, ShakeRandomnessMode.Harmonic);
                break;
            case Strength.Low:
                cam.DOShakePosition(.1f, .25f, 10, 10, true, ShakeRandomnessMode.Full);
                break;
            case Strength.Medium:
                cam.DOShakePosition(.3f, .3f, 40, 90, true, ShakeRandomnessMode.Full);
                break;
            case Strength.High:
                cam.DOShakePosition(.3f, .3f, 60, 90, true, ShakeRandomnessMode.Full);
                break;
            case Strength.VeryHigh:
                cam.DOShakePosition(.3f, .3f, 100, 90, true, ShakeRandomnessMode.Full);
                break;
            default:
                break;
        }

    }
}

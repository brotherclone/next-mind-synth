using System.Collections;
using NextMind.Calibration;
using NextMind.NeuroTags;
using UnityEngine;

public class CalibrationTagBehavior : TagCalibrationBehaviour
{
    public override void OnInitialize(NeuroTag tag)
    {
        Debug.Log("init");
        return;
    }

    public override IEnumerator OnStartCalibrating(NeuroTag tag)
    {
        Debug.Log("start");
        CalibrationUIManager.Instance.TagCounterAnimate();
        yield break;
    }
    
    public override IEnumerator OnEndCalibrating(NeuroTag tag)
    {
        Debug.Log("end");
        CalibrationUIManager.Instance.AdvanceTagCounter();
        yield break;
    }
}

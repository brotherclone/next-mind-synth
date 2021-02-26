using System.Collections;
using NextMind.Calibration;
using NextMind.NeuroTags;

public class CalibrationTagBehavior : TagCalibrationBehaviour
{
    public override void OnInitialize(NeuroTag tag)
    {
        return;
    }

    public override IEnumerator OnStartCalibrating(NeuroTag tag)
    {
        CalibrationUIManager.Instance.TagCounterAnimate();
        yield break;
    }

    public override IEnumerator OnEndCalibrating(NeuroTag tag)
    {
        CalibrationUIManager.Instance.AdvanceTagCounter();
        yield break;
    }
}
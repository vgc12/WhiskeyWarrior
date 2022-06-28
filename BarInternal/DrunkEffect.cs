using DrunkMan.BuiltIn;
using System.Collections;
using UnityEngine;


public class DrunkEffect : MonoBehaviour
{


    [SerializeField] private AnimationCurve curve;
 
    public float duration,t, min, max, speed;
  
    private bool inverted;
    [SerializeField] private Demo drunkShader;
    private string RGBShiftFactor,
            RGBShiftPower,
            GhostSeeAmplitude,
            GhostSeeRadius,
            GhostSeeMix,
            Frequency,
            Period,
            Amplitude,
            BlurMax,
            BlurMin,
            BlurSpeed,
            Intensity,
            InterpSpeed;


    private Keyframe keyframe;
    private void Start()
    {
        //this changes the values of the camera based on how much the player has drank, the more they drink the harder it is too see

        RGBShiftFactor = "RGBShiftFactor";
        RGBShiftPower = "RGBShiftFactor";
        GhostSeeAmplitude = "GhostSeeAmplitude";
        GhostSeeRadius = "GhostSeeRadius";
        GhostSeeMix = "GhostSeeMix";
        Frequency = "Frequency";
        Period = "Period";
        Amplitude = "Amplitude";
        BlurMax = "BlurMax";
        BlurMin = "BlurMin";
        BlurSpeed = "BlurSpeed";
        Intensity = "Intensity";
        InterpSpeed = "InterpSpeed";


        if (DialogueVariables.amountDrunk == 0)
        {
            drunkShader.m_DrunkIntensity = 0;
            drunkShader.m_RGBShiftFactor = 0;
            drunkShader.m_RGBShiftPower = 0;
            drunkShader.m_GhostSeeAmplitude = 0;
            drunkShader.m_GhostSeeRadius = 0;
            drunkShader.m_GhostSeeMix = 0;
            drunkShader.m_Frequency = 0;
            drunkShader.m_Period = 0;
            drunkShader.m_Amplitude = 0;
            drunkShader.m_BlurMax = 0;
            drunkShader.m_BlurMin = 0f;
            drunkShader.m_BlurSpeed = 0;
            speed = 0;
        }
        else
            GetDrunkStats();


        min = 0f;
        max = 0.4f;

        if (speed > 1)
            speed = 1;
        
        keyframe = curve[curve.length - 1];
        duration = keyframe.time;

        t = 0.0f;
        inverted = false;
    }

    private void SaveDrunkStats()
    {
        PlayerPrefs.SetFloat(RGBShiftFactor, drunkShader.m_RGBShiftFactor);
        PlayerPrefs.SetFloat(RGBShiftPower, drunkShader.m_RGBShiftPower);
        PlayerPrefs.SetFloat(GhostSeeAmplitude, drunkShader.m_GhostSeeAmplitude);
        PlayerPrefs.SetFloat(GhostSeeRadius, drunkShader.m_GhostSeeRadius);
        PlayerPrefs.SetFloat(GhostSeeMix, drunkShader.m_GhostSeeMix);
        PlayerPrefs.SetFloat(Frequency, drunkShader.m_Frequency);
        PlayerPrefs.SetFloat(Intensity, drunkShader.m_DrunkIntensity);
        PlayerPrefs.SetFloat(Period, drunkShader.m_Period);
        PlayerPrefs.SetFloat(Amplitude, drunkShader.m_Amplitude);
        PlayerPrefs.SetFloat(BlurMax, drunkShader.m_BlurMax);
        PlayerPrefs.SetFloat(BlurSpeed, drunkShader.m_BlurSpeed);
        PlayerPrefs.SetFloat(InterpSpeed, speed);

    }

    private void GetDrunkStats()
    {
        drunkShader.m_RGBShiftFactor = PlayerPrefs.GetFloat(RGBShiftFactor);
        drunkShader.m_RGBShiftPower = PlayerPrefs.GetFloat(RGBShiftPower);
        drunkShader.m_GhostSeeAmplitude = PlayerPrefs.GetFloat(GhostSeeAmplitude);
        drunkShader.m_GhostSeeRadius = PlayerPrefs.GetFloat(GhostSeeRadius);
        drunkShader.m_GhostSeeMix = PlayerPrefs.GetFloat(GhostSeeMix);
        drunkShader.m_Frequency = PlayerPrefs.GetFloat(Frequency);
        drunkShader.m_DrunkIntensity = PlayerPrefs.GetFloat(Intensity);
        drunkShader.m_Period = PlayerPrefs.GetFloat(Period);
        drunkShader.m_Amplitude = PlayerPrefs.GetFloat(Amplitude);
        drunkShader.m_BlurMax = PlayerPrefs.GetFloat(BlurMax);
        drunkShader.m_BlurSpeed = PlayerPrefs.GetFloat(BlurSpeed);
        speed = PlayerPrefs.GetFloat(InterpSpeed);
    }



    public void UpdateStats(float increase, bool dmLight)
    {

        float actualIncrease = increase / 10f;

        if (drunkShader.m_DrunkIntensity + actualIncrease <= 0.8f && !dmLight)
            drunkShader.m_DrunkIntensity += actualIncrease;

        else
            drunkShader.m_DrunkIntensity = 0.8f;



        if (!dmLight)
        {
            //writing it like this is better then having like 13 if then statements, puts a cap on the values to avoid errors
            drunkShader.m_DrunkIntensity = drunkShader.m_DrunkIntensity + actualIncrease > 0.99f ? drunkShader.m_DrunkIntensity = 0.99f : drunkShader.m_DrunkIntensity + actualIncrease;
            drunkShader.m_RGBShiftFactor = drunkShader.m_RGBShiftFactor + actualIncrease > 0.13f ? drunkShader.m_RGBShiftFactor = 0.13f : drunkShader.m_RGBShiftFactor + actualIncrease;
            drunkShader.m_RGBShiftPower = drunkShader.m_RGBShiftPower + actualIncrease > 5.7f ? drunkShader.m_RGBShiftPower = 5.7f : drunkShader.m_RGBShiftPower + actualIncrease;
            drunkShader.m_GhostSeeAmplitude = drunkShader.m_GhostSeeAmplitude > 0.2f ? drunkShader.m_GhostSeeAmplitude = 0.19999f : drunkShader.m_GhostSeeAmplitude + actualIncrease;
            drunkShader.m_GhostSeeRadius = drunkShader.m_GhostSeeRadius + actualIncrease > 0.06f ? drunkShader.m_GhostSeeAmplitude = 0.0599999f : drunkShader.m_GhostSeeAmplitude + actualIncrease;
            drunkShader.m_GhostSeeMix = drunkShader.m_GhostSeeMix + actualIncrease > 1f ? drunkShader.m_GhostSeeMix = 0.9999999f : drunkShader.m_GhostSeeMix + actualIncrease;
            drunkShader.m_Frequency = drunkShader.m_Frequency + actualIncrease > 8f ? drunkShader.m_Frequency = 7.999999f : drunkShader.m_Frequency + actualIncrease;
            drunkShader.m_Period = drunkShader.m_Period + actualIncrease > 4f ? drunkShader.m_Period = 3.99999999f : drunkShader.m_Period + actualIncrease;
            drunkShader.m_Amplitude = drunkShader.m_Amplitude + actualIncrease > 16f ? drunkShader.m_Amplitude = 15.999999f : drunkShader.m_Amplitude + actualIncrease;
            drunkShader.m_BlurMax = drunkShader.m_BlurMax + actualIncrease > 1f ? drunkShader.m_BlurMax = 0.999999999f : drunkShader.m_BlurMax + actualIncrease;
            drunkShader.m_BlurMin = 0f;
            drunkShader.m_BlurSpeed = drunkShader.m_BlurSpeed + actualIncrease > 6f ? drunkShader.m_BlurSpeed = 5.9999999f : drunkShader.m_BlurSpeed + actualIncrease;
            if (DialogueVariables.amountDrunk > 6)
            {
                speed = speed + actualIncrease > 1f ? speed = .9999f : speed + actualIncrease;
            }

        }
        else
        {
            // these are the values of the shader if dmlight is drunk, maxes out everything.
            drunkShader.m_DrunkIntensity = .9999f;
            drunkShader.m_RGBShiftFactor = 0.13f;
            drunkShader.m_RGBShiftPower = 5.7f;
            drunkShader.m_GhostSeeAmplitude = 0.19999f;
            drunkShader.m_GhostSeeRadius = 0.0599999f;
            drunkShader.m_GhostSeeMix = 0.9999999f;
            drunkShader.m_Frequency = 7.999999f;
            drunkShader.m_Period = 3.99999999f;
            drunkShader.m_Amplitude = 15.999999f;
            drunkShader.m_BlurMax = 0.999999999f;
            drunkShader.m_BlurMin = 0f;
            drunkShader.m_BlurSpeed = 5.9999999f;
            speed = 0.9999f;
        }



        SaveDrunkStats();


       
    }



    private void Update()
    {
        // closes and opens eye effect based on amount drunk.

        if (DialogueVariables.amountDrunk > 6)
        {
            drunkShader.m_SleepyEye = true;
            if (t < duration)
            {
                t += Time.deltaTime * speed;
                drunkShader.m_EyeClose = Mathf.Lerp(min, max, curve.Evaluate(t));
            }

            if (t > duration)
            {
                inverted = !inverted;
                float temp = max;
                max = min;
                min = temp;
                t = 0.0f;

            }

        }

    }

}
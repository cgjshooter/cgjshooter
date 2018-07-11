using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class FFTEffects : MonoBehaviour {

    private List<Filter> filters;

    private PostProcessVolume ppBehaviour;
    private PostProcessProfile ppProfile;

    private int fftSize = 1024;
    private List<float[]> history;
    private int historyInd;
    private float[] result;
    // Use this for initialization
    void Start () {
        result = new float[fftSize];
        this.filters = new List<Filter>();
        this.filters.Add(new Filter(0, 600,true));
        this.filters.Add(new Filter(80, 1200, true));
        this.filters.Add(new Filter(1200, 6200, true));
        ppProfile = Camera.main.GetComponent<PostProcessVolume>().profile;
        history = new List<float[]>();
        history.Add(new float[fftSize]);
        history.Add(new float[fftSize]);
        history.Add(new float[fftSize]);
        history.Add(new float[fftSize]);
        history.Add(new float[fftSize]);
        history.Add(new float[fftSize]);

    }
    // Update is called once per frame
    void Update() {
        
        
        AudioListener.GetSpectrumData(history[historyInd%history.Count], 0, FFTWindow.Rectangular);
        historyInd++;
        for (int i = 0; i < fftSize; i++)
            result[i] = history[0][i];

        for (int j = 1; j < history.Count; j++)
        {
            var hs = history[j];
            for (int i = 0; i < fftSize; i++)
                result[i] += hs[i];
        }

        foreach (Filter filter in filters)
        {
            filter.max = 0;
            filter.min = 1;
        }
        
        for (int i = 0; i < fftSize; i++)
        {
            foreach (Filter filter in filters)
            {
                float v = result[i]/history.Count;
                float band = 44100 / fftSize * i;
                if (filter.inclusive && band > filter.low && band < filter.high)
                {
                    filter.max = Mathf.Max(v, filter.max);
                    filter.min = Mathf.Min(v, filter.min);
                }
                else if (!filter.inclusive && (band < filter.low || band > filter.high))
                {
                    filter.max = Mathf.Max(v, filter.max);
                    filter.min = Mathf.Min(v, filter.min);
                }
            }
        }

        //Update the effects
        var vignetSettings = ppProfile.GetSetting<Vignette>();
        
        vignetSettings.intensity.value = Mathf.Clamp(filters[0].max*1f+0.10f, 0f, 0.25f);
        //ppProfile.vignette.settings = vignetSettings;

        var bloomSettings = ppProfile.GetSetting<Bloom>();
        bloomSettings.intensity.value = 5.0f + Mathf.SmoothStep(0.0f, 1.8f, filters[1].max*3.2f);
        //ppProfile.bloom.settings = bloomSettings;

        var rgbSettings = ppProfile.GetSetting<RGBShift>();
        Debug.Log(filters[2].max);
        if(filters[2].max > 0.02f)
        {
            rgbSettings.bShift.value = (filters[2].max-0.02f)*0.05f;
            rgbSettings.gShift.value = -(filters[2].max-0.02f)*0.05f;
        }
        else
        {
            rgbSettings.bShift.value = 0;// Mathf.SmoothStep(0.0f, 0.02f, filters[2].max);
            rgbSettings.gShift.value = 0;// -Mathf.SmoothStep(0.0f, 0.02f, filters[2].max);
        }

        var distortSettings = ppProfile.GetSetting<Distort>();
        distortSettings.intensity.value = filters[2].min*25f;
    }
}

class Filter
{
    public float low;
    public float high;
    public float max;
    public float min;
    public bool inclusive;

    public Filter(float low, float high, bool inclusive)
    {
        this.low = low;
        this.high = high;
        this.inclusive = inclusive;
    }
}
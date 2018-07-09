using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PostProcessing;

public class FFTEffects : MonoBehaviour {

    private List<Filter> filters;

    private PostProcessingBehaviour ppBehaviour;
    private PostProcessingProfile ppProfile;
	// Use this for initialization
	void Start () {
        this.filters = new List<Filter>();
        this.filters.Add(new Filter(0, 600,true));
        this.filters.Add(new Filter(6000, 12000, true));
        ppProfile = Camera.main.GetComponent<PostProcessingBehaviour>().profile;
    }
    private List<float[]> history;
    private int historyInd;
    // Update is called once per frame
    void Update() {
        var fftSize = 1024;
        float[] spectrum = new float[fftSize];

        AudioListener.GetSpectrumData(spectrum, 0, FFTWindow.Rectangular);
        historyInd++;
        history[historyInd % history.Count] = spectrum;

        float[] result = new float[fftSize];

        foreach (Filter filter in filters)
        {
            filter.max = 0;
            filter.min = 1;
        }
        for (int i = 1; i < spectrum.Length - 1; i++)
        {
            foreach (Filter filter in filters)
            {
                float v = spectrum[i];
                float band = 44100 / fftSize * i;
                if (filter.inclusive && band > filter.low && band < filter.high)
                {
                    filter.max = Mathf.Max(v, filter.max);
                    filter.min = Mathf.Max(v, filter.min);
                }
                else if (!filter.inclusive && (band < filter.low || band > filter.high))
                {
                    filter.max = Mathf.Max(v, filter.max);
                    filter.min = Mathf.Max(v, filter.min);
                }
            }
        }

        //Update the effects
        var vignetSettings = ppProfile.vignette.settings;
        vignetSettings.intensity = Mathf.Clamp(filters[0].max*4f, 0f, 0.35f);
        ppProfile.vignette.settings = vignetSettings;

        var bloomSettings = ppProfile.bloom.settings;
        bloomSettings.bloom.intensity = Mathf.Clamp(filters[1].max*8f , 0f, 0.35f)+1f;
        ppProfile.bloom.settings = bloomSettings;

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
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class FFTEffects : MonoBehaviour {

    private List<Filter> filters;
    
    private PostProcessVolume[] ppBehaviour;

    public float blend;

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

        ppBehaviour = Camera.main.GetComponents<PostProcessVolume>();
        
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
        foreach (PostProcessVolume ppVolume in ppBehaviour)
        {
            var ppProfile = ppVolume.profile;    
            
            var vignetSettings = ppProfile.GetSetting<Vignette>();
            
            vignetSettings.intensity.value = Mathf.Clamp(filters[0].max*1f+0.10f, 0f, 0.25f);
            //ppProfile.vignette.settings = vignetSettings;

            var bloomSettings = ppProfile.GetSetting<Bloom>();
            bloomSettings.intensity.value = 5.0f + Mathf.SmoothStep(0.0f, 1.8f, filters[1].max*3.2f);
            //ppProfile.bloom.settings = bloomSettings;

            var rgbSettings = ppProfile.GetSetting<RGBShift>();
        
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
            distortSettings.intensity.value = Mathf.SmoothStep(0f,0.4f, filters[2].min*55f);
            distortSettings.posOff.value = UnityEngine.Random.value;

            Pixelate pixelate;
            var pixelateFound = ppProfile.TryGetSettings<Pixelate>(out pixelate);
            if(pixelateFound)
            {
             //   pixelate.pixelate.value = new Vector2(filters[0].max * Screen.width / 2 + Screen.width / 4, filters[0].max * Screen.height / 2 + Screen.height / 4);
                distortSettings.intensity.value = Mathf.SmoothStep(0f, 0.4f, (filters[0].max + filters[1].max + filters[2].max) * 25f)+0.25f;

            }
        }

        if(ppBehaviour.Length> 0)
            ppBehaviour[0].weight = Mathf.Clamp( 1f - blend, 0f, 1f);
        if (ppBehaviour.Length > 1)
            ppBehaviour[1].weight = Mathf.Clamp( blend < 1f ? blend : 2f-blend, 0f, 1f);
        if (ppBehaviour.Length > 2)
            ppBehaviour[2].weight = Mathf.Clamp( blend < 2f ? blend - 1f : 3f - blend, 0f, 1f);
        if (ppBehaviour.Length > 3)
            ppBehaviour[3].weight = Mathf.Clamp( blend < 3f ? blend - 2f : 4f - blend,0f, 1f);
        if (ppBehaviour.Length > 4)
            ppBehaviour[4].weight = Mathf.Clamp( blend < 4f ? blend - 3f : 5f - blend, 0f, 1f);

    //    blend += Time.deltaTime / 10f;
    //    if (blend > 4f) blend = 0f; 

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
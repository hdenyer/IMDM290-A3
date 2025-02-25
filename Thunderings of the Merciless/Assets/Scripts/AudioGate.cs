using UnityEngine;
using System.Collections;

public class AudioGate : MonoBehaviour
{
    public AudioSource audioSource;
    public float[] spectrumData = new float[256];
    public float minScaleY = 180f;
    public float maxScaleY = 200f;
    public float sensitivity = 150f; // Adjust sensitivity to amplify scale changes
    private Vector3 initialScale;
    private bool isActive = false;
    private float targetScaleY;
    private float currentScaleY;

    // Threshold to ignore insignificant scale changes
    public float scaleThreshold = 3f;

    void Start()
    {
        initialScale = transform.localScale;
        targetScaleY = initialScale.y; // Maintain starting scale before activation
        currentScaleY = initialScale.y; // Start with initial scale
        StartCoroutine(ActivateGate()); // Activate gate after 15 seconds
    }

    IEnumerator ActivateGate()
    {
        yield return new WaitForSeconds(15f);
        isActive = true;
    }

    void Update()
    {
        if (!isActive) return;

        audioSource.GetSpectrumData(spectrumData, 0, FFTWindow.BlackmanHarris);
        float audioLevel = GetAudioLevel();
        targetScaleY = Mathf.Lerp(minScaleY, maxScaleY, Mathf.Clamp01(audioLevel * sensitivity));

        // Only change scale if the difference is above the threshold
        if (Mathf.Abs(targetScaleY - currentScaleY) > scaleThreshold)
        {
            currentScaleY = Mathf.Lerp(currentScaleY, targetScaleY, Time.deltaTime * 5f); // Smooth transition
            transform.localScale = new Vector3(initialScale.x, currentScaleY, initialScale.z);
        }
    }

    float GetAudioLevel()
    {
        float sum = 0f;
        for (int i = 0; i < spectrumData.Length; i++)
        {
            sum += spectrumData[i];
        }
        return sum / spectrumData.Length;
    }
}

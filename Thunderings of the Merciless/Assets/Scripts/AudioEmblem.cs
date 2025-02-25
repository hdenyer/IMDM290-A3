using UnityEngine;
using System.Collections;

public class AudioReactiveEmblem : MonoBehaviour
{
    public AudioSource audioSource;
    public float[] spectrumData = new float[256];
    public float rotationSensitivity = 100f;
    public float lowFrequencyThreshold = 0.1f;
    public float pulseScaleAmount = 2f;
    public float pulseSpeed = 5f;
    private Vector3 originalScale;

    void Start()
    {
        originalScale = transform.localScale;
    }

    void Update()
    {
        audioSource.GetSpectrumData(spectrumData, 0, FFTWindow.BlackmanHarris);
        AnalyzeFrequencies();
    }

    void AnalyzeFrequencies()
    {
        float overallEnergy = GetFrequencyRangeEnergy(0, 256);
        float lowFrequencyEnergy = GetFrequencyRangeEnergy(0, 20);

        RotateEmblem(overallEnergy);
        if (lowFrequencyEnergy > lowFrequencyThreshold)
        {
            StartCoroutine(PulseEmblem());
        }
    }

    float GetFrequencyRangeEnergy(int minIndex, int maxIndex)
    {
        float sum = 0f;
        for (int i = minIndex; i < maxIndex; i++)
        {
            sum += spectrumData[i];
        }
        return sum / (maxIndex - minIndex);
    }

    void RotateEmblem(float energy)
    {
        float rotationSpeed = energy * rotationSensitivity;
        transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);
    }

    IEnumerator PulseEmblem()
    {
        Vector3 targetScale = originalScale * (1f + pulseScaleAmount);
        float elapsedTime = 0f;

        while (elapsedTime < 0.1f)
        {
            transform.localScale = Vector3.Lerp(originalScale, targetScale, elapsedTime * pulseSpeed);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        elapsedTime = 0f;
        while (elapsedTime < 0.1f)
        {
            transform.localScale = Vector3.Lerp(targetScale, originalScale, elapsedTime * pulseSpeed);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }
}

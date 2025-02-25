using UnityEngine;
using System.Collections;

public class AudioLightning : MonoBehaviour
{
    public AudioSource audioSource;
    public float[] spectrumData = new float[256];
    public float highFrequencyThreshold = 0.1f;
    public bool isCenter;
    private Transform[] lightningBolts;
    private bool lightningVisible = false;
    private float flashDuration = 1f; // Time in seconds the lightning stays visible after activation
    private float flashTimer = 0f;

    void Start()
    {
        lightningBolts = GetComponentsInChildren<Transform>(true);
        SetChildrenVisibility(false); // Ensure all lightning is initially invisible
        StartCoroutine(ManageLightning());
    }

    void Update()
    {
        if (!isCenter && Time.time >= 110f && Time.time <= 170f)
        {
            audioSource.GetSpectrumData(spectrumData, 0, FFTWindow.BlackmanHarris);
            float highFrequencyEnergy = GetFrequencyRangeEnergy(0, 20);
            if (highFrequencyEnergy > highFrequencyThreshold && !lightningVisible)
            {
                SetChildrenVisibility(true); // Show lightning bolts
                lightningVisible = true;
                flashTimer = flashDuration; // Reset the flash timer
            }

            if (lightningVisible)
            {
                flashTimer -= Time.deltaTime; // Decrease the timer
                if (flashTimer <= 0f) // After flash duration ends
                {
                    SetChildrenVisibility(false); // Hide lightning bolts
                    lightningVisible = false;
                }
            }
        } 
        else if (!isCenter)
        {
            SetChildrenVisibility(false);
        }
    }

    IEnumerator ManageLightning()
    {
        if (isCenter)
        {
            yield return new WaitForSeconds(162f); 
            SetChildrenVisibility(true);
            yield return new WaitForSeconds(10f);
            SetChildrenVisibility(false);
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

    void SetChildrenVisibility(bool visible)
    {
        foreach (Transform child in lightningBolts)
        {
            if (child != transform) // Exclude the parent object itself
            {
                child.gameObject.SetActive(visible);
            }
        }
    }
}

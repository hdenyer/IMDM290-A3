using UnityEngine;
using System.Collections;

public class AudioReactiveRock : MonoBehaviour
{
    public AudioSource audioSource;
    public float[] spectrumData = new float[256];
    public float lowFrequencyThreshold = 0.05f;
    public float rockJumpHeight = 2f;
    public float rockFallSpeed = 5f;
    public string group;
    private static string activeGroup = "A";
    private bool isMoving = false;

    void Update()
    {
        audioSource.GetSpectrumData(spectrumData, 0, FFTWindow.BlackmanHarris);
        AnalyzeFrequencies();
    }

    void AnalyzeFrequencies()
    {
        float lowFrequencyEnergy = GetFrequencyRangeEnergy(0, 20);
        
        if (lowFrequencyEnergy > lowFrequencyThreshold && !isMoving && group == activeGroup)
        {
            StartCoroutine(RockMovement());
            SwitchActiveGroup();
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

    IEnumerator RockMovement()
    {
        isMoving = true;
        Vector3 startPosition = transform.position;
        Vector3 peakPosition = startPosition + Vector3.up * rockJumpHeight;
        
        // Move up quickly
        float elapsedTime = 0f;
        while (elapsedTime < 0.1f)
        {
            transform.position = Vector3.Lerp(startPosition, peakPosition, elapsedTime / 0.1f);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        transform.position = peakPosition;
        
        // Fall down
        while (transform.position.y > startPosition.y)
        {
            transform.position -= Vector3.up * rockFallSpeed * Time.deltaTime;
            yield return null;
        }
        transform.position = startPosition;
        isMoving = false;
    }

    void SwitchActiveGroup()
    {
        if (activeGroup == "A")
            activeGroup = "B";
        else if (activeGroup == "B")
            activeGroup = "C";
        else
            activeGroup = "A";
    }
}

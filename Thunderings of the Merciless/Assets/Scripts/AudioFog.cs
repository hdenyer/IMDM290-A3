using UnityEngine;
using System.Collections;

public class AudioFog : MonoBehaviour
{
    public Vector3 endPosition;
    public float moveDuration = 60f;
    private Vector3 startPosition;

    void Start()
    {
        startPosition = transform.position;
        StartCoroutine(ManageFogMovement());
    }

    IEnumerator ManageFogMovement()
    {
        yield return new WaitForSeconds(40f);
        yield return MoveFog (startPosition, endPosition, 5f);
        yield return new WaitForSeconds(10f);
        yield return MoveFog (endPosition, startPosition, 5f); // 1min
        yield return new WaitForSeconds(40f); // Wait until 1min 40s
        yield return MoveFog(startPosition, endPosition, 5f);
        yield return new WaitForSeconds(15f); // Hold position til 2min
        yield return MoveFog(endPosition, startPosition, 5f);
        yield return new WaitForSeconds(20f);
        yield return MoveFog (startPosition, endPosition, 20f);
        yield return new WaitForSeconds(10f);
        yield return MoveFog(endPosition, startPosition, 20f);
    }

    IEnumerator MoveFog(Vector3 from, Vector3 to, float duration)
    {
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            transform.position = Vector3.Lerp(from, to, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        transform.position = to;
    }
}
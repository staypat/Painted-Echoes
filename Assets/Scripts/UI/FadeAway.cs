using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeAway : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // turn the object invisible after 3 seconds over a span of 1 second
        StartCoroutine(FadeOut(3f, 1f));
    }

    private IEnumerator FadeOut(float delay, float duration)
    {
        yield return new WaitForSeconds(delay);

        //this.gameObject.SetActive(false);

        // Get the RawImage component
        var rawImage = GetComponent<UnityEngine.UI.RawImage>();
        if (rawImage != null)
        {
            // Start fading out the image
            float elapsedTime = 0f;
            Color color = rawImage.color;

            while (elapsedTime < duration)
            {
                elapsedTime += Time.deltaTime;
                float alpha = Mathf.Lerp(1f, 0f, elapsedTime / duration);
                color.a = alpha;
                rawImage.color = color;
                yield return null; // Wait for the next frame
            }

            // Ensure the final alpha is set to 0
            color.a = 0f;
            rawImage.color = color;

            // Optionally, deactivate the GameObject after fading out
            this.gameObject.SetActive(false);
        }
        else
        {
            Debug.LogWarning("RawImage component not found on the GameObject.");
        }
    }

}

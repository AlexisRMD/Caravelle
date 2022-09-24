using UnityEngine;
using System.Collections;

public class LightFlicker : MonoBehaviour
{

    private Transform thisTransform;
    private Light thisLight;
    public float baseLightIntensity = 1f;
    private float previousIntensity;
    private float nextIntensity;

    public float flickerRange = 0.2f;
    public float movementFlickerAmount = 0.05f;
    public float flickerRate = 0.1f;
    private float flickerClock;
    private Vector3 startPosition;
    public bool smoothMotion = false;
    private Vector3 lastPosition;
    private Vector3 nextPosition;
	// Use this for initialization
	void Start ()
	{
	    if (thisLight == null) thisLight = GetComponent<Light>();
	    baseLightIntensity = thisLight.intensity;
        previousIntensity = baseLightIntensity;
        nextIntensity = baseLightIntensity + Random.Range(-flickerRange, flickerRange);
        thisTransform = transform;
	    startPosition = thisTransform.localPosition;
        lastPosition = startPosition;
        nextPosition = startPosition + Random.insideUnitSphere * movementFlickerAmount;
    }
	
	// Update is called once per frame
	void Update ()
	{
        if (flickerRate == 0f)
            return;

        if (flickerClock < flickerRate)
        {
            flickerClock += Time.deltaTime;
        }

        if (smoothMotion)
        {
            flickerClock = Mathf.Clamp(flickerClock, 0f, flickerRate);
            thisTransform.localPosition = Vector3.Lerp(lastPosition, nextPosition, flickerClock / flickerRate);
            thisLight.intensity = Mathf.Lerp(previousIntensity,nextIntensity, flickerClock / flickerRate);

        }

        if (flickerClock >= flickerRate)
        {
            if (flickerRange > 0f)
            {
                thisLight.intensity = nextIntensity;

                previousIntensity = nextIntensity;
                nextIntensity = baseLightIntensity + Random.Range(-flickerRange, flickerRange);
            }

	        if (movementFlickerAmount > 0f)
	        {
                thisTransform.localPosition = nextPosition;

                lastPosition = nextPosition;
                nextPosition = startPosition + Random.insideUnitSphere * movementFlickerAmount;

	        }

	        flickerClock = 0f;
	    }



    }
}

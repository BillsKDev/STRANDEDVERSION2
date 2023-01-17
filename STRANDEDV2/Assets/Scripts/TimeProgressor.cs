using UnityEngine;

public class TimeProgressor : MonoBehaviour
{
    [Range(0, 24)]
    public float timeOfDay;
    public float orbitSpeed;

    public float axisOffset;
    public Light sun;
    public Light moon;
    public Gradient nightLight;


    public int hour;
    public int minute;
    public int hourPM;

    public bool nightTime = false;
    public bool dayTime = false;
    public AnimationCurve sunCurve;



    // public AnimationCurve moonCurve;

    private void OnValidate()
    {
        ProgressTime();
    }

    //Called every frame in play mode

    private void Start()
    {


    }
    void Update()
    {

        timeOfDay += Time.deltaTime * orbitSpeed;
        ProgressTime();
        if (timeOfDay >= 19)
        {
            nightTime = true;
            dayTime = false;

        }
        if (timeOfDay < 19 && timeOfDay > 6)
        {
            dayTime = true;
            nightTime = false;

        }
    }


    private void ProgressTime()
    {

        float currentTime = timeOfDay / 24;
        float sunRotation = Mathf.Lerp(-90, 270, currentTime);
        float moonRotation = Mathf.Lerp(90, -270, currentTime);

        sun.transform.rotation = Quaternion.Euler(sunRotation, axisOffset, 0);
        // moon.transform.rotation = Quaternion.Euler(moonRotation, axisOffset, 0); 


        hour = Mathf.FloorToInt(timeOfDay);
        minute = Mathf.FloorToInt((timeOfDay / (24f / 1440f) % 60));

        RenderSettings.ambientLight = nightLight.Evaluate(currentTime);
        sun.intensity = sunCurve.Evaluate(currentTime);


        if (hour > 12)
        {
            hourPM = hour - 12;
        }
        if (hour <= 12)
        {
            hourPM = hour;

        }
        if (hour == 0)
        {
            hourPM = 12;
        }

        timeOfDay %= 24;
    }
}
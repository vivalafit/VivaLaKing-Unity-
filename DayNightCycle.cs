using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

public class DayNightCycle : MonoBehaviour
{
    public float time;
    public TimeSpan currentTime;
    public Text timeText;
    public int days;
    public new Light light;
    public float intensity;
    public Color day;
    public Color night;

    public int speed;

    Image back;
    void Start()
    {
        
        back = GetComponent<Image>();
        day = Color.white;//back.color
        night = back.color;
    }

    void ChangeTime()
    {
        time += Time.deltaTime * speed;
        if (time>86400)
        {
            days += 1;
            time = 0;
        }
        currentTime = TimeSpan.FromSeconds(time);
        string[] tempoTime = currentTime.ToString().Split(":"[0]);
        timeText.text = tempoTime[0] + ":" + tempoTime[1];
        if (time <43200)
        {
               light.intensity = 0 - ((43200 - time) / 43200 * (-1));
           
             intensity = 1 - (43200 - time) / 43200;

        } 
        else
        {
            light.intensity = 0 - (43200 - time) / 43200;


            intensity = 1 - ((43200 - time) / 43200*(-1)); 
        }

     //   RenderSettings.fogColor = Color.Lerp(day, night, intensity * intensity);
        back.color = Color.Lerp(night, day, intensity*intensity);
      
    } 


    void Update()
    {
        ChangeTime();
      //  back.color = Color.Lerp(Color.black, Color.red, Mathf.PingPong(Time.time, 1));
    }
}

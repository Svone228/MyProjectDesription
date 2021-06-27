using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightScript : MonoBehaviour
{
    LightEnabler localLight;
    public GameObject localLightComponent;//Принимаем компонент со всеми светильниками, которые будем включать/выключать в зависимости от времени суток
    public GameObject[] childLights;//Масив светильников, которые будем обрабатывать
    bool localLightState = false;
    public Color morning;
    public Color noon;
    public Color evening;
    public Color night;
    public UnityEngine.Experimental.Rendering.Universal.Light2D lightComponent;
    public int currentTime;
    public int quarterDayTime;
    float r, g, b;
    float[] deltaMtoNoon, deltaNoontoE, deltaEtoNight, deltaNighttoM;
    float[] CalcDelta(Color from, Color to, int quarterDayTime)//Возвращает масив дельт{R,G,B}
    {
        float[] a = { (to.r - from.r) / quarterDayTime, (to.g - from.g) / quarterDayTime, (to.b - from.b) / quarterDayTime };
        return a;
    }
    Color AddDelta(Color color, float[] delta)
    {
        float r = color.r;
        float g = color.g;
        float b = color.b;
        r += delta[0];
        g += delta[1];
        b += delta[2];
        return new Color(r, g, b);
    }
    Color SetColor(Color from, Color to, int quarterDayTime, int currentTime)
    {
        r = from.r;
        g = from.g;
        b = from.b;
        r += (to.r - from.r) / quarterDayTime * (currentTime% quarterDayTime);
        g += (to.g - from.g) / quarterDayTime * (currentTime % quarterDayTime);
        b += (to.b - from.b) / quarterDayTime * (currentTime % quarterDayTime);
        //Debug.Log((to.r + " - " + from.r) + " / " + quarterDayTime + " * " + currentTime + " % " + quarterDayTime);
        return new Color(Mathf.Abs(r), Mathf.Abs(g), Mathf.Abs(b));        
    }
    void GetLocalLightList()
    {
        childLights = new GameObject[localLightComponent.transform.childCount];
        for (int i = 0; i < childLights.Length; i++)
        {
            childLights[i] = localLightComponent.transform.GetChild(i).gameObject;
        }
    }
    void SetLocalLightState(bool state)
    {
        if (state)
        {
            localLightState = true;
            
            for (int i = 0; i < childLights.Length; i++)
            {
                if(childLights[i]!=null)
                childLights[i].GetComponent<LightEnabler>().LightEnable();
            }
        }
        else
        {
            localLightState = false;
            for (int i = 0; i < childLights.Length; i++)
            {
                if (childLights[i].GetComponent<LightEnabler>() != null)
                    childLights[i].GetComponent<LightEnabler>().LightDisable();
            }
        }
    }
    /*Утро
    0.5896226
    0.6054063
    1
    полдень
    1
    1
    1
    закат
    0.6603774
    0.3888327
    0.3644536
    ночь
    0.1716358
    0.1766469
    0.2735849
    Delta
    0,00082075
    -0,00067925
    -0,00097748
    0,00083597*/
    

    // Start is called before the first frame update
    void Start()
    {
        GetLocalLightList();
        deltaMtoNoon = CalcDelta(morning, noon, quarterDayTime);
        deltaNoontoE = CalcDelta(noon, evening, quarterDayTime);
        deltaEtoNight = CalcDelta(evening, night, quarterDayTime);
        deltaNighttoM = CalcDelta(night, morning, quarterDayTime);
        if (currentTime < quarterDayTime)
        {
            lightComponent.color = SetColor(morning, noon, quarterDayTime, currentTime);
        }
        else if (currentTime < quarterDayTime * 2)
        {
            lightComponent.color = SetColor(noon, evening, quarterDayTime, currentTime);
        }
        else if (currentTime < quarterDayTime * 3)
        {
            lightComponent.color = SetColor(evening, night, quarterDayTime, currentTime);
        }
        else if (currentTime >= quarterDayTime * 4)
        {
            currentTime = 0;
            lightComponent.color = night;
        }
        else if (currentTime < quarterDayTime * 4)
        {
            lightComponent.color = SetColor(night, morning, quarterDayTime, currentTime);
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //Debug.Log(r);        
        currentTime += 1;
        if (currentTime <= quarterDayTime)//Время с утра до дня
        {
            lightComponent.color = AddDelta(lightComponent.color, deltaMtoNoon);            
            if (localLightState)
            {                
                SetLocalLightState(false);
            }
        }
        else if (currentTime <= quarterDayTime * 2)//Время с дня до вечера
        {
            lightComponent.color = AddDelta(lightComponent.color, deltaNoontoE);
            if (localLightState)
            {
                SetLocalLightState(false);
            }
        }
        else if (currentTime <= quarterDayTime * 3)//Время с вечера до ночи
        {
            lightComponent.color = AddDelta(lightComponent.color, deltaEtoNight);
            if (!localLightState)
            {
                SetLocalLightState(true);
            }
        }
        else if (currentTime >= quarterDayTime * 4)
        {
            currentTime = 0;
        }
        else//Время с ночи до утра
        {
            lightComponent.color = AddDelta(lightComponent.color, deltaNighttoM);
            if (!localLightState)
            {
                SetLocalLightState(true);
            }
        }
        /*if (currentTime == quarterDayTime)
        {
            Debug.Log(currentCollor.r);
        }
        if (currentTime == quarterDayTime*2)
        {
            Debug.Log(currentCollor.r);
        }
        if (currentTime == quarterDayTime*3)
        {
            Debug.Log(currentCollor.r);
        }
        if (currentTime == quarterDayTime*4)
        {
            Debug.Log(currentCollor.r);
        }*/
    }
}

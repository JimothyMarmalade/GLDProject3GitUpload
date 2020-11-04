using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider HealthSlider;
    public Image FillRect;
    private Coroutine Flasher;
    private bool FlasherNeeded = false;
    private WaitForSeconds Ticks = new WaitForSeconds(0.1f);

    public void SetMaxHealth(float health)
    {
        HealthSlider.maxValue = health;
    }

    public void SetHealth(float health)
    {
        HealthSlider.value = health;

        if (HealthSlider.value < HealthSlider.maxValue * 0.15f)
        {
            FlasherNeeded = true;
            if (Flasher != null)
                StopCoroutine(Flasher);

            Flasher = StartCoroutine(BarFlash());
        }
        else
        {
            Flasher = null;
            FlasherNeeded = false;
            FillRect.color = Color.red;
        }
    }

    IEnumerator BarFlash()
    {
        while (FlasherNeeded)
        {
            FillRect.color = Color.red;
            yield return Ticks;
            FillRect.color = Color.white;
            yield return Ticks;
        }
    }
}

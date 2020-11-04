using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StaminaBar : MonoBehaviour
{
    public Slider StaminaSlider;
    public Gradient StaminaGradient;
    public Image Fill;

    public void SetMaxStamina(float sta)
    {
        //Debug.Log("SetMaxStamina() with value: " + sta);
        StaminaSlider.maxValue = sta;
        StaminaSlider.value = sta;

        Fill.color = StaminaGradient.Evaluate(1f);
    }


    public void SetStamina(float sta)
    {
        //Debug.Log("SetStamina() with value: " + sta);
        StaminaSlider.value = sta;

        Fill.color = StaminaGradient.Evaluate(StaminaSlider.normalizedValue);
    }




}

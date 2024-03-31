using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResistencePlayer : MonoBehaviour
{

    public float resistenceInitial = 100f;
    public float velocityRunning = 1f;
    public float resistenceXHit = 10f;
    public Slider sliderResistence; 

    public float actualResistance;

    public float velocityRegeneration = 10f;

    private float tiemeWithoutActivity;

    public float tiemeOfInactivity = 5f;
    // Start is called before the first frame update
    void Start()
    {
        actualResistance = resistenceInitial;
        ActualizeResistance();
    }

    // Update is called once per frame
    void Update()
    {
        MovementRes();

        RestartResistance();
    }

    public void Hit()
    {
        actualResistance -= resistenceXHit;
        actualResistance = Mathf.Max(0, actualResistance);
        ActualizeResistance();
        ResetTime();
    }

    void ActualizeResistance()
    {
        sliderResistence.value = actualResistance / resistenceInitial;
    }

    public void MovementRes()
    {
        if (Input.GetKey(KeyCode.LeftShift) && actualResistance > 0)
        {
            actualResistance -= velocityRunning * Time.deltaTime;
            ActualizeResistance();
            ResetTime();
        }
    }

    void RestartResistance()
    {
        if (actualResistance < resistenceInitial)
        {
            tiemeWithoutActivity += Time.deltaTime;
            if (tiemeWithoutActivity >= tiemeOfInactivity)
            {
                actualResistance += velocityRegeneration * Time.deltaTime;
                actualResistance = Mathf.Min(actualResistance, resistenceInitial);
                ActualizeResistance();
            }
        }
    }

    void ResetTime()
    {
        tiemeWithoutActivity = 0f;
    }
}

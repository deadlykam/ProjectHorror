using UnityEngine;
using System.Collections;

public class Player_Info : MonoBehaviour {

    //Health Fields
    public float health_Max = 1; // Maximum health
    public float health_Current = 1; // Current health
    private float health_Buff; // For increasing/decreasing health
    private float health_Buff_Timer; // For how long the buff will last

    //Stamina Fields
    public float stamina_Max = 1; // Maximum stamina
    public float stamina_Current = 1; // Current stamina
    public float stamina_Replenish_Rate = 1; // Rate at which stamina is regained
    public float stamina_Drop_Rate = 1; // Rate at which stamina is lost
    public bool isRun = true; // player running boolean
    private float stamina_Buff; // For increasing/decreasing health
    private float stamina_Buff_Timer; // For how long the buff will last

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        Replenish_Stamina();
        BuffUpdates();
	}
    
    /// <summary>
    /// This method takes care of all the buff related tasks.
    /// </summary>
    void BuffUpdates()
    {
        //Health
        if (health_Buff_Timer > 0)
        {
            health_Buff_Timer -= Time.deltaTime;
        }
        else
        {
            health_Buff = 0;
            health_Buff_Timer = 0;
        }

        //Stamina
        if (stamina_Buff_Timer > 0)
        {
            stamina_Buff_Timer -= Time.deltaTime;
        }
        else
        {
            stamina_Buff = 0;
            stamina_Buff_Timer = 0;
        }
    }

    /// <summary>
    /// This method increases health
    /// </summary>
    void Replenish_Health()
    {
        if(health_Buff_Timer > 0)
        {
            if(health_Current < health_Max)
            {
                health_Current += health_Buff * Time.deltaTime;
            }
        }
    }
    
    void Replenish_Stamina()
    {
        if(stamina_Current < stamina_Max && !Input.GetButton("L_Shift"))
        {
            if (stamina_Current <= stamina_Max)
            {
                stamina_Current += (stamina_Replenish_Rate + stamina_Buff) * Time.deltaTime;
            }

            if(stamina_Current >= stamina_Max)
            {
                isRun = true;
            }
        }
    }

    public float GetHealthTimer()
    {
        return health_Buff_Timer;
    }

    public void SetHealthBuff(float timer, float value)
    {
        health_Buff_Timer = timer;
        health_Buff = value;
    }

    public float GetStaminaTimer()
    {
        return stamina_Buff_Timer;
    }

    public void SetStaminaBuff(float timer, float value)
    {
        stamina_Buff_Timer = timer;
        stamina_Buff = value;
    }

    public void lowerStamina()
    {
        if(stamina_Current > 0)
        {
            stamina_Current -= stamina_Drop_Rate * Time.deltaTime;
        }

        if(stamina_Current <= 0)
        {
            isRun = false;
        }
    }
}

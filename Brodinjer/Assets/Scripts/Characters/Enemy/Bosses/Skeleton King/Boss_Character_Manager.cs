using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_Character_Manager : MonoBehaviour
{
    public Boss_Health health;
    public float damageCoolDown;
    public List<Phase_Base> PhaseMovements;
    public List<int> healthMarks; // Health at which the phase will change
    public bool moveOnStart;
    private int currentHealthMark;
    private Phase_Base currentPhase;
    private bool damaged, dead;
    public Animator anim;
    private ResetTriggers reset;
    private bool pause;
    public SoundController damageSound;
    public BossHealthBar healthbar;
    
    private void Start()
    {
        pause = false;
        reset = anim.gameObject.GetComponent<ResetTriggers>();
        damaged = false;
        dead = false;
        Init();        
        if (moveOnStart)
        {
            StartFight();
        }
    }
    
    public void GiveDamage(float amount)
    {
        TakeDamage(amount, false, 0, "");
    }

    public void Init()
    {
        health.Init(this, null, true);
        if (GetComponent<Death_Event_Setup>() != null && health.Death_Version is Death_Event)
        {
            Death_Event death = health.Death_Version as Death_Event;
            if (death != null)
            {
                death._event = GetComponent<Death_Event_Setup>().DeathEvent;
            }
        }
        healthbar.AddHealth(health.health as LimitFloatData);
    }

    public void StartFight()
    {
        healthbar.StartHealthCheck();
        currentHealthMark = 0;
        currentPhase = PhaseMovements[currentHealthMark];
        currentPhase.StartPhase();
    }

    public void StartNextPhase()
    {
        currentPhase.StopPhase();
        currentPhase = PhaseMovements[currentHealthMark];
        currentPhase.StartPhase();
    }

    public void TakeDamage(float amount, bool armor, float ArmorAmount, string DamageTrigger = "Damage")
    {
        if (!dead && !damaged)
        {            damaged = true;
            health.TakeDamage(amount, armor, ArmorAmount);
            StartCoroutine(PauseDamage());
            if (reset)
                reset.ResetAllTriggers();
            anim.SetTrigger(DamageTrigger);
            damageSound.Play();
            if (health.health.value <= 0)
            {
                health.Death();
                dead = true;
                return;
            }
            currentPhase.StopDamage();
            if (health.health.value <= healthMarks[currentHealthMark])
            {
                currentHealthMark++;
                if(currentHealthMark < healthMarks.Count)
                    StartNextPhase();
            }
        }
    }

    private IEnumerator PauseDamage()
    {
        yield return new WaitForSeconds(damageCoolDown);
        damaged = false;
    }

    public void PauseAttack()
    {
        if(currentPhase!= null)
            currentPhase.StopPhase();
    }

    public void ResumeAttack()
    {
        if(currentPhase!= null)
            currentPhase.ResumeAttack();
    }



}

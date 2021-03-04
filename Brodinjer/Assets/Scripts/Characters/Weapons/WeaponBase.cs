using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WeaponBase : MonoBehaviour
{
    public KeyCode WeaponNum;
    public Sprite WeaponSprite;
    protected IEnumerator attack;
    [HideInInspector] public bool currWeapon;
    [HideInInspector] public bool inUse;
    public bool frozen;
    public Animator anim;
    public string AttackTrigger;
    public string AttackEndTrigger;
    private float animSpeed=1;
    public string useButton;
    public BoolData collected;
    protected Coroutine weaponFunc;
    public IntData NumItems;
    
    public abstract void Initialize();
    public abstract IEnumerator Attack();
    public abstract void End();

    public abstract void Off();
    public abstract void On();
    public virtual void Freeze()
    {
        frozen = true;
    }
    public virtual void Unfreeze()
    {
        frozen = false;
    }

    public abstract void Activate();

}

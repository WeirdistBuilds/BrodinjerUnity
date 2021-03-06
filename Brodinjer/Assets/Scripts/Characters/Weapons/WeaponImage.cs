using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WeaponImage : MonoBehaviour
{
    public GameObject Highlight;
    public Image Weapon;
    public TextMeshProUGUI Num;
    public IntData NumItems;
    private bool running;

    private void Start()
    {
        running = false;
        Num.text = "";
        /*if(NumItems!= null)
            StartUpdate();*/
    }

    private void FixedUpdate()
    {
        if (NumItems != null)
            Num.text = NumItems.value.ToString();

    }

    public void StartUpdate()
    {
        if (!running)
        {
            StartCoroutine(UpdateNum());
        }
    }
    
    private IEnumerator UpdateNum()
    {
        while (true)
        {
            Num.text = NumItems.value.ToString();
            yield return new WaitForSeconds(.1f);
        }
    }
}

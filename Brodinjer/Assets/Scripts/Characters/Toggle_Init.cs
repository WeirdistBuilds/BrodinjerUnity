using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Toggle_Init : MonoBehaviour
{
    private Toggle toggle;
    public BoolData toggleData;
    private void Start()
    {
        toggle = GetComponent<Toggle>();
        toggle.isOn = toggleData.value;
    }


}

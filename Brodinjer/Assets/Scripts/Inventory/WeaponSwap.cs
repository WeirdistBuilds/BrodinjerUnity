using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class WeaponSwap : MonoBehaviour
{
    public List<WeaponBase> AllWeapons;
    private List<WeaponBase> AvailableWeapons;
    private List<GameObject> highlights;
    private List<Image> weaponImages;
    private List<KeyCode> WeaponKeys;
    private List<IntData> weaponAmounts;
    public IntData currentWeapon;
    public WeaponManager wm;
    private float scrollWheel;
    public GameObject ImagePrefab;
    private GameObject tempobj;
    private WeaponImage tempImage;
    public string PutAwayWeapon;
    private int weapon = 0;
    private bool canChange;
    public bool WeaponOnStart;
    public float Sensitivity = .05f;
    public float InBetweenTime = .1f;
    private IEnumerator Start()
    {
        yield return new WaitForSeconds(.1f);
        AvailableWeapons = new List<WeaponBase>();
        foreach (var wb in AllWeapons)
        {
            if(wb.collected.value)
                AvailableWeapons.Add(wb);
        }
        WeaponKeys = new List<KeyCode>();
        foreach (var weapon in AvailableWeapons)
        {
            WeaponKeys.Add(weapon.WeaponNum);
        }

        if(currentWeapon.value <= -1)
        {
            currentWeapon.value = 0;
        }

        if (currentWeapon.value > -1)
        {
            try
            {
                wm.SwapWeapon(AvailableWeapons[currentWeapon.value], canChange);
                if (WeaponOnStart)
                    wm.currentWeapon.Initialize();
                else
                    wm.currentWeapon.Activate();

            }
            catch
            {
                
            }
        }
        
        InitDisplay();

        StartCoroutine(CheckSwap());

    }

    private IEnumerator CheckSwap()
    {
        while (true)
        {
            if (canChange && wm.currentWeapon != null && !wm.currentWeapon.inUse)
            {
                if (AvailableWeapons.Count > 0)
                {
                    scrollWheel = Input.GetAxis("Mouse ScrollWheel");
                    /*if (Input.GetButtonDown(PutAwayWeapon))
                    {
                        if (currentWeapon.value == -1)
                        {
                            currentWeapon.value = weapon;
                            wm.SwapWeapon(AvailableWeapons[currentWeapon.value], canChange);
                        }
                        else
                        {
                            weapon = currentWeapon.value;
                            currentWeapon.value = -1;
                            wm.PutAwayWeapon();
                        }

                        UpdateDisplay();
                    }*/
                    if (scrollWheel > Sensitivity)
                    {
                        currentWeapon.value--;
                        if (currentWeapon.value < 0)
                        {
                            currentWeapon.value = AvailableWeapons.Count - 1;
                        }

                        wm.SwapWeapon(AvailableWeapons[currentWeapon.value], canChange);
                        UpdateDisplay();
                        yield return new WaitForSeconds(InBetweenTime);
                    }
                    else if (scrollWheel < -Sensitivity)
                    {
                        currentWeapon.value++;
                        if (currentWeapon.value > AvailableWeapons.Count - 1)
                        {
                            currentWeapon.value = 0;
                        }

                        wm.SwapWeapon(AvailableWeapons[currentWeapon.value], canChange);
                        UpdateDisplay();
                        yield return new WaitForSeconds(InBetweenTime);
                    }
                    else
                    {
                        for (int i = 0; i < WeaponKeys.Count; i++)
                        {
                            if (Input.GetKeyDown(WeaponKeys[i]))
                            {
                                currentWeapon.value = i;
                                wm.SwapWeapon(AvailableWeapons[i], canChange);
                            }
                        }

                        UpdateDisplay();
                    }
                }
            }
            /*else if(canChange)
            {
                if (Input.GetButtonDown(PutAwayWeapon))
                {
                    if (currentWeapon.value == -1)
                    {
                        currentWeapon.value = weapon;
                        wm.SwapWeapon(AvailableWeapons[currentWeapon.value], canChange);
                    }
                    else
                    {
                        weapon = currentWeapon.value;
                        currentWeapon.value = -1;
                        wm.PutAwayWeapon();
                    }
                    yield return new WaitForSeconds(.1f);
                }
            }*/
            yield return new WaitForFixedUpdate();
        }
    }

    public void AddWeapon(WeaponBase w)
    {
        if (!AvailableWeapons.Contains(w))
        {
            AvailableWeapons.Add(w);
            WeaponKeys.Add(w.WeaponNum);
            tempobj = Instantiate(ImagePrefab, ImagePrefab.transform.parent);
            tempImage = tempobj.GetComponent<WeaponImage>();
            tempImage.Weapon.sprite = w.WeaponSprite;
            weaponImages.Add(tempImage.Weapon);
            highlights.Add(tempImage.Highlight);
            highlights[highlights.Count-1].gameObject.SetActive(false);
            if (AvailableWeapons[AvailableWeapons.Count-1].NumItems)
            {
                tempImage.NumItems = AvailableWeapons[AvailableWeapons.Count-1].NumItems;
            }
            tempobj.SetActive(true);
            UpdateDisplay();
        }
    }

    public void InitDisplay()
    {
        highlights = new List<GameObject>();
        weaponImages = new List<Image>();
        weaponAmounts = new List<IntData>();
        for (var i = 0; i < AvailableWeapons.Count; i++)
        {
            tempobj = Instantiate(ImagePrefab, ImagePrefab.transform.parent);
            tempImage = tempobj.GetComponent<WeaponImage>();
            tempImage.Weapon.sprite = AvailableWeapons[i].WeaponSprite;
            weaponImages.Add(tempImage.Weapon);
            highlights.Add(tempImage.Highlight);
            highlights[i].gameObject.SetActive(false);
            if (AvailableWeapons[i].NumItems)
            {
                tempImage.NumItems = AvailableWeapons[i].NumItems;
            }
        }
        ImagePrefab.SetActive(false);
        UpdateDisplay();

    }

    public void UpdateDisplay()
    {
        for (int i = 0; i < highlights.Count; i++)
        {
            if (i == currentWeapon.value)
            {
                highlights[i].gameObject.SetActive(true);
            }
            else
            {
                highlights[i].gameObject.SetActive(false);
            }
        }
    }

    public void DisableChange()
    {
        canChange = false;
    }

    public void EnableChange()
    {
        canChange = true;
    }

    public void SetWeapon(int num)
    {
        try
        {
            wm.SwapWeapon(AvailableWeapons[num], canChange);
            currentWeapon.value = num;
            wm.currentWeapon.Initialize();
            UpdateDisplay();
        }
        catch
        {
            
        }
    }
    
    
    
    
}

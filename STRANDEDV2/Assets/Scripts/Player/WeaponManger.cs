using System.Collections;
using UnityEngine;

public class WeaponManger : MonoBehaviour
{
    public enum weaponSelect
    {
        bat,
        pistol,
    }
    public weaponSelect chosenWeapon;
    public GameObject[] weapons;
    private int weaponID = 0;
    
    void Start()
    {
        weaponID = (int)chosenWeapon;
        ChangeWeapons();
    }
   
    void Update()
    {
        if
       (Input.GetKeyDown(KeyCode.X))
        {
            if (weaponID < weapons.Length - 1)
            {
                weaponID++;
                ChangeWeapons();
            }
        }
        if
       (Input.GetKeyDown(KeyCode.Z))
        {
            if (weaponID > 0)
            {
                weaponID--;
                ChangeWeapons();
            }
        }
    }
    private void ChangeWeapons()
    {
        foreach
       (GameObject weapon in weapons)
        {
            weapon.SetActive(false);
        }
        weapons[weaponID].SetActive(true);
        chosenWeapon = (weaponSelect)weaponID;
        StartCoroutine(WeaponReset());
    }
    IEnumerator WeaponReset()
    {
        yield return new WaitForSeconds(0.5f);
    }
}
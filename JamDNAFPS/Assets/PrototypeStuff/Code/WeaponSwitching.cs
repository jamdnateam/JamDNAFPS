using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSwitching : MonoBehaviour
{
    public int StartWeapon = 0;
    public int selectedWeapon;
    // public GameObject[] Weapons;
    public List<GameObject> Weapons = new List<GameObject>();
    public List<int> Inventory = new List<int>();
    public int InventoryLimit = 0;
    public int MaxLimit = 2;

 

    // Use this for initialization
    void Start()
    {
        SelectWeapon(0);
        AddWeapon(StartWeapon);
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.C))
        {
            if (Inventory.Count > 1)
            {

                SwitchWeapon();
            }
        }
    }

    public void SwitchWeapon()
    {

        selectedWeapon += 1;
        if (selectedWeapon > Inventory.Count - 1)
        {
            selectedWeapon = 0;
        }
        SelectWeapon(Inventory[selectedWeapon]);

    }

    public void AddWeapon(int index)
    {
        // this was alterd
        if (Inventory.Contains(index))
        {
            gameObject.GetComponentInChildren<Gun>().FillAmmo();
            selectedWeapon = Inventory[index];
            SelectWeapon(index);
            return;
        }
        if (InventoryLimit <= MaxLimit)
        {
            Inventory.Add(index);
            SelectWeapon(index);
            selectedWeapon = InventoryLimit;
            InventoryLimit += 1;
        }
        else
        {
            for (int i = 0; i < Inventory.Count; i++)
            {
                if (i == Inventory[selectedWeapon])
                {
                    Inventory.Remove(i);
                    break;
                }
            }
            Inventory.Add(index);
            SelectWeapon(index);
            selectedWeapon = 0;
        }
    }

    void SelectWeapon(int index)
    {
        foreach (GameObject weapon in Weapons)
        {
            weapon.gameObject.SetActive(true);
            if (weapon != Weapons[index])
            {
                weapon.gameObject.SetActive(false);
            }
        }


    }
}
using System.Collections;
using System.Collections.Generic;
using MoreMountains.TopDownEngine;
using UnityEngine;

public class CharacterClass : MonoBehaviour
{
    [SerializeField] CharacterClassData data;

    private Character character;
    private CharacterHandleWeapon weaponHandle;
    private Weapon weapon;

    private void Awake()
    {
        character = GetComponent<Character>();
    }

    // Start is called before the first frame update
    void Start()
    {
        character._health.MaximumHealth += data.healthModifier;
        weaponHandle = character.FindAbility<CharacterHandleWeapon>();
        if (data.initialWeapon != null)
        {
            weaponHandle.ChangeWeapon(data.initialWeapon, data.initialWeapon.WeaponName);
        }
        // weapon = weaponHandle.CurrentWeapon;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void AddModifier(DamageOnTouch damagingScript)
    {
        Debug.Log("Adding modifier from " + this.gameObject.name + " to " + damagingScript.gameObject.name);
        Debug.Log("Changing from " + damagingScript.DamageCaused + " to " + (damagingScript.DamageCaused + data.attackDamageModifier));
        damagingScript.DamageCaused += data.attackDamageModifier;
    }
}

using System.Collections;
using System.Collections.Generic;
using MoreMountains.TopDownEngine;
using UnityEngine;

public class CharacterClass : MonoBehaviour
{
    [SerializeField] CharacterClassData data;

    private Character character;

    private void Awake()
    {
        character = GetComponent<Character>();
    }

    // Start is called before the first frame update
    void Start()
    {
        character._health.MaximumHealth += data.healthModifier;
        if (data.initialWeapon != null)
        {
            SetWeapon();
        }
        // weapon = weaponHandle.CurrentWeapon;
    }

    void SetWeapon()
    {
        var weaponHandle = character.FindAbility<CharacterHandleWeapon>();
        weaponHandle.ChangeWeapon(data.initialWeapon, data.initialWeapon.WeaponName);
    }

    void ModifyMovementSpeed()
    {
        var movementAbility = character.FindAbility<CharacterMovement>();
        movementAbility.WalkSpeed *= data.movementSpeedMultiplier;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void AddModifier(DamageOnTouch damagingScript, bool isRanged)
    {
        Debug.Log("Adding modifier from " + this.gameObject.name + " to " + damagingScript.gameObject.name);

        if (isRanged)
        {
            Debug.Log("Changing from " + damagingScript.DamageCaused + " to " + (damagingScript.DamageCaused + data.rangedAttackDamageModifier));
            damagingScript.DamageCaused += data.rangedAttackDamageModifier;
        }
        else
        {
            Debug.Log("Changing from " + damagingScript.DamageCaused + " to " + (damagingScript.DamageCaused + data.meleeAttackDamageModifier));
            damagingScript.DamageCaused += data.meleeAttackDamageModifier;
        }
    }
}

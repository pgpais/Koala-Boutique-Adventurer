using System.Collections;
using System.Collections.Generic;
using MoreMountains.TopDownEngine;
using UnityEngine;
using UnityEngine.Events;

public class CharacterClass : MonoBehaviour
{
    [HideInInspector]
    public UnityEvent<Health> DamagedEnemy;

    [SerializeField] CharacterClassData data;
    [SerializeField] List<Buff> currentBuffs;

    public Character Character => character;
    private Character character;

    private void Awake()
    {
        character = GetComponent<Character>();
        if (GameManager.instance != null)
        {
            data = GameManager.instance.currentSelectedClass;
        }
        DamagedEnemy = new UnityEvent<Health>();
        DamagedEnemy.AddListener((health) => Debug.Log("Event fired"));
    }

    // Start is called before the first frame update
    void Start()
    {
        ModifyMaximumHealth();

        if (data.initialWeapon != null)
        {
            SetWeapon();
        }
        // weapon = weaponHandle.CurrentWeapon;

        foreach (var buff in currentBuffs)
        {
            buff.Initialize(this);
        }
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

    void ModifyMaximumHealth()
    {
        character._health.MaximumHealth += data.healthModifier;
        character._health.GetHealth(data.healthModifier, gameObject);
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

    public void AddBuff(Buff buff)
    {
        if (!currentBuffs.Contains(buff))
        {
            currentBuffs.Add(buff);
            buff.Initialize(this);
        }
    }
}

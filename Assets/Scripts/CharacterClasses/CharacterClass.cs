using System.Collections;
using System.Collections.Generic;
using MoreMountains.TopDownEngine;
using UnityEngine;
using UnityEngine.Events;

public class CharacterClass : MonoBehaviour
{
    [HideInInspector]
    public UnityEvent<Health> DamagedEnemy;
    public Character Character => character;

    [SerializeField] CharacterClassData data;
    [SerializeField] List<Buff> currentBuffs;

    private int weaponDamageModifier;
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

        // weapon = weaponHandle.CurrentWeapon;
        List<string> boughtBuffs = MissionManager.instance.GetStartingBuffs();

        foreach (var buffName in boughtBuffs)
        {
            currentBuffs.Add(MissionManager.instance.BuffList.GetBuffByName(buffName));
        }

        foreach (var buff in currentBuffs)
        {
            buff.Initialize(this);
        }

        ModifyMaximumHealthWithClass();
        ModifyMovementSpeedWithClass();
        ModifyWeaponDamage();

        if (data.initialWeapon != null)
        {
            SetWeapon();
        }
    }

    void SetWeapon()
    {
        var weaponHandle = character.FindAbility<CharacterHandleWeapon>();
        weaponHandle.ChangeWeapon(data.initialWeapon, data.initialWeapon.WeaponName);
    }

    void ModifyMovementSpeedWithClass()
    {
        var movementAbility = character.FindAbility<CharacterMovement>();
        movementAbility.WalkSpeed *= data.movementSpeedMultiplier;
    }
    public void ModifyMovementSpeed(int amount)
    {
        var movementAbility = character.FindAbility<CharacterMovement>();
        movementAbility.WalkSpeed += amount;
    }

    void ModifyMaximumHealthWithClass()
    {
        character._health.MaximumHealth += data.healthModifier;
        character._health.GetHealth(data.healthModifier, gameObject);
    }

    public void ModifyMaximumHealth(int amount)
    {
        character._health.MaximumHealth += amount;
        character._health.GetHealth(amount, gameObject);
    }

    void ModifyWeaponDamage()
    {
        weaponDamageModifier += data.attackDamageModifier;
    }

    public void ModifyWeaponDamage(int amount)
    {
        weaponDamageModifier += amount;
    }



    // Update is called once per frame
    void Update()
    {

    }

    public void ApplyDamageModifier(DamageOnTouch damagingScript, bool isRanged)
    {
        Debug.Log("Adding modifier from " + this.gameObject.name + " to " + damagingScript.gameObject.name);

        Debug.Log("Changing from " + damagingScript.DamageCaused + " to " + (damagingScript.DamageCaused + data.attackDamageModifier));
        damagingScript.DamageCaused += weaponDamageModifier;
    }

    public void AddBuff(Buff buff)
    {
        if (!currentBuffs.Contains(buff))
        {
            currentBuffs.Add(buff);
            buff.Initialize(this);
        }
    }

    public void OnDeath()
    {
        if (GameManager.instance != null)
        {
            GameManager.instance.FinishLevel(true);
        }
    }
}

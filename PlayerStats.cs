using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [SerializeField] protected string _name; // 메인 메뉴에서 결정
    [SerializeField] protected int _job; // Healer, Tanker, Dealer
    [SerializeField] protected float _currentHealth;
    [SerializeField] protected float _maxHealth;
    [SerializeField] protected float _healthRegen;
    [SerializeField] protected float _mana;
    [SerializeField] protected float _manaRegen;
    [SerializeField] protected float _damage;
    [SerializeField] protected float _criticalDamage;
    [SerializeField] protected float _criticalChance; // 크리 확률
    [SerializeField] protected float _defense;
    [SerializeField] protected int _money;

    public string Name { get { return _name; } set { _name = value; } }
    public int Job { get { return _job; } set { _job = value; } }
    public float CurrentHealth { get { return _currentHealth;} set { _currentHealth = value; } }
    public float MaxHealth { get { return _maxHealth;} set { _maxHealth = value; } }
    public float HealthRegen { get { return _healthRegen;} set { _healthRegen = value; } }
    public float Mana { get { return _mana; } set { _mana = value; } }
    public float ManaRegen { get { return _manaRegen;} set { _manaRegen = value; } }
    public float Damage { get { return _damage; } set { _damage = value; } }
    public float CriticalDamage { get { return _criticalDamage; } set { _criticalDamage = value; } }
    public float CriticalChance { get { return _criticalChance; } set { _criticalChance = value; } }
    public float Defense { get { return _defense; } set { _defense = value; } }
    public int Money { get { return _money; } set { _money = value; } }

    private void Start()
    {
        MaxHealth = 10f;
        CurrentHealth = MaxHealth;
    }

}

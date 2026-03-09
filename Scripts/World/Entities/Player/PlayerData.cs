using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PlayerSkills
{
    public int melee = 1;
    public int distance = 1;
    public int defense = 1;
    public int magic = 1;
}

[Serializable]
public class PlayerCoreStats
{
    public int armor = 0;

    public float maxHP = 100f;
    public float currentHP = 100f;

    public float accuracy = 100f;
    public float dodge = 0f;

    public float critHit = 0f;
    public float critChance = 0f;

    public float lifeSteal = 0f;
    public float attackSpeed = 1f;
    public float manaSteal = 0f;

    public float maxMana = 50f;
    public float currentMana = 50f;

    public float maxRage = 100f;
    public float currentRage = 0f;

    public List<string> passives = new List<string>();
}

[Serializable]
public class PlayerResistances
{
    public float magicResist = 0f;
    public float fireResist = 0f;
    public float deathResist = 0f;
    public float earthResist = 0f;
    public float iceResist = 0f;
    public float holyResist = 0f;
}

[Serializable]
public class PlayerIndicators
{
    public float stamina = 100f;
    public int kills = 0;
    public int powerScore = 0;

    public List<string> passives = new List<string>();
}

[Serializable]
public class PlayerAbilities
{
    public List<string> learnedAbilities = new List<string>();
}

[Serializable]
public class PlayerMovementRules
{
    [Header("Collision / Blocking")]
    public bool blocksMonsters = true;
    public bool blocksPlayers = true;

    [Header("PZ Rules")]
    public bool canPlayersPassInPZOnly = true;

    [Tooltip("Se true, este player está sob restrição PZ e não pode entrar em zona PZ.")]
    public bool cannotEnterPZWhileInPZLock = false;
}

[Serializable]
public class PlayerIdentity
{
    public string playerName = "Player";
    public PlayerClass playerClass = PlayerClass.None;
    public int level = 1;
    public int experience = 0;
}

[Serializable]
public class PlayerData
{
    public PlayerIdentity identity = new PlayerIdentity();
    public PlayerSkills skills = new PlayerSkills();
    public PlayerCoreStats stats = new PlayerCoreStats();
    public PlayerResistances resistances = new PlayerResistances();
    public PlayerIndicators indicators = new PlayerIndicators();
    public PlayerAbilities abilities = new PlayerAbilities();
    public PlayerMovementRules movementRules = new PlayerMovementRules();
}
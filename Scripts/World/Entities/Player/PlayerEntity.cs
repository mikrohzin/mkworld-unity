using UnityEngine;

public class PlayerEntity : MonoBehaviour
{
    [Header("Runtime Data")]
    public PlayerData data = new PlayerData();

    [Header("Grid Position (Read Only Runtime)")]
    public Vector2Int gridPosition;

    [Header("Zone State")]
    public bool isInsidePZ = false;
    public bool isInsideProtectionZone = false;

    private PlayerGridMovement gridMovement;

    private void Awake()
    {
        gridMovement = GetComponent<PlayerGridMovement>();

        ClampCurrentValues();
        RecalculatePowerScore();
    }

    private void Update()
    {
        if (gridMovement != null)
        {
            gridPosition = gridMovement.GridPosition;
        }
    }

    public void ClampCurrentValues()
    {
        data.stats.currentHP = Mathf.Clamp(data.stats.currentHP, 0, data.stats.maxHP);
        data.stats.currentMana = Mathf.Clamp(data.stats.currentMana, 0, data.stats.maxMana);
        data.stats.currentRage = Mathf.Clamp(data.stats.currentRage, 0, data.stats.maxRage);
        data.indicators.stamina = Mathf.Max(0, data.indicators.stamina);
    }

    public void RecalculatePowerScore()
    {
        int score = 0;

        score += data.identity.level * 10;

        score += data.skills.melee * 2;
        score += data.skills.distance * 2;
        score += data.skills.defense * 2;
        score += data.skills.magic * 2;

        score += Mathf.RoundToInt(data.stats.maxHP * 0.2f);
        score += Mathf.RoundToInt(data.stats.maxMana * 0.2f);
        score += data.stats.armor * 3;

        score += Mathf.RoundToInt(data.stats.accuracy * 0.1f);
        score += Mathf.RoundToInt(data.stats.dodge * 0.1f);
        score += Mathf.RoundToInt(data.stats.critChance * 0.5f);
        score += Mathf.RoundToInt(data.stats.critHit * 0.3f);

        score += Mathf.RoundToInt(data.resistances.magicResist);
        score += Mathf.RoundToInt(data.resistances.fireResist);
        score += Mathf.RoundToInt(data.resistances.deathResist);
        score += Mathf.RoundToInt(data.resistances.earthResist);
        score += Mathf.RoundToInt(data.resistances.iceResist);
        score += Mathf.RoundToInt(data.resistances.holyResist);

        data.indicators.powerScore = score;
    }

    public bool CanEnterPZ()
    {
        if (data.movementRules.cannotEnterPZWhileInPZLock)
            return false;

        return true;
    }

    public bool BlocksMonsterMovement()
    {
        return data.movementRules.blocksMonsters;
    }

    public bool BlocksPlayerMovement(bool targetTileIsPZ)
    {
        if (!data.movementRules.blocksPlayers)
            return false;

        if (data.movementRules.canPlayersPassInPZOnly && targetTileIsPZ)
            return false;

        return true;
    }

    public void ReceiveDamage(float amount)
    {
        data.stats.currentHP -= amount;
        ClampCurrentValues();

        if (data.stats.currentHP <= 0)
        {
            Die();
        }
    }

    public void Heal(float amount)
    {
        data.stats.currentHP += amount;
        ClampCurrentValues();
    }

    public void SpendMana(float amount)
    {
        data.stats.currentMana -= amount;
        ClampCurrentValues();
    }

    public void RestoreMana(float amount)
    {
        data.stats.currentMana += amount;
        ClampCurrentValues();
    }

    public void AddKill()
    {
        data.indicators.kills++;
    }

    public void Die()
    {
        Debug.Log($"{data.identity.playerName} morreu.");
    }
}
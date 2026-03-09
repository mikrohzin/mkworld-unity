using UnityEngine;

public class PlayerInitializer : MonoBehaviour
{
    [SerializeField] private PlayerEntity playerEntity;

    private void Reset()
    {
        playerEntity = GetComponent<PlayerEntity>();
    }

    private void Start()
    {
        if (playerEntity == null)
            return;

        SetupDefaultPlayer();
    }

    private void SetupDefaultPlayer()
    {
        var data = playerEntity.data;

        data.identity.playerName = "Player";
        data.identity.playerClass = PlayerClass.None;
        data.identity.level = 1;
        data.identity.experience = 0;

        data.skills.melee = 10;
        data.skills.distance = 10;
        data.skills.defense = 10;
        data.skills.magic = 10;

        data.stats.armor = 5;
        data.stats.maxHP = 150f;
        data.stats.currentHP = 150f;
        data.stats.accuracy = 95f;
        data.stats.dodge = 5f;
        data.stats.critHit = 10f;
        data.stats.critChance = 5f;
        data.stats.lifeSteal = 0f;
        data.stats.attackSpeed = 1f;
        data.stats.manaSteal = 0f;
        data.stats.maxMana = 50f;
        data.stats.currentMana = 50f;
        data.stats.maxRage = 100f;
        data.stats.currentRage = 0f;

        data.resistances.magicResist = 0f;
        data.resistances.fireResist = 0f;
        data.resistances.deathResist = 0f;
        data.resistances.earthResist = 0f;
        data.resistances.iceResist = 0f;
        data.resistances.holyResist = 0f;

        data.indicators.stamina = 100f;
        data.indicators.kills = 0;

        data.movementRules.blocksMonsters = true;
        data.movementRules.blocksPlayers = true;
        data.movementRules.canPlayersPassInPZOnly = true;
        data.movementRules.cannotEnterPZWhileInPZLock = false;

        playerEntity.ClampCurrentValues();
        playerEntity.RecalculatePowerScore();
    }
}
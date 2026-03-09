using UnityEngine;

[RequireComponent(typeof(PlayerEntity))]
[RequireComponent(typeof(PlayerGridMovement))]
public class PlayerCombat : MonoBehaviour
{
    [Header("Input")]
    public KeyCode attackKey = KeyCode.Space;

    [Header("Combat")]
    public float attackDamage = 10f;
    public int attackRangeSqm = 1;
    public float attackCooldown = 0.6f;

    private float nextAttackTime = 0f;

    private PlayerEntity playerEntity;
    private PlayerGridMovement playerMovement;

    private void Awake()
    {
        playerEntity = GetComponent<PlayerEntity>();
        playerMovement = GetComponent<PlayerGridMovement>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(attackKey))
        {
            TryAttackNearestMonster();
        }
    }

    public void TryAttackNearestMonster()
    {
        if (Time.time < nextAttackTime)
            return;

        MonsterEntity target = FindNearestMonsterInRange();

        if (target == null)
        {
            Debug.Log("Nenhum monstro em alcance.");
            return;
        }

        nextAttackTime = Time.time + attackCooldown;

        target.ReceiveDamage(attackDamage);

        string playerName = playerEntity != null ? playerEntity.data.identity.playerName : "Player";
        Debug.Log($"{playerName} atacou {target.monsterName} por {attackDamage}.");
    }

    private MonsterEntity FindNearestMonsterInRange()
    {
        MonsterEntity[] monsters = FindObjectsByType<MonsterEntity>(FindObjectsSortMode.None);

        MonsterEntity bestTarget = null;
        int bestDistance = int.MaxValue;

        Vector2Int playerGrid = playerMovement.GridPosition;
        int playerFloor = playerMovement.currentFloor;

        foreach (MonsterEntity monster in monsters)
        {
            if (monster == null)
                continue;

            if (monster.currentHP <= 0f)
                continue;

            if (monster.currentFloor != playerFloor)
                continue;

            int distance = CombatUtility.GetChebyshevDistance(playerGrid, monster.gridPosition);

            if (distance > attackRangeSqm)
                continue;

            if (distance < bestDistance)
            {
                bestDistance = distance;
                bestTarget = monster;
            }
        }

        return bestTarget;
    }
}
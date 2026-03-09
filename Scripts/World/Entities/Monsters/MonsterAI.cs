using System.Collections;
using UnityEngine;

[RequireComponent(typeof(MonsterEntity))]
public class MonsterAI : MonoBehaviour
{
    private MonsterEntity monster;
    private PlayerEntity playerEntity;
    private PlayerGridMovement playerMovement;

    private bool isMoving = false;
    private float nextMoveTime = 0f;
    private float nextAttackTime = 0f;

    private void Awake()
    {
        monster = GetComponent<MonsterEntity>();
    }

    private void Start()
    {
        playerEntity = FindFirstObjectByType<PlayerEntity>();
        playerMovement = FindFirstObjectByType<PlayerGridMovement>();
    }

    private void Update()
    {
        if (monster == null || playerEntity == null || playerMovement == null)
            return;

        if (monster.currentHP <= 0f)
            return;

        Vector2Int monsterGrid = monster.gridPosition;
        Vector2Int playerGrid = playerMovement.GridPosition;

        int distanceToPlayer = GridMapUtility.ChebyshevDistance(monsterGrid, playerGrid);

        if (distanceToPlayer > monster.chaseRangeSqm)
            return;

        if (distanceToPlayer <= monster.attackRangeSqm)
        {
            TryAttack();
            return;
        }

        TryChase(monsterGrid, playerGrid);
    }

    private void TryAttack()
    {
        if (Time.time < nextAttackTime)
            return;

        nextAttackTime = Time.time + monster.attackCooldown;

        playerEntity.ReceiveDamage(monster.attackDamage);

        Debug.Log($"{monster.monsterName} atacou {playerEntity.data.identity.playerName} por {monster.attackDamage}.");
    }

    private void TryChase(Vector2Int monsterGrid, Vector2Int playerGrid)
    {
        if (isMoving)
            return;

        if (Time.time < nextMoveTime)
            return;

        Vector2Int step = GetBestStepTowards(monsterGrid, playerGrid);

        if (step == Vector2Int.zero)
            return;

        Vector2Int targetGrid = monsterGrid + step;

        if (!GridMapUtility.CanWalkTo(monster.currentFloor, targetGrid.x, targetGrid.y))
            return;

        if (EntityOccupancyManager.Instance != null)
        {
            GameObject occupant = EntityOccupancyManager.Instance.GetOccupant(monster.currentFloor, targetGrid.x, targetGrid.y);

            if (occupant != null && occupant != playerEntity.gameObject)
                return;
        }

        nextMoveTime = Time.time + monster.moveInterval;
        StartCoroutine(MoveRoutine(targetGrid));
    }

    private Vector2Int GetBestStepTowards(Vector2Int from, Vector2Int to)
    {
        int dx = to.x - from.x;
        int dy = to.y - from.y;

        Vector2Int primaryStep = new Vector2Int(
            dx == 0 ? 0 : (dx > 0 ? 1 : -1),
            dy == 0 ? 0 : (dy > 0 ? 1 : -1)
        );

        if (primaryStep != Vector2Int.zero)
        {
            Vector2Int primaryTarget = from + primaryStep;
            if (CanMonsterEnter(primaryTarget))
                return primaryStep;
        }

        Vector2Int horizontalStep = new Vector2Int(
            dx == 0 ? 0 : (dx > 0 ? 1 : -1),
            0
        );

        if (horizontalStep != Vector2Int.zero)
        {
            Vector2Int horizontalTarget = from + horizontalStep;
            if (CanMonsterEnter(horizontalTarget))
                return horizontalStep;
        }

        Vector2Int verticalStep = new Vector2Int(
            0,
            dy == 0 ? 0 : (dy > 0 ? 1 : -1)
        );

        if (verticalStep != Vector2Int.zero)
        {
            Vector2Int verticalTarget = from + verticalStep;
            if (CanMonsterEnter(verticalTarget))
                return verticalStep;
        }

        return Vector2Int.zero;
    }

    private bool CanMonsterEnter(Vector2Int targetGrid)
    {
        if (!GridMapUtility.CanWalkTo(monster.currentFloor, targetGrid.x, targetGrid.y))
            return false;

        if (EntityOccupancyManager.Instance == null)
            return true;

        GameObject occupant = EntityOccupancyManager.Instance.GetOccupant(monster.currentFloor, targetGrid.x, targetGrid.y);

        if (occupant == null)
            return true;

        if (playerEntity != null && occupant == playerEntity.gameObject)
            return false;

        if (occupant == gameObject)
            return true;

        return false;
    }

    private IEnumerator MoveRoutine(Vector2Int targetGrid)
    {
        isMoving = true;

        Vector2Int oldGrid = monster.gridPosition;

        if (EntityOccupancyManager.Instance != null)
        {
            bool movedInOccupancy = EntityOccupancyManager.Instance.Move(monster.currentFloor, oldGrid, targetGrid, gameObject);

            if (!movedInOccupancy)
            {
                isMoving = false;
                yield break;
            }
        }

        Vector3 startPosition = transform.position;
        Vector3 targetPosition = GridMapUtility.GridToWorld(targetGrid.x, targetGrid.y, monster.currentFloor);

        float elapsed = 0f;
        float duration = monster.moveInterval * 0.85f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            transform.position = Vector3.Lerp(startPosition, targetPosition, t);
            yield return null;
        }

        transform.position = targetPosition;
        monster.gridPosition = targetGrid;
        isMoving = false;
    }
}
using UnityEngine;

public class MonsterEntity : MonoBehaviour
{
    [Header("Identity")]
    public string monsterName = "Rat";

    [Header("Stats")]
    public float maxHP = 30f;
    public float currentHP = 30f;
    public float attackDamage = 10f;

    [Header("Behavior")]
    public int currentFloor = 0;
    public int chaseRangeSqm = 6;
    public int attackRangeSqm = 1;

    [Header("Timing")]
    public float moveInterval = 0.35f;
    public float attackCooldown = 1.0f;

    [Header("Runtime")]
    public Vector2Int gridPosition;

    private void Awake()
    {
        currentHP = Mathf.Clamp(currentHP, 0f, maxHP);
        gridPosition = GridMapUtility.WorldToGrid(transform.position);
    }

    private void Start()
    {
        RegisterInitialOccupancy();
    }

    private void Update()
    {
        gridPosition = GridMapUtility.WorldToGrid(transform.position);
    }

    private void RegisterInitialOccupancy()
    {
        if (EntityOccupancyManager.Instance == null)
        {
            Debug.LogError("EntityOccupancyManager.Instance is null.");
            return;
        }

        bool registered = EntityOccupancyManager.Instance.TryRegister(currentFloor, gridPosition.x, gridPosition.y, gameObject);

        if (!registered)
        {
            Debug.LogWarning($"{monsterName} could not register occupancy at {gridPosition} on floor {currentFloor}.");
        }
    }

    public void ReceiveDamage(float amount)
    {
        currentHP -= amount;
        currentHP = Mathf.Clamp(currentHP, 0f, maxHP);

        if (currentHP <= 0f)
        {
            Die();
        }
    }

    public void Die()
    {
        if (EntityOccupancyManager.Instance != null)
        {
            EntityOccupancyManager.Instance.Unregister(currentFloor, gridPosition.x, gridPosition.y, gameObject);
        }

        Debug.Log($"{monsterName} morreu.");
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        if (EntityOccupancyManager.Instance != null)
        {
            EntityOccupancyManager.Instance.Unregister(currentFloor, gridPosition.x, gridPosition.y, gameObject);
        }
    }
}
using DefaultNamespace;
using TMPro;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;
using UnityEngine.InputSystem;

public class DisplayFoxes : MonoBehaviour
{
    public TextMeshProUGUI tmp;
    private EntityQuery foxQuery;

    private EntityQuery foxSpawnerQuery;

    private EntityManager _entityManager;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
        foxQuery = new EntityQueryBuilder(Allocator.Persistent).WithAll<Fox>().Build(_entityManager);
        foxSpawnerQuery = new EntityQueryBuilder(Allocator.Persistent).WithAllRW<FoxSpawner>().Build(_entityManager);
    }

    // Update is called once per frame
    void Update()
    {
        tmp.text = $"Foxes: {foxQuery.CalculateEntityCount()}";
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            var spawner = foxSpawnerQuery.GetSingletonRW<FoxSpawner>();
            spawner.ValueRW.amountToSpawn += 500;
            _entityManager.DestroyEntity(foxQuery.ToEntityArray(Allocator.Temp));
            
        }
        
        if (Mouse.current.rightButton.wasPressedThisFrame)
        {
            var spawner = foxSpawnerQuery.GetSingletonRW<FoxSpawner>();
            spawner.ValueRW.amountToSpawn -= 500;
            _entityManager.DestroyEntity(foxQuery.ToEntityArray(Allocator.Temp));
        }
        
    }
}

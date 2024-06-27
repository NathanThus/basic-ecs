using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Rendering;
using UnityEngine;
using Random = Unity.Mathematics.Random;

public partial struct TankSpawnSystem : ISystem
{
    private const float InverseGoldenRatio = 0.618034005f;
    private const int Seed = 123;

    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<Config>();
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        // Disabling a system will stop further updates.
        // By disabling this system in its first update, 
        // it will effectively update only once.
        state.Enabled = false;

        // Can be useful for keeping certain properties in one place.
        var config = SystemAPI.GetSingleton<Config>();

        // Random numbers from a hard-coded seed.
        var random = new Random(Seed);

        for (int i = 0; i < config.TankCount; i++)
        {
            var tankEntity = state.EntityManager.Instantiate(config.TankPrefab);
            if (i == 0)
            {
                state.EntityManager.AddComponent<Player>(tankEntity);
            }
            // Special Unity.Entities property for colour.
            var color = new URPMaterialPropertyBaseColor { Value = RandomColor(ref random) };

            // Every root entity instantiated from a prefab has a LinkedEntityGroup component, which
            // is a list of all the entities that make up the prefab hierarchy (including the root).
            var linkedEntities = state.EntityManager.GetBuffer<LinkedEntityGroup>(tankEntity);
            foreach (var entity in linkedEntities)
            {
                // We want to set the URPMaterialPropertyBaseColor component only on the
                // entities that have it, so we first check.
                if (state.EntityManager.HasComponent<URPMaterialPropertyBaseColor>(entity.Value))
                {
                    // Set the color of each entity that makes up the tank.
                    state.EntityManager.SetComponentData(entity.Value, color);
                }
            }
        }
    }

    // Return a random color that is visually distinct.
    static float4 RandomColor(ref Random random)
    {
        var hue = (random.NextFloat() + InverseGoldenRatio) % 1;
        return (Vector4)Color.HSVToRGB(hue, 1.0f, 1.0f);
    }
}
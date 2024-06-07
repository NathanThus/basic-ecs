using Unity.Entities;
using Unity.Transforms;
using Unity.Burst;

public partial struct CubeRotationSystem : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var deltaTime = SystemAPI.Time.DeltaTime;

        // RefRW == ReadWrite perms, RefRO == ReadOnly perms
        foreach (var (transform, rotationSpeed) in SystemAPI.Query<RefRW<LocalTransform>, RefRO<RotationSpeed>>())
        {
            var radians = rotationSpeed.ValueRO.RadiansPerSecond * deltaTime; // Calculate the rotation.
            transform.ValueRW = transform.ValueRW.RotateY(radians); // Rotate around the Y-Axis by Radians.
        }
    }
}
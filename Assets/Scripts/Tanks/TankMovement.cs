using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

public partial struct TankMovementSystem : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var deltaTime = SystemAPI.Time.DeltaTime;

        foreach (var (transform, entity) in
                 SystemAPI.Query<RefRW<LocalTransform>>()
                     .WithAll<Tank>()
                     .WithNone<Player>() // Ignore the player
                     .WithEntityAccess())
        {
            var pos = transform.ValueRO.Position;

            // Seed the noise
            pos.y = (float)entity.Index;

            var angle = (0.5f + noise.cnoise(pos / 10f)) * 4.0f * math.PI;
            var dir = float3.zero;
            math.sincos(angle, out dir.x, out dir.z);

            // Update the LocalTransform.
            transform.ValueRW.Position += dir * deltaTime * 5.0f;
            transform.ValueRW.Rotation = quaternion.RotateY(angle);
        }
    }
}
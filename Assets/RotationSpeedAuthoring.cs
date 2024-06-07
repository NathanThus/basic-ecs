using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;


public struct RotationSpeed : IComponentData
{
    public float RadiansPerSecond;
}

public class RotationSpeedAuthoring : MonoBehaviour
{
    [SerializeField] private float _degreesPerSecond = 360f;
    public float DegreesPerSecond => _degreesPerSecond;
}

class RotationSpeedBaker : Baker<RotationSpeedAuthoring>
{
    public override void Bake(RotationSpeedAuthoring authoring)
    {
        var entity = GetEntity(authoring, TransformUsageFlags.Dynamic);

        var rotationSpeed = new RotationSpeed
        {
            RadiansPerSecond = math.radians(authoring.DegreesPerSecond)
        };

        AddComponent(entity, rotationSpeed);
    }
}

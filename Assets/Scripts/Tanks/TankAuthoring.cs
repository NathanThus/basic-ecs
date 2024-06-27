using UnityEngine;
using Unity.Entities;

public class TankAuthoring : MonoBehaviour
{
    [SerializeField] private GameObject _turret;
    [SerializeField] private GameObject _cannon;
    
    class Baker : Baker<TankAuthoring>
    {
        public override void Bake(TankAuthoring authoring)
        {
            // GetEntity returns the Entity baked from the GameObject
            var entity = GetEntity(authoring, TransformUsageFlags.Dynamic);
            AddComponent(entity, new Tank
            {
                Turret = GetEntity(authoring._turret, TransformUsageFlags.Dynamic),
                Cannon = GetEntity(authoring._cannon, TransformUsageFlags.Dynamic)
            });
        }
    }
}

// A component that will be added to the root entity of every tank.
public struct Tank : IComponentData
{
    public Entity Turret;
    public Entity Cannon;
} 

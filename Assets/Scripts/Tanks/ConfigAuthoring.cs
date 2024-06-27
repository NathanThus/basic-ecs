using Unity.Entities;
using UnityEngine;

public class ConfigAuthoring : MonoBehaviour
{
    [SerializeField] private GameObject _tankPrefab;
    [SerializeField] private GameObject _cannonBallPrefab;
    [SerializeField] private int _tankCount;

    class Baker : Baker<ConfigAuthoring>
    {
        public override void Bake(ConfigAuthoring authoring)
        {
            // The config entity itself doesn’t need transform components,
            // so we use TransformUsageFlags.None
            var entity = GetEntity(authoring, TransformUsageFlags.None);
            AddComponent(entity, new Config
            {
                // Bake the prefab into entities. GetEntity will return the 
                // root entity of the prefab hierarchy.
                TankPrefab = GetEntity(authoring._tankPrefab, TransformUsageFlags.Dynamic),
                CannonBallPrefab = GetEntity(authoring._cannonBallPrefab, TransformUsageFlags.Dynamic),
                TankCount = authoring._tankCount,
            });
        }
    }
}
public struct Config : IComponentData
{
    public Entity TankPrefab;
    public Entity CannonBallPrefab;
    public int TankCount;
}
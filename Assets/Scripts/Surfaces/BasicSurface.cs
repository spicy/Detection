using UnityEngine;

namespace Detection
{
    public class BasicSurface : MonoBehaviour, IScannable
    {
        [SerializeField] private Color defaultColor;

        void IScannable.EmitParticle(RaycastHit hit, VFXEmitArgs overrideArgs)
        {
            Color color = defaultColor;
            float lifetime = 5f;
            float size = 0.015f;

            if (overrideArgs.color != null) color = (Color)overrideArgs.color;
            if (overrideArgs.lifetime != null) lifetime = (float)overrideArgs.lifetime;
            if (overrideArgs.size != null) size = (float)overrideArgs.size;

            ParticleSpawner.spawner.Spawn(color, hit.point, lifetime, size);
        }
    }
}
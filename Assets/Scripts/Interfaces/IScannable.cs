using UnityEngine;
namespace Detection
{
    public struct VFXEmitArgs
    {
        public Color? color;
        public float? size;
        public float? lifetime;

        public VFXEmitArgs(Color? newColor, float? newSize, float? newLifetime)
        {
            color = newColor;
            size = newSize;
            lifetime = newLifetime;
        }
    }
    public interface IScannable
    {
        public void EmitParticle(RaycastHit hit, VFXEmitArgs overrideArgs);
    }
}
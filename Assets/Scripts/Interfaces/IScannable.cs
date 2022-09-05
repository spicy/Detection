using UnityEngine;
namespace Detection
{
    public interface IScannable
    {
        public void EmitParticle(Vector3 position, ParticleSystem overrideParticleSystem);
    }
}
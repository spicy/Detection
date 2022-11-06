using UnityEngine;
namespace Detection
{
    public interface IScannable
    {
        public void EmitParticle(RaycastHit hit, ParticleSystem overrideParticleSystem);
    }
}
using UnityEngine;

namespace Detection
{
    public class EnemySurface : MonoBehaviour, IScannable, IRevealable
    {
        [SerializeField] private ParticleSystem _particleSystem;
        void IScannable.EmitParticle(Vector3 position, ParticleSystem overrideParticleSystem)
        {
            if (_particleSystem == null) return;

            var emitArgs = new ParticleSystem.EmitParams();
            emitArgs.position = position;
            emitArgs.velocity = new Vector3(0.0f, 0.0f, 0.0f);

            if (overrideParticleSystem != null) overrideParticleSystem.Emit(emitArgs, 1);
            else _particleSystem.Emit(emitArgs, 1);
        }

        // need to implement, want to show outline of enemy after x amount of scans
        // possibly show through walls?
        void IRevealable.Reveal()
        {

        }
    }
}
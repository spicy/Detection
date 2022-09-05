using UnityEngine;

namespace Detection
{
    public class WallSurface : MonoBehaviour, IScannable
    {
        [SerializeField] ParticleSystem _particleSystem;
        void IScannable.EmitParticle(Vector3 position, ParticleSystem overrideParticleSystem)
        {
            if (_particleSystem == null) return;

            var emitArgs = new ParticleSystem.EmitParams();
            emitArgs.position = position;
            emitArgs.velocity = new Vector3(0.0f, 0.0f, 0.0f);

            if (overrideParticleSystem != null) overrideParticleSystem.Emit(emitArgs, 1);
            else _particleSystem.Emit(emitArgs, 1);
        }
    }
}
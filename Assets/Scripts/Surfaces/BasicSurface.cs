using UnityEngine;

namespace Detection
{
    public class BasicSurface : MonoBehaviour, IScannable
    { 

        [SerializeField] ParticleSystem _particleSystem;
        [SerializeField] MusicAnalyzer musicAnalyzer;

        void IScannable.EmitParticle(Vector3 position, ParticleSystem overrideParticleSystem)
        {
            var emitArgs = new ParticleSystem.EmitParams();
            emitArgs.position = position;
            emitArgs.velocity = new Vector3(0.0f, 0.0f, 0.0f);
            emitArgs.startSize = musicAnalyzer.currentLoudness;

            if (overrideParticleSystem == null)
            {
                if (_particleSystem == null) return;
                _particleSystem.Emit(emitArgs, 1);
            }
            else overrideParticleSystem.Emit(emitArgs, 1);
        }
    }
}
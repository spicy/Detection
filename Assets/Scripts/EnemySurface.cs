using UnityEngine;

namespace Detection
{
    public class EnemySurface : MonoBehaviour, IScannable, IRevealable
    {
        [SerializeField] private ParticleSystem particleSystem;

        void IScannable.EmitParticle(Vector3 position)
        {
            //Debug.Log("enemy count: " + particleSystem.particleCount);
            var emitArgs = new ParticleSystem.EmitParams();
            emitArgs.position = position;
            emitArgs.velocity = new Vector3(0.0f, 0.0f, 0.0f);
            particleSystem.Emit(emitArgs, 1);
        }

        // need to implement, want to show outline of enemy after x amount of scans
        void IRevealable.Reveal()
        {

        }
    }
}
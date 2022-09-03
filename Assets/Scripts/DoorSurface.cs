using UnityEngine;

// user input starts main scanner
// main scanner explains how we should emit particles from a ray we shoot from the player camera
// 


namespace Detection
{
    public class DoorSurface : MonoBehaviour, IScannable
    {
        [SerializeField] ParticleSystem particleSystem;

        void IScannable.EmitParticle(Vector3 position)
        {
            //Debug.Log("door count: " + particleSystem.particleCount);
            var emitArgs = new ParticleSystem.EmitParams();
            emitArgs.position = position;
            emitArgs.velocity = new Vector3(0.0f, 0.0f, 0.0f);
            particleSystem.Emit(emitArgs, 1);
        }
    }
}
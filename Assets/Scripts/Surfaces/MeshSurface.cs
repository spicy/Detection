using UnityEngine;

namespace Detection
{
    [RequireComponent(typeof(MeshRenderer), typeof(MeshCollider))]
    public class MeshSurface : MonoBehaviour, IScannable
    { 
        [SerializeField] ParticleSystem _particleSystem;

        void IScannable.EmitParticle(RaycastHit hit, ParticleSystem overrideParticleSystem)
        {
            try
            {
                var emitArgs = new ParticleSystem.EmitParams();
                emitArgs.position = hit.point;
                emitArgs.velocity = new Vector3(0.0f, 0.0f, 0.0f);

                // Assumes there is a MeshRenderer AND a MeshCollider
                Renderer renderer = hit.collider.GetComponent<MeshRenderer>();
                Texture2D texture2D = renderer.material.mainTexture as Texture2D;
                Vector2 pCoord = hit.textureCoord;
                pCoord.x *= texture2D.width;
                pCoord.y *= texture2D.height;

                Vector2 tiling = renderer.material.mainTextureScale;
                int x = Mathf.FloorToInt(pCoord.x * tiling.x);
                int y = Mathf.FloorToInt(pCoord.y * tiling.y);
                Color color = texture2D.GetPixel(x, y);
                emitArgs.startColor = color;

                if (overrideParticleSystem == null)
                {
                    if (_particleSystem == null) return;
                    _particleSystem.Emit(emitArgs, 1);
                }
                else overrideParticleSystem.Emit(emitArgs, 1);
            }
            catch
            {
                int test = 0;
            }
        }
    }
}
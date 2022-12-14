using UnityEngine;

namespace Detection
{
    [RequireComponent(typeof(MeshRenderer), typeof(MeshCollider))]
    public class MeshSurface : MonoBehaviour, IScannable
    {
        void IScannable.EmitParticle(RaycastHit hit, VFXEmitArgs overrideArgs)
        {
            Color color = new Color(255, 255, 255);
            float lifetime = 5f;
            float size = 0.015f;

            Renderer renderer;
            Texture2D texture2D;
            Vector2 pCoord;

            if (overrideArgs.color != null) color = (Color)overrideArgs.color;
            else
            {
                // Get the color at the specific point on the mesh texture
                renderer = hit.collider.GetComponent<MeshRenderer>();
                texture2D = renderer.material.mainTexture as Texture2D;

                // sometimes not able to get texture2d from texture? in this case we use default color
                if (texture2D)
                {
                    pCoord = hit.textureCoord;
                    pCoord.x *= texture2D.width;
                    pCoord.y *= texture2D.height;

                    Vector2 tiling = renderer.material.mainTextureScale;
                    int x = Mathf.FloorToInt(pCoord.x * tiling.x);
                    int y = Mathf.FloorToInt(pCoord.y * tiling.y);
                    color = texture2D.GetPixel(x, y);
                }
            }
            if (overrideArgs.lifetime != null) lifetime = (float)overrideArgs.lifetime;
            if (overrideArgs.size != null) size = (float)overrideArgs.size;

            ParticleSpawner.spawner.Spawn(color, hit.point, lifetime, size);
        }
    }
}
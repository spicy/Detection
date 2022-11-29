using System.Collections;
using UnityEngine;
using UnityEngine.VFX;

public class ParticleSpawner : MonoBehaviour
{
    public static ParticleSpawner spawner;
    private VisualEffect m_TargetVisualEffect;
    private VFXEventAttribute m_EventAttribute;

    private static readonly int s_FireID = Shader.PropertyToID("fire");
    private static readonly int s_ColorID = Shader.PropertyToID("color");
    private static readonly int s_LifetimeID = Shader.PropertyToID("lifetime");
    private static readonly int s_PositionID = Shader.PropertyToID("position");
    private static readonly int s_SizeID = Shader.PropertyToID("size");

    private void Awake()
    {
        if (spawner != null && spawner != this) Destroy(this);
        else spawner = this;
    }

    public void Spawn(Color color, Vector3 position, float lifetime, float size)
    {
        if (m_TargetVisualEffect == null)
            m_TargetVisualEffect = GetComponent<VisualEffect>();

        if (m_EventAttribute == null)
            m_EventAttribute = m_TargetVisualEffect.CreateVFXEventAttribute();

        m_EventAttribute.SetVector3(s_ColorID, new Vector3(color.r, color.g, color.b));
        m_EventAttribute.SetVector3(s_PositionID, position);
        m_EventAttribute.SetFloat(s_LifetimeID, lifetime);
        m_EventAttribute.SetFloat(s_SizeID, size);
        m_TargetVisualEffect.SendEvent(s_FireID, m_EventAttribute);
    }
}

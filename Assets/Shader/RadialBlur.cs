using UnityEngine;

[ExecuteInEditMode]
public class RadialBlur : MonoBehaviour
{
    [SerializeField]
    private Shader m_shader = default;
    [SerializeField, Range(1, 16)]
    private int m_sampleCount = 8; //ブラーのかけ具合(増やすほど処理負荷増大)
    [SerializeField, Range(0.0f, 1.0f)]
    private float m_strength = 0.5f; //ブラーの強さ

    private Material m_material = default;

    //すべてのレンダリングがRenderImageへと完了したときに呼び出される
    private void OnRenderImage(RenderTexture source, RenderTexture dest)
    {
        if (m_material == null)
        {
            if (m_shader == null)
            {
                Graphics.Blit(source, dest);
                return;
            }
            else
            {
                m_material = new Material(m_shader);
            }
        }
        m_material.SetInt("_SampleCount", m_sampleCount);
        m_material.SetFloat("_Strength", m_strength);
        Graphics.Blit(source, dest, m_material);
    }
}
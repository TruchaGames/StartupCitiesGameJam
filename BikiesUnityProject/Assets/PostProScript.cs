using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class PostProScript : MonoBehaviour
{
    [Header("PolutionBar Script Here")]
    public GameObject PolutionBarObject = null;

    [Header("MaxPollution Value Here")]
    public float MaxPolution = 2.0f;

    ParticleSystem m_Particles;
    PostProcessVolume m_Volume = null;
    ColorGrading m_ColorCorrection = null;
    PolutionBar m_PolutionBar = null;

    // Start is called before the first frame update
    void Start()
    {
        m_Particles = GetComponentInChildren<ParticleSystem>();

        m_Volume = GetComponentInChildren<PostProcessVolume>();
        m_Volume.profile.TryGetSettings(out m_ColorCorrection);

        m_PolutionBar = PolutionBarObject.GetComponent<PolutionBar>();
    }

    // Update is called once per frame
    void Update()
    {
        if(m_PolutionBar == null)
            m_PolutionBar = PolutionBarObject.GetComponent<PolutionBar>();

        if (m_PolutionBar.slider.value > 0.2f)
        {
            float pollution_normalized = m_PolutionBar.slider.value / MaxPolution;

            m_ColorCorrection.temperature.value = new FloatParameter { value = Mathf.Lerp(-5.9f, 64.0f, pollution_normalized) };
            m_ColorCorrection.tint.value = new FloatParameter { value = Mathf.Lerp(-5.1f, -22.0f, pollution_normalized) };
            m_ColorCorrection.lift.value.w = new FloatParameter { value = Mathf.Lerp(0.0f, 0.6f, pollution_normalized) };

            m_Particles.startSpeed = Mathf.Lerp(0.8f, -0.1f, pollution_normalized);
            m_Particles.startLifetime = Mathf.Lerp(36.9f, 104.31f, pollution_normalized);
            m_Particles.startSize = Mathf.Lerp(0.2f, 0.4f, pollution_normalized);
            m_Particles.maxParticles = (int)Mathf.Lerp(1000.0f, 4000.0f, pollution_normalized);
        }
        else
        {
            m_ColorCorrection.temperature.value = new FloatParameter { value = -5.9f };
            m_ColorCorrection.tint.value = new FloatParameter { value = -5.1f };
            m_ColorCorrection.lift.value.w = new FloatParameter { value = 0.0f };

            m_Particles.startSpeed = 0.8f;
            m_Particles.startLifetime = 36.9f;
            m_Particles.startSize = 0.2f;
            m_Particles.maxParticles = 1000;
        }
    }
}

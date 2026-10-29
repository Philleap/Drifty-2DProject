
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class CarAudio : MonoBehaviour
{
    [Header("Engine Clips")]
    public AudioClip idleClip;
    public AudioClip lowClip;
    public AudioClip mediumClip;
    public AudioClip highClip;

    [Header("Settings")]
    public float maxSpeed = 8f;
    public float masterVolume = 0.5f;
    public float transitionSpeed = 5f;

    private Rigidbody rb;
    private AudioSource[] sources;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        AudioClip[] clips =
        {
            idleClip, lowClip, mediumClip, highClip
        };

        sources = new AudioSource[4];

        for (int i = 0; i < clips.Length; i++)
        {
            AudioSource source = gameObject.AddComponent<AudioSource>();

            source.clip = clips[i];
            source.loop = true;
            source.playOnAwake = false;
            source.spatialBlend = 0f;
            source.volume = 0f;

            sources[i] = source;

            if (source.clip != null)
                source.Play();
        }
    }

    void Update()
    {
        float speed = rb.linearVelocity.magnitude;

        // Normalize speed from 0 to 1
        float normalizedSpeed = Mathf.Clamp01(speed / maxSpeed);

        // Four audio regions: idle, low, medium, high
        float audioPosition = normalizedSpeed * 3f;

        for (int i = 0; i < sources.Length; i++)
        {
            if (sources[i].clip == null)
                continue;

            // Crossfade neighboring clips
            float targetVolume =
                Mathf.Clamp01(1f - Mathf.Abs(audioPosition - i));

            sources[i].volume = Mathf.MoveTowards(
                sources[i].volume,
                targetVolume * masterVolume,
                transitionSpeed * Time.deltaTime
            );
        }
    }
}

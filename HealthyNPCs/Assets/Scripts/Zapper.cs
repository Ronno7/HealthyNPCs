using UnityEngine;
using UnityEngine.InputSystem;

public class Zapper : MonoBehaviour
{
    [SerializeField] private int damagePerZap = 25;
    [SerializeField] private float missDistance = 30f;
    [SerializeField] private float beamDuration = 0.08f;
    [SerializeField] private float beamJitter = 0.25f;
    [SerializeField] private LineRenderer beam = null;
    [SerializeField] private AudioSource zapAudio = null;

    private Camera zapCamera;
    private AudioClip zapClip;
    private float beamHideTime;

    public int ZapCount { get; private set; }
    public bool CanZap { get; set; } = true;

    private void Start()
    {
        zapCamera = GetComponent<Camera>();
        zapClip = CreateZapClip();
        beam.enabled = false;
    }

    private void Update()
    {
        if (beam.enabled && Time.time >= beamHideTime)
            beam.enabled = false;

        if (CanZap && Pointer.current != null && Pointer.current.press.wasPressedThisFrame)
            Zap(Pointer.current.position.ReadValue());
    }

    private void Zap(Vector2 screenPosition)
    {
        ZapCount++;

        Ray ray = zapCamera.ScreenPointToRay(screenPosition);
        Vector3 target = ray.GetPoint(missDistance);

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            target = hit.point;

            NPC npc = hit.collider.GetComponent<NPC>();
            if (npc != null)
                npc.TakeDamage(damagePerZap);
        }

        ShowBeam(target);
        zapAudio.PlayOneShot(zapClip);
    }

    private void ShowBeam(Vector3 target)
    {
        Vector3 start = zapCamera.ViewportToWorldPoint(new Vector3(0.5f, 0f, 3f));

        for (int i = 0; i < beam.positionCount; i++)
        {
            float t = (float)i / (beam.positionCount - 1);
            bool isEndPoint = i == 0 || i == beam.positionCount - 1;
            Vector3 jitter = isEndPoint ? Vector3.zero : Random.insideUnitSphere * beamJitter;
            beam.SetPosition(i, Vector3.Lerp(start, target, t) + jitter);
        }

        beam.enabled = true;
        beamHideTime = Time.time + beamDuration;
    }

    // A falling square wave mixed with noise, so the project doesn't need an audio asset.
    private static AudioClip CreateZapClip()
    {
        const int sampleRate = 44100;
        var samples = new float[sampleRate / 8];
        float phase = 0f;

        for (int i = 0; i < samples.Length; i++)
        {
            float t = (float)i / samples.Length;
            phase += Mathf.Lerp(1400f, 200f, t) / sampleRate;
            float square = phase % 1f < 0.5f ? 1f : -1f;
            samples[i] = (square * 0.6f + Random.Range(-1f, 1f) * 0.4f) * (1f - t) * 0.4f;
        }

        AudioClip clip = AudioClip.Create("Zap", samples.Length, 1, sampleRate, false);
        clip.SetData(samples, 0);
        return clip;
    }
}

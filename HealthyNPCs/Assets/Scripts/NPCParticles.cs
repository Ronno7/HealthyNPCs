using UnityEngine;

public class NPCParticles : MonoBehaviour
{
    [SerializeField] private ParticleSystem deathParticlePrefab = null;

    private void Start()
    {
        GetComponent<IHealth>().OnDied += HandleNPCDied;
    }

    private void HandleNPCDied()
    {
        var deathParticle = Instantiate(deathParticlePrefab, transform.position, deathParticlePrefab.transform.rotation);
        Destroy(deathParticle.gameObject, 4f);
    }
}

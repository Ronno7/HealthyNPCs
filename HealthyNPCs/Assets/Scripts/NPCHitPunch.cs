using System.Collections;
using UnityEngine;

public class NPCHitPunch : MonoBehaviour
{
    [SerializeField] private float punchScale = 1.35f;
    [SerializeField] private float punchDuration = 0.15f;

    private Vector3 restingScale;
    private Coroutine punch;

    private void Start()
    {
        restingScale = transform.localScale;
        GetComponent<NPC>().OnHit += HandleNPCHit;
    }

    private void HandleNPCHit()
    {
        if (punch != null)
            StopCoroutine(punch);

        punch = StartCoroutine(Punch());
    }

    private IEnumerator Punch()
    {
        for (float elapsed = 0f; elapsed < punchDuration; elapsed += Time.deltaTime)
        {
            transform.localScale = restingScale * Mathf.Lerp(punchScale, 1f, elapsed / punchDuration);
            yield return null;
        }

        transform.localScale = restingScale;
    }
}

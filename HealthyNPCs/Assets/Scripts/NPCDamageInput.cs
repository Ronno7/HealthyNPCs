using UnityEngine;
using UnityEngine.InputSystem;

public class NPCDamageInput : MonoBehaviour
{
    [SerializeField] private int damagePerPress = 10;

    private NPC npc;

    private void Start()
    {
        npc = GetComponent<NPC>();
    }

    private void Update()
    {
        if (Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            npc.TakeDamage(damagePerPress);
        }
    }
}

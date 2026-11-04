using UnityEngine;
using UnityEngine.UI;

public class HPBar : MonoBehaviour
{
    [SerializeField] private Color fullHpColor = new Color(0.25f, 0.9f, 0.35f);
    [SerializeField] private Color lowHpColor = new Color(0.95f, 0.2f, 0.15f);

    private Slider slider;
    private Image fill;

    private void Start()
    {
        slider = GetComponentInChildren<Slider>();
        fill = slider.fillRect.GetComponent<Image>();
        fill.color = fullHpColor;
        GetComponentInParent<IHealth>().OnHPPctChanged += HandleHPPctChanged;
    }

    private void Update()
    {
        // Copies the camera's angle rather than aiming at its position, so bars stay level across the screen.
        Transform cam = Camera.main.transform;
        slider.transform.rotation = Quaternion.LookRotation(-cam.forward, cam.up);
    }

    private void HandleHPPctChanged(float pct)
    {
        slider.value = pct;
        fill.color = Color.Lerp(lowHpColor, fullHpColor, pct);
    }
}

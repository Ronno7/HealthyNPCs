using UnityEngine;
using UnityEngine.UI;

public class HPBar : MonoBehaviour
{
    private Slider slider;

    private void Start()
    {
        slider = GetComponentInChildren<Slider>();
        GetComponentInParent<IHealth>().OnHPPctChanged += HandleHPPctChanged;
    }

    private void Update()
    {
        slider.transform.LookAt(Camera.main.transform);
    }

    private void HandleHPPctChanged(float pct)
    {
        slider.value = pct;
    }
}

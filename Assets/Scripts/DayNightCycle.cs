using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    [Header("Ciclo Día-Noche")]
    public float dayDuration = 60f;
    public float timeOfDay = 6f;

    public int dayCount = 1;
    public Light sunLight;
    public Light nightLamp;

    [Header("stats")]
    public System.Action<int> OnDayPassed;
    public System.Action<float> OnTemperatureChanged;


    [Header("Temperatura")]
    public float dayTemperature = 28f;
    public float nightTemperature = 10f;

    public PlantGrowthManager plant;
    [SerializeField] TMPro.TextMeshProUGUI clockText;
    [SerializeField] TMPro.TextMeshProUGUI dayText;

    private void Start()
    {
        timeOfDay = 6f;
    }

    private void Update()
    {
        if (plant == null) return;

        float effectiveSpeed = plant.growthSpeed * (1f + plant.fertilizer / 100f * 0.25f);
        float dayProgressSpeed = (24f / dayDuration) * effectiveSpeed;
        timeOfDay += dayProgressSpeed * Time.deltaTime;

        if (timeOfDay >= 24f)
        {
            timeOfDay -= 24f;
            dayCount++;

            dayText.text = "Día " + dayCount;

            OnDayPassed?.Invoke(dayCount);
        }


        UpdateSunLight();
        UpdateTemperature();
        UpdateNightLamp();
    }

    private void LateUpdate()
    {
        if (clockText == null) return;

        int hours = Mathf.FloorToInt(timeOfDay);
        int minutes = Mathf.FloorToInt((timeOfDay - hours) * 60f);

        clockText.text = $"{hours:00}:{minutes:00}";
    }

    private void UpdateSunLight()
    {
        if (sunLight == null) return;

        float sunRotation = (timeOfDay / 24f) * 360f;
        sunLight.transform.rotation = Quaternion.Euler(sunRotation - 90f, 170f, 0f);

        float normalizedTime = timeOfDay / 24f;
        float intensity = Mathf.Clamp01(Mathf.Cos((normalizedTime - 0.25f) * Mathf.PI * 2f));

        sunLight.intensity = intensity * 1.2f;
    }

    private void UpdateTemperature()
    {
        if (plant == null) return;

        float normalizedTime = timeOfDay / 24f;
        float temperatureCurve = Mathf.Clamp01(Mathf.Cos((normalizedTime - 0.25f) * Mathf.PI * 2f));

        plant.temperature = Mathf.Lerp(nightTemperature, dayTemperature, temperatureCurve);

        OnTemperatureChanged?.Invoke(plant.temperature);
    }

    private void UpdateNightLamp()
    {
        if (nightLamp == null) return;

        nightLamp.enabled = (sunLight.intensity < 0.1f);
    }
}

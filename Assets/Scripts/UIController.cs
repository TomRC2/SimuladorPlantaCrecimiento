using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIPlantController : MonoBehaviour
{
    public PlantGrowthManager plant;

    [Header("Sliders")]
    public Slider waterSlider;
    public Slider temperatureSlider;
    public Slider fertilizerSlider;
    public Slider timeSlider;

    [Header("Texts")]
    public Text waterText;
    public Text temperatureText;
    public Text fertilizerText;
    public Text timeText;

    private void Start()
    {
        waterSlider.maxValue = 100f;
        fertilizerSlider.maxValue = 100f;

        waterSlider.value = plant.water;
        temperatureSlider.value = plant.temperature;
        fertilizerSlider.value = plant.fertilizer;
        timeSlider.value = plant.growthSpeed;

        UpdateTexts();

        waterSlider.onValueChanged.AddListener(v => plant.water = v);
        temperatureSlider.onValueChanged.AddListener(v => plant.temperature = v);
        fertilizerSlider.onValueChanged.AddListener(v => plant.fertilizer = v);
        timeSlider.onValueChanged.AddListener(v => plant.growthSpeed = v);
    }
    private void Update()
    {
        waterSlider.value = plant.water;
        temperatureSlider.value = plant.temperature;
        fertilizerSlider.value = plant.fertilizer;
        timeSlider.value = plant.growthSpeed;

        UpdateTexts();
    }

    public void AddWater(float amount)
    {
        plant.water += amount;
        plant.water = Mathf.Clamp(plant.water, 0, 100);
    }

    private void UpdateTexts()
    {
        waterText.text = $"{plant.water:0}";
        temperatureText.text = $"{plant.temperature:0}°C";
        fertilizerText.text = $"{plant.fertilizer:0}";
        timeText.text = $"{plant.growthSpeed:0.0}x";
    }
}

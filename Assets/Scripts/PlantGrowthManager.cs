using TMPro;
using UnityEngine;

public enum PlantState
{
    Seed,
    Sprout,
    Young,
    Adult,
    Dead
}

public class PlantGrowthManager : MonoBehaviour
{
    [Header("Estados de la Planta")]
    public PlantState currentState = PlantState.Seed;
    public TMP_Text plantStateText;

    [Header("Parámetros dinámicos")]
    [Range(0f, 100f)] public float water = 50f;
    [Range(-20f, 50f)] public float temperature = 20f;
    [Range(0f, 100f)] public float fertilizer = 0f;

    [Header("Velocidad del Tiempo")]
    public float growthSpeed = 1f;
    private float growthTimer = 0f;

    [Header("Referencias visuales")]
    public GameObject seedModel;
    public GameObject sproutModel;
    public GameObject youngModel;
    public GameObject adultModel;

    [Header("Colores de estado")]
    public Color healthyColor = Color.green;
    public Color heatColor = Color.red;
    public Color coldColor = Color.cyan;
    public Color dryColor = new Color(0.5f, 0.3f, 0f);
    public Color deadColor = Color.black;

    [Header("Consumo y Evaporación")]
    public float waterDecayRate = 0.1f;

    private MeshRenderer currentRenderer;

    public System.Action<float> OnGrowthSpeedChanged;

    private void Start()
    {
        UpdatePlantModel();
    }

    private void Update()
    {
        float effectiveSpeed = growthSpeed * (1f + fertilizer / 100f * 0.25f);

        UpdateGrowth(effectiveSpeed);
        EvaporateWater(effectiveSpeed);
        UpdateColorBasedOnConditions();
        UpdatePlantStatusUI();
    }

    private void UpdatePlantStatusUI()
    {
        if (plantStateText == null) return;

        string status = currentState switch
        {
            PlantState.Dead => "Estado: Muerta",
            _ when IsExtremeConditions() => "Estado: Muriendo",
            _ when IsHealthyConditions() => "Estado: Saludable",
            _ => "Estado: Estresada"
        };

        plantStateText.text = status;
    }

    private void EvaporateWater(float effectiveSpeed)
    {
        float fertilizerMultiplier = 1f + (fertilizer / 100f * 0.5f);
        water -= waterDecayRate * effectiveSpeed * fertilizerMultiplier * Time.deltaTime;
        water = Mathf.Clamp(water, 0f, 100f);
    }

    private void UpdateGrowth(float effectiveSpeed)
    {
        if (currentState == PlantState.Dead) return;

        growthTimer += Time.deltaTime * effectiveSpeed;

        switch (currentState)
        {
            case PlantState.Seed:
                if (growthTimer >= 60f) ChangeState(PlantState.Sprout);
                break;
            case PlantState.Sprout:
                if (growthTimer >= 180f) ChangeState(PlantState.Young);
                break;
            case PlantState.Young:
                if (growthTimer >= 360f) ChangeState(PlantState.Adult);
                break;
        }

        if (IsExtremeConditions())
            ChangeState(PlantState.Dead);
    }

    private void UpdateColorBasedOnConditions()
    {
        if (currentRenderer == null) return;

        Color targetColor = Color.white;

        if (water < 30f) targetColor = new Color(1f, 0.9f, 0.2f);
        if (temperature > 35f) targetColor = new Color(1f, 0.4f, 0.1f);
        if (temperature < 5f) targetColor = new Color(0.2f, 0.8f, 1f);
        if (water <= 0f) targetColor = Color.black;

        currentRenderer.material.SetColor("_BaseColor", targetColor);
    }

    private bool IsHealthyConditions() => water >= 40f && water <= 80f && temperature >= 10f && temperature <= 30f;
    private bool IsExtremeConditions()
    {
        float tolerance = fertilizer / 100f * 5f;
        return water <= 0f || water >= 100f || temperature >= 50f + tolerance || temperature <= -20f - tolerance;
    }

    private void ChangeState(PlantState newState)
    {
        currentState = newState;
        UpdatePlantModel();
    }

    private void UpdatePlantModel()
    {
        seedModel.SetActive(false);
        sproutModel.SetActive(false);
        youngModel.SetActive(false);
        adultModel.SetActive(false);

        switch (currentState)
        {
            case PlantState.Seed: seedModel.SetActive(true); currentRenderer = seedModel.GetComponentInChildren<MeshRenderer>(); break;
            case PlantState.Sprout: sproutModel.SetActive(true); currentRenderer = sproutModel.GetComponentInChildren<MeshRenderer>(); break;
            case PlantState.Young: youngModel.SetActive(true); currentRenderer = youngModel.GetComponentInChildren<MeshRenderer>(); break;
            case PlantState.Adult: adultModel.SetActive(true); currentRenderer = adultModel.GetComponentInChildren<MeshRenderer>(); break;
        }
    }

    public void SetGrowthSpeed(float newSpeed)
    {
        growthSpeed = newSpeed;
        OnGrowthSpeedChanged?.Invoke(growthSpeed);
    }

    public void ResetPlantState()
    {
        growthTimer = 0f;
        currentState = PlantState.Seed;
        UpdatePlantModel();
    }
}

using UnityEngine;
using System.IO;

public class PlantStatsManager : MonoBehaviour
{
    public static PlantStatsManager Instance;

    public PlantStats stats = new PlantStats();
    private string savePath;

    [Header("Referencias")]
    public PlantGrowthManager plant;
    public DayNightCycle dayCycle;

    private string folderPath;
    private int currentPlantID;


    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        folderPath = Application.persistentDataPath + "/PlantSaves/";
        if (!Directory.Exists(folderPath))
            Directory.CreateDirectory(folderPath);

        currentPlantID = PlayerPrefs.GetInt("PlantID", 1);

        stats = new PlantStats();
        stats.plantID = currentPlantID;
        stats.startTime = System.DateTime.Now.ToString("dd/MM/yyyy HH:mm");
    }

    private void Start()
    {
        if (plant == null || dayCycle == null)
        {
            Debug.LogError("⚠ Debes asignar plant y dayCycle en PlantStatsManager");
            return;
        }

        plant.OnStateChanged += OnPlantStateChanged;
        plant.OnWaterChanged += OnWaterChanged;
        plant.OnTemperatureChanged += OnTemperatureChanged;
        plant.OnFertilizerChanged += OnFertilizerChanged;
        plant.OnPlantDied += OnPlantDied;
        plant.OnBecameAdult += OnPlantBecameAdult;

        dayCycle.OnDayPassed += OnDayPassed;
        dayCycle.OnTemperatureChanged += OnTemperatureChanged;

        stats.startTime = System.DateTime.Now.ToString("dd/MM/yyyy HH:mm"); 
    }
    private void OnPlantStateChanged(PlantState newState)
    {
        stats.lastState = newState.ToString();

        if (newState == PlantState.Sprout) stats.reachedSprout = true;
        if (newState == PlantState.Young) stats.reachedYoung = true;

        SaveStats();
    }

    private void OnWaterChanged(float value)
    {
        stats.totalWaterUsed += value;
        stats.lastWater = value;
        SaveStats();
    }

    private void OnTemperatureChanged(float temp)
    {
        stats.lastTemperature = temp;
        SaveStats();
    }

    private void OnFertilizerChanged(float value)
    {
        stats.totalFertilizerUsed += value;
        stats.lastFertilizer = value;
        SaveStats();
    }

    private void OnPlantDied()
    {
        stats.died = true;
        stats.deathTime = System.DateTime.Now.ToString("dd/MM/yyyy HH:mm");
        SaveStats();

        RegisterNewPlantID();
    }


    private void OnPlantBecameAdult()
    {
        stats.reachedAdult = true;
        stats.adultTime = System.DateTime.Now.ToString("dd/MM/yyyy HH:mm");
        SaveStats();
    }

    private void OnDayPassed(int dayNumber)
    {
        stats.daysAlive = dayNumber;
        SaveStats();
    }
    public void SaveStats()
    {
        string path = folderPath + "Plant_" + stats.plantID + ".json";
        string json = JsonUtility.ToJson(stats, true);
        File.WriteAllText(path, json);

        Debug.Log("Guardado en: " + path);
    }

    private int GetNextPlantID()
    {
        int id = 1;

        while (File.Exists(folderPath + "Plant_" + id + ".json"))
        {
            id++;
        }

        return id;
    }

    public void RegisterNewPlantID()
    {
        currentPlantID++;
        PlayerPrefs.SetInt("PlantID", currentPlantID);
    }

    public void OpenStatsFolder()
    {
        string path = folderPath;

#if UNITY_EDITOR

        System.Diagnostics.Process.Start("explorer.exe", "/open," + path.Replace("/", "\\"));
#else

    System.Diagnostics.Process.Start("explorer.exe", "/open," + path.Replace("/", "\\"));
#endif

        Debug.Log("Abriendo carpeta: " + path);
    }

}


[System.Serializable]
public class PlantStats
{
    public int plantID = 0;

    public string startTime = "";
    public string deathTime = "";
    public string adultTime = "";
    public bool reachedSprout;
    public bool reachedYoung;
    public bool reachedAdult;
    public bool died;

    public string lastState = "Seed";
    public float daysAlive = 0;

    public float totalWaterUsed = 0;
    public float totalFertilizerUsed = 0;

    public float lastWater = 0;
    public float lastTemperature = 0;
    public float lastFertilizer = 0;
}



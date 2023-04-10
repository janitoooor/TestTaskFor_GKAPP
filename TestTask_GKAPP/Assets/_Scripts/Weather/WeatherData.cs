[System.Serializable]
public class WeatherData
{
    public string name;
    public Weather[] weather;
    public Main main;
    public Wind wind;

    [System.Serializable]
    public class Main
    {
        public float temp;
        public float pressure;
        public float humidity;
    }

    [System.Serializable]
    public class Weather
    {
        public string description;
    }

    [System.Serializable]
    public class Wind
    {
        public float speed;
    }
}

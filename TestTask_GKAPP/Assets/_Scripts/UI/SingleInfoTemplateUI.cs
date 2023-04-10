using System;
using TMPro;
using UnityEngine;

public class SingleInfoTemplateUI : MonoBehaviour
{
    [SerializeField] private InfoTemplateType _currentTemplateType;
    [SerializeField] private ResponseUI _responseUI;
    [SerializeField] private TextMeshProUGUI _currentText;
    private const double KELVIN_DOUBLE = 273.15f;

    private void Start()
    {
        _responseUI.OnWeatherDatGet += ResponseUI_OnWeatherDatGet;
    }

    private void ResponseUI_OnWeatherDatGet(WeatherData weatherData)
    {

        switch (_currentTemplateType)
        {
            case InfoTemplateType.name: SetCurrentText(weatherData.name); break;
            case InfoTemplateType.temp: SetCurrentText($"{(ConvertTemperatureFromKeToCo(weatherData.main.temp))}C"); break;
            case InfoTemplateType.weather: SetCurrentText((weatherData.weather[0].description).ToString()); break;
            case InfoTemplateType.wind: SetCurrentText($"{(weatherData.wind.speed)}Ï/Ò"); break;
            case InfoTemplateType.pressure: SetCurrentText($"{(weatherData.main.pressure)}Íœ‡"); break;
            case InfoTemplateType.humidity: SetCurrentText($"{(weatherData.main.humidity)}%"); break;
        }
    }

    private float ConvertTemperatureFromKeToCo(float tempK)
    {
        return (float)Math.Round(tempK - KELVIN_DOUBLE, 1);
    }

    private void SetCurrentText(string text)
    {
        _currentText.text = text;
    }
}

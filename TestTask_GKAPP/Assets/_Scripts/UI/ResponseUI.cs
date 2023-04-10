using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResponseUI : MonoBehaviour
{
    public event Action<WeatherData> OnWeatherDatGet;

    [SerializeField] private WeatherService _weatherService;
    [Space]
    [Header("UI elements")]
    [SerializeField] private TextMeshProUGUI _exceptionText;
    [SerializeField] private TMP_InputField _regionInputField;
    [SerializeField] private Button _tryGetResponseButton;

    private string _regionName;

    #region Mono

    private void Awake()
    {
        _tryGetResponseButton.onClick.AddListener(() =>
        {
            _weatherService.SendRequest();
        });

        _regionInputField.onValueChanged.AddListener((string str) =>
        {
            _regionName = str;
            _weatherService.SetCityName(_regionName);
        });
    }

    private void Start()
    {
        _regionInputField.text = _weatherService.CurrentRegionName;
    }

    private void OnEnable()
    {
        _weatherService.OnResponseSuccessed += JsonPlaceHolderService_OnResponseSuccessed;

        _weatherService.ExceptionHandler.OnExceptionСatch += ExceptionHandler_OnExceptionСatch;
    }

    #endregion

    private void ExceptionHandler_OnExceptionСatch(object sender, ExceptionHandler.ExceptionEventArgs e)
    {
        print(e.ExceptionDescription + e.Exception.Message);
        ChangeText(ref _exceptionText, Color.red, e.ExceptionDescription);
    }

    private void JsonPlaceHolderService_OnResponseSuccessed(object sender, WeatherService.WeatherDataEventArgs weatherDataEventArgs)
    {
        OnWeatherDatGet?.Invoke(weatherDataEventArgs.WeatherData);
        _exceptionText.text = null;
    }

    private void ChangeText(ref TextMeshProUGUI textMeshProUGUI, Color color, string text)
    {
        textMeshProUGUI.color = color;
        textMeshProUGUI.text = text;
    }
}

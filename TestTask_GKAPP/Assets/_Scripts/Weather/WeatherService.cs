using System;
using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Text.RegularExpressions;

public class WeatherService : MonoBehaviour
{
    public event EventHandler<WeatherDataEventArgs> OnResponseSuccessed;
    public class WeatherDataEventArgs : EventArgs
    {
        public WeatherData WeatherData;
    }

    private const string API_KEY = "77c515a717b15398216f94830a1a59a6";
    private const string DEFAULT_API_KEY = "API_KEY";

    [SerializeField] private string _currentCityName = "Minsk";
    [SerializeField] private string _currentLanguage = "ru";

    public string CurrentRegionName => _currentCityName;

    private string _url;

    private readonly ExceptionHandler _exceptionHandler = new();
    public ExceptionHandler ExceptionHandler => _exceptionHandler;

    #region Mono

    private void Start()
    {
        SetCityName(_currentCityName);
    }

    #endregion

    public void SendRequest()
    {

        if (string.IsNullOrEmpty(_currentCityName) || !Regex.IsMatch(_currentCityName, "^[à-ÿ, À-ß, a-z, A-Z]+$"))
        {
            _exceptionHandler.HandleException(new Exception("Invalid string format "));
            return;
        }

        StartCoroutine(StartRequest());
    }

    public void SetCityName(string regionName)
    {
        _currentCityName = regionName;
        SetUrl();
    }

    public void SetLanguage(string currentLanguage)
    {
        _currentLanguage = currentLanguage;
        SetUrl();
    }

    private void SetUrl()
    {
        _url = $"https://api.openweathermap.org/data/2.5/weather?q={_currentCityName}&appid={API_KEY}&lang={_currentLanguage}";
    }

    private IEnumerator StartRequest()
    {
        UnityWebRequest request = GetRequest(out bool isFailedRequest);

        if (isFailedRequest)
            yield break;

        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            _exceptionHandler.HandleException(new Exception(request.error));
            yield break;
        }

        GetResponse(request);
    }

    private UnityWebRequest GetRequest(out bool isFailedRequest)
    {
        isFailedRequest = false;
        UnityWebRequest request = null;

        try
        {
            request = UnityWebRequest.Get(_url.Replace(DEFAULT_API_KEY, API_KEY));
        }
        catch (Exception ex)
        {
            _exceptionHandler.HandleException(ex);
            isFailedRequest = true;
        }

        return request;
    }

    private void GetResponse(UnityWebRequest request)
    {
        try
        {
            WeatherData weatherData = JsonUtility.FromJson<WeatherData>(request.downloadHandler.text);

            OnResponseSuccessed?.Invoke(this, new WeatherDataEventArgs
            {
                WeatherData = weatherData
            });
        }
        catch (Exception ex)
        {
            _exceptionHandler.HandleException(ex);
        }
        finally
        {
            request?.Dispose();
        }
    }
}

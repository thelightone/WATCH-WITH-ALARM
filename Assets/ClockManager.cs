using System;
using System.Collections;
using System.Globalization;
using System.Net;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ClockManager : MonoBehaviour
{
    [SerializeField]
    private TMP_Text _timeString;
    [SerializeField]
    private Transform _hourTransform, _minuteTransform, _secondTransform;
    [SerializeField]
    private GameObject _error;

    private float _degreesInHour, _degreesInMinute, _degreesInSecond;
    private DateTime _realTime;
    private int _hour, _minute, _second;

    private void Start()
    {
        _degreesInHour = 30;
        _degreesInMinute = 6;
        _degreesInSecond = 6;

        StartManager();
    }

    private void SetTime()
    {
        _hour = _realTime.Hour;
        _minute = _realTime.Minute;
        _second = _realTime.Second;

        _hourTransform.localRotation = Quaternion.Euler(0f, 0f, _hour * -_degreesInHour);
        _minuteTransform.localRotation = Quaternion.Euler(0f, 0f, _minute * -_degreesInMinute);
        _secondTransform.localRotation = Quaternion.Euler(0f, 0f, _second * -_degreesInSecond);

        StartCoroutine(Rotate());
    }

    private void CheckAlarmDig()
    {
        var timeString = string.Format("{0:d2}:{1:d2}:{2:d2}", _hour, _minute, _second);

        if (timeString == AlarmManager.Instance.timeAlarm)
        {
            AlarmManager.Instance.Alarm();
        }
    }

    private void CheckAlarmArrow()
    {
        var hourToTwelve = _hour;
        if (_hour > 12)
        {
            hourToTwelve = _hour - 12;
        }
        var timeStringToTwelve = string.Format("{0:d2}:{1:d2}:{2:d2}", hourToTwelve, _minute, _second);

        if (timeStringToTwelve == AlarmManager.Instance.timeAlarm)
        {
            AlarmManager.Instance.Alarm();
        }
    }

    private void PrintTime()
    {
        _second++;
        if (_second > 59)
        {
            _second = 0;
            _minute++;
            {
                if (_minute > 59)
                {
                    _minute = 0;
                    _hour++;

                    if (_hour > 23)
                    {
                        _hour = 0;
                    }
                }
            }
        }
        _timeString.text = string.Format("{0:d2}:{1:d2}:{2:d2}",
            _hour, _minute, _second);

        CheckAlarmDig();
        CheckAlarmArrow();
    }

    public void StartManager()
    {
        var timeYahoo = GetDateTime("https://msdn.com");
        var timeGoogle = GetDateTime("https://google.com");

        if (Math.Abs(timeGoogle.Second - timeYahoo.Second) <= 1)
        {
            _realTime = timeGoogle;
            SetTime();
        }

        else
        {
            Debug.Log("Error: Different data in sources");
            _realTime = timeGoogle;
            SetTime();
        }
    }

    public void Restart()
    {
        SceneManager.LoadScene(0);
    }

    IEnumerator Rotate()
    {
        for (int h = 0; h < 3600; h++)
        {
            _hourTransform.transform.Rotate(0f, 0f, -_degreesInHour / 3600);
            _minuteTransform.transform.Rotate(0f, 0f, -_degreesInMinute / 60);
            _secondTransform.transform.Rotate(0f, 0f, -6.000f);

            PrintTime();

            yield return new WaitForSeconds(1f);
        }
        StartManager();
    }
    DateTime GetDateTime(string source)
    {
        try
        {
            var myHttpWebRequest = (HttpWebRequest)WebRequest.Create(source);
            using (var response = myHttpWebRequest.GetResponse())
            {
                string time = response.Headers["date"];
                return DateTime.ParseExact(time,
                                           "ddd, dd MMM yyyy HH:mm:ss 'GMT'",
                                           CultureInfo.InvariantCulture.DateTimeFormat,
                                           DateTimeStyles.AssumeUniversal);
            }
        }
        catch (WebException ex)
        {
            _error.SetActive(true);
            throw ex;
        }
    }
}


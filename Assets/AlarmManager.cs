using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;

public class AlarmManager : MonoBehaviour
{
    [SerializeField]
    private TMP_InputField _input;
    [SerializeField]
    private GameObject _hourArrow;
    [SerializeField]
    private GameObject _minuteArrow;
    [SerializeField]
    private GameObject _secondArrow;

    private AudioSource _source;

    public static AlarmManager Instance;
    public string timeAlarm=" ";

    private void Awake()
    {
        Instance = this;
        _source = GetComponent<AudioSource>();
    }

    public void SetAlarm()
    {
        if (_input.text != "00:00:00")
        {
            timeAlarm = _input.text;
            _input.text = "00:00:00";
        }
        else
        {
            int hours = (int)Math.Round(12 - _hourArrow.transform.localEulerAngles.z / 30);
            int min = (int)Math.Round(60 - _minuteArrow.transform.localEulerAngles.z / 6);
            int sec = (int)Math.Round(60 - (_secondArrow.transform.localEulerAngles.z / 6));

            timeAlarm = string.Format("{0:d2}:{1:d2}:{2:d2}", hours, min, sec);
        }
    }

    public void Alarm()
    {
        timeAlarm = "";
        _source.Play();
    }
}

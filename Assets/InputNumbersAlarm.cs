using TMPro;
using UnityEngine;

public class InputNumbersAlarm : MonoBehaviour
{
    [SerializeField]
    private TMP_InputField _inputField;

    private int _prevLength;

    void Awake()
    {
        _inputField = GetComponent<TMP_InputField>();
        _prevLength = 0;
    }

    public void OnValueChanged()
    {
        var length = _inputField.text.Length;

        if (length > _prevLength && (length == 2 || length == 5))
        {
            _inputField.text = _inputField.text + ":";
            _inputField.stringPosition = _inputField.stringPosition + 1;
        }

        else if (length == 3 || length == 6)
        {
            if (_inputField.text[length - 1] != ':')
            {
                _inputField.text = _inputField.text.Substring(0, length - 1) + ':' + _inputField.text[length - 1];
                _inputField.stringPosition = _inputField.stringPosition + 1;
            }
        }
        _prevLength = length;
    }
}

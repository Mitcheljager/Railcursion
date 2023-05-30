using System.Text.RegularExpressions;
using UnityEngine;
using System.Linq;
using TMPro;

public class ValidateInput : MonoBehaviour {
    private TMP_InputField inputField;

    void Start() {
        inputField = GetComponent<TMP_InputField>();
    }

    public void NumbersOnly(int max = 8) {
        string value = inputField.text;

        if (value == "") return;

        string numericValue = new string(value.Where(char.IsDigit).ToArray());
        inputField.text = Mathf.Clamp(int.Parse(numericValue), 0, max).ToString();
    }
}

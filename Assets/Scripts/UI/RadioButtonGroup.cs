using UnityEngine;

public class RadioButtonGroup : MonoBehaviour {
    public RadioButton[] radioButtons;

    void Start() {
        radioButtons = GetComponentsInChildren<RadioButton>();
    }

    public void ToggleRadioButtons(string value) {
        foreach(RadioButton radioButton in radioButtons) {
            radioButton.SetState(radioButton.value == value);
        }
    }
}

using UnityEngine;

public class ViewManager : MonoBehaviour {
    public View[] views;

    public void ShowView(View viewToShow) {
        foreach (View view in views) {
            view.gameObject.SetActive(view.key == viewToShow.key);
        }
    }
}

using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class EventsOnEnable : MonoBehaviour {
    public UnityEvent assignedEvent;

    public void OnEnable() {
        assignedEvent.Invoke();
    }
}

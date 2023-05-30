using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class EventsOnDisable : MonoBehaviour {
    public UnityEvent assignedEvent;

    public void OnDisable() {
        assignedEvent.Invoke();
    }
}

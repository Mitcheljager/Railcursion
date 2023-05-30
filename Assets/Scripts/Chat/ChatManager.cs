using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using FishNet;
using FishNet.Broadcast;
using FishNet.Connection;

public struct Message : IBroadcast {
    public string username;
    public string text;
}

//Made by Bobsi Unity - for youtube
public class ChatManager : MonoBehaviour {
    [Header("Config")]
    public Transform chatHolder;
    public GameObject messageElement;
    public TMP_InputField messageInput;
    public ScrollRect scrollRect;
    [Header("Animation")]
    public AnimationHelper animationHelper;
    public float fadeOutTimer = 10f;

    public float timer = 10f;
    private bool isFocused = false;

    private void OnEnable() {
        messageInput.onSelect.AddListener(OnFocus);
        messageInput.onDeselect.AddListener(OnBlur);

        InstanceFinder.ClientManager.RegisterBroadcast<Message>(OnMessageReceived);
        InstanceFinder.ServerManager.RegisterBroadcast<Message>(OnClientMessageReceived);

        ResetTimer();
    }

    private void OnDisable() {
        messageInput.onSelect.RemoveListener(OnFocus);
        messageInput.onDeselect.RemoveListener(OnBlur);

        InstanceFinder.ClientManager.UnregisterBroadcast<Message>(OnMessageReceived);
        InstanceFinder.ServerManager.UnregisterBroadcast<Message>(OnClientMessageReceived);
    }

    private void Update() {
        if(Input.GetKeyDown(KeyCode.Return)) HandleReturnKey();

        if (isFocused) {
            ResetTimer();
            return;
        }

        if (timer > 0f) timer -= Time.deltaTime;
        else animationHelper.FadeOut(0.5f);
    }

    private void OnFocus(string text) {
        isFocused = true;
    }

    private void OnBlur(string text) {
        isFocused = false;
    }

    private void HandleReturnKey() {
        Debug.Log(isFocused);
        if (isFocused) SendMessage();
        else messageInput.ActivateInputField();
    }

    private void SendMessage() {
        if (messageInput.text.Trim() == "") return;

        Message message = new Message() {
            username = "Some name",
            text = messageInput.text
        };

        messageInput.text = "";

        if (InstanceFinder.IsServer) InstanceFinder.ServerManager.Broadcast(message);
        else if (InstanceFinder.IsClient) InstanceFinder.ClientManager.Broadcast(message);
    }

    private void OnMessageReceived(Message message) {
        ResetTimer();

        GameObject finalMessage = Instantiate(messageElement, chatHolder);
        finalMessage.GetComponent<TextMeshProUGUI>().text = message.username + ": " + message.text;

        StartCoroutine(ScrollToBottom());
    }

    private void OnClientMessageReceived(NetworkConnection networkConnection, Message message) {
        ResetTimer();

        InstanceFinder.ServerManager.Broadcast(message);
        scrollRect.normalizedPosition = new Vector2(0, 0);

        ScrollToBottom();
    }

    private void ResetTimer() {
        if (timer <= 0f) animationHelper.FadeIn(0.25f);
        timer = fadeOutTimer;
    }

    private IEnumerator ScrollToBottom() {
        yield return new WaitForEndOfFrame();
        scrollRect.verticalNormalizedPosition = 0f;
    }
}

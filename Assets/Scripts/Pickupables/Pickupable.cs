using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using FishNet.Object;

public enum PickupableType {
    Throwable,
    Usable
}

public class Pickupable : NetworkBehaviour {
    public string itemName = "";
    public float cooldown = 1f;
    public PickupableType itemType;
    public GameObject prefab;
    public ObjectPool itemObjectPool;
    public GameObject orb;
    public GameObject UI;
    public TextMeshProUGUI text;
    public Canvas inWorldCanvas;
    public Image cooldownImage;

    public PlayerReference playerInside;
    private AnimationHelper animationHelper;

    public float cooldownTimer = 0f;

    private PlayersManager playersManager;

    void Start() {
        playersManager = FindObjectOfType<PlayersManager>();
        animationHelper = GetComponent<AnimationHelper>();

        cooldownImage.type = Image.Type.Filled;
        cooldownImage.fillMethod = Image.FillMethod.Radial360;
        cooldownImage.fillOrigin = (int)Image.Origin360.Bottom;
    }

    void Update() {
        if (cooldownTimer >= 0) UpdateCooldown();
    }

    void OnTriggerEnter(Collider player) {
        if (player.tag != "Player") return;

        PlayerReference playerReference = player.gameObject.GetComponent<PlayerReference>();
        if (playerReference == null) return;
        if (playerReference != playersManager.currentPlayer) return;

        playerInside = playerReference;

        SetUIText();
    }

    void OnTriggerStay(Collider player) {
        if (cooldownTimer > 0f) return;
        if (player.tag != "Player") return;
        if (playerInside == null) return;

        SetUIText();

        if (playerInside != playersManager.currentPlayer) return;
        if (!playerInside.playerInventory.IsPickingUp()) return;

        PickUpForPlayer(playerInside.playerInventory);
    }

    void OnTriggerExit(Collider player) {
        if (player.tag != "Player") return;
        if (playerInside == null) return;
        Unset();
    }

    void OnDisable() {
        Unset();
    }

    private void SetUIText() {
        if (!UI.activeInHierarchy) {
            animationHelper.FlyIn(10f, 0.2f);
            UI.SetActive(true);
        }

        text.text = cooldownTimer > 0f ? "Unavailable" : "Pick up <" + itemName + ">";
    }

    private void Unset() {
        playerInside = null;
        HideUI();
    }

    private void HideUI() {
        UI.SetActive(false);
    }

    private void UpdateCooldown() {
        cooldownTimer -= Time.deltaTime;
        cooldownImage.fillAmount = Mathf.Clamp01(1f / cooldown * (cooldown - cooldownTimer));

        if (cooldownTimer <= 0) {
            inWorldCanvas.gameObject.SetActive(false);
            orb.SetActive(true);
        }
    }

    private void SetCooldownUI(bool setTimer = true) {
        if (cooldownTimer > 0f) return;

        if (setTimer) cooldownTimer = cooldown;

        inWorldCanvas.gameObject.SetActive(true);
        orb.SetActive(false);
    }

    private void PickUpForPlayer(PlayerInventory playerInventory) {
        if (cooldownTimer > 0f) return;

        SetCooldownUI(false);
        HideUI();

        ServerPickUp(playerInventory);
    }

    [ServerRpc(RequireOwnership = false)]
    private void ServerPickUp(PlayerInventory playerInventory) {
        if (cooldownTimer > 0f) return;

        playerInventory.PickUp(this, itemObjectPool);

        SetCooldownUI();
        SetCooldownForObservers();
    }

    [ObserversRpc]
    private void SetCooldownForObservers() {
        SetCooldownUI();
        HideUI();
    }
}

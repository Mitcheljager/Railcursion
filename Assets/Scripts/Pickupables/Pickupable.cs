using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum PickupableType {
    Throwable,
    Usable
}

public class Pickupable : MonoBehaviour {
    public string itemName = "";
    public float cooldown = 1f;
    public PickupableType itemType;
    public GameObject prefab;
    public GameObject orb;
    public GameObject UI;
    public TextMeshProUGUI text;
    public Canvas inWorldCanvas;
    public Image cooldownImage;

    private PlayerReference playerInside;
    private AnimationHelper animationHelper;

    public float cooldownTimer = 0f;

    void Start() {
        animationHelper = GetComponent<AnimationHelper>();

        cooldownImage.type = Image.Type.Filled;
        cooldownImage.fillMethod = Image.FillMethod.Radial360;
        cooldownImage.fillOrigin = (int)Image.Origin360.Bottom;
    }

    void Update() {
        if (cooldownTimer > 0f) UpdateCooldown();
    }

    void OnTriggerEnter(Collider player) {
        if (player.tag != "Player") return;

        PlayerReference playerReference = player.gameObject.GetComponent<PlayerReference>();
        if (playerReference == null) return;

        playerInside = playerReference;

        SetUIText();
    }

    void OnTriggerStay(Collider player) {
        if (cooldownTimer > 0f) return;
        if (player.tag != "Player") return;
        if (!playerInside) return;

        if (!playerInside.playerInventory.IsPickingUp()) return;

        playerInside.playerInventory.PickUp(this);

        SetCooldown();
    }

    void OnTriggerExit(Collider player) {
        if (player.tag != "Player") return;
        Reset();
    }

    void OnDisable() {
        Reset();
    }

    private void SetUIText() {
        animationHelper.FlyIn(10f, 0.2f);
        UI.SetActive(true);
        text.text = cooldownTimer > 0f ? "Unavailable" : "Pick up <" + itemName + ">";
    }

    private void Reset() {
        playerInside = null;
        UI.SetActive(false);
    }

    private void SetCooldown() {
        cooldownTimer = cooldown;
        inWorldCanvas.gameObject.SetActive(true);
        orb.SetActive(false);
    }

    private void UpdateCooldown() {
        cooldownTimer -= Time.deltaTime;
        cooldownImage.fillAmount = Mathf.Clamp01(1f / cooldown * (cooldown - cooldownTimer));

        if (cooldownTimer <= 0) {
            inWorldCanvas.gameObject.SetActive(false);
            orb.SetActive(true);
        }
    }
}

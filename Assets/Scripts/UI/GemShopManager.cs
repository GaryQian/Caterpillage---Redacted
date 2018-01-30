using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GemShopManager : MonoBehaviour {

    public Text gemCount;
    public GameObject IAPMenuPrefab;
    GameObject IAPMenu;

    public Text healthPotionDesc;
    public Text speedPotionDesc;
    public Text berserkPotionDesc;

    public Text healthPotionPriceText;
    public Text speedPotionPriceText;
    public Text berserkPotionPriceText;

    public int healthPotionPrice = 10;
    public int speedPotionPrice = 11;
    public int berserkPotionPrice = 12;
    public int goldPrice = 20;

    public Text healthPotionCount;
    public Text speedPotionCount;
    public Text berserkPotionCount;

    public GameObject noMoneyPrefab;

    public AudioClip undergroundRumbleClip;
    public AudioClip coins1Clip;
    public AudioClip gems1Clip;

    public static bool IAPOpen;

    // Use this for initialization
    void Start () {

        healthPotionDesc.fontSize = (int)(Util.fontScale * healthPotionDesc.fontSize);
        speedPotionDesc.fontSize = (int)(Util.fontScale * speedPotionDesc.fontSize);
        berserkPotionDesc.fontSize = (int)(Util.fontScale * berserkPotionDesc.fontSize);

        healthPotionPriceText.text = "" + healthPotionPrice;
        speedPotionPriceText.text = "" + speedPotionPrice;
        berserkPotionPriceText.text = "" + berserkPotionPrice;
        UpdateCount();

        IAPOpen = false;
    }

    public void BuyHealthPotion() {
        if (Util.gm.SpendGems(healthPotionPrice)) {
            GameManager.slots.slots[GameManager.slotId].progress.consumableCount[Progress.healthPotionId]++;
            UpdateCount();
            GameManager.sm.SaveCurrentSlot();
            SoundManager.PlaySingleSfx(gems1Clip);
        }
        else {
            OpenIAP();
        }
    }

    public void BuySpeedPotion() {
        if (Util.gm.SpendGems(speedPotionPrice)) {
            GameManager.slots.slots[GameManager.slotId].progress.consumableCount[Progress.speedPotionId]++;
            UpdateCount();
            GameManager.sm.SaveCurrentSlot();
            SoundManager.PlaySingleSfx(gems1Clip);
        }
        else {
            OpenIAP();
        }
    }

    public void BuyBerserkPotion() {
        if (Util.gm.SpendGems(berserkPotionPrice)) {
            GameManager.slots.slots[GameManager.slotId].progress.consumableCount[Progress.berserkPotionId]++;
            UpdateCount();
            GameManager.sm.SaveCurrentSlot();
            SoundManager.PlaySingleSfx(gems1Clip);
        }
        else {
            OpenIAP();
        }
    }

    public void BuyGold() {
        if (Util.gm.SpendGems(goldPrice)) {
            Util.gm.AddMoney(goldPrice*100f);
            UpdateCount();
            GetComponentInParent<UpgradeManager>().UpdateMoney();
            GameManager.sm.SaveCurrentSlot();
            SoundManager.PlaySingleSfx(coins1Clip);
        }
        else {
            OpenIAP();
        }
    }



    void UpdateCount() {
        Util.gm.upgradeMenu.GetComponent<UpgradeManager>().UpdateGems();
        //gemCount.text = "" + GameManager.slots.slots[GameManager.slotId].progress.gems;
        healthPotionCount.text = "x" + GameManager.slots.slots[GameManager.slotId].progress.consumableCount[Progress.healthPotionId];
        speedPotionCount.text = "x" + GameManager.slots.slots[GameManager.slotId].progress.consumableCount[Progress.speedPotionId];
        berserkPotionCount.text = "x" + GameManager.slots.slots[GameManager.slotId].progress.consumableCount[Progress.berserkPotionId];
    }

    public void OpenIAP() {
        if (IAPMenu != null) {
            IAPDie();
        }
        IAPOpen = true;
        IAPMenu = Instantiate(IAPMenuPrefab, Vector3.zero, Quaternion.identity, Util.gm.canvas.GetComponent<RectTransform>());
    }

    public void CloseIAP() {
        Invoke("IAPDie", 1f);

        IAPMenu.GetComponent<GoToTarget>().ChangeTarget(new Vector3(0, 1000f, 0));
    }

    void IAPDie() {
        IAPOpen = false;
        Destroy(IAPMenu);
    }

    public void ExitButtonPressed() {
        GameManager.upgradeManager.GetComponent<GoToTarget>().targetPosition = new Vector3(0, 0, 0);
        Invoke("SetGemShopFalse", 0.5f);
        SoundManager.PlaySfx(undergroundRumbleClip);
    }

    void SetGemShopFalse() {
        GameManager.inGemShop = false;
    }
}

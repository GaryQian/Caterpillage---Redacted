using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum categories { none, spitter, wings, shell, traits };

public class UpgradeManager : MonoBehaviour {

    public GameObject selectorButtons;
    public GameObject upgradeInfoPanel;
    public GameObject levelSelector;
    public GameObject MoneyStarsPanel;

    public GameObject area1;
    public GameObject area2;

    public Image wormMenu;
    public Sprite wormMenuSprite;
    public Sprite wormMenuTallSprite;

    //EVOLVE
    public GameObject evolveMenu;
    public Text evolveDesc;
    public Text evolvePrice;
    float evolvePriceFloat;
    public Image evolveIcon;
    public GameObject evolveButton;

    //BAR
    public GameObject bar1Current;
    public GameObject bar1Increment;
    public Text number1Current;
    public Text number1Increment;
    public GameObject bar2Current;
    public GameObject bar2Increment;
    public Text number2Current;
    public Text number2Increment;
    
    public Button upgrade1Button;
    public Button upgrade2Button;
    public Text description1;
    public Text description2;
    public Text title1;
    public Text title2;
    public Text categoryTitle;

    public Text priceText1;
    public Text priceText2;

    public Sprite[] spitterIcons;
    public Sprite[] wingsIcons;
    public Sprite[] shellIcons;
    public Sprite[] traitsIcons;
    public Image icon1;
    public Image icon2;

    public Text moneyDisplay;
    public Text gemDisplay;
    public Text gemDisplayStore;
    public Text starDisplay;
    bool flashRed = false;
    float redTimer = 0f;

    public GameObject rateUsPrefab;

    public GameObject diveTutorialPrefab;
    public GameObject flapTutorialPrefab;
    public GameObject spitTutorialPrefab;

    //public Button optionsButton;
    public GameObject optionsPrefab;
    GameObject optionsMenu;
    //public Button 
    public bool optionsPressed = false;

    public GameObject noMoneyPref;
    
    public Button spitterButton;
    public Image spitterIndicator;
    public Text spitterText;
    public Button wingsButton;
    public Image wingsIndicator;
    public Text wingsText;
    public Button shellButton;
    public Image shellIndicator;
    public Text shellText;
    public Button traitsButton;
    public Image traitsIndicator;
    public Text traitsText;

    public Image cocoon;

    public Button playButton;

    

    UpgradeDescription desc1;
    UpgradeDescription desc2;

    
    categories selectedCat;

    float scale;
    float angle = 0;
    float angularVel = 35;
    float pulse = 0;
    float pulseVel = 30;
    RectTransform spitterIndicatorTrans;
    RectTransform spitterButtonTrans;
    RectTransform wingsIndicatorTrans;
    RectTransform wingsButtonTrans;
    RectTransform shellIndicatorTrans;
    RectTransform shellButtonTrans;
    RectTransform traitsIndicatorTrans;
    RectTransform traitsButtonTrans;
    RectTransform cocoonTrans;
    Image cocoonImage;

    public AudioClip undergroundRumbleClip;
    public AudioClip pwopClip;
    public AudioClip noMoneyClip;

    public GameObject MinusTextPrefab;
    public GameObject CoinWrapper;
    public GameObject GemShopWrapper;

    public AudioClip[] coinsClips;

    private void Awake() {
        transform.localPosition = transform.localPosition + new Vector3(-1930, 0, 0);
    }

    // Use this for initialization
    void Start() {

        GameManager.upgradeManager = this;

        desc1 = new UpgradeDescription();
        desc2 = new UpgradeDescription();

        if (Util.screenRatio < 1.5f) {
            Debug.Log(Util.screenRatio);
            wormMenu.sprite = wormMenuTallSprite;
        }

        UpdateMoney();
        UpdateGems();
        UpdateStars();

        upgrade1Button.gameObject.SetActive(false);
        upgrade2Button.gameObject.SetActive(false);

        spitterIndicatorTrans = spitterIndicator.GetComponent<RectTransform>();
        spitterButtonTrans = spitterButton.GetComponent<RectTransform>();
        wingsIndicatorTrans = wingsIndicator.GetComponent<RectTransform>();
        wingsButtonTrans = wingsButton.GetComponent<RectTransform>();
        shellIndicatorTrans = shellIndicator.GetComponent<RectTransform>();
        shellButtonTrans = shellButton.GetComponent<RectTransform>();
        traitsIndicatorTrans = traitsIndicator.GetComponent<RectTransform>();
        traitsButtonTrans = traitsButton.GetComponent<RectTransform>();

        description1.fontSize = (int)(description1.fontSize * Util.fontScale);
        description2.fontSize = (int)(description2.fontSize * Util.fontScale);
        spitterText.fontSize = (int)(spitterText.fontSize * Util.fontScale);
        wingsText.fontSize = (int)(wingsText.fontSize * Util.fontScale);
        shellText.fontSize = (int)(shellText.fontSize * Util.fontScale);
        traitsText.fontSize = (int)(traitsText.fontSize * Util.fontScale);

        cocoonTrans = cocoon.GetComponent<RectTransform>();
        cocoonImage = cocoon.GetComponent<Image>();
        angle = 0;
        pulse = 0;

        //Ask for a rating if they dismiss.
        if (GameManager.level > 21 && !GameManager.settings.rated && (UnbiasedTime.Instance.Now() - GameManager.slots.slots[GameManager.slotId].rateTime).TotalMinutes 
            > (Mathf.Pow(2, GameManager.slots.slots[GameManager.slotId].rateCount + 1) + 0) * 1440) {
            Instantiate(rateUsPrefab, Util.canvas.transform);
            GameManager.slots.slots[GameManager.slotId].rateCount++;
            GameManager.slots.slots[GameManager.slotId].rateTime = UnbiasedTime.Instance.Now();
            GameManager.sm.SaveCurrentSlot();
        }

        SoundManager.instance.ChangeMusic(MusicType.menu);
    }

    // Update is called once per frame
    void Update() {
        if (flashRed) {
            redTimer += 0.05f ;
            moneyDisplay.color = Color.Lerp(Color.red, Color.white, redTimer);
            if (redTimer >= 1) {
                flashRed = false;
                redTimer = 0;
            }
        }
        angle += angularVel*Time.deltaTime;
        spitterIndicatorTrans.rotation = Quaternion.Euler(0, 0, angle);
        spitterButtonTrans.rotation = Quaternion.Euler(0, 0, 0);
        wingsIndicatorTrans.rotation = Quaternion.Euler(0, 0, angle);
        wingsButtonTrans.rotation = Quaternion.Euler(0, 0, 0);
        shellIndicatorTrans.rotation = Quaternion.Euler(0, 0, angle);
        shellButtonTrans.rotation = Quaternion.Euler(0, 0, 0);
        traitsIndicatorTrans.rotation = Quaternion.Euler(0, 0, angle);
        traitsButtonTrans.rotation = Quaternion.Euler(0, 0, 0);

        float sc = 1f + Mathf.Sin(Time.time * 6f) * .08f;
        float cocoonSC = 1.02f + Mathf.Sin(Time.time * 2f) * .02f;
        float glow = 0.9f + Mathf.Sin(Time.time * 2f) * 0.1f;
        cocoonTrans.localScale = new Vector3(cocoonSC, cocoonSC, cocoonSC);
        cocoonImage.color = new Color(1, 1, 1, glow);

        spitterIndicatorTrans.localScale = new Vector3(1, 1, 1);
        wingsIndicatorTrans.localScale = new Vector3(1, 1, 1);
        shellIndicatorTrans.localScale = new Vector3(1, 1, 1);
        traitsIndicatorTrans.localScale = new Vector3(1, 1, 1);
        switch (selectedCat) {
            case categories.spitter:
                spitterIndicatorTrans.localScale = new Vector3(sc, sc, sc);
                break;
            case categories.wings:
                wingsIndicatorTrans.localScale = new Vector3(sc, sc, sc);
                break;
            case categories.shell:
                shellIndicatorTrans.localScale = new Vector3(sc, sc, sc);
                break;
            case categories.traits:
                traitsIndicatorTrans.localScale = new Vector3(sc, sc, sc);
                break;
        }
    }

    public void LeaderboardButtonPress() {
        Debug.Log("Showing Leaderboard UI");
        Social.ShowLeaderboardUI();
    }

    //####################################################### PLANET SCROLLER #################################################################
    public void PlayButtonPress() {
        //loads selected level
    }

    //####################################################### UPGRADES MENU #################################################################
    public void Upgrade1ButtonPress() {
        if (!Util.gm.SpendMoney(desc1.cost)) {
            NoMoney();
            return;
        }
        if (selectedCat == categories.spitter) {
            
            GameManager.wormStats.SpitterFeatureModify(GameManager.wormStats.NextSpitterFeature(), true);
            SpitterButtonPress();
        }
        else if (selectedCat == categories.wings) {
            if (GameManager.wormStats.NextWingFeature() == WingFeatures.dive) {
                Instantiate(diveTutorialPrefab, Util.gm.canvas.transform);
            }
            if (GameManager.wormStats.NextWingFeature() == WingFeatures.flap) {
                Instantiate(flapTutorialPrefab, Util.gm.canvas.transform);
            }
            GameManager.wormStats.WingFeatureModify(GameManager.wormStats.NextWingFeature(), true);
            WingsButtonPress();
            
        }
        else if (selectedCat == categories.shell) {
            GameManager.wormStats.ShellFeatureModify(GameManager.wormStats.NextShellFeature(), true);
            ShellButtonPress();
        }
        else if (selectedCat == categories.traits) {
            if (GameManager.wormStats.size >= GameManager.wormStats.maxSize) return;
            GameManager.wormStats.size += 1;
            TraitsButtonPress();
        }
        CheckUpgradeButtonEnable();
        SoundManager.PlaySfx(coinsClips);
    }

    public void Upgrade2ButtonPress() {
        if (!Util.gm.SpendMoney(desc2.cost)) {
            NoMoney();
            return;
        }
        if (selectedCat == categories.spitter) {
            if (GameManager.wormStats.spitLevel >= GameManager.wormStats.maxSpitLevel) return;
            GameManager.wormStats.spitLevel += 1;
            SpitterButtonPress();
        }
        else if (selectedCat == categories.wings) {
            if (GameManager.wormStats.wingsLevel >= GameManager.wormStats.maxWingsLevel) return;
            GameManager.wormStats.wingsLevel += 1;
            WingsButtonPress();
        }
        else if (selectedCat == categories.shell) {
            if (GameManager.wormStats.armorLevel >= GameManager.wormStats.maxArmorLevel) return;
            GameManager.wormStats.armorLevel += 1;
            ShellButtonPress();
        }
        else if (selectedCat == categories.traits) {
            if (GameManager.wormStats.speed >= GameManager.wormStats.maxSpeed) return;
            GameManager.wormStats.speed += 1;
            TraitsButtonPress();
        }
        CheckUpgradeButtonEnable();
        SoundManager.PlaySfx(coinsClips);
    }

    public void EvolveButtonPress() {
        CheckUpgradeButtonEnable();
        if (!Util.gm.SpendMoney(evolvePriceFloat)) {
            NoMoney();
            return;
        }
        if (selectedCat == categories.spitter) {
            Instantiate(spitTutorialPrefab, Util.gm.canvas.transform);
            GameManager.wormStats.EvolveFeature(selectedCat);
            SpitterButtonPress();
        }
        else if (selectedCat == categories.wings) {
            GameManager.wormStats.EvolveFeature(selectedCat);
            WingsButtonPress();
        }
        else if (selectedCat == categories.shell) {
            GameManager.wormStats.EvolveFeature(selectedCat);
            ShellButtonPress(); 
        }
        SoundManager.PlaySfx(coinsClips);
    }

    public void UpdateMoney(float minusAmount = -1f) {
        moneyDisplay.text = "" + (int)Util.gm.GetMoney();
        if (minusAmount > 0) {
            GameObject mt = Instantiate(MinusTextPrefab, moneyDisplay.transform.position, Quaternion.identity, CoinWrapper.transform);
            mt.GetComponent<MinusText>().amount = minusAmount;
        }
    }

    public void UpdateGems(int minusAmount = -1) {
        string text = "" + GameManager.slots.slots[GameManager.slotId].progress.gems;
        gemDisplay.text = text;
        gemDisplayStore.text = text;
        if (minusAmount > 0) {
            GameObject mt = Instantiate(MinusTextPrefab, moneyDisplay.transform.position, Quaternion.identity, gemDisplayStore.transform);
            mt.GetComponent<MinusText>().amount = minusAmount;
            mt.GetComponent<MinusText>().downSpeed = mt.GetComponent<MinusText>().downSpeed * -1f;
        }
    }

    public void UpdateStars() {
        starDisplay.text = "" + GameManager.slots.slots[GameManager.slotId].progress.TotalStars();
    }

    void NoMoney() {
        GameObject noMoneyMessage = Instantiate(noMoneyPref, Vector3.zero, Quaternion.identity, GetComponent<RectTransform>());
        noMoneyMessage.transform.SetAsLastSibling();
        MoneyStarsPanel.GetComponent<RectTransform>().SetAsLastSibling();
        flashRed = true;
        if (flashRed) {
            redTimer = 0f;
        }
        else flashRed = true;

        SoundManager.PlaySingleSfx(noMoneyClip, 1, 1);

    }

    public void optionsMenuPress() {
        if (optionsPressed) {
            optionsPressed = false;
            Destroy(optionsMenu);
        }
        else {
            optionsPressed = true;
            optionsMenu = Instantiate(optionsPrefab, Vector3.zero, Quaternion.identity, GetComponent<RectTransform>());
            RectTransform oRT = optionsMenu.GetComponent<RectTransform>();
            oRT.SetAsLastSibling();
            MoneyStarsPanel.GetComponent<RectTransform>().SetAsLastSibling();
        }
    }

    void CheckUpgradeButtonEnable() {
        if (evolvePriceFloat > GameManager.money) {
            evolveButton.GetComponent<Image>().color = Color.gray;
        }
        else {
            evolveButton.GetComponent<Image>().color = Color.white;
        }
    
        if (desc1.cost > GameManager.money) {
            upgrade1Button.GetComponent<Image>().color = Color.gray;
        }
        else {
            upgrade1Button.GetComponent<Image>().color = Color.white;
        }
        if (desc2.cost > GameManager.money) {
            upgrade2Button.GetComponent<Image>().color = Color.gray;
        }
        else {
            upgrade2Button.GetComponent<Image>().color = Color.white;
        }

    }

    void SelectorButtonPress(categories cat) {
        //CheckUpgradeButtonEnable();
        pulse = 0;
        selectedCat = cat;
        SetInformation(cat);
        //SetLevelDescription(cat);
        //changes text
        title1.text = desc1.title;
        description1.text = desc1.description;
        priceText1.text = "" + (int)desc1.cost;
        title2.text = desc2.title;
        description2.text = desc2.description;
        priceText2.text = "" + (int)desc2.cost;
        //sets icons
        icon1.sprite = desc1.icon;
        icon2.sprite = desc2.icon;

        //sets bar scale
        bar1Current.GetComponent<RectTransform>().anchorMax = new Vector2(1, desc1.percent); 
        if (desc1.unit + desc1.percent > 1) {
            bar1Increment.GetComponent<RectTransform>().anchorMax = new Vector2(1, 1);
            bar1Increment.GetComponent<RectTransform>().anchorMin = new Vector2(0, desc1.percent);
        }
        else {
            bar1Increment.GetComponent<RectTransform>().anchorMax = new Vector2(1, desc1.unit + desc1.percent);
            bar1Increment.GetComponent<RectTransform>().anchorMin = new Vector2(0, desc1.percent);
        }
        bar2Current.GetComponent<RectTransform>().anchorMax = new Vector2(1, desc2.percent);
        if (desc2.unit + desc2.percent > 1) {
            bar2Increment.GetComponent<RectTransform>().anchorMax = new Vector2(1, 1);
            bar2Increment.GetComponent<RectTransform>().anchorMin = new Vector2(0, desc2.percent);
        }
        else {
            bar2Increment.GetComponent<RectTransform>().anchorMax = new Vector2(1, desc2.unit + desc2.percent);
            bar2Increment.GetComponent<RectTransform>().anchorMin = new Vector2(0, desc2.percent);
        }
        //updates bar numbers
        if (cat == categories.traits) {
            number1Current.text = (desc1.current).ToString();
            number1Increment.text = "+" + desc1.increment.ToString();
        }
        else {
            number1Current.text = "";
            number1Increment.text = "";
        }
        number2Current.text = (desc2.current).ToString();
        number2Increment.text = "+" + desc2.increment.ToString();
        CheckUpgradeButtonEnable();
        SoundManager.PlaySfx(0.3f, pwopClip);
    }

    public void SpitterButtonPress() {
        SelectorButtonPress(categories.spitter);
        categoryTitle.text = "SPITTER";
    }

    public void WingsButtonPress() {
        SelectorButtonPress(categories.wings);
        categoryTitle.text = "WINGS";
    }

    public void ShellButtonPress() {
        SelectorButtonPress(categories.shell);
        categoryTitle.text = "SHELL";
    }

    public void TraitsButtonPress() {
        SelectorButtonPress(categories.traits);
        categoryTitle.text = "BODY";

    }

    void SetInformation(categories selCat) {
        CheckUpgradeButtonEnable();
        switch (selCat) {
            //####################################### SPITTER #######################################################################
            case categories.spitter:
                if (!GameManager.wormStats.hasSpit) {
                    upgradeInfoPanel.gameObject.SetActive(false);
                    area1.gameObject.SetActive(false);
                    area2.gameObject.SetActive(false);
                    evolveMenu.gameObject.SetActive(true);
                    evolveIcon.gameObject.SetActive(true);
                    evolveButton.gameObject.SetActive(true);
                    evolveDesc.text = "Spitter. Rain havoc from afar. Tap to spit.";
                    evolvePriceFloat = CostManager.GetSpitterEvolveCost();
                    evolvePrice.text = "" + evolvePriceFloat;
                    evolveIcon.sprite = spitterIcons[0];
                    return;
                }
                evolveMenu.gameObject.SetActive(false);
                upgradeInfoPanel.gameObject.SetActive(true);
                area1.gameObject.SetActive(true);
                area2.gameObject.SetActive(true);
                desc1.percent = GameManager.wormStats.GetSpitterPercent();
                desc1.unit = 0.25f;
                desc1.cost = CostManager.GetSpitterFeatureCost();
                switch (GameManager.wormStats.NextSpitterFeature()) {
                    case SpitterFeatures.explosiveSpit:
                        SetInformation1("ABILITY: Explosive Spit", "Spit damages surrounding enemies, buildings, and terrain.", true, true);
                        desc1.icon = spitterIcons[1];
                        break;
                    case SpitterFeatures.tripleSpit:
                        SetInformation1("ABILITY: Triple Spit", "Spitter shoots 3 shots at a time.", true, true);
                        desc1.icon = spitterIcons[2];
                        break;
                    case SpitterFeatures.acidSpit:
                        SetInformation1("ABILITY: Acid Spit", "Spit applies acid burn to all enemies hit.", true, true);
                        desc1.icon = spitterIcons[3];
                        break;
                    case SpitterFeatures.quintupleSpit:
                        SetInformation1("ABILITY: Quintuple Spit", "Spitter shoots 5 shots at a time.", true, true);
                        desc1.icon = spitterIcons[4];
                        break;
                    case SpitterFeatures.none:
                        SetInformation1("", "Spitter abilities fully evolved!", false, false);
                        break;
                }
                //Area 2
                desc2.title = "Spitter damage";
                desc2.description = "Increase spit damage. While in the air, tap to spit. Increases bite damage.";
                desc2.level = GameManager.wormStats.spitLevel;
                desc2.current = (float)(GameManager.wormStats.startSpitDamage + (float)GameManager.wormStats.spitDamageIncrement * GameManager.wormStats.spitLevel);
                desc2.increment = GameManager.wormStats.spitDamageIncrement;
                desc2.percent = (float)desc2.current / (float)GameManager.wormStats.maxSpitDamage;
                desc2.unit = (float)desc2.increment / (float)GameManager.wormStats.maxSpitDamage;
                desc2.cost = CostManager.GetSpitterLevelCost();
                desc2.icon = spitterIcons[0];
                if (GameManager.wormStats.spitLevel >= GameManager.wormStats.maxSpitLevel) {
                    desc2.description = "Spitter damage is maxed!";
                    upgrade2Button.gameObject.SetActive(false);
                    icon2.gameObject.SetActive(false);
                }
                else {
                    upgrade2Button.gameObject.SetActive(true);
                    icon2.gameObject.SetActive(true);
                    desc2.icon = spitterIcons[0];
                }
                break;
            //####################################### WINGS #######################################################################
            case categories.wings:
                if (!GameManager.wormStats.hasWings) {
                    upgradeInfoPanel.gameObject.SetActive(false);
                    area1.gameObject.SetActive(false);
                    area2.gameObject.SetActive(false);
                    evolveMenu.gameObject.SetActive(true);
                    evolveIcon.gameObject.SetActive(true);
                    evolveButton.gameObject.SetActive(true);
                    evolveDesc.text = "Wings allow you to control the caterpillar in the air and allows flaps, diving, and flying.";
                    evolvePriceFloat = CostManager.GetWingsEvolveCost();
                    evolvePrice.text = "" + evolvePriceFloat;
                    evolveIcon.sprite = wingsIcons[0];
                    return;
                }
                evolveMenu.gameObject.SetActive(false);
                upgradeInfoPanel.gameObject.SetActive(true);
                area1.gameObject.SetActive(true);
                area2.gameObject.SetActive(true);
                desc1.percent = GameManager.wormStats.GetWingsPercent();
                desc1.unit = 0.25f;
                desc1.cost = CostManager.GetWingFeatureCost();
                switch (GameManager.wormStats.NextWingFeature()) {
                    case WingFeatures.flap:
                        SetInformation1("ABILITY: Flap", "When in the air, touch above caterpillar to flap and cover more height.", true, true);
                        desc1.icon = wingsIcons[1];
                        break;
                    case WingFeatures.dive:
                        SetInformation1("ABILITY: Dive", "When in the air, caterpillar can quickly dive into the ground. Damage doubled while diving.", true, true);
                        desc1.icon = wingsIcons[2];
                        break;
                    case WingFeatures.doubleFlap:
                        SetInformation1("ABILITY: Double Flap", "Wings can flap twice in one jump.", true, true);
                        desc1.icon = wingsIcons[3];
                        break;
                    case WingFeatures.butterfly:
                        SetInformation1("ABILITY: Butterfly", "Caterpillar? more like caterfly! 10 flaps and much stronger wings.", true, true);
                        desc1.icon = wingsIcons[4];
                        break;
                    case WingFeatures.none:
                        SetInformation1("", "Wing abilities fully evolved!", false, false);
                        break;
                }
                //area 2
                desc2.title = "Wing strength";
                desc2.description = "Increases wing strength. Gives more control while in the air.";
                desc2.level = GameManager.wormStats.wingsLevel;
                desc2.current = (float)(GameManager.wormStats.startWingStrength + (float)GameManager.wormStats.wingStrengthIncrement * GameManager.wormStats.wingsLevel);
                desc2.increment = GameManager.wormStats.wingStrengthIncrement;
                desc2.percent = (float)desc2.current / (float)GameManager.wormStats.maxWingStrength;
                desc2.unit = (float)desc2.increment / (float)GameManager.wormStats.maxWingStrength;
                desc2.cost = CostManager.GetWingLevelCost();
                desc2.icon = wingsIcons[0];
                if (GameManager.wormStats.wingsLevel >= GameManager.wormStats.maxWingsLevel) {
                    desc2.description = "Wing strength is maxed!";
                    upgrade2Button.gameObject.SetActive(false);
                    icon2.gameObject.SetActive(false);
                }
                else {
                    upgrade2Button.gameObject.SetActive(true);
                    icon2.gameObject.SetActive(true);
                    desc2.icon = wingsIcons[0];
                }
                break;

            //################################### SHELL #############################################################
            case categories.shell:
                if (!GameManager.wormStats.hasArmor) {
                    upgradeInfoPanel.gameObject.SetActive(false);
                    area1.gameObject.SetActive(false);
                    area2.gameObject.SetActive(false);
                    evolveMenu.gameObject.SetActive(true);
                    evolveIcon.gameObject.SetActive(true);
                    evolveButton.gameObject.SetActive(true);
                    evolveDesc.text = "Shell reduces damage and add powerful effects to your body.";
                    evolvePriceFloat = CostManager.GetWingsEvolveCost();
                    evolvePrice.text = "" + evolvePriceFloat;
                    evolveIcon.sprite = shellIcons[0];
                    return;
                }
                evolveMenu.gameObject.SetActive(false);
                upgradeInfoPanel.gameObject.SetActive(true);
                area1.gameObject.SetActive(true);
                area2.gameObject.SetActive(true);
                desc1.percent = GameManager.wormStats.GetShellPercent();
                desc1.unit = 0.25f;
                desc1.cost = CostManager.GetShellFeatureCost();
                switch (GameManager.wormStats.NextShellFeature()) {
                    case ShellFeatures.thorns:
                        SetInformation1("ABILITY: Thorns", "Thorns stab all enemies touching your body", true, true);
                        desc1.icon = shellIcons[1];
                        break;
                    case ShellFeatures.poison:
                        SetInformation1("ABILITY: Acid Skin", "Enemies are burned over time if touched by your body.", true, true);
                        desc1.icon = shellIcons[2];
                        break;
                    case ShellFeatures.reflective:
                        SetInformation1("ABILITY: Reflective", "Space Laser does half damage and attacks reflect off, damaging enemies.", true, true);
                        desc1.icon = shellIcons[3];
                        break;
                    case ShellFeatures.tentacles:
                        SetInformation1("ABILITY: Tentacles", "Tentacles sprout from the caterpillar and pull in nearby enemies.", true, true);
                        desc1.icon = shellIcons[4];
                        break;
                    case ShellFeatures.none:
                        SetInformation1("", "Shell abilities fully evolved!", false, false);
                        break;
                }
                //Area 2
                desc2.title = "Armor";
                desc2.level = GameManager.wormStats.armorLevel;
                desc2.current = (float)(GameManager.wormStats.startDamageReduction + (float)GameManager.wormStats.damageReductionIncrement * GameManager.wormStats.armorLevel);
                desc2.increment = GameManager.wormStats.damageReductionIncrement;
                desc2.percent = (float)desc2.current / (float)GameManager.wormStats.maxDamageReduction;
                desc2.unit = (float)desc2.increment / (float)GameManager.wormStats.maxDamageReduction;
                if (desc2.level == 0) desc2.description = "Increase incoming damage reduction to 12.5%";
                else desc2.description = "Increase incoming damage reduction to " + (desc2.current + desc2.increment) + "%.";
                desc2.cost = CostManager.GetShellLevelCost();

                if (GameManager.wormStats.armorLevel >= GameManager.wormStats.maxArmorLevel) {
                    desc2.description = "Armor is maxed!";
                    upgrade2Button.gameObject.SetActive(false);
                    icon2.gameObject.SetActive(false);
                }
                else {
                    upgrade2Button.gameObject.SetActive(true);
                    icon2.gameObject.SetActive(true);
                    desc2.icon = shellIcons[0];
                }
                break;

            //################################### TRAITS ###########################################################################
            case categories.traits:
                evolveMenu.gameObject.SetActive(false);
                upgradeInfoPanel.gameObject.SetActive(true);
                area1.gameObject.SetActive(true);
                area2.gameObject.SetActive(true);
                desc1.title = "Size";
                desc1.description = "Adds a new segment, increasing length and maximum health";
                desc1.current = (float)(GameManager.wormStats.startHealth + (float)GameManager.wormStats.healthIncrement * GameManager.wormStats.size);
                desc1.increment = GameManager.wormStats.healthIncrement;
                desc1.percent = (float)desc1.current / (float) GameManager.wormStats.maxHealth;
                desc1.unit = (float) desc1.increment / (float) GameManager.wormStats.maxHealth;
                desc1.cost = CostManager.GetSizeCost();

                if (GameManager.wormStats.size >= GameManager.wormStats.maxSize) {
                    desc1.description = "Size is maxed!";
                    upgrade1Button.gameObject.SetActive(false);
                    icon1.gameObject.SetActive(false);
                }
                else {
                    upgrade1Button.gameObject.SetActive(true);
                    icon1.gameObject.SetActive(true);
                    desc1.icon = traitsIcons[0];
                }
                //area 2
                desc2.title = "Dig speed";
                desc2.description = "Increases dig speed and jump height. Increases bite damage.";
                desc2.current = (float)(GameManager.wormStats.startDisplaySpeed + (float)GameManager.wormStats.displaySpeedIncrement * GameManager.wormStats.speed);
                desc2.increment = GameManager.wormStats.displaySpeedIncrement;
                desc2.percent = (float)desc2.current / (float)GameManager.wormStats.maxDisplaySpeed;
                desc2.unit = (float) desc2.increment / (float) GameManager.wormStats.maxDisplaySpeed;
                desc2.cost = CostManager.GetSpeedCost();

                if (GameManager.wormStats.speed >= GameManager.wormStats.maxSpeed) {
                    desc2.description = "Speed is maxed!";
                    upgrade2Button.gameObject.SetActive(false);
                    icon2.gameObject.SetActive(false);
                }
                else {
                    upgrade2Button.gameObject.SetActive(true);
                    icon2.gameObject.SetActive(true);
                    desc2.icon = traitsIcons[1];
                }
                break;
        }
    }

    public void OpenShopButtonPress() {
        GetComponent<GoToTarget>().speed = 5f;
        GetComponent<GoToTarget>().ChangeTarget(new Vector3(0, 1530f, 0));
        GameManager.inGemShop = true;

        SoundManager.PlaySfx(undergroundRumbleClip);
    }

    public void Exit() {
        //Debug.Log("UM exit called");
        Invoke("Die", 0.4f);
    }

    void Die() {
        Destroy(gameObject);
    }

    private void OnDestroy() {
        GameManager.upgradeManager = null;
        GameManager.inGemShop = false;
    }

    public void SetInformation1(string title, string desc, bool iconOn, bool buttonOn) {
        desc1.title = title;
        desc1.description = desc;
        icon1.gameObject.SetActive(iconOn);
        upgrade1Button.gameObject.SetActive(buttonOn);
    }

}


public class UpgradeDescription {
    public string title;
    public string description;

    public float percent;
    public float unit;

    public int level;
    public float current;
    public float increment;

    public Sprite icon;

    public float cost;

    public UpgradeDescription(string title, string description, float max, float start, float increment) {
        this.title = title;
        this.description = description;
        this.increment = increment;
    }

    public UpgradeDescription() {
        this.title = "";
        this.description = "";
        this.percent = 0;
        this.unit = 0;
        this.increment = 0;
    }



}

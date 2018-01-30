using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum enemy {citizen, car, soldier, satellite, helicopter, tank, general};

public class NewEnemyNotification : MonoBehaviour {

    public Sprite citizenSprite;
    public Sprite carSprite;
    public Sprite soldierSprite;
    public Sprite satelliteSprite;
    public Sprite helicopterSprite;
    public Sprite tankSprite;
    public Sprite generalSprite;

    public Text nameText;
    public Text descriptionText;
    public Text damageText;
    public Text healthText;
    public Image enemyImage;

    enemy selectedEnemy;

    GoToTarget GTT;

    public Button continueButton;


	// Use this for initialization
	void Start () {
        WorldManager.buttonManager.pauseButtonEnabled = false;
        descriptionText.fontSize = (int)(descriptionText.fontSize * Util.fontScale);

        GTT = GetComponent<GoToTarget>();
		switch (selectedEnemy) {
            case enemy.citizen:
                nameText.text = "Citizen";
                descriptionText.text = "Harmless caterpillar snacks.";
                damageText.text = "none";
                healthText.text = "low";
                enemyImage.sprite = citizenSprite;
                break;
            case enemy.car:
                nameText.text = "Car";
                descriptionText.text = "Releases 3 citizens on death.";
                damageText.text = "none";
                healthText.text = "medium";
                enemyImage.sprite = carSprite;
                break;
            case enemy.soldier:
                nameText.text = "Soldier";
                descriptionText.text = "They shoot caterpillars.";
                damageText.text = "low";
                healthText.text = "medium";
                enemyImage.sprite = soldierSprite;
                break;
            case enemy.satellite:
                nameText.text = "Satellite";
                descriptionText.text = "Charges up a mega anti-caterpillar space laser. Hide underground to avoid.";
                damageText.text = "very high";
                healthText.text = "high";
                enemyImage.sprite = satelliteSprite;
                break;
            case enemy.helicopter:
                nameText.text = "Helipig";
                descriptionText.text = "Air unit that fires homing missiles. Spit them down!";
                damageText.text = "medium";
                healthText.text = "medium";
                enemyImage.sprite = helicopterSprite;
                break;
            case enemy.tank:
                nameText.text = "Tank";
                descriptionText.text = "It's a tank. Fires slow, but hits hard.";
                damageText.text = "high";
                healthText.text = "high";
                enemyImage.sprite = tankSprite;
                break;
            case enemy.general:
                nameText.text = "General";
                descriptionText.text = "A seasoned military commander that likes to smash insects. EVOLVE 'DIVE'!";
                damageText.text = "very high";
                healthText.text = "very high";
                enemyImage.sprite = generalSprite;
                break;
        }
	}
	
    public void SetEnemy(enemy e) {
        selectedEnemy = e;
    }

    public void ContinueButtonPress() {
        WorldManager.buttonManager.pauseButtonEnabled = true;
        WorldManager.togglePause();
        GTT.ChangeTarget(new Vector3(0, -1000f, 0), false);
        Invoke("Die", 0.5f);
    }

    void Die() {
        Destroy(gameObject);
    }
}

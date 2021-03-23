using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    enum PlayerSelect { Player1, Player2 };
    [SerializeField] PlayerSelect playerSelect; 

    //[SerializeField] Slider superMeterBar;
    [SerializeField] Slider healthBar;
    //[SerializeField] Slider mashingMeterBar;
    [SerializeField] Vector3 startingPosition;
    [SerializeField] int superMeter = 0;
    [SerializeField] bool autoFire = false;

    //public int health;
    //[SerializeField] float shootMashRate = 5;
    //[SerializeField] GameObject superFlash;

    public Character character;


    private bool allowInput = true;
    [SerializeField] bool isFacingRight;
    private string enemyTag;
    private string projectileTag;
    private string enemyProjectileTag;
    private string enemyProximityTag;
    private string proximityTag;

    public bool isBuffering = false;
    //private float chargeTimer;
    float shootMashing = 0;
    float newShootMashing;
    bool isParrying = false;
    int direction;
    int actionCount = 0;

    bool allowShoot = true;

    [SerializeField] bool cpuMode = false;

    float shootPower;

    public float ShootMashing { get { return shootPower; } set { shootMashing = value; } }
    public bool AllowInput { get { return allowInput; } set { allowInput = value; } }
    public int Direction { get { return direction; } }
    public bool IsFacingRight { get { return isFacingRight; } }
    public string EnemyTag { get { return enemyTag; } }
    public string ProjectileTag { get { return projectileTag; } }
    public string EnemyProjectileTag { get { return enemyProjectileTag; } }
    public string EnemyProximityTag { get { return enemyProximityTag; } }
    public string ProximityTag { get { return proximityTag; } }


    public Vector3 StartingPosition { get { return startingPosition; } }
    public int SuperMeter { get { return superMeter; } set { superMeter = value; } }



    private Coroutine chargeCoroutine;
    int chargeLevel;
    bool isCharging = false;
    bool parryCancel = false;

    Coroutine buffer;
    Coroutine charge;

    Coroutine buttonHold = null;

    Animator charAnim;

    float pressedTimer = 0f;

    void Awake()
    {

        Instantiate(character, transform.position, Quaternion.identity, transform);
        character = GetComponentInChildren<Character>();
        charAnim = character.GetComponent<Animator>();

        switch (playerSelect)
        { 
            case PlayerSelect.Player1:

                enemyTag = "Player 2";
                enemyProjectileTag = "P2_Projectile";
                enemyProximityTag = "P2_Proximity";

                character.tag = "Player 1";
                proximityTag = "P1_Proximity";
                projectileTag = "P1_Projectile";

                if (isFacingRight) { direction = 1; }

                break;

            case PlayerSelect.Player2:
                enemyTag = "Player 1";
                enemyProjectileTag = "P1_Projectile";
                enemyProximityTag = "P1_Proximity";

                character.tag = "Player 2";
                proximityTag = "P2_Proximity";
                projectileTag = "P2_Projectile";

                if (!isFacingRight) { direction = -1; }

                break;

            default:
                break;
        }

    }

    void Start()
    {
        if(healthBar != null)
        {
            healthBar.maxValue = character.maxHealth;
            UpdateHealth();
        }
        if (cpuMode)
        {

            StartCoroutine(FireContinouslyCPU());
        }
    }

    private void Update()
    {
        MashingMeter();
        //mashingMeterBar.value = shootMashing;
        CheckInput();

    }

    private void MashingMeter()
    {
        shootMashing = Mathf.MoveTowards(shootMashing, 0, 3f * Time.deltaTime);
    }

    IEnumerator FireContinouslyCPU()
    {
        yield return new WaitForSeconds(3f); // pause before starting
        print("wait until loop");
        float pauseFactor = 1f;
        int regularShootCount = 0;
        print("action count is " + actionCount);
        if(actionCount >= 15)
        {
            actionCount = 0;
            pauseFactor = 3f;
        }
        while (true)
        {
            print("starting loop");

            actionCount++;

            var newNumber = Random.Range(0, 500);
            //switch (newNumber){
            //    case > 0):
            //        Shoot();
            //        break;
            //    case 2:
            //        ShootSpecial();
            //        break;
            //    case 3:
            //        ShootSuper();
            //        break;
            //}
            pauseFactor = pauseFactor - 0.2f;
            if(pauseFactor < 0f)
            {
                pauseFactor = 0f;
            }

            print(pauseFactor);

            if (newNumber >= 1 && newNumber <= 299)
            {
                Shoot();
                regularShootCount++;
                print("count is " + regularShootCount);

                if (regularShootCount > 10)
                {
                    regularShootCount = 0;
                    pauseFactor = 1f;
                }
            }
            else if(newNumber >= 300 && newNumber <= 498 && actionCount > 3)
            {
                if(character.currentHealth > 80)
                {
                    ShootSpecial();
                }
            }
            else if(newNumber >= 499 && actionCount > 14)
            {
                ShootSuper();
            }
            print("end of loop");

            yield return new WaitForSeconds(pauseFactor);
        }

    }

    public void ResetRound()
    {
        character.ResetPosition();
    }

   



    private void CheckInput()
    {
        switch (playerSelect)
        {
            case PlayerSelect.Player1:

                if (Input.GetKeyDown(KeyCode.S))
                {      
                    Parry();
                }
                if (Input.GetKeyDown(KeyCode.D))
                {
                    Shoot();
                }
                if (Input.GetKeyUp(KeyCode.D))
                {
                    //Shoot();
                    //StopAllCoroutines();
                }
                if (Input.GetKeyDown(KeyCode.A))
                {
                    ShootSpecial();
                }
                if (Input.GetKeyDown(KeyCode.W))
                {
                    ShootSuper();
                }
                break;
            case PlayerSelect.Player2:
                if (Input.GetKeyDown(KeyCode.K))
                {
                    Parry();
                }
                if (Input.GetKeyDown(KeyCode.L))
                {
                    Shoot();
                    //buffer = StartCoroutine(InputBuffer());

                }
                if (Input.GetKeyUp(KeyCode.L))
                {
                    //Shoot();
                    //StopAllCoroutines();
                }
                if (Input.GetKeyDown(KeyCode.J))
                {
                    ShootSpecial();
                }
                if (Input.GetKeyDown(KeyCode.I))
                {
                    ShootSuper();
                }
                break;
        }
    }

    private void UpdateMashingMeter()
    {
        //mashingMeterBar.value = shootMashing;
    }

    public void UpdateSuperMeter()
    {
        //superMeterBar.value = character.superMeter;
    }

    public void UpdateHealth()
    {
        if (healthBar == null) return;
        healthBar.value = character.currentHealth;
    }

    public void SuperFlash(bool isActive)
    {
        //if (isActive)
        //{
        //    superFlash.gameObject.SetActive(true);
        //}
        //else
        //{
        //    superFlash.gameObject.SetActive(false);
        //}
    }

    public void Parry()
    {
        if(allowInput == true)
        {
            character.StartParry();
        }
    }

    public void Shoot()
    {
        if(allowInput && allowShoot)
        {
            ShootingPower();
            character.StartShootAnimation();
        }
    }

    public void ShootingPower()
    {
        if(shootMashing < 2f)
        {
            shootMashing += .75f;
        }
        else if(shootMashing >= 2 && shootMashing < 4f )
        {
            shootMashing += .55f;
        }
        else if(shootMashing >= 4)
        {
            shootMashing += .50f;
        }

        if (shootMashing >= 5)
        {
            shootMashing = 5;
        }
        shootPower = shootMashing / 5; 
    }

    public void ShootSpecial()
    {
        if(allowInput == true)
        {
            character.StartShootAnimationEx(2);
        }
    }
    public void ShootSuper()
    {
        if(allowInput == true)
        {
            character.StartShootAnimationSuper();
        }
    }

    public void ButtonDown()
    {
        //Parry();
        Shoot();
        buffer = StartCoroutine(InputBuffer());
    }

    public void ButtonUp()
    {
        //StopAllCoroutines();
        if(buffer != null) 
        {
            StopCoroutine(buffer);
        }
        if(charge != null)
        {
            StopCoroutine(charge);
        }

        if (isCharging)
        {
            isCharging = false;
            charAnim.SetBool("IsCharging", false);
            character.ShootChargeCalculator(chargeLevel);
        }
    }

    private IEnumerator InputBuffer()
    {
        pressedTimer = 0f;
        while (pressedTimer <= 0.4f)
        {
            pressedTimer += Time.deltaTime;
            yield return null;
        }
        isCharging = true;
        chargeLevel = 0;
        charge = StartCoroutine(Charge());
    }

    public IEnumerator Charge()
    {
        charAnim.SetBool("IsCharging", true);
        var chargeRate = 0.25f;
        bool initialCharge = false;
        while (true)
        {
            if(initialCharge == false)
            {
                yield return new WaitForSeconds(0.10f);
                ChargeFactor();
                initialCharge = true;
            }
            else
            {
                yield return new WaitForSeconds(chargeRate);
                ChargeFactor();
                chargeRate -= 0.02f;
            }

            //yield return new WaitForSeconds(chargeRate);
            //ChargeFactor();
            //chargeRate -= 0.05f;

            if (chargeRate < 0.15f)
            {
                chargeRate = 0.15f;
            }
        }
    }

    private void ChargeFactor()
    {
        chargeLevel++;
        character.ChargeFX();
        if (chargeLevel >= 5)
        {
            chargeLevel = 5;
        }
    }

    public void UpgradeWeaponSpeed()
    {
        character.UpgradeWeapon();
    }

    public void StartSilence()
    {
        StartCoroutine(Silence());
    }

    public IEnumerator Silence()
    {
        allowShoot = false;
        yield return new WaitForSeconds(3f);
        allowShoot = true;
    }
}

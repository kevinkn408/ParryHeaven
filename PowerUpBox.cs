using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerUpBox : MonoBehaviour
{
    enum PowerUpType { IncreaseSpeed, RecoveryDown, Silence};
    [SerializeField] PowerUpType typeSelect;
    enum PlayerSelect { Player1, Player2};
    [SerializeField] PlayerSelect playerSelect;
    Player player;
    Character character;
    string playerTag;
    Text text;
    string typeName;
    int randomNumGen;
    public delegate void SetType();
    SetType setType;

    // Start is called before the first frame update
    private void Awake()
    {
        SelectPlayer();
    }

    private void OnEnable()
    {
        SelectType();
    }

    private void SelectPlayer()
    {
        switch (playerSelect)
        {
            case PlayerSelect.Player1:
                playerTag = ("Player 1");
                break;
            case PlayerSelect.Player2:
                playerTag = ("Player 2");
                break;
        }
    }

    private void SelectType()
    {
        player = GameObject.FindWithTag(playerTag).GetComponent<Player>();
        character = player.GetComponentInChildren<Character>();

        //switch (typeSelect)
        //{
        //    case PowerUpType.IncreaseSpeed:
        //        setType = IncreaseSpeed_;
        //        typeName = "Speed Up";
        //        break;
        //    case PowerUpType.RecoveryDown:
        //        setType = DecreaseRecovery_;
        //        typeName = "Recovery Down";
        //        break;
        //}
        randomNumGen = Random.Range(1, 100);

        if (randomNumGen <= 50)
        {
            setType = IncreaseSpeed_;
            typeName = "Speed Up";
        }
        else if(randomNumGen > 50 && randomNumGen <= 85)
        {
            setType = DecreaseRecovery_;
            typeName = "Recovery Down";
        }
        else if (randomNumGen > 85 && randomNumGen <= 95)
        {
            setType = Super_;
            typeName = "SUPER!!";
        }
        else if (randomNumGen > 95)
        {
            setType = SilenceEnemy_;
            typeName = "SILENCE :x";
        }

        UpdateText(typeName);
    }

   

    public void OnSelect()
    {
        setType();
        GetComponentInParent<ActiveSwitch>().OnPress();
    }

    private void UpdateText(string currentType)
    {
        text = GetComponentInChildren<Text>();
        text.text = currentType;
    }

    //moves
    private void IncreaseSpeed_()
    {
        character.FireballSpeed += 1f;
    }
    private void DecreaseRecovery_()
    {
        character.FireballAnimationSpeed += 0.05f;
    }
    private void Super_()
    {
        character.StartShootAnimationSuper();
    }
    private void SilenceEnemy_()
    {
        Player enemy = GameObject.FindWithTag(player.EnemyTag).GetComponent<Player>();
        enemy.StartSilence();

        //Coroutine startSilence = StartCoroutine(SilenceEnemy());
    }

    //private IEnumerator SilenceEnemy()
    //{
    //    var duration = 3f;
    //    Player enemy = GameObject.FindWithTag(player.EnemyTag).GetComponent<Player>();
    //    enemy.AllowInput = false;
    //    yield return new WaitForSecondsRealtime(duration);
    //    enemy.AllowInput = true;
    //}
}

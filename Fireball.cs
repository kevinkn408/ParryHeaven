using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    [SerializeField] Transform deathFXposition;
    [SerializeField] GameObject deathFx;
    Projectile projectile;
    Player player;
    [SerializeField] bool canPierce;
    Coroutine freezer = null;
    Vector3 freezPos;
    bool canBeFrozen;
    GameManager gameManager;

    bool hasHit = false;

    private void Start()
    {
        canBeFrozen = false;
        player = GetComponentInParent<Player>();
        projectile = GetComponentInParent<Projectile>();
        gameManager = FindObjectOfType<GameManager>();
    }

    private void DisableAndReturnToPool()
    {
        print("something");
        hasHit = true;
        //playVFX();
        if (hasHit)
        {
            hasHit = false;
            gameObject.SetActive(false);
            projectile.SubtractChildren();
        }

    }

    private void PlayVFX()
    {
        if (!player.IsFacingRight)
        {
            Instantiate(deathFx, deathFXposition.position, transform.localRotation);
        }
        else
        {
            Instantiate(deathFx, deathFXposition.position, Quaternion.Euler(0, 0, 180));
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!projectile.IsIndependent)
        {
            MultiplayerLogic(other);
        }
        else
        {
            SinglePlayerLogic(other);
        }
        if (other.CompareTag("Shredder"))
        {
            DisableAndReturnToPool();
        }
    }

    private void MultiplayerLogic(Collider2D other)
    {
        if (other.CompareTag(player.EnemyTag))
        {
            Character character = other.gameObject.GetComponent<Character>();
            PlayVFX();
            HurtPlayer(other, character);
        }
        if (other.CompareTag(player.EnemyProjectileTag))
        {
            gameManager.StartGameFreeze(0.05f);
            if (!canPierce)
            {
                PlayVFX();
                DisableAndReturnToPool();
            }
            else return;
        }
        if (other.CompareTag(player.EnemyProximityTag))
        {
            projectile.InFreezeRange = true;
            canBeFrozen = true;
            if (projectile.gameObject.activeInHierarchy == true)
            {
                projectile.FreezeProjectiles();

            }
        }
    }

    private void SinglePlayerLogic(Collider2D other)
    {
        if (other.CompareTag("Player 1"))
        {
            Character character = other.gameObject.GetComponent<Character>();
            HurtPlayer(other, character);
        }
        if (other.CompareTag("P1_Projectile"))
        {
            if (!canPierce)
            {
                DisableAndReturnToPool();
            }
            else return;
        }
        if (other.CompareTag("P1_Proximity"))
        {
            projectile.InFreezeRange = true;
            canBeFrozen = true;
            if(projectile.gameObject.activeInHierarchy == true)
            {
                projectile.FreezeProjectiles();

            }
        }
    }

    private void HurtPlayer(Collider2D other, Character character)
    {
        character.Hurt();

        DisableAndReturnToPool();
        //player.SuperMeter += 3;
    }


}
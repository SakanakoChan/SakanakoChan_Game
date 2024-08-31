using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.U2D;

public class Arrow_Controller : MonoBehaviour
{
    [SerializeField] private string targetLayerName = "Player";
    //[SerializeField] private int damage;

    //private float xVelocity;
    private Vector2 flySpeed;
    private Rigidbody2D rb;
    private CharacterStats archerStats;

    [SerializeField] private bool canMove = true;
    [SerializeField] private bool flipped = false;

    private bool isStuck = false;


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (canMove)
        {
            rb.velocity = flySpeed;
            transform.right = rb.velocity;
        }

        //make arrow transparent and destroy it in 3 ~ 5 seconds after stuck into object
        if (isStuck)
        {
            Invoke("BecomeTransparentAndDestroyArrow", Random.Range(3, 5));
        }

        //if the arrow flies too far and donsen't hit any targets, auto destroy it
        if (Vector2.Distance(transform.position, archerStats.transform.position) > 25)
        {
            Invoke("BecomeTransparentAndDestroyArrow", 0);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //if the arrow hits player
        if (collision.gameObject.layer == LayerMask.NameToLayer(targetLayerName))
        {
            //collision.GetComponent<CharacterStats>()?.TakeDamage(damage, transform, collision.transform, false); ;
            archerStats.DoDamge(collision.GetComponent<CharacterStats>());

            StuckIntoCollidedObject(collision);
        }
        //if the arrow hits ground
        else if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            StuckIntoCollidedObject(collision);
        }
    }

    public void SetupArrow(Vector2 _speed, CharacterStats _archerStats)
    {
        flySpeed = _speed;

        //if the arrow is flying to the left side.
        //needs to flip it first
        if (flySpeed.x < 0)
        {
            transform.Rotate(0, 180, 0);
        }

        archerStats = _archerStats;
    }

    private void StuckIntoCollidedObject(Collider2D collision)
    {
        //turn off the trail effect
        GetComponentInChildren<ParticleSystem>()?.Stop();

        //to prevent the arrow from damaging player multiple times after getting stuck into player
        GetComponent<CapsuleCollider2D>().enabled = false;

        //stuck into object
        canMove = false;
        rb.isKinematic = true;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        transform.parent = collision.transform;

        //destroy this arrow in several seconds after stuck into object
        isStuck = true;
    }

    private void BecomeTransparentAndDestroyArrow()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();

        sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, sr.color.a - (5 * Time.deltaTime));

        if (sr.color.a <= 0)
        {
            Destroy(gameObject);
        }
    }

    public void FlipArrow()
    {
        if (flipped)
        {
            return;
        }

        //flip the arrow
        flySpeed.x *= -1;
        flySpeed.y *= -1;
        transform.Rotate(0, 180, 0);
        flipped = true;

        //the arrow now will attack enemy
        targetLayerName = "Enemy";
    }
}

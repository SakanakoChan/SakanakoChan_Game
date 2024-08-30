using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.U2D;

public class Arrow_Controller : MonoBehaviour
{
    [SerializeField] private string targetLayerName = "Player";
    [SerializeField] private int damage;

    [SerializeField] private float xVelocity;
    [SerializeField] private Rigidbody2D rb;

    [SerializeField] private bool canMove = true;
    [SerializeField] private bool flipped = false;

    private bool isStuck = false;

    private void Update()
    {
        if (canMove)
        {
            rb.velocity = new Vector2(xVelocity, rb.velocity.y);
        }

        //make arrow transparent and destroy it in 3 ~ 5 seconds after stuck into object
        if (isStuck)
        {
            Invoke("BecomeTransparentAndDestroyArrow", Random.Range(3, 5));
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //if the arrow hits player
        if (collision.gameObject.layer == LayerMask.NameToLayer(targetLayerName))
        {
            collision.GetComponent<CharacterStats>()?.TakeDamage(damage, transform, collision.transform, false); ;

            StuckIntoCollidedObject(collision);
        }
        //if the arrow hits ground
        else if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            StuckIntoCollidedObject(collision);
        }
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
        xVelocity *= -1;
        transform.Rotate(0, 180, 0);
        flipped = true;

        //the arrow now will attack enemy
        targetLayerName = "Enemy";
    }
}

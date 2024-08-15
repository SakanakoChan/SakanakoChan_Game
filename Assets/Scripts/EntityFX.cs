using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class EntityFX : MonoBehaviour
{
    private SpriteRenderer sr;

    [Header("Flash FX")]
    [SerializeField] private float flashDuration;
    [SerializeField] private Material hitMaterial;
    private Material originalMaterial;

    [Header("Ailment Colors")]
    [SerializeField] private Color[] igniteColor;
    [SerializeField] private Color chillColor;
    [SerializeField] private Color[] shockColor;

    private bool canApplyAilmentColor;

    [Header("Ailment Particles")]
    [SerializeField] private ParticleSystem igniteFX;
    [SerializeField] private ParticleSystem chillFX;
    [SerializeField] private ParticleSystem shockFX;

    [Header("Hit FX")]
    [SerializeField] private GameObject hitFXPrefab;
    [SerializeField] private GameObject critHitFXPrefab;

    private void Awake()
    {
        sr = GetComponentInChildren<SpriteRenderer>();
    }

    private void Start()
    {
        originalMaterial = sr.material;
    }

    private IEnumerator FlashFX()
    {
        canApplyAilmentColor = false;

        sr.material = hitMaterial;
        Color originalColor = sr.color;
        sr.color = Color.white;

        yield return new WaitForSeconds(flashDuration);

        sr.color = originalColor;
        sr.material = originalMaterial;

        canApplyAilmentColor = true;
    }

    private void RedColorBlink()
    {
        if (sr.color != Color.white)
        {
            sr.color = Color.white;
        }
        else
        {
            sr.color = Color.red;
        }
    }

    private void CancelColorChange()
    {
        CancelInvoke();

        sr.color = Color.white;

        igniteFX.Stop();
        chillFX.Stop();
        shockFX.Stop();
    }

    public void MakeEntityTransparent(bool _transparent)
    {
        if (_transparent)
        {
            sr.color = Color.clear;
        }
        else
        {
            sr.color = Color.white;
        }
    }

    #region Ailment FX
    public void EnableIgniteFXForTime(float _seconds)
    {
        igniteFX.Play();
        InvokeRepeating("IgniteColorFX", 0, 0.3f);
        Invoke("CancelColorChange", _seconds);
    }

    private void IgniteColorFX()
    {
        if (sr.color != igniteColor[0])
        {
            sr.color = igniteColor[0];
        }
        else
        {
            sr.color = igniteColor[1];
        }
    }

    public void EnableChillFXForTime(float _seconds)
    {
        chillFX.Play();
        ChillColorFX();
        Invoke("CancelColorChange", _seconds);
    }

    private void ChillColorFX()
    {
        if (sr.color != chillColor)
        {
            sr.color = chillColor;
        }
    }

    public void EnableShockFXForTime(float _seconds)
    {
        shockFX.Play();
        InvokeRepeating("ShockColorFX", 0, 0.3f);
        Invoke("CancelColorChange", _seconds);
    }

    private void ShockColorFX()
    {
        if (sr.color != shockColor[0])
        {
            sr.color = shockColor[0];
        }
        else
        {
            sr.color = shockColor[1];
        }
    }

    //To make sure entity is first colored by Hit ColorFX then Ailment ColorFX
    #region AilmentColorFX_Coroutine
    public IEnumerator EnableIgniteFXForTime_Coroutine(float _seconds)
    {
        yield return new WaitUntil(() => canApplyAilmentColor == true);
        EnableIgniteFXForTime(_seconds);

        //Debug.Log("Ignite ColorFX called");
    }

    public IEnumerator EnableChillFXForTime_Coroutine(float _seconds)
    {
        yield return new WaitUntil(() => canApplyAilmentColor == true);
        EnableChillFXForTime(_seconds);

        //Debug.Log("Chill ColorFX called");
    }

    public IEnumerator EnableShockFXForTime_Coroutine(float _seconds)
    {
        yield return new WaitUntil(() => canApplyAilmentColor == true);
        EnableShockFXForTime(_seconds);

        //Debug.Log("Shock ColorFX called");
    }
    #endregion

    #endregion

    public void CreateHitFX(Transform _targetTransform, bool _canCrit)
    {
        float zRotation = Random.Range(-90, 90);
        //randomly give some position offset to hit fx every time
        float xOffset = Random.Range(-0.5f, 0.5f);
        float yOffset = Random.Range(-0.5f, 0.5f);

        Vector3 generationPosition = new Vector3(_targetTransform.position.x + xOffset, _targetTransform.position.y + yOffset);
        Vector3 generationRotation = new Vector3(0, 0, zRotation);

        GameObject prefab = hitFXPrefab;

        if (_canCrit)
        {
            prefab = critHitFXPrefab;

            zRotation = Random.Range(-30, 30);

            //yRotation controls the crit hit fx direction according to entity facing direction
            float yRotation = 0;
            if (GetComponent<Entity>().facingDirection == -1)
            {
                yRotation = 180;
            }

            generationRotation = new Vector3(0, yRotation, zRotation);
        }

        GameObject newHitFX = Instantiate(prefab, generationPosition, Quaternion.identity);

        newHitFX.transform.Rotate(generationRotation);

        Destroy(newHitFX, 0.5f);
    }
}

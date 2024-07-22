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
        if(sr.color != Color.white)
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
    }

    #region Ailment Color FX
    public void EnableIgniteFXForTime(float _seconds)
    {
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

        Debug.Log("Ignite ColorFX called");
    }

    public IEnumerator EnableChillFXForTime_Coroutine(float _seconds)
    {
        yield return new WaitUntil(() => canApplyAilmentColor == true);
        EnableChillFXForTime(_seconds);

        Debug.Log("Chill ColorFX called");
    }

    public IEnumerator EnableShockFXForTime_Coroutine(float _seconds)
    {
        yield return new WaitUntil(() => canApplyAilmentColor == true);
        EnableShockFXForTime(_seconds);

        Debug.Log("Shock ColorFX called");
    }
    #endregion

    #endregion
}

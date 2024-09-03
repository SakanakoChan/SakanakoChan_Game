using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager instance;

    public CinemachineVirtualCamera cm;
    public float defaultCameraLensSize;
    public float defaultCameraYPosition;

    private Player player;
    private CinemachineFramingTransposer ft;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        ft = cm.GetCinemachineComponent<CinemachineFramingTransposer>();
    }

    private void Start()
    {
        player = PlayerManager.instance.player;
    }


    private void Update()
    {
        //if player is on downable platform, camera lens size will increase
        //meanwhile camera y position will decrease
        if (player.isOnPlatform)
        {
            //m means mirroring the default unity camera
            if (cm.m_Lens.OrthographicSize < 14)
            {
                cm.m_Lens.OrthographicSize = Mathf.Lerp(cm.m_Lens.OrthographicSize, 14, 2 * Time.deltaTime);
            }

            if (ft.m_ScreenY > 0.5f)
            {
                ft.m_ScreenY = Mathf.Lerp(ft.m_ScreenY, 0.5f, 3 * Time.deltaTime);
            }
        }
        //vice versa
        else
        {
            if (cm.m_Lens.OrthographicSize > defaultCameraLensSize)
            {
                cm.m_Lens.OrthographicSize = Mathf.Lerp(cm.m_Lens.OrthographicSize, 10, 2 * Time.deltaTime);
            }

            if (ft.m_ScreenY < defaultCameraYPosition)
            {
                ft.m_ScreenY = Mathf.Lerp(ft.m_ScreenY, defaultCameraYPosition, 3 * Time.deltaTime);
            }
        }
    }

}

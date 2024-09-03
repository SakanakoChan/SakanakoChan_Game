using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager instance;

    public CinemachineVirtualCamera cm;
    public int cameraDefaultLensSize;

    private Player player;


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
    }

    private void Start()
    {
        player = PlayerManager.instance.player;
    }


    private void Update()
    {
        if (player.isOnPlatform)
        {
            //m means mirroring the default unity camera
            if (cm.m_Lens.OrthographicSize < 14)
            {
                //cm.m_Lens.OrthographicSize += 0.01f;
                cm.m_Lens.OrthographicSize = Mathf.Lerp(cm.m_Lens.OrthographicSize, 14, Time.deltaTime);
            }

            if (cm.GetCinemachineComponent<CinemachineFramingTransposer>()?.m_ScreenY > 0.5f)
            {
                cm.GetCinemachineComponent<CinemachineFramingTransposer>().m_ScreenY -= 0.01f;
            }
        }
        else
        {
            if (cm.m_Lens.OrthographicSize > cameraDefaultLensSize)
            {
                //cm.m_Lens.OrthographicSize -= 0.01f;
                cm.m_Lens.OrthographicSize = Mathf.Lerp(cm.m_Lens.OrthographicSize, 10, Time.deltaTime);
            }

            if (cm.GetCinemachineComponent<CinemachineFramingTransposer>()?.m_ScreenY < 0.65f)
            {
                cm.GetCinemachineComponent<CinemachineFramingTransposer>().m_ScreenY += 0.01f;
            }
        }
    }

}

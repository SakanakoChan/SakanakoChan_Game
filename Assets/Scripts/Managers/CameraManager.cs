using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager instance;

    public CinemachineVirtualCamera cm;

    [Header("Camera Lens Info")]
    public float defaultCameraLensSize;
    public float targetCameraLensSize;
    public float cameraLensSizeChangeSpeed;

    [Header("Camera Screen Y Position Info")]
    public float defaultCameraYPosition;
    public float targetCameraYPosition;
    public float cameraYPositionChangeSpeed;

    [Header("Camera Screen X Position Info")]
    public float defaultCameraXPosition;
    public float targetCameraXPositionOffset;
    public float cameraXPositionChangeSpeed;

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
        CameraMovementOnDownablePlatform();

        CameraMovementOnPit();
    }

    private void CameraMovementOnPit()
    {
        //screen X in camera is opossite to player facing direction
        //meaning increase of screen X value will make camera move to left instead
        float targetXPosition = defaultCameraXPosition + targetCameraXPositionOffset * (-player.facingDirection);

        if (player.isFacingPit)
        {
            //m means mirroring the default unity camera
            if (cm.m_Lens.OrthographicSize < targetCameraLensSize)
            {
                cm.m_Lens.OrthographicSize = Mathf.Lerp(cm.m_Lens.OrthographicSize, targetCameraLensSize, cameraLensSizeChangeSpeed * Time.deltaTime);
            }

            if (ft.m_ScreenY > targetCameraYPosition)
            {
                ft.m_ScreenY = Mathf.Lerp(ft.m_ScreenY, targetCameraYPosition - 0.2f, cameraYPositionChangeSpeed * Time.deltaTime);
            }
            
            //if player is facing left when facing pit, camera will move to left, vice versa
            ft.m_ScreenX = Mathf.Lerp(ft.m_ScreenX, targetXPosition, cameraXPositionChangeSpeed * Time.deltaTime);
        }
        else
        {
            if (cm.m_Lens.OrthographicSize > defaultCameraLensSize)
            {
                cm.m_Lens.OrthographicSize = Mathf.Lerp(cm.m_Lens.OrthographicSize, defaultCameraLensSize, cameraLensSizeChangeSpeed * Time.deltaTime);
            }

            if (ft.m_ScreenY < defaultCameraYPosition)
            {
                ft.m_ScreenY = Mathf.Lerp(ft.m_ScreenY, defaultCameraYPosition, cameraYPositionChangeSpeed * Time.deltaTime);
            }

            ft.m_ScreenX = Mathf.Lerp(ft.m_ScreenX, defaultCameraXPosition, cameraXPositionChangeSpeed * Time.deltaTime);
        }
    }


    private void CameraMovementOnDownablePlatform()
    {
        //if player is on downable platform, camera lens size will increase
        //meanwhile camera y position will decrease
        if (player.isOnPlatform)
        {
            //m means mirroring the default unity camera
            if (cm.m_Lens.OrthographicSize < targetCameraLensSize)
            {
                cm.m_Lens.OrthographicSize = Mathf.Lerp(cm.m_Lens.OrthographicSize, targetCameraLensSize, cameraLensSizeChangeSpeed * Time.deltaTime);
            }

            if (ft.m_ScreenY > targetCameraYPosition)
            {
                ft.m_ScreenY = Mathf.Lerp(ft.m_ScreenY, targetCameraYPosition, cameraYPositionChangeSpeed * Time.deltaTime);
            }
        }
        //vice versa
        else
        {
            if (cm.m_Lens.OrthographicSize > defaultCameraLensSize)
            {
                cm.m_Lens.OrthographicSize = Mathf.Lerp(cm.m_Lens.OrthographicSize, defaultCameraLensSize, cameraLensSizeChangeSpeed * Time.deltaTime);
            }

            if (ft.m_ScreenY < defaultCameraYPosition)
            {
                ft.m_ScreenY = Mathf.Lerp(ft.m_ScreenY, defaultCameraYPosition, cameraYPositionChangeSpeed * Time.deltaTime);
            }
        }
    }
}

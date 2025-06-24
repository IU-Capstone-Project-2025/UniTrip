using UnityEngine;
using Cinemachine;

public class CameraSwitcher : MonoBehaviour
{
    public CinemachineVirtualCamera mainVCam;
    public CinemachineVirtualCamera dialogueVCam;

    public void SwitchToDialogueCam()
    {
        mainVCam.Priority = 0;
        dialogueVCam.Priority = 10;
    }

    public void ReturnToMainCam()
    {
        dialogueVCam.Priority = 0;
        mainVCam.Priority = 10;
    }
}
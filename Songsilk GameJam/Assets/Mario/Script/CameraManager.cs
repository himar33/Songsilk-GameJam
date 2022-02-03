using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Cinemachine.Editor;

public class CameraManager : MonoBehaviour
{

    [SerializeField] private GameObject HorizontalCam;
    [SerializeField] private GameObject VerticalCam;

    [SerializeField] private GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        HorizontalCam.GetComponent<CinemachineVirtualCamera>().Follow = player.transform;
        VerticalCam.GetComponent<CinemachineVirtualCamera>().Follow = player.transform;
    }

 
}

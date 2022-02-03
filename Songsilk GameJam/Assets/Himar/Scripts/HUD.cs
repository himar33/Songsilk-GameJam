using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUD : MonoBehaviour
{
    public GameObject hudText;
    public void OpenMessagePanel(Vector3 position)
    {
        position.y += 3;
        hudText.transform.position = position;
        hudText.SetActive(true);
    }
    public void CloseMessagePanel()
    {
        hudText.SetActive(false);
    }
}

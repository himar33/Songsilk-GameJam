using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{

    public Transform player = null;

    [Header("Separacion")]
    public Vector3 offset;
    public float smooth = 0.01f;
    private Vector3 newPos;
    
    private void LateUpdate()
    {
        /*cameraPos.position = new Vector3(player.position.x, player.position.y + y, player.position.z - z);
        cameraPos.eulerAngles = new Vector3(gameObject.transform.eulerAngles.x + angle, 0, 0);
        transform.position = Vector3.Lerp(gameObject.transform.position, cameraPos.transform.position, 0.5f);
        transform.eulerAngles = Vector3.Lerp(gameObject.transform.eulerAngles, cameraPos.transform.eulerAngles, 0.5f);*/

        switch (player.gameObject.GetComponent<characterMove>().state)
        {
            case characterMove.State.MOVE:
            case characterMove.State.UP:

                Vector3 target = player.position + offset;
                Vector3 movement = Vector3.Lerp(transform.position, target, smooth);

                transform.position = movement;
                break;
            case characterMove.State.TP:
                gameObject.SetActive(false);
                transform.position = player.gameObject.GetComponent<characterMove>().GetSpawnPos().position;
                gameObject.SetActive(true);

                player.gameObject.GetComponent<characterMove>().state = characterMove.State.MOVE;
                break;
            default:
                break;
        }
        

    }
    
    
}


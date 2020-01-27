using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public Player player;
    Vector3 offset;

    // Start is called before the first frame update
    void Start()
    {
       // player = FindObjectOfType<Player>();
        offset = transform.position - player.transform.position;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        var playerTransform = player.transform;
        transform.position = playerTransform.position + offset;

        //transform.rotation = Quaternion.LookRotation(navMeshAgent.velocity.normalized);
        /* player

         transform.rotation.SetR
         transform.rotation.z = playerTrabsform.rotation.z;*/
        //transform.rotation = playerTrabsform.rotation.x;
    }
}

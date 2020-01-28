using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Player : MonoBehaviour
{
    public Cursor cursor;
    public Shot shot;
    public Transform gunBarrel;
    public float ShotDelay;

    NavMeshAgent navMeshAgent;
    public float moveSpeed;

    float m_currentShotDelay;

    // Start is called before the first frame update
    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.updateRotation = false;
        m_currentShotDelay = ShotDelay;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 forward = cursor.transform.position - transform.position;
        transform.rotation = Quaternion.LookRotation(new Vector3(forward.x, 0, forward.z));

        Vector3 dir = Vector3.zero;
        if (Input.GetKey(KeyCode.LeftArrow))
            dir.z = 1.0f;
        if (Input.GetKey(KeyCode.RightArrow))
            dir.z = -1.0f;
        if (Input.GetKey(KeyCode.UpArrow))
            dir.x = 1.0f;
        if (Input.GetKey(KeyCode.DownArrow))
            dir.x = -1.0f;
        navMeshAgent.velocity = dir.normalized * moveSpeed;

        /*if (Input.GetMouseButtonDown(0))
        {
            Shot();
        }*/

        //Autshot
        m_currentShotDelay -= Time.deltaTime;
        if (m_currentShotDelay < 0)
        {
            FindClosetEnemy();
            //Shot();
            m_currentShotDelay = ShotDelay;
        }


        if (navMeshAgent.velocity.magnitude > 0)
        {
           // Debug.Log($"VElocity = {navMeshAgent.velocity.magnitude}");
        }
    }

    void Shot()
    {
        var from = gunBarrel.position;
        var target = cursor.transform.position;
        var to = new Vector3(target.x, from.y, target.z);

        var direction = (to - from).normalized;

        RaycastHit hit;
        if (Physics.Raycast(from, to - from, out hit, 100))
        {
            to = new Vector3(hit.point.x, from.y, hit.point.z);
            if (hit.transform != null)
            {
                var zombie = hit.transform.GetComponent<Zombie>();
                if (zombie != null)
                    zombie.Kill();
            }
        }
        else
        {
            to = from + direction * 100;
        }
        shot.Show(from, to);
    }

    void FindClosetEnemy()
    {
        Zombie[] enemies = FindObjectsOfType<Zombie>();
        if (enemies.Length > 0)
        {
            var currentPosition = transform.position;
            Zombie target = null;
            float closetDist = float.MaxValue;//Vector3.Distance(target.transform.position, currentPosition);
            foreach (var enemy in enemies)
            //for (int i = 1; i < enemies.Length; ++i)
            {
                //var enemy = enemies[i];
                if (!enemy.dead)
                {
                    float dist = Vector3.Distance(enemy.transform.position, currentPosition);
                    if (closetDist > dist)
                    {
                        closetDist = dist;
                        target = enemy;
                    }
                }
            }
            if (target != null)
            {
                ShotTarget(target);
            }
        }
    }
    void ShotTarget(Zombie a_target)
    {
        /*var from = gunBarrel.position;
        var target = a_target.transform.position;
        shot.Show(from, target);
        a_target.Kill();*/
        var from = gunBarrel.position;
        var target = a_target.transform.position;


        //Check for some objects between target and player.
        RaycastHit hit;
        if (Physics.Raycast(from, target - from, out hit, 100))
        {
            target = new Vector3(hit.point.x, from.y, hit.point.z);
            if (hit.transform != null)
            {
                var zombie = hit.transform.GetComponent<Zombie>();
                if (zombie != null)
                    zombie.Kill();
            }
        }
        else
        {
            var direction = (target - from).normalized;
            target = from + direction * 100;
        }
        shot.Show(from, target);
    }
}

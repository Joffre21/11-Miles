using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveRoad : MonoBehaviour
{
    private Player player;
    private GameManager gm;

    void Start()
    {
        gm = FindObjectOfType<GameManager>();
        player = FindObjectOfType<Player>();
    }

    void Update()
    {
        if (gm != null && gm.GameStarted && player != null)
        {
            float speed = player.CurrentSpeed;
            transform.position += new Vector3(0f, 0f, -speed) * Time.deltaTime;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Destroyer"))
        {
            Destroy(gameObject);
        }
    }
}

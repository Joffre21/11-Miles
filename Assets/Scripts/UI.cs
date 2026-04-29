
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UI : MonoBehaviour
{
    private GameManager gm;
    float speed = 0f;
    [SerializeField] TextMeshProUGUI speedText;
    [SerializeField] Player player;

    void Start()
    {
        gm = FindObjectOfType<GameManager>();
        if (gm != null && gm.GameStarted)
        {
            speedText.text = "";
        }
    }

    void Update()
    {
        if (gm != null && gm.GameStarted && speedText != null && player != null)
        {
            speed = player.CurrentSpeed;
            speedText.text = $"Speed: {speed:F1}";
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerEnd : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        GameManager manager = GameObject.Find("_GameManager").GetComponent<GameManager>();
        if (other.gameObject.tag == "Player" && manager.GetEnemies().Count == 0)
        {            
            manager.gameover = true;
            manager.finished = true;
            Time.timeScale = 0f;
        }
    }
}

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddScore : MonoBehaviour {

    void OnTriggerEnter2D(Collider2D col) {
        //add to score and display if player touches star
        if (col.tag == "Player") {
            Player.score++;
            Player.scoreBox.text = Player.score.ToString();
            Destroy(gameObject);
        }
    }
}

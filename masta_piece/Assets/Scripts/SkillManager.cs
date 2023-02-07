using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour {
    public float ballLiftHeight = 5f;
    [SerializeField] private Rigidbody ballRb;

    private void Awake() {
        ballRb = GameObject.FindGameObjectWithTag("Ball").GetComponent<Rigidbody>();
    }

    public void doSkill(string comboName) {
        if (comboName == "Test") {
            ballRb.AddForce(0, ballLiftHeight, 0, ForceMode.Impulse);
        }
    }
}

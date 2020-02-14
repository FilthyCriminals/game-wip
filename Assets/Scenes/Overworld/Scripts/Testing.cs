using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Testing : MonoBehaviour {

    private Grid grid;

    private void Start() {
        grid = new Grid(10, 10, 10f, new Vector3(0, 0));
    }

    private void Update() {
        if (Input.GetMouseButtonDown(0)) {
            grid.SetValue(Utilities.GetMouseWorldPosition(), 1);
        }

        if (Input.GetMouseButtonDown(1)) {
            grid.GetValue(Utilities.GetMouseWorldPosition());
        }
    }
}
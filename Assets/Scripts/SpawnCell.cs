using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnCell : MonoBehaviour
{
    public GameObject protoCell;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        Vector3 mouseScreenPosition = mouseWorldPosition;
        mouseScreenPosition.z -= mouseScreenPosition.z;

        if (Input.GetMouseButton(0))
        {
            Instantiate(protoCell, mouseScreenPosition, new Quaternion());
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossHairAnim : MonoBehaviour
{

    public RectTransform topLeft, topRight, bottomLeft, bottomRight, center;
    public float bumpSpeed, bumpForce, rotationSpeed;

    private float startScale, minXStart, minYStart, maxXStart, maxYStart, timer, nrvBooster;

    // Start is called before the first frame update
    void Start()
    {
        minXStart = bottomLeft.position.x;
        minYStart = bottomLeft.position.y;
        maxXStart = topRight.position.x;
        maxYStart = topRight.position.y;
        timer = 0;
        nrvBooster = 1;
        startScale = center.localScale.x;
    }

    // Update is called once per frame
    void Update()
    {
        if (VariableManager.variableManager.battery <= 0)
        {
            topLeft.gameObject.SetActive(false);
            topRight.gameObject.SetActive(false);
            bottomLeft.gameObject.SetActive(false);
            bottomRight.gameObject.SetActive(false);
            center.gameObject.SetActive(false);
        }
        else
        {
            topLeft.gameObject.SetActive(true);
            topRight.gameObject.SetActive(true);
            bottomLeft.gameObject.SetActive(true);
            bottomRight.gameObject.SetActive(true);
            center.gameObject.SetActive(true);
        }
        if (VariableManager.variableManager.IsDamaging())
        {
            nrvBooster = 4;
        }
        else
        {
            nrvBooster = 1;
        }
        timer += bumpSpeed * nrvBooster * Time.deltaTime;
        center.rotation = Quaternion.Euler(0, 0, center.rotation.eulerAngles.z + rotationSpeed * Time.deltaTime);
        center.localScale = new Vector3(startScale + Mathf.Sin(timer)/2f, startScale + Mathf.Sin(timer)/2f, 1);
        topLeft.position = new Vector3(minXStart - Mathf.Sin(timer*2) * bumpForce, maxYStart + Mathf.Sin(timer*2) * bumpForce, 0);
        topRight.position = new Vector3(maxXStart + Mathf.Sin(timer*2) * bumpForce, maxYStart + Mathf.Sin(timer*2) * bumpForce, 0);
        bottomLeft.position = new Vector3(minXStart - Mathf.Sin(timer*2) * bumpForce, minYStart - Mathf.Sin(timer*2) * bumpForce, 0);
        bottomRight.position = new Vector3(maxXStart + Mathf.Sin(timer*2) * bumpForce, minYStart - Mathf.Sin(timer*2) * bumpForce, 0);
    }
}

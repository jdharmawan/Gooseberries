using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bowstring : MonoBehaviour
{
    public Transform bowstringTop, bowstringBottom, pullbackPos;
    public LineRenderer lineRenderer;

    bool renderLines = false;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if(renderLines)
        { 
            var points = new Vector3[3];

            //points[0] = bowstringTop.transform.position;
            //points[1] = pullbackPos.transform.position;
            //points[2] = bowstringBottom.transform.position;

            points[0] = new Vector3(bowstringTop.transform.position.x, bowstringTop.transform.position.y, bowstringTop.transform.position.z - 0.01f);
            points[1] = new Vector3(pullbackPos.transform.position.x, pullbackPos.transform.position.y, pullbackPos.transform.position.z - 0.01f);
            points[2] = new Vector3(bowstringBottom.transform.position.x, bowstringBottom.transform.position.y, bowstringBottom.transform.position.z - 0.01f);

            lineRenderer.SetPositions(points);
        }
    }

    private void SetRenderLines()
    {
        var points = new Vector3[3];

        //points[0] = bowstringTop.transform.position;
        //points[1] = pullbackPos.transform.position;
        //points[2] = bowstringBottom.transform.position;

        points[0] = new Vector3(bowstringTop.transform.position.x, bowstringTop.transform.position.y, bowstringTop.transform.position.z - 0.01f);
        points[1] = new Vector3(pullbackPos.transform.position.x, pullbackPos.transform.position.y, pullbackPos.transform.position.z - 0.01f);
        points[2] = new Vector3(bowstringBottom.transform.position.x, bowstringBottom.transform.position.y, bowstringBottom.transform.position.z - 0.01f);

        lineRenderer.SetPositions(points);
    }
}

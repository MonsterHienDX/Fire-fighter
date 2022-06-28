using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoyStick : SingletonMonobehaviour<JoyStick>
{
    [SerializeField] private Transform Root, Pad;
    [SerializeField] float MaxR = 1;

    Vector2 Origin = new Vector2(0, 0);
    [SerializeField] bool _IsOriginSet = false;

    [SerializeField] private List<GameObject> notActiveOnObjects = new List<GameObject>();

    void Update()
    {
        ListenJoyStick();
        ReleaseTouch();
    }

    void ListenJoyStick()
    {
        Debug.LogWarning(GetJoyVector());

        if (!Input.GetMouseButton(0))
            return;
        IgnoreObjects(notActiveOnObjects);

        if (!_IsOriginSet)
        {
            _IsOriginSet = true;
            Origin = Input.mousePosition;
            Root.position = Origin;
            Pad.transform.position = Origin;
            Root.gameObject.SetActive(true);
            return;
        }
        Vector2 currentTouch = (Vector2)Input.mousePosition - Origin;
        if (currentTouch == Vector2.zero)
            return;
        if (currentTouch.magnitude <= MaxR)
        {
            Pad.transform.position = Input.mousePosition;
            return;
        }

        float currentAngle = Mathf.Atan2(currentTouch.y, currentTouch.x);
        float X = Origin.x + MaxR * Mathf.Cos(currentAngle);
        float Y = Origin.y + MaxR * Mathf.Sin(currentAngle);
        Pad.transform.position = new Vector2(X, Y);
    }

    void ReleaseTouch()
    {
        if (!_IsOriginSet)
            return;
        if (Input.GetMouseButtonUp(0))
        {
            _IsOriginSet = false;
            Root.gameObject.SetActive(false);
            Origin = Vector2.zero;
        }
    }

    public Vector2 GetJoyVector()
    {
        if (!_IsOriginSet)
            return Vector2.zero;
        Vector2 tmp = (Vector2)Input.mousePosition - Origin;
        return tmp.normalized;
    }

    private bool MouseOnElement(GameObject GO)
    {
        RectTransform rectTransform = GO.GetComponent<RectTransform>();
        Vector3[] corners = new Vector3[4];
        rectTransform.GetWorldCorners(corners);

        float xMin = corners[0].x;
        float xMax = corners[2].x;

        float yMin = corners[0].y;
        float yMax = corners[2].y;

        if ((Input.mousePosition.x > xMax || Input.mousePosition.x < xMin)
            ||
            (Input.mousePosition.y > yMax || Input.mousePosition.y < yMin))
        {
            return false;
        }

        return true;
    }

    private bool IgnoreObjects(List<GameObject> GOList)
    {
        if (GOList.Count > 0)
            foreach (GameObject go in GOList)
            {
                if (MouseOnElement(go))
                    return false;
            }
        return true;
    }

}
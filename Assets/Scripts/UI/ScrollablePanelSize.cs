using UnityEngine;

public class ScrollablePanelSize : MonoBehaviour
{
    public Transform Min;
    public Transform Max;

    public AnimationCurve ScaleCurve;

    private void Start()
    {
        GetPos();
    }
    private void Update()
    {
        UpdateScale();
    }

    public void UpdateScale()
    {
        var point = Mathf.InverseLerp(Min.localPosition.x, Max.localPosition.x, transform.localPosition.x);
        transform.localScale = ScaleCurve.Evaluate(point) * Vector3.one;
    }

    public void GetPos()
    {
        var point = Mathf.InverseLerp(Min.localPosition.x, Max.localPosition.x, transform.localPosition.x);
        var eva = ScaleCurve.Evaluate(point);
        Debug.Log($"{point} / {eva}\n");
    }
}


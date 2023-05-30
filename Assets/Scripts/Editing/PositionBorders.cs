using UnityEngine;

public class PositionBorders : MonoBehaviour {
    public Renderer parentRenderer;
    [Header("Inner Border")]
    public GameObject innerBorder;
    [Header("Outer Border")]
    public GameObject outerBorder;
    [Header("Sides")]
    public bool z = true;
    public bool x = true;

    // This code is terrible, but it only runs in the editor, so who cares.
    public void PositionObject() {
        DestroyAll();

        Vector3 size = parentRenderer.bounds.size;
        transform.position = parentRenderer.transform.position + new Vector3(0, 0.0001f + (size.y / 2), 0);

        Debug.Log(size);

        if (z) {
            foreach(float direction in new float[] { 1f, -1f }) {
                GameObject zOuterBorder = Instantiate(outerBorder);
                Renderer zOuterBorderRenderer = zOuterBorder.GetComponent<Renderer>();
                float zOuterBorderSizeX = zOuterBorderRenderer.bounds.size.x;
                zOuterBorder.transform.localPosition = new Vector3(0, transform.localPosition.y, ((size.z / 2) - (zOuterBorderSizeX / 2)) * direction);
                zOuterBorder.transform.localRotation = Quaternion.Euler(0f, -90f, 0f);
                zOuterBorder.transform.localScale = new Vector3(zOuterBorder.transform.localScale.x, 1f, size.x / 10);
                zOuterBorder.transform.parent = transform;

                GameObject zInnerBorder = Instantiate(innerBorder);
                Renderer zInnerBorderRenderer = zInnerBorder.GetComponent<Renderer>();
                zInnerBorder.transform.localPosition = new Vector3(0, transform.localPosition.y, ((size.z / 2) - (zInnerBorderRenderer.bounds.size.x / 2) - zOuterBorderSizeX) * direction);
                zInnerBorder.transform.localRotation = Quaternion.Euler(0f, -90f, 0f);
                zInnerBorder.transform.localScale = new Vector3(zInnerBorder.transform.localScale.x, 1f, (size.x - (x ? zInnerBorder.transform.localScale.z : 0)) / 10);
                zInnerBorder.transform.parent = transform;
            }
        }

        if (x) {
            foreach(float direction in new float[] { 1f, -1f }) {
                GameObject xOuterBorder = Instantiate(outerBorder);
                Renderer xOuterBorderRenderer = xOuterBorder.GetComponent<Renderer>();
                float xOuterBorderSizeX = xOuterBorderRenderer.bounds.size.x;
                xOuterBorder.transform.localPosition = new Vector3(((size.x / 2) - (xOuterBorderSizeX / 2)) * direction, transform.localPosition.y, 0);
                xOuterBorder.transform.localRotation = Quaternion.Euler(0f, -180f, 0f);
                xOuterBorder.transform.localScale = new Vector3(xOuterBorder.transform.localScale.x, 1f, size.z / 10);
                xOuterBorder.transform.parent = transform;

                GameObject xInnerBorder = Instantiate(innerBorder);
                Renderer xInnerBorderRenderer = xInnerBorder.GetComponent<Renderer>();
                xInnerBorder.transform.localPosition = new Vector3(((size.x / 2) - (xInnerBorderRenderer.bounds.size.x / 2) - xOuterBorderSizeX) * direction, transform.localPosition.y, 0);
                xInnerBorder.transform.localRotation = Quaternion.Euler(0f, -180f, 0f);
                xInnerBorder.transform.localScale = new Vector3(xInnerBorder.transform.localScale.x, 1f, (size.z - (z ? xInnerBorder.transform.localScale.y : 0)) / 10);
                xInnerBorder.transform.parent = transform;
            }
        }
    }

    public void DestroyAll() {
        while (transform.childCount > 0) {
            DestroyImmediate(transform.GetChild(0).gameObject);
        }
    }
}

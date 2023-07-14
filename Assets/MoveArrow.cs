using UnityEngine;

public class MoveArrow : MonoBehaviour
{
    private void OnMouseDrag()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 direction = (mousePos - transform.position).normalized;
        float angle = 360 - Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg;
        transform.eulerAngles = new Vector3(0, 0, angle);
    }
}

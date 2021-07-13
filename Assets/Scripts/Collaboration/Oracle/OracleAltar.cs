using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class OracleAltar : MonoBehaviour
{
    [SerializeField] OracleUI tooltip;

    private void OnTriggerEnter2D(Collider2D other)
    {
        // SHow tooltip
        tooltip.Show(this);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        // Hide tooltip
        tooltip.Hide();
    }
}
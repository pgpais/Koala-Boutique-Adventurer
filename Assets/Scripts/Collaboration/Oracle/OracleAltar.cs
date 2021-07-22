using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class OracleAltar : MonoBehaviour
{
    [SerializeField] OracleUI tooltip;

    OracleData data;

    private void Awake()
    {
    }

    private void Start()
    {
        data = OracleManager.Instance.GetOracleData();
        tooltip = FindObjectOfType<OracleUI>(true);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // SHow tooltip
        tooltip.Show(data);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        // Hide tooltip
        tooltip.Hide();
    }
}
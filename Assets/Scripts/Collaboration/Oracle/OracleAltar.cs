using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class OracleAltar : MonoBehaviour
{
    [SerializeField] OracleUI tooltip;
    [SerializeField] AudioClip onOracleActivatedSound;

    OracleData data;

    private void Awake()
    {
    }

    private void Start()
    {
        data = OracleManager.Instance.GetNewOracleData();
        tooltip = FindObjectOfType<OracleUI>(true);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // SHow tooltip
        tooltip.Show(data);
        OracleManager.Instance.SendNewOracleData();

        // Play sound
        AudioSource.PlayClipAtPoint(onOracleActivatedSound, transform.position);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        // Hide tooltip
        tooltip.Hide();
    }
}
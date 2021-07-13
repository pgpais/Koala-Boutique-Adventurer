using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OracleUI : MonoBehaviour
{
    [SerializeField] TMP_Text m_Text;
    [SerializeField] Image m_Image;
    [SerializeField] GameObject tooltip;


    void Start()
    {
        tooltip.SetActive(false);
    }

    public void Initialize(Item item, string text)
    {
        tooltip.SetActive(true);
        SetText(text);
        SetSprite(item.sprite);
    }

    private void SetText(string text)
    {
        m_Text.text = text;
    }

    private void SetSprite(Sprite itemSprite)
    {
        m_Image.sprite = itemSprite;
    }
}
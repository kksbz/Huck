using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemSlot : MonoBehaviour
{
    public bool HasItem { get; private set; } = false;
<<<<<<< HEAD
    public ItemData Item = default;
    public int itemAmount = 0;

=======
    public ItemData itemData = default;
    public int itemAmount = 0;
    private Image itemIconImg = default;

    private bool isEmpty = true;
>>>>>>> 906dbeb8e19fb7a93a4a77a683abc26c4204f498
    private GameObject itemAmountObj = default;
    private TMP_Text itemAmountText = default;
    private Color defaultAlpha = new Color(1f, 1f, 1f, 0f);
    private Color itemAlpha = new Color(1f, 1f, 1f, 1f);
<<<<<<< HEAD
=======

    public delegate void OnUseDel(ItemSlot itemSlot_);
    public OnUseDel itemUseDel = default;
    // 델리게이트로 지금 캐싱하고 있는 아이템이 사용되었을 때의 동작을 캐싱하고 있음.
    // 델리게이트 = default;

    private PlayerStat playerStat = default;
>>>>>>> 906dbeb8e19fb7a93a4a77a683abc26c4204f498
    // Start is called before the first frame update
    void Start()
    {
        itemAmountObj = transform.GetChild(0).GetChild(0).gameObject;
        itemAmountText = itemAmountObj.GetComponent<TMP_Text>();
<<<<<<< HEAD
=======
        itemIconImg = transform.GetChild(0).GetComponent<Image>();
        playerStat = GameManager.Instance.playerObj.GetComponent<PlayerStat>();
>>>>>>> 906dbeb8e19fb7a93a4a77a683abc26c4204f498
    }

    // Update is called once per frame
    void Update()
    {
<<<<<<< HEAD
        if (Item != null && Item != default)
        {
            transform.GetChild(0).GetComponent<Image>().sprite = Item.itemIcon;
            transform.GetChild(0).GetComponent<Image>().color = itemAlpha;
            HasItem = true;
            ItemCountText();
        }
        else
        {
            transform.GetChild(0).GetComponent<Image>().sprite = default;
            transform.GetChild(0).GetComponent<Image>().color = defaultAlpha;
            HasItem = false;
            itemAmountObj.SetActive(false);
        }
    }

    private void ItemCountText()
    {
        if (Item.itemType == ItemType.CombineAble)
=======
        if (itemData != null && itemData != default)
        {
            if (itemData.ItemType == EItemType.CombineAble)
            {
                // 아이템의 종류를 이 시점에서 알 수 있음 -> 해당 아이템의 클래스도 가져올 수 있음.
                itemIconImg.sprite = itemData.ItemIcon;
                itemIconImg.color = itemAlpha;
                HasItem = true;
                isEmpty = false;
                ItemCountText();
            } // 합칠 수 있는 아이템일 경우
            else
            {
                itemIconImg.sprite = itemData.ItemIcon;
                itemIconImg.color = itemAlpha;
                HasItem = true;
                isEmpty = false;
                itemAmountObj.SetActive(false);
            } // 합칠 수 없는 아이템일 경우

            if (itemAmount <= 0)
            {
                DisableImg();
            }
        } // 아이템 데이터가 있는지 없는지 판단
        else
        {
            if (!isEmpty)
            {
                itemIconImg.sprite = default;
                itemIconImg.color = defaultAlpha;
                itemAmountObj.SetActive(false);
                HasItem = false;
                itemData = default;
                isEmpty = true;
            }
        }
    }

    public void DisableImg()
    {
        itemIconImg.sprite = default;
        itemIconImg.color = defaultAlpha;
        HasItem = false;
        itemData = default;
        itemUseDel = default;
        itemAmountObj.SetActive(false);
    }

    private void ItemCountText()
    {
        if (itemData.ItemType == EItemType.CombineAble)
>>>>>>> 906dbeb8e19fb7a93a4a77a683abc26c4204f498
        {
            itemAmountObj.SetActive(true);
            itemAmountText.text = $"{itemAmount}";
        }
    }


}

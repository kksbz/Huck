using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerOther : MonoBehaviour
{
<<<<<<< HEAD
    public GameObject inven = default;
    public GameObject map = default;
    public GameObject GUI = default;

    public static bool isInvenOpen = false;
    public static bool isMapOpen = false;

    private InventoryArray invenSlot = default;

    private void Start()
    {
        invenSlot = inven.transform.GetChild(0).GetChild(0).GetComponent<InventoryArray>();
        CursorSet();
    }

    private void Update() 
    {
        InvenOpen();
        MapOpen();
    }
    
    // { Player Inventory
#region Inven
    public void InvenOpen()
    {

        if(Input.GetKeyDown(KeyCode.Tab) && isMapOpen == false)
        {
            isInvenOpen = !isInvenOpen;
            if(isInvenOpen == true)
=======
    [SerializeField]
    private InventoryArray invenSlot = default;

    [SerializeField]
    private Item itemInfo = default;

    private GameObject menu = default;
    private GameObject inven = default;
    private GameObject map = default;
    private GameObject GUI = default;
    private GameObject invenCam = default;
    private GameObject settingMenu = default;


    public static bool isInvenOpen = false;
    public static bool isMapOpen = false;
    public static bool isMenuOpen = false;
    public static bool isWorkbenchOpen = false;
    public static bool isStoveOpen = false;
    public static bool isAnvilOpen = false;

    private Vector3 enableScale = new Vector3(0.00001f, 0.00001f, 0.00001f);
    private Vector3 ableScale = new Vector3(1f, 1f, 1f);


    private void Start()
    {
        GameObject UiObjs = UIManager.Instance.UiObjs;
        GameObject Ui = UiObjs.transform.GetChild(1).gameObject;

        menu = Ui.transform.GetChild(4).gameObject;
        inven = UiObjs.transform.GetChild(0).gameObject;
        map = Ui.transform.GetChild(3).gameObject;
        GUI = Ui.transform.GetChild(0).gameObject;
        invenCam = transform.GetChild(3).gameObject;
        settingMenu = Ui.transform.GetChild(5).gameObject;

        CursorSet();
        inven.SetLocalScale(enableScale);
        invenSlot = UIManager.Instance.inventory.GetComponent<InventoryArray>();
    }

    private void Update()
    {
        InvenOpen();
        MapOpen();
        MenuOpen();
        RootItem();
    }

    // { Player Inventory
    #region Inven
    public void InvenOpen()
    {

        if (Input.GetKeyDown(KeyCode.Tab) && isMapOpen == false && isMenuOpen == false && PlayerMove.isDead == false
            && PlayerOther.isStoveOpen == false && PlayerOther.isAnvilOpen == false
            && PlayerOther.isWorkbenchOpen == false && LoadingManager.Instance.isLoadingEnd == true)
        {
            isInvenOpen = !isInvenOpen;
            if (isInvenOpen == true)
>>>>>>> 906dbeb8e19fb7a93a4a77a683abc26c4204f498
            {
                gameObject.GetComponent<PlayerAtk>().enabled = false;
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
<<<<<<< HEAD
                inven.SetActive(true);
                GUI.SetActive(false);
            }
            if(isInvenOpen == false)
=======
                invenCam.SetActive(true);
                inven.SetLocalScale(ableScale);
                GUI.SetActive(false);
            }
            if (isInvenOpen == false)
>>>>>>> 906dbeb8e19fb7a93a4a77a683abc26c4204f498
            {
                gameObject.GetComponent<PlayerAtk>().enabled = true;
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
<<<<<<< HEAD
                inven.SetActive(false);
=======
                invenCam.SetActive(false);
                inven.SetLocalScale(enableScale);
>>>>>>> 906dbeb8e19fb7a93a4a77a683abc26c4204f498
                GUI.SetActive(true);
            }
        }
    }
<<<<<<< HEAD
#endregion
    // } Player Inventory 

    // { Player Interaction 
#region Interact
    public void Interaction()
    {
        if(Input.GetKeyDown(KeyCode.E))
        {
            
        }
    }
#endregion
    // } Player Interaction

    // { Player Map
#region Map
    public void MapOpen()
    {
        if(Input.GetKeyDown(KeyCode.M) && isInvenOpen == false)
        {
            isMapOpen = !isMapOpen;
            if(isMapOpen == true)
=======
    #endregion
    // } Player Inventory 

    // { Player Map
    #region Map
    public void MapOpen()
    {
        if (Input.GetKeyDown(KeyCode.M) && isInvenOpen == false && isMenuOpen == false && PlayerMove.isDead == false
            && PlayerOther.isStoveOpen == false && PlayerOther.isAnvilOpen == false
            && PlayerOther.isWorkbenchOpen == false && LoadingManager.Instance.isLoadingEnd == true)
        {
            isMapOpen = !isMapOpen;
            if (isMapOpen == true)
>>>>>>> 906dbeb8e19fb7a93a4a77a683abc26c4204f498
            {
                gameObject.GetComponent<PlayerAtk>().enabled = false;
                map.SetActive(true);
                GUI.SetActive(false);
            }
<<<<<<< HEAD
            if(isMapOpen == false)
=======
            if (isMapOpen == false)
>>>>>>> 906dbeb8e19fb7a93a4a77a683abc26c4204f498
            {
                gameObject.GetComponent<PlayerAtk>().enabled = true;
                map.SetActive(false);
                GUI.SetActive(true);
            }
        }
    }
<<<<<<< HEAD
#endregion
    // } Player Map

    // { Cursor Setting
#region Cursor
=======
    #endregion
    // } Player Map

    // { Cursor Setting
    #region Cursor
>>>>>>> 906dbeb8e19fb7a93a4a77a683abc26c4204f498
    private void CursorSet()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
<<<<<<< HEAD
#endregion
    // } Cursor Setting
}
=======
    #endregion
    // } Cursor Setting

    //{ Game Menu
    #region Menu
    private void MenuOpen()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || UIManager.Instance.isResumeOn == true)
        {
            if (UIManager.Instance.isSetMenuOpen == false && PlayerMove.isDead == false
                && LoadingManager.Instance.isLoadingEnd == true)
            {
                isMenuOpen = !isMenuOpen;
                if (isMenuOpen == true)
                {
                    gameObject.GetComponent<PlayerAtk>().enabled = false;
                    Cursor.visible = true;
                    Cursor.lockState = CursorLockMode.None;
                    menu.SetActive(true);
                    Time.timeScale = 0f;
                }
                if (isMenuOpen == false)
                {
                    gameObject.GetComponent<PlayerAtk>().enabled = true;
                    Cursor.visible = false;
                    Cursor.lockState = CursorLockMode.Locked;
                    UIManager.Instance.isResumeOn = false;
                    menu.SetActive(false);
                    Time.timeScale = 1f;
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.Escape) && UIManager.Instance.isSetMenuOpen == true)
        {
            settingMenu.SetActive(false);
            UIManager.Instance.isSetMenuOpen = false;
        }
    }
    #endregion
    //} Game Menu

    // { Item Root
    #region  RootItem
    private void RootItem()
    {
        if (Camera.main.GetComponent<ItemRange>().getItem != null)
        {
            itemInfo = Camera.main.GetComponent<ItemRange>().getItem.GetComponent<Item>();
        }
        else
        {
            itemInfo = default;
        }
        if (itemInfo != null && Input.GetKeyDown(KeyCode.E))
        {
            invenSlot.AddItem(itemInfo);
            if(!invenSlot.isFillAll)
                Destroy(itemInfo.gameObject);
        }
    }
    #endregion
    // } Item Root
}
>>>>>>> 906dbeb8e19fb7a93a4a77a683abc26c4204f498

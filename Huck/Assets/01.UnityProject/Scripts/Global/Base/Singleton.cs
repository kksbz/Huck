using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
<<<<<<< HEAD
=======
using UnityEngine.SceneManagement;
>>>>>>> 906dbeb8e19fb7a93a4a77a683abc26c4204f498

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;
    public static T Instance
    {
        get
        {
<<<<<<< HEAD
            // ¿¹¿ÜÃ³¸® instance°¡ null°ªÀÏ ¶§
            if (instance == null)
            {
                // ¾îµò°¡¿¡ instance°¡ ÀÖÀ¸¸é Ã£¾Æ¼­ ÀúÀå
                instance = (T)FindObjectOfType(typeof(T));
                // Ã£¾ÆºÃ´Âµ¥µµ ¾øÀ¸¸é »õ·Î¿î ¿ÀºêÁ§Æ®¸¦ ÀúÀåÇÔ
=======
            // ï¿½ï¿½ï¿½ï¿½Ã³ï¿½ï¿½ instanceï¿½ï¿½ nullï¿½ï¿½ï¿½ï¿½ ï¿½ï¿½
            if (instance == null)
            {
                // ï¿½ï¿½ò°¡¿ï¿½ instanceï¿½ï¿½ ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ Ã£ï¿½Æ¼ï¿½ ï¿½ï¿½ï¿½ï¿½
                instance = (T)FindObjectOfType(typeof(T));
                // Ã£ï¿½ÆºÃ´Âµï¿½ï¿½ï¿½ ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ ï¿½ï¿½ï¿½Î¿ï¿½ ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½Æ®ï¿½ï¿½ ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½
>>>>>>> 906dbeb8e19fb7a93a4a77a683abc26c4204f498
                if (instance == null)
                {
                    GameObject obj = new GameObject(typeof(T).Name, typeof(T));
                    instance = obj.GetComponent<T>();
                }
            }
            return instance;
        }
    }

    private void Awake()
    {
<<<<<<< HEAD
    // Á¶°Ç: ÇØ´ç ¿ÀºêÁ§Æ®ÀÇ ºÎ¸ð ¿ÀºêÁ§Æ®³ª ÃÖ»óÀ§¿¡ ¹«¾ð°¡°¡ Á¸ÀçÇÑ´Ù¸é
        if (transform.parent != null && transform.root != null)
        {
            // Á¸ÀçÇÑ´Ù¸é ±× ´ë»óÀ» À¯Áö½ÃÅ´
=======
        if(SceneManager.GetActiveScene().name == GData.SCENENAME_TITLE)
        {
            return;
        }
        // ï¿½ï¿½ï¿½ï¿½: ï¿½Ø´ï¿½ ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½Æ®ï¿½ï¿½ ï¿½Î¸ï¿½ ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½Æ®ï¿½ï¿½ ï¿½Ö»ï¿½ï¿½ï¿½ï¿½ï¿½ ï¿½ï¿½ï¿½ð°¡°ï¿½ ï¿½ï¿½ï¿½ï¿½ï¿½Ñ´Ù¸ï¿½
        if (transform.parent != null && transform.root != null)
        {
            // ï¿½ï¿½ï¿½ï¿½ï¿½Ñ´Ù¸ï¿½ ï¿½ï¿½ ï¿½ï¿½ï¿½ï¿½ï¿½ ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½Å´
>>>>>>> 906dbeb8e19fb7a93a4a77a683abc26c4204f498
            DontDestroyOnLoad(this.transform.root.gameObject);
        }
        else
        {
<<<<<<< HEAD
            // Á¸ÀçÇÏÁö¾ÊÀ¸¸é ÀÚ±âÀÚ½ÅÀ» À¯Áö½ÃÅ´
            DontDestroyOnLoad(this.gameObject);
        }
        // ÀÌ·¯ÇÑ ¿¹¿ÜÃ³¸®¸¦ ÇÏ´Â ÀÌÀ¯´Â ÃÖ»óÀ§¿¡ Managers°°Àº ºó¿ÀºêÁ§Æ®¸¦ ¸¸µé°í
        // ±× ¾È¿¡ Game, Sound, UiµîÀÇ Manager¸¦ ³Ö¾î¼­ °ü¸®ÇÒ·Á°í ÇÑ °ÍÀÓ
    }
}
// [ÃâÃ³] ½Ì±ÛÅæ | ÀÛ¼ºÀÚ ºÎµÎÁ»ºñ
=======
            // ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ ï¿½Ú±ï¿½ï¿½Ú½ï¿½ï¿½ï¿½ ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½Å´
            DontDestroyOnLoad(this.gameObject);
        }
        Init();
        // ï¿½Ì·ï¿½ï¿½ï¿½ ï¿½ï¿½ï¿½ï¿½Ã³ï¿½ï¿½ï¿½ï¿½ ï¿½Ï´ï¿½ ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ ï¿½Ö»ï¿½ï¿½ï¿½ï¿½ï¿½ Managersï¿½ï¿½ï¿½ï¿½ ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½Æ®ï¿½ï¿½ ï¿½ï¿½ï¿½ï¿½ï¿½
        // ï¿½ï¿½ ï¿½È¿ï¿½ Game, Sound, Uiï¿½ï¿½ï¿½ï¿½ Managerï¿½ï¿½ ï¿½Ö¾î¼­ ï¿½ï¿½ï¿½ï¿½ï¿½Ò·ï¿½ï¿½ï¿½ ï¿½ï¿½ ï¿½ï¿½ï¿½ï¿½
        Init();
    }

    protected virtual void Init()
    {
        // Awake() ï¿½ï¿½ï¿½ï¿½
    }
}
// [ï¿½ï¿½Ã³] ï¿½Ì±ï¿½ï¿½ï¿½ | ï¿½Û¼ï¿½ï¿½ï¿½ ï¿½Îµï¿½ï¿½ï¿½ï¿½ï¿½
>>>>>>> 906dbeb8e19fb7a93a4a77a683abc26c4204f498

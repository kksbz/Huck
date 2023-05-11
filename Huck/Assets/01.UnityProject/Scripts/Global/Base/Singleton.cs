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
            // ����ó�� instance�� null���� ��
            if (instance == null)
            {
                // ��򰡿� instance�� ������ ã�Ƽ� ����
                instance = (T)FindObjectOfType(typeof(T));
                // ã�ƺôµ��� ������ ���ο� ������Ʈ�� ������
=======
            // ����ó�� instance�� null���� ��
            if (instance == null)
            {
                // ��򰡿� instance�� ������ ã�Ƽ� ����
                instance = (T)FindObjectOfType(typeof(T));
                // ã�ƺôµ��� ������ ���ο� ������Ʈ�� ������
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
    // ����: �ش� ������Ʈ�� �θ� ������Ʈ�� �ֻ����� ���𰡰� �����Ѵٸ�
        if (transform.parent != null && transform.root != null)
        {
            // �����Ѵٸ� �� ����� ������Ŵ
=======
        if(SceneManager.GetActiveScene().name == GData.SCENENAME_TITLE)
        {
            return;
        }
        // ����: �ش� ������Ʈ�� �θ� ������Ʈ�� �ֻ����� ���𰡰� �����Ѵٸ�
        if (transform.parent != null && transform.root != null)
        {
            // �����Ѵٸ� �� ����� ������Ŵ
>>>>>>> 906dbeb8e19fb7a93a4a77a683abc26c4204f498
            DontDestroyOnLoad(this.transform.root.gameObject);
        }
        else
        {
<<<<<<< HEAD
            // �������������� �ڱ��ڽ��� ������Ŵ
            DontDestroyOnLoad(this.gameObject);
        }
        // �̷��� ����ó���� �ϴ� ������ �ֻ����� Managers���� �������Ʈ�� �����
        // �� �ȿ� Game, Sound, Ui���� Manager�� �־ �����ҷ��� �� ����
    }
}
// [��ó] �̱��� | �ۼ��� �ε�����
=======
            // �������������� �ڱ��ڽ��� ������Ŵ
            DontDestroyOnLoad(this.gameObject);
        }
        Init();
        // �̷��� ����ó���� �ϴ� ������ �ֻ����� Managers���� �������Ʈ�� �����
        // �� �ȿ� Game, Sound, Ui���� Manager�� �־ �����ҷ��� �� ����
        Init();
    }

    protected virtual void Init()
    {
        // Awake() ����
    }
}
// [��ó] �̱��� | �ۼ��� �ε�����
>>>>>>> 906dbeb8e19fb7a93a4a77a683abc26c4204f498

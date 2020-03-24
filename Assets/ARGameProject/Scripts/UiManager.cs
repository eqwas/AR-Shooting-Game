using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UiManager : MonoBehaviour
{
    public RawImage scope;
    public RawImage shooted;
    public Text guide;
    public Image guide_p;

    public static bool scopeMessenger=false;
    public static bool shootedMessenger=false;
    public static bool isUnbeatable = false;


    // Start is called before the first frame update
    void Start()
    {
        scope.enabled = false;
        shooted.enabled = false;
    }
    void Update()
    {
        if (scopeMessenger == true)
        {
            guide.enabled = false;
            guide_p.enabled = false;
            scope.enabled = true;
        }

        if (shootedMessenger == true)
        {
            shooted.enabled =true;
            StartCoroutine("Fade");
        }

       
    }
    public static bool isgaming()
    {
        if (scopeMessenger==true)
            return true;
        else
            return false;

    }
    IEnumerator Fade()
    {
        if (isgaming())
        {
            for (float i = 1f; i >= 0; i -= 0.05f)
            {
                Color color = new Vector4(1, 1, 1, i);
                shooted.color = color;
                yield return 0;

            }
            shootedMessenger = false;
            shooted.enabled = false;
            isUnbeatable = false;
        }
        else
        {
            Color color = new Vector4(1, 1, 1, 1f);
            shooted.color = color;
            yield return new WaitForSeconds(0.1f);
            shootedMessenger = false;
            shooted.enabled = false;
            isUnbeatable = false;
        }
    }
}

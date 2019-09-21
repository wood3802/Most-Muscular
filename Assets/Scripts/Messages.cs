using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public interface ICustomMessageTarget : IEventSystemHandler
{
    void newMessage(string message);
}

public class Messages : MonoBehaviour, ICustomMessageTarget
{
    Text myText;
    // Start is called before the first frame update
    void Start()
    {
        myText = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void newMessage(string message)
    {
        myText.text = "";
        myText.text = message;
        myText.CrossFadeAlpha(1f, 0.08f, false);
        Invoke("FadeOut", 1f);
        
    }

    void FadeOut()
    {
        myText.CrossFadeAlpha(0f, 0.2f, false);
    }
}

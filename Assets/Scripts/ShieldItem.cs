using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldItem : MonoBehaviour, IShieldItem
{
    public float ShieldTimeToGive = 2;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public float GetShieldTime()
    {
        return ShieldTimeToGive;
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}

public interface IShieldItem
{
    float GetShieldTime();
    void Hide();
}

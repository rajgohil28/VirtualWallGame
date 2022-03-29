using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeColorOnStrike : MonoBehaviour
{
    private Renderer _materialRenderer;
    public float ChangeFactor= 0.1f;
    public int TileScore = 10;
    // Start is called before the first frame update
    void Start()
    {
        _materialRenderer = GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ChangeColorOnHit()
    {
        StartCoroutine(ColorChange());
    }
    IEnumerator ColorChange()
    {
        Material MaterialColor = _materialRenderer.material;
        MaterialColor.SetColor("_BaseColor", new Color(MaterialColor.color.r, MaterialColor.color.g, MaterialColor.color.b, 0));
        yield return new WaitForSeconds(0.5f);
        MaterialColor.SetColor("_BaseColor", new Color(MaterialColor.color.r, MaterialColor.color.g, MaterialColor.color.b, 1));
        
    }
}

using UnityEngine;

public class ScrollBackground : MonoBehaviour
{
    [SerializeField] private float speed = 1f;
    [ReadOnly][SerializeField] private Renderer rend;
    void Start() => rend = GetComponent<Renderer>();
    
    // Update is called once per frame
    void Update()
    {
        float repeat = Mathf.Repeat(Time.time * speed, 1);
        Vector2 offset = new Vector2(repeat, 0);
        rend.sharedMaterial.mainTextureOffset = offset;
    }
}

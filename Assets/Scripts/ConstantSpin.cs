using UnityEngine;

public class ConstantSpin : MonoBehaviour
{
    public float spinSpeed;
    public Vector3 spinAxis;
    void Update()
    {
        transform.Rotate(spinAxis, spinSpeed * Time.deltaTime); 
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIResetter : MonoBehaviour {

    float timer;
    Rigidbody rigid;
    public float delay;
    public MeshRenderer carRenderer;
    Material material;
    public BoxCollider resetBox;
    public BoxCollider mainCollider;

	void Start () {
        rigid = GetComponent<Rigidbody>();
        material = carRenderer.material;
	}
	
	void Update () {
		if(rigid.linearVelocity.magnitude < 1)
        {
            timer += Time.deltaTime;
        }
        else
        {
            timer = 0f;
        }
        if(timer >= delay)
        {
            timer = 0;
            if (GetComponent<WaypointFollower>().enabled)
            {
                GetComponent<WaypointFollower>().ResetToLastCheckpoint();
                SetAsGhost();
            }
        }
	}

    void SetAsGhost()
    {
        SetTransparent();
        resetBox.gameObject.SetActive(true);
        mainCollider.gameObject.layer = LayerMask.NameToLayer("AfterResetColliders");
    }

    public void SetAsCar()
    {
        SetOpaque();
        resetBox.gameObject.SetActive(false);
        mainCollider.gameObject.layer = LayerMask.NameToLayer("Vehicle");
    }

    void SetOpaque()
    {
        material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
        material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
        material.SetInt("_ZWrite", 1);
        material.DisableKeyword("_ALPHATEST_ON");
        material.DisableKeyword("_ALPHABLEND_ON");
        material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
        material.renderQueue = -1;
    }

    void SetTransparent()
    {
        material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
        material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
        material.SetInt("_ZWrite", 0);
        material.DisableKeyword("_ALPHATEST_ON");
        material.DisableKeyword("_ALPHABLEND_ON");
        material.EnableKeyword("_ALPHAPREMULTIPLY_ON");
        material.renderQueue = 3000;
    }
}

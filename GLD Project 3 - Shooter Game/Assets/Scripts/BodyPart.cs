using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyPart : MonoBehaviour
{
    private MeshRenderer ThisRenderer;
    private Material ThisMaterial;
    private Color OriginalColor;
    private float FadePercent;
    private float DeathTime;
    private bool IsFading;

    private float LifeTime = 5;


    // Start is called before the first frame update
    void Start()
    {
        ThisRenderer = GetComponent<MeshRenderer>();
        if (ThisRenderer != null)
        {
            ThisMaterial = ThisRenderer.material;
            OriginalColor = ThisMaterial.color;
        }

        DeathTime = Time.time + LifeTime + Random.Range(-LifeTime, LifeTime);
    }


    public void PushObject()
    {
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        GetComponent<Rigidbody>().AddForce(forward * 400);
    }

    public void BeginFadeout()
    {
        StartCoroutine("FadeOut");

        PushObject();
    }

    IEnumerator FadeOut()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.2f);

            if (IsFading)
            {
                FadePercent += Time.deltaTime;
                ThisMaterial.color = Color.Lerp(OriginalColor, Color.clear, FadePercent);

                if (FadePercent >= 1)
                {
                    Destroy(gameObject);
                }
            }
            else
            {
                if (Time.time > DeathTime)
                    IsFading = true;
            }
        }
    }

    private void OnCollision(Collider other)
    {
        if (other.tag == "Ground")
        {
            Debug.Log("OnCollision() - other.tag == Ground");
            GetComponent<Rigidbody>().isKinematic = true;
            GetComponent<Rigidbody>().Sleep();
        }
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwapTextures : MonoBehaviour
{
    public float swapSpeed = 0.4f;
    public List<Texture> textures;

    // Starting a coroutine 
    void Start()
    {
        StartCoroutine(DelayedTextureSwap());
    }

    // Swapping textures from the List with given delay
    IEnumerator DelayedTextureSwap()
    {
        int index = 0;

        while (true)
        {
            yield return new WaitForSeconds(swapSpeed);
            
            GetComponent<Renderer>().material.mainTexture = textures[index];
            index++;

            if (index == textures.Count)
            {
                index = 0;
            }
        }
    }
}

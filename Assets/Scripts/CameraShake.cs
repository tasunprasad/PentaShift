using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraShake : MonoBehaviour {

    public GameObject flash;
    public float duration, magnitude;
    private bool didShake;

    private void Start() {
        didShake = false;
    }

    private void Update() {
        //start shaking camera if player is dead
        if(!Player.isAlive && !didShake) {
            StartCoroutine(Shake(duration, magnitude));
            didShake = true;
        } 
    }
    //Coroutine to shake camera every frame
    IEnumerator Shake(float duration, float magnitude) {
        //Get original position
        Vector3 originalPos = transform.localPosition;
        float elapsed = 0f;
        flash.SetActive(true);
        Image image = flash.GetComponent<Image>(); 
        //loop to shake camera every frame
        while (elapsed < duration) {
            //flash screen by displaying white image and slowly lowering it's alpha
            Color c = image.color;
            if (c.a > 0) {
                c.a = image.color.a - (2f * Time.deltaTime);
                image.color = c;
            }
            if (c.a <= 0) {
                flash.SetActive(false);
            }
            //shake camera here
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;
            transform.localPosition = new Vector3(originalPos.x + x, originalPos.y + y, originalPos.z);
            elapsed += Time.deltaTime;
            magnitude -= 1.5f * Time.deltaTime;
            yield return null;
        }
        //set camera back after shake
        transform.localPosition = originalPos;
    }
}

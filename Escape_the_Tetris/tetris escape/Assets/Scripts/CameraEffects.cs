using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

[ExecuteInEditMode]
public class CameraEffects : MonoBehaviour {

	public Material EffectMaterial;
	public bool activeFade = true;
	void Awake() {
		EffectMaterial.SetFloat ("_Fade", 1);
	}

	void OnRenderImage(RenderTexture src, RenderTexture dst) {
//		StartCoroutine (fadeCoroutine (src, dst));
		if (activeFade) {
			if (EffectMaterial.GetFloat ("_Fade") > .8f)
				EffectMaterial.SetFloat ("_Fade", EffectMaterial.GetFloat ("_Fade") - Time.deltaTime / 20);
			if (EffectMaterial.GetFloat ("_Fade") > 0)
				EffectMaterial.SetFloat ("_Fade", EffectMaterial.GetFloat ("_Fade") - Time.deltaTime / 3);
		}

		Graphics.Blit (src, dst, EffectMaterial);

	}

//	IEnumerator fadeCoroutine(RenderTexture src, RenderTexture dst) {
//		while (EffectMaterial.GetFloat("_Fade") > 0) {
//			EffectMaterial.SetFloat ("_Fade", EffectMaterial.GetFloat ("_Fade") - Time.deltaTime);
//			yield return new WaitForSeconds(5);
//		}
//	}
}

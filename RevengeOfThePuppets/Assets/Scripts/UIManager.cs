using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoSingleton<UIManager> {

	[SerializeField]
	Animator menusAnimator;
	[SerializeField]
	TextMeshProUGUI bestScoreText;
	[SerializeField]
	GameObject newBestScoreLabel;

	public void DisplayPauseMenu() {
		menusAnimator.SetTrigger("Pause");
	}

	public void DisplayEndGame(int bestScore, bool newBest = false) {
		newBestScoreLabel.SetActive(newBest);
		bestScoreText.text = "Best score: " + bestScore.ToString();
		menusAnimator.SetTrigger("EndGame");
	}

	public void HideMenu() {
		menusAnimator.SetTrigger("Hide");
	}
}

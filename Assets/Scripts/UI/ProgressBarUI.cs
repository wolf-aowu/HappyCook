using System;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBarUI : MonoBehaviour {
    [SerializeField] private GameObject hasProgressGameOjbect;
    [SerializeField] private Image barImage;

    private IHasProgress hasProgress;

    private void Start() {
        hasProgress = hasProgressGameOjbect.GetComponent<IHasProgress>();

        if (hasProgress == null) {
            Debug.Log(hasProgressGameOjbect + " 没有继承 IHasProgress 接口");
        }

        hasProgress.OnProgressChanged += HasProgress_OnProgressChanged; ;

        barImage.fillAmount = 0;

        Hide();
    }

    private void HasProgress_OnProgressChanged(object sender, IHasProgress.OnProgressChangedEventArgs e) {
        barImage.fillAmount = e.progressNormalized;

        if (e.progressNormalized == 0 || e.progressNormalized == 1) {
            Hide();
        }
        else {
            Show();
        }
    }

    private void Show() {
        gameObject.SetActive(true);
    }

    private void Hide() {
        gameObject.SetActive(false);
    }
}
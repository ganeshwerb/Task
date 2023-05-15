using System.Collections;
using UnityEngine;
using DG.Tweening;
using Oculus.Interaction.Input;

public class TeleportationPoint : MonoBehaviour
{
    [SerializeField] GameObject Player;
    [SerializeField] OVRScreenFade fader;
    Vector3 _scaleTo;
    private void Awake()
    {
        Player = FindAnyObjectByType<OVRCameraRig>().gameObject;
        fader = Player.GetComponentInChildren<OVRScreenFade>();
        _scaleTo = transform.localScale * .75f;
    }
    private void Start()
    {
        transform.DOScale(_scaleTo, 0.5f).SetEase(Ease.InSine).SetLoops(-1,LoopType.Yoyo);
    }
    public void OnSelectThis()
    {
        StartCoroutine(Teleport());
    }

    IEnumerator Teleport()
    {
        fader.FadeOut();
        yield return new WaitForSecondsRealtime(1);
        Player.transform.position = this.transform.position;
        yield return new WaitForSecondsRealtime(1);
        fader.FadeIn();
    }
}

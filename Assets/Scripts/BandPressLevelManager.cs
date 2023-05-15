using Oculus.Interaction;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Unity.VisualScripting;
using Oculus.Interaction.Input;

public class BandPressLevelManager : MonoBehaviour
{
    public static BandPressLevelManager Instance { get; private set; }
    [Header("Bool for control flow")]
    static bool _hasStarted = false;
    static bool _isPowerOff = false;
    static bool _isPulleyLocked = false;
    static bool _isLockPicked = false;
    static bool _teleportedToTable = false;
    //GameObjects
    [Header("Grabbables")]
    [SerializeField] Grabbable Pulley;
    [SerializeField] Grabbable DoorHandle;
    [SerializeField] SnapZone PadlockZone;
    [SerializeField] Grabbable Padlock;
    [SerializeField] Grabbable PressureLock;
    [SerializeField] BoxCollider Button;
    [SerializeField] HandPhysicsCapsules capsules;

    [SerializeField] Button StartButton;
    //Canvases
    [Header("Pointer Arrows")]
    [SerializeField] Transform ArrowCupboard;
    [SerializeField] Transform ArrowButton;
    [SerializeField] Transform ArrowPulley;
    [SerializeField] Transform ArrowPressureLock;
    [SerializeField] Transform ArrowSnapZone;

    [Header("Audio")]
    [SerializeField]AudioClip[] voiceOvers;
    [SerializeField]AudioSource audioSource;
    
    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(this);
        if (Instance == null)
            Instance = this;
    }

    private void Start()
    {
        audioSource = GetComponent<AudioSource>(); 
        Button.enabled = false;
        Pulley.enabled = false;
        PressureLock.enabled = false;
        PadlockZone.enabled = false;
        DoorHandle.enabled = false;
        StartCoroutine(PowerOffSequence());
        
    }
    void PlayAudio(int n)
    {
        audioSource.clip = voiceOvers[n];
        audioSource.Play();
    }
    IEnumerator PowerOffSequence()
    {
        capsules.enabled = false;
        //goto here;
        PlayAudio(0);
        yield return new WaitUntil(() => !audioSource.isPlaying);
        StartButton.enabled = true;
        yield return new WaitUntil(() => _hasStarted);
        PlayAudio(1);
        yield return new WaitUntil(() => !audioSource.isPlaying);
        yield return new WaitUntil(() => _teleportedToTable);
        capsules.enabled = true;
        yield return new WaitForSecondsRealtime(2);
        PlayAudio(2);
        //Starts
        Button.enabled = true;
        //enable button arrow
        ArrowButton.gameObject.SetActive(true);
        ArrowButton.DOMoveX(0.05f, 1f)
            .SetLoops(-1, LoopType.Yoyo)
            .SetRelative()
            .SetEase(Ease.InOutQuad);
        yield return new WaitUntil(() => _isPowerOff);
        capsules.enabled = false;
        yield return new WaitUntil(() => !audioSource.isPlaying);
        
        PlayAudio(3);

        //Power is turned Off
        //disable power button
        Button.enabled = false;
        //Disable Button Arrow
        ArrowButton.gameObject.SetActive(false);
        //enable Pulley Arrow
        ArrowPulley.gameObject.SetActive(true);

        ArrowPulley.DOMoveX(0.1f, 1f)
            .SetLoops(-1, LoopType.Yoyo)
            .SetRelative()
            .SetEase(Ease.InOutQuad);
        Pulley.enabled = true;
        yield return new WaitUntil(() => !audioSource.isPlaying);
        //enable Pulley Grabbable

        yield return new WaitUntil(() => Pulley.transform.GetComponent<OneGrabRotateTransformer>()._constrainedRelativeAngle >= Pulley.transform.GetComponent<OneGrabRotateTransformer>().Constraints.MaxAngle.Value);
        PlayAudio(4);
        //Pulley is Down
        //Disable Pulley Arrow
        ArrowPulley.gameObject.SetActive(false);
        //Disable Pulley Grabbable
        Pulley.enabled = false;
        //Enable Pressure Lock Arrow
        ArrowPressureLock.gameObject.SetActive(true);
        ArrowPressureLock.DOMoveX(0.1f, 1f)
            .SetLoops(-1, LoopType.Yoyo)
            .SetRelative()
            .SetEase(Ease.InOutQuad);
        yield return new WaitUntil(() => !audioSource.isPlaying);
        //Enable Pressure Lock
        PressureLock.enabled = true;
        yield return new WaitUntil(() => PressureLock.transform.GetComponent<OneGrabRotateTransformer>()._constrainedRelativeAngle >= PressureLock.transform.GetComponent<OneGrabRotateTransformer>().Constraints.MaxAngle.Value);
        PlayAudio(5);
        //Pressure has been Cut
        //Disable PressureLock Grabbable
        PressureLock.enabled = false;
        //Disable PressureLock Arrow
        ArrowPressureLock.gameObject.SetActive(false);
        //Enable Cupboard Arrow
        ArrowCupboard.gameObject.SetActive(true);
        ArrowCupboard.DOMoveX(0.1f, 1f)
            .SetLoops(-1, LoopType.Yoyo)
            .SetRelative()
            .SetEase(Ease.InOutQuad);
        //enable door handle
        yield return new WaitUntil(() => !audioSource.isPlaying);
        DoorHandle.enabled = true;
        yield return new WaitUntil(() => _isLockPicked);
        PlayAudio(6);
        //disable arrow cupboard
        ArrowCupboard.gameObject.SetActive(false);
        //enable Padlock Zone
        yield return new WaitUntil(() => !audioSource.isPlaying);
        PadlockZone.enabled = true;
        //Enable PadlockZone Arrow
        ArrowSnapZone.gameObject.SetActive(true);
        ArrowSnapZone.DOMoveX(0.1f, 1f)
            .SetLoops(-1, LoopType.Yoyo)
            .SetRelative()
            .SetEase(Ease.InOutQuad);
        yield return new WaitUntil(() => _isPulleyLocked);
        ArrowSnapZone.gameObject.SetActive(false);
        //Diable Padlock Grabbable
        Padlock.enabled = false;
        PlayAudio(7);
    }

    public static void TurnMachineOff()
    {
        _isPowerOff = true;
        Debug.Log("Power off");
    }
    public static void LockPulley()
    {
        _isPulleyLocked = true;
    }
    public static void LockPicked()
    {
        _isLockPicked = true;
    }
    
    public static void Started()
    {
        _hasStarted = true;
    }
    public static void TeleporttoTable()
    {
        _teleportedToTable = true;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ExitTrigger : MonoBehaviour
{
    public string exitName;
    public string toScene;
    public string toExit;
    public Transform spawnPoint;
    public Transform exitPoint;

    private BoxCollider boxCollider;

    void Start() {
        boxCollider = GetComponent<BoxCollider>();
    }

    void Update() {
    }

    private void UpdateLock() {
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Player")
            return;
        var playerController = other.GetComponent<PlayerController>();

        ExitSequence(playerController);
    }

    public void ExitSequence(PlayerController player) {

        EventBus<FadeInOutRequest>.Pub(new FadeInOutRequest() {fadeIn = true});

        var sequenceTime = 0.7f;
        if (Player.Instance.playerWasSprinting)
            sequenceTime = 0.4f;

        Player.Instance.toScene = toScene;
        Player.Instance.toExit = toExit;
        Player.Instance.playerWasSprinting = player.GetComponent<PlayerInputValues>().sprint;

        float dist = Vector3.Distance(spawnPoint.position, exitPoint.position);
        float motionSpeed = 1f;

        boxCollider.enabled = false;
        player.disableControls = true;
        player.transform.DOMove(exitPoint.position, sequenceTime).SetEase(Ease.Linear).OnComplete(()=> {
            DOTween.Kill(player.transform);
            SceneController.Instance.LoadAnyScene(toScene);
        });
        player.transform.DOLookAt(exitPoint.position, sequenceTime / 4f).SetEase(Ease.Linear);
        if (Player.Instance.playerWasSprinting)
            player.GetComponent<Animator>().SetFloat("Speed", player.SprintSpeed);
        else
            player.GetComponent<Animator>().SetFloat("Speed", player.MoveSpeed);
        player.GetComponent<Animator>().SetFloat("MotionSpeed", motionSpeed);
    }

    public void EnterSequence(PlayerController player) {
        boxCollider = GetComponent<BoxCollider>();
        var sequenceTime = 0.7f;
        if (Player.Instance.playerWasSprinting)
            sequenceTime = 0.4f;

        float dist = Vector3.Distance(spawnPoint.position, exitPoint.position);
        float motionSpeed = 1f;

        player.disableControls = true;
        boxCollider.enabled = false;
        player.transform.DOMove(spawnPoint.position, sequenceTime).SetEase(Ease.Linear).OnComplete(()=> {
            DOTween.Kill(player.transform);
            player.disableControls = false;
            boxCollider.enabled = true;
            player.GetComponent<PlayerInputValues>().sprint = Player.Instance.playerWasSprinting;
        });
        player.transform.DOLookAt(spawnPoint.position, sequenceTime / 4f).SetEase(Ease.Linear);
        if (Player.Instance.playerWasSprinting)
            player.GetComponent<Animator>().SetFloat("Speed", player.SprintSpeed);
        else
            player.GetComponent<Animator>().SetFloat("Speed", player.MoveSpeed);
        player.GetComponent<Animator>().SetBool("Grounded", true);
        player.GetComponent<Animator>().SetFloat("MotionSpeed", motionSpeed);
    }
}

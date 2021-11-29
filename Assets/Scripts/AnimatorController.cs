using UnityEngine;
using DG.Tweening;
using System.Collections.Generic;
using System.Linq;

public enum AnimationType
{
    StandUp,
    FastRun,
    RightPunch,
    LeftPunch,
    CrossPunch,
    RightLegKick,
    Death,
    Victory
}


[RequireComponent(typeof(Animator))]
public class AnimatorController : MonoBehaviour
{
    [SerializeField]
    private CapsuleCollider _collision;

    [SerializeField]
    private LayerMask _layerMask;

    private Transform _mainTransform;
    private Animator _troopAnimator;
    private Rigidbody _mainRB;

    private const float DRAG_DURING_THE_FIGTH = 100f;
    private const float ROTATE_DURATION = 0.2f;

    /// <summary>
    /// Change manualy
    /// </summary>
    public const int PUNCH_ANIM_COUNT = 4;

    #region Animation names

    private const string START_LEFT_PUNCH = "StartLeftPunch";
    private const string STOP_LEFT_PUNCH = "StopLeftPunch";

    private const string START_RIGHT_PUNCH = "StartRightPunch";
    private const string STOP_RIGHT_PUNCH = "StopRightPunch";

    private const string START_CROSS_PUNCH = "StartCrossPunch";
    private const string STOP_CROSS_PUNCH = "StopCrossPunch";

    private const string START_RIGHT_LEG_KICK = "StartRightLegKick";
    private const string STOP_RIGHT_LEG_KICK = "StopRightLegKick";

    private const string FAST_RUN_SWITCH = "FastRunSwitch";

    private const string START_STAND_UP = "StartStandUp";

    private const string DEATH = "Death";

    private const string VICTORY = "Victory";

    #endregion


    void Start()
    {
        _mainTransform = GetComponentInParent<Transform>();
        _mainRB = GetComponentInParent<Rigidbody>();
        _troopAnimator = GetComponent<Animator>();

        TurnOffCollisionCollider();
    }


    public void StartStandUpAnim()
    {
        _troopAnimator.enabled = true;

        _mainRB.gameObject.layer = LayerMask.NameToLayer("Default");

        TurnOffRagdoll();
        TurnOnCollisionCollider();
        ChangeRotation();

        StartAnim(AnimationType.StandUp);
    }


    public float GetCurrentAnimDuration()
    {
        return _troopAnimator.GetCurrentAnimatorStateInfo(0).length;
    }


    public void StartAnim(AnimationType animation)
    {
        switch (animation)
        {
            case AnimationType.RightPunch:
                _troopAnimator.SetTrigger(START_RIGHT_PUNCH);
                break;
            case AnimationType.LeftPunch:
                _troopAnimator.SetTrigger(START_LEFT_PUNCH);
                break;
            case AnimationType.CrossPunch:
                _troopAnimator.SetTrigger(START_CROSS_PUNCH);
                break;
            case AnimationType.RightLegKick:
                _troopAnimator.SetTrigger(START_RIGHT_LEG_KICK);
                break;
            case AnimationType.FastRun:
                _troopAnimator.SetBool(FAST_RUN_SWITCH, true);
                break;
            case AnimationType.StandUp:
                _troopAnimator.SetTrigger(START_STAND_UP);
                break;
            case AnimationType.Victory:
                _troopAnimator.SetTrigger(VICTORY);
                break;
            case AnimationType.Death:
                _troopAnimator.SetTrigger(DEATH);
                break;
        }
    }


    public void StopAnim(AnimationType animation)
    {
        switch (animation)
        {
            case AnimationType.RightPunch:
                _troopAnimator.SetTrigger(STOP_RIGHT_PUNCH);
                break;
            case AnimationType.LeftPunch:
                _troopAnimator.SetTrigger(STOP_LEFT_PUNCH);
                break;
            case AnimationType.CrossPunch:
                _troopAnimator.SetTrigger(STOP_CROSS_PUNCH);
                break;
            case AnimationType.RightLegKick:
                _troopAnimator.SetTrigger(STOP_RIGHT_LEG_KICK);
                break;
            case AnimationType.FastRun:
                _troopAnimator.SetBool(FAST_RUN_SWITCH, false);
                break;
        }
    }


    private void ChangeRotation()
    {
        _mainTransform.DORotateQuaternion(new Quaternion(0, transform.rotation.y, 0, transform.rotation.w), ROTATE_DURATION);
    }


    private void TurnOffRagdoll()
    {
        CharacterJoint[] characterJoint = GetComponentsInChildren<CharacterJoint>();
        FixedJoint[] fixedJoint = GetComponentsInChildren<FixedJoint>();
        Rigidbody[] rigidbody = GetComponentsInChildren<Rigidbody>();
        SphereCollider sphereCollider = GetComponentInParent<SphereCollider>();
        SphereCollider[] childSphereCollider = GetComponentsInChildren<SphereCollider>();
        BoxCollider[] childBoxCollider = GetComponentsInChildren<BoxCollider>();

        var capsuleCollider = new HashSet<CapsuleCollider>(GetComponentsInChildren<CapsuleCollider>());
        capsuleCollider.Remove(_collision);

        DestroyComponents(capsuleCollider.ToArray());
        DestroyComponents(childBoxCollider);
        DestroyComponents(childSphereCollider);
        DestroyComponents(characterJoint);
        DestroyComponents(fixedJoint);
        DestroyComponents(rigidbody);
        Destroy(sphereCollider);

        _mainRB.constraints = RigidbodyConstraints.FreezeRotation;

        _mainRB.drag = DRAG_DURING_THE_FIGTH;
    }


    private void TurnOffCollisionCollider()
    {
        _collision.enabled = false;
    }


    private void TurnOnCollisionCollider()
    {
        _collision.enabled = true;
    }


    private void DestroyComponents(Component[] items)
    {
        for (int i = 0; i < items.Length; i++)
        {
            Destroy(items[i]);
        }
    }
}

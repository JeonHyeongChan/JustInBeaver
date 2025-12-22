using UnityEngine;
using Unity.Cinemachine;

[ExecuteInEditMode]
[SaveDuringPlay]
[AddComponentMenu("")] // 메뉴에서 숨김 (Add Extension으로만 추가)
public class CinemachineLockRotation : CinemachineExtension
{
    [Tooltip("고정할 X, Y, Z 회전 각도")]
    public Vector3 m_FixedRotation = new Vector3(60f, 0f, 0f);

    protected override void PostPipelineStageCallback(
        CinemachineVirtualCameraBase vcam,
        CinemachineCore.Stage stage,
        ref CameraState state,
        float deltaTime)
    {
        // 시네머신이 카메라의 '조준(Aim)' 계산을 마친 직후에 실행됨
        if (stage == CinemachineCore.Stage.Aim)
        {
            // 계산된 회전값을 무시하고, 우리가 설정한 값으로 덮어씌움
            state.RawOrientation = Quaternion.Euler(m_FixedRotation);
        }
    }
}
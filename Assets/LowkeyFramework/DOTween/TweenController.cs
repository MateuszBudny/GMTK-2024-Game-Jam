using DG.Tweening;
using Sirenix.OdinInspector;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class TweenController : MonoBehaviour
{
    [SerializeField]
    private TweenType tweenType;
    [SerializeField]
    private float duration = 1f;
    [SerializeField]
    private Ease ease = Ease.Linear;
    [SerializeField]
    [FormerlySerializedAs("targetAsTransform")]
    private bool targetAsObject;
    [SerializeField]
    [ShowIf(nameof(ShowLocal))]
    [Tooltip("This does nothing for scaling tweens - all scale tweens are local.")]
    private bool local;
    [SerializeField]
    [FormerlySerializedAs("useThisTransformToTween")]
    private bool useThisObjectToTween = true;

    [SerializeField]
    [ShowIf(nameof(ShowTransformToTween))]
    private Transform otherTransformToTween;

    [SerializeField]
    [ShowIf(nameof(ShowColorComponentType))]
    private ColorComponentType colorComponentType;
    [SerializeField]
    [ShowIf(nameof(ShowTextMeshProColorComponentToTween))]
    private TMP_Text otherTextMeshProToTween;
    [SerializeField]
    [ShowIf(nameof(ShowSpriteColorComponentToTween))]
    private SpriteRenderer otherSpriteToTween;
    [ShowIf(nameof(ShowImageColorComponentToTween))]
    private Image otherImageToTween;

    [BoxGroup("Target")]
    [SerializeField]
    [ShowIf(nameof(ShowTransformTarget))]
    private Transform targetTransform;
    [BoxGroup("Target")]
    [SerializeField]
    [ShowIf(nameof(ShowTextMeshProTarget))]
    private TMP_Text targetTextMeshPro;
    [BoxGroup("Target")]
    [SerializeField]
    [ShowIf(nameof(ShowSpriteTarget))]
    private SpriteRenderer targetSprite;
    [BoxGroup("Target")]
    [SerializeField]
    [ShowIf(nameof(ShowImageTarget))]
    private Image targetImage;

    [BoxGroup("Target")]
    [SerializeField]
    [ShowIf(nameof(ShowTargetPosition))]
    private Vector3 targetPosition;
    [BoxGroup("Target")]
    [SerializeField]
    [ShowIf(nameof(ShowUseEulerForRotation))]
    private bool useEulerForRotation;
    [BoxGroup("Target")]
    [SerializeField]
    [ShowIf(nameof(ShowTargetQuaternionRotation))]
    private Vector4 targetQuaternionRotation;
    [BoxGroup("Target")]
    [SerializeField]
    [ShowIf(nameof(ShowTargetEulerRotation))]
    private Vector3 targetEulerRotation;
    [BoxGroup("Target")]
    [SerializeField]
    [ShowIf(nameof(ShowTargetScale))]
    private Vector3 targetScale;
    [BoxGroup("Target")]
    [SerializeField]
    [ShowIf(nameof(ShowTargetColor))]
    private Color targetColor;

    public Ease Ease { get => ease; set => ease = value; }
    public Transform TargetTransform { get => targetTransform; set => targetTransform = value; }
    public Vector3 TargetPosition { get => targetPosition; set => targetPosition = value; }
    public bool UseEulerForRotation { get => useEulerForRotation; set => useEulerForRotation = value; }
    public Vector4 TargetQuaternionRotation { get => targetQuaternionRotation; set => targetQuaternionRotation = value; }
    public Vector3 TargetEulerRotation { get => targetEulerRotation; set => targetEulerRotation = value; }
    public Vector3 TargetScale { get => targetScale; set => targetScale = value; }
    public Color TargetColor { get => targetColor; set => targetColor = value; }

    private bool ShowLocal => !targetAsObject && !tweenType.HasFlag(TweenType.Scale) && !tweenType.HasFlag(TweenType.Color);
    private bool ShowTransformToTween => !useThisObjectToTween && IsTransformBasedTween;
    private bool ShowColorComponentType => tweenType.HasFlag(TweenType.Color);
    private bool ShowTextMeshProColorComponentToTween => !useThisObjectToTween && tweenType.HasFlag(TweenType.Color) && colorComponentType.HasFlag(ColorComponentType.TextMeshPro);
    private bool ShowSpriteColorComponentToTween => !useThisObjectToTween && tweenType.HasFlag(TweenType.Color) && colorComponentType.HasFlag(ColorComponentType.Sprite);
    private bool ShowImageColorComponentToTween => !useThisObjectToTween && tweenType.HasFlag(TweenType.Color) && colorComponentType.HasFlag(ColorComponentType.Image);

    private bool ShowTransformTarget => targetAsObject && IsTransformBasedTween;
    private bool ShowTextMeshProTarget => targetAsObject && tweenType.HasFlag(TweenType.Color) && colorComponentType.HasFlag(ColorComponentType.TextMeshPro);
    private bool ShowSpriteTarget => targetAsObject && tweenType.HasFlag(TweenType.Color) && colorComponentType.HasFlag(ColorComponentType.Sprite);
    private bool ShowImageTarget => targetAsObject && tweenType.HasFlag(TweenType.Color) && colorComponentType.HasFlag(ColorComponentType.Image);

    private bool ShowTargetPosition => !targetAsObject && tweenType.HasFlag(TweenType.Move);
    private bool ShowUseEulerForRotation => !targetAsObject && tweenType.HasFlag(TweenType.Rotate);
    private bool ShowTargetQuaternionRotation => !targetAsObject && !useEulerForRotation && tweenType.HasFlag(TweenType.Rotate);
    private bool ShowTargetEulerRotation => !targetAsObject && useEulerForRotation && tweenType.HasFlag(TweenType.Rotate);
    private bool ShowTargetScale => !targetAsObject && tweenType.HasFlag(TweenType.Scale);
    private bool ShowTargetColor => !targetAsObject && tweenType.HasFlag(TweenType.Color);

    private bool IsLocalTween => local && !targetAsObject;

    private Transform TransformToTween => useThisObjectToTween ? transform : otherTransformToTween;
    private TMP_Text TextMeshProToTween => useThisObjectToTween ? GetComponent<TMP_Text>() : otherTextMeshProToTween;
    private SpriteRenderer SpriteToTween => useThisObjectToTween ? GetComponent<SpriteRenderer>() : otherSpriteToTween;
    private Image ImageToTween => useThisObjectToTween ? GetComponent<Image>() : otherImageToTween;
    private bool IsTransformBasedTween => tweenType.HasFlag(TweenType.Move) || tweenType.HasFlag(TweenType.Scale) || tweenType.HasFlag(TweenType.Rotate);

    private Sequence currentSequence;

    public void Play() => Play(null);

    public void Play(Action onComplete)
    {
        currentSequence?.Kill();
        currentSequence = DOTween.Sequence();

        bool onCompleteInvoked = false;
        void onSequenceComplete()
        {
            if(onCompleteInvoked)
                return;

            onCompleteInvoked = true;
            onComplete?.Invoke();
        }

        if(tweenType.HasFlag(TweenType.Move))
        {
            Vector3 target = targetAsObject ? TargetTransform.position : targetPosition;
            if(IsLocalTween)
            {
                currentSequence.Insert(0f, TransformToTween.DOLocalMove(target, duration).SetEase(ease).OnComplete(onSequenceComplete));
            }
            else
            {
                currentSequence.Insert(0f, TransformToTween.DOMove(target, duration).SetEase(ease).OnComplete(onSequenceComplete));
            }
        }

        if(tweenType.HasFlag(TweenType.Rotate))
        {
            if(useEulerForRotation)
            {
                Vector3 target = targetAsObject ? TargetTransform.eulerAngles : targetEulerRotation;
                if(IsLocalTween)
                {
                    currentSequence.Insert(0, TransformToTween.DOLocalRotate(target, duration).SetEase(ease).OnComplete(onSequenceComplete));
                }
                else
                {
                    currentSequence.Insert(0, TransformToTween.DORotate(target, duration).SetEase(ease).OnComplete(onSequenceComplete));
                }
            }
            else
            {
                Quaternion target = targetAsObject ? TargetTransform.rotation : new Quaternion(targetQuaternionRotation.x, targetQuaternionRotation.y, targetQuaternionRotation.z, targetQuaternionRotation.w);
                if(IsLocalTween)
                {
                    currentSequence.Insert(0, TransformToTween.DOLocalRotateQuaternion(target, duration).SetEase(ease).OnComplete(onSequenceComplete));
                }
                else
                {
                    currentSequence.Insert(0, TransformToTween.DORotateQuaternion(target, duration).SetEase(ease).OnComplete(onSequenceComplete));
                }
            }
        }

        if(tweenType.HasFlag(TweenType.Scale))
        {
            Vector3 target = targetAsObject ? targetTransform.localScale : TargetScale;
            currentSequence.Insert(0, TransformToTween.DOScale(target, duration).SetEase(ease).OnComplete(onSequenceComplete));
        }

        if(tweenType.HasFlag(TweenType.Color))
        {
            Color target;
            if(colorComponentType == ColorComponentType.TextMeshPro)
            {
                target = targetAsObject ? targetTextMeshPro.color : TargetColor;
                currentSequence
                    .Insert(0, DOTween.To(() => TextMeshProToTween.color, value => TextMeshProToTween.color = value, target, duration)
                        .SetEase(ease)
                        .OnComplete(onSequenceComplete));
            }
            if(colorComponentType == ColorComponentType.Sprite)
            {
                target = targetAsObject ? targetSprite.color : TargetColor;
                currentSequence
                    .Insert(0, DOTween.To(() => SpriteToTween.color, value => SpriteToTween.color = value, target, duration)
                        .SetEase(ease)
                        .OnComplete(onSequenceComplete));

            }
            if(colorComponentType == ColorComponentType.Image)
            {
                target = targetAsObject ? targetImage.color : TargetColor;
                currentSequence
                    .Insert(0, DOTween.To(() => ImageToTween.color, value => ImageToTween.color = value, target, duration)
                        .SetEase(ease)
                        .OnComplete(onSequenceComplete));
            }
        }

        currentSequence.Play();
    }

    [Button]
    private void OpenEasesVisualization()
    {
        Application.OpenURL("https://p1-juejin.byteimg.com/tos-cn-i-k3u1fbpfcp/862a7322e00e446f806517dc8c7edf4e~tplv-k3u1fbpfcp-zoom-in-crop-mark:4536:0:0:0.image");
    }

    [Flags]
    public enum TweenType
    {
        None = 0,
        Move = 1,
        Rotate = 1 << 1,
        Scale = 1 << 2,
        Color = 1 << 3,
    }

    [Flags]
    public enum ColorComponentType
    {
        None = 0,
        TextMeshPro = 1,
        Sprite = 1 << 2,
        Image = 1 << 3,
    }
}

using System;
using UnityEngine;

public interface IItemJumpTo
{
    public void JumpTo(Vector3 target, bool scaleTo0, Action onComplete = null, bool turnOffColliders = false);
    public GameObject GameObject { get; }
}

using NUnit.Framework;
using UnityEngine;

public class ClickToMoveTests
{
    [Test]
    public void SetDestination_UpdatesTarget()
    {
        var go = new GameObject();
        var ctm = go.AddComponent<ClickToMove>();
        var newTarget = new Vector3(1, 0, 1);

        ctm.SetDestination(newTarget);

        Assert.AreEqual(newTarget, ctm.GetTargetPosition());
    }
}
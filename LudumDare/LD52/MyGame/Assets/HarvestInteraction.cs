using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HarvestInteraction : Interaction
{
    public override string Text => "Harvest";

    public override bool CanInvoke(Interactor interactor, GameObject target)
    {
        Debug.Log(interactor?.transform.Find("ItemPrototype")?.GetComponentInChildren<Scythe>());
        return interactor?.GetComponentInChildren<Scythe>() != null
            && target?.GetComponent<Harvestable>() != null;
    }

    public override void Invoke(Interactor interactor, GameObject target)
    {
        target.GetComponent<Harvestable>().Harvest();
    }
}

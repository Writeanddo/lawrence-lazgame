using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Libs.Base.Extensions;
using UnityEngine;

public class PlantInteraction : Interaction
{
    public override string Text => "Plant {held}";
    public GameObject PlantedSeedPrefab;

    public override bool CanInvoke(Interactor interactor, GameObject target)
    {
        TextTerm.Set("{held}", GetComponent<Label>()?.Text);
        return true;
    }

    public override void Invoke(Interactor interactor, GameObject target)
    {
        var holder = interactor.GetComponentInChildren<Holder>();
        if (holder == null)
        {
            return;
        }

        var item = holder.Items.FirstOrDefault(item => item.GetComponent<Label>().Text == "seed");
        if (item == null)
        {
            Debug.Log("Why this happens? :/");
            return;
        }

        var result = Instantiate(PlantedSeedPrefab);
        result.transform.position = transform.position;
        result.EnableAllComponentsInChildren<Collider2D>();

        holder.TryDrop(item.GetComponent<Pickable>());
        Destroy(item);
    }
}

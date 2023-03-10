using UnityEngine;

[RequireComponent(typeof(HealthBehaviour))]
[RequireComponent(typeof(AgingBehaviour))]
[RequireComponent(typeof(ReproductionBehaviour))]
[RequireComponent(typeof(HungerBehaviour))]
[RequireComponent(typeof(MovingBehaviour))]
[RequireComponent(typeof(LocatorBehaviour))]
[RequireComponent(typeof(EatingBehaviour))]
public class HerbivoreBehaviour : MonoBehaviour
{
    [SerializeField]
    private float reproductionAge;
    [SerializeField]
    [Range(0, 1)]
    private float reproductionHunger;
    [SerializeField]
    private float reproductionRange;
    [SerializeField]
    private float reproductionRate;
    [SerializeField]
    [Range(0, 1)]
    private float starvingThreshold;
    [SerializeField]
    private float eatRange;
    [SerializeField]
    private float dangerRange;

    private AgingBehaviour age;
    private HungerBehaviour hunger;
    private MovingBehaviour move;
    private LocatorBehaviour locator;
    private ReproductionBehaviour reproduce;
    private Transform herbivores;
    private EatingBehaviour eating;
    private Transform plants;

    private void OnEnable()
    {
        age = GetComponent<AgingBehaviour>();
        hunger = GetComponent<HungerBehaviour>();
        move = GetComponent<MovingBehaviour>();
        locator = GetComponent<LocatorBehaviour>();
        reproduce = GetComponent<ReproductionBehaviour>();
        herbivores = GameObject.FindGameObjectWithTag("HerbivoreContainer").transform;
        eating = GetComponent<EatingBehaviour>();
        plants = GameObject.FindGameObjectWithTag("PlantContainer").transform;
    }

    private void Start()
    {
        InvokeRepeating("ChooseBehaviour", 1, 1 + Random.Range(0f, 1f));
    }

    private void ChooseBehaviour()
    {
        if (GetComponent<SleepingBehaviour>() != null)
            return;

        GameObject danger = locator.LocateByTag("Carnivore", dangerRange);
        if (danger != null)
        {
            move.Flee = true;
            move.TargetTransform = danger.transform;
            eating.Target = null;

            return;
        }
        move.Flee = false;

        if (ShouldReproduce())
        {
            eating.Target = null;
            GameObject mate = locator.LocateByTag("Herbivore", reproductionRange);
            if (mate != null)
            {
                reproduce.Reproduce();
                move.TargetTransform = null;
            }
            else
            {
                move.TargetTransform = LocatorBehaviour.GetClosest(transform, herbivores);
            }
        }
        else if (IsHungry())
        {
            GameObject food = locator.LocateByTag("Plant_Food", eatRange);
            if (food != null)
            {
                eating.Target = food.transform;
            }
            else
            {
                move.TargetTransform = LocatorBehaviour.GetClosest(transform, plants);
            }
        }
    }

    private bool IsHungry()
    {
        return (hunger.Hunger / hunger.HungerCap) <= starvingThreshold;
    }

    private bool ShouldReproduce()
    {
        return reproductionAge <= age.Age 
            && (hunger.Hunger / hunger.HungerCap) >= reproductionHunger
            && reproduce.LastTimeReproduced + reproductionRate <= Time.time;
    }
}

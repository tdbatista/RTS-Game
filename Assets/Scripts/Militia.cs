using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;

public class Militia : UnitData
{

    public bool SeekEnemys = false;
    //float detectionDelayTime =0.5f;
    //private float detectionDelayTimeCount;

     
    
    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        //detectionDelayTimeCount = 0;   
        
        
    }

    // Update is called once per frame
    public override void Update()
    {
        

    }
    public UnitData pickNearbyEnemys(){
        Collider[] hits = Physics.OverlapSphere(transform.position, LOS, LayerMask.GetMask("Units"));
        UnitData nearestTarget = null;
        foreach (Collider hit in hits){
            UnitData dataTarget = hit.gameObject.GetComponent<UnitData>();
            if(dataTarget.unitTeam!= unitTeam)
                if (nearestTarget == null)
                    nearestTarget = dataTarget;
                else{
                    Vector3 nearestTargetDist = nearestTarget.transform.position - transform.position;
                    Vector3 newTargetDist = hit.transform.position - transform.position;
                    if (newTargetDist.sqrMagnitude< nearestTargetDist.sqrMagnitude ){ //verifica quem ta mais próximo
                        nearestTarget = dataTarget;
                    }
                }

        }
        return nearestTarget;
    }



    


    public void Hit(){
        if (target!=null)
            target.TakeDamage(this,10);
    }

    public void End(){
        //GetComponent<Animator>().SetBool("Attacking", false);
    }

   
}

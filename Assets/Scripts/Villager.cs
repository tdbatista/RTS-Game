using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Villager : Militia
{
    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    public override void Update()
    {
        
    }

    public void BuildHit(){
        if (target!=null){
            if (target is Building){
                Building building = (Building)target;
                building.Build();
            }else{
                Debug.Log("Não é construção");
            }
        }
            

    }

}

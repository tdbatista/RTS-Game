using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Player : MonoBehaviour
{
    public List<UnitData> units =  new List<UnitData>();
    public Color color = Color.blue;
    public int team =1;
    public string playerName;
    // Start is called before the first frame update
    
    public List<GameObject> AvaliableBuildings = new List<GameObject>();
    public List<GameObject> AvaliableUnits = new List<GameObject>();

    public List<UnitData> UnitsSelected = new List<UnitData>();

    public void placeConstruction(GameObject building,Vector2 position){
        
        
        GameObject new_building = GameServer.Instance.SpawUnit(building,position,this);
        new_building.layer = LayerMask.NameToLayer("Units");
        new_building.GetComponentInChildren<MeshRenderer>().material.color = Color.white;
        new_building.GetComponent<Building>().buildingState = BuildingState.inConstruction;
    }


    public void MoveUnitsToTarget(UnitData target){
        foreach (UnitData unit in UnitsSelected){
            NavMeshAgent agent = unit.GetComponent<NavMeshAgent>();
            Animator animator = unit.GetComponent<Animator>();
            foreach (AnimatorControllerParameter controller in animator.parameters){
                animator.SetBool(controller.name,false);
            }
            if (agent!=null){
                agent.SetDestination(target.transform.position); 
                animator.SetBool("Following",true);                  
            }
            unit.target = target;
        }
    }

    public void MoveUnitsToPoint(Vector3 point){
        NavMeshHit navMeshHit;
        if (NavMesh.SamplePosition(point,out navMeshHit,10.0f,NavMesh.AllAreas)){
            Vector3 walkablePosition =  navMeshHit.position;
            foreach (UnitData unit in UnitsSelected){
                NavMeshAgent agent = unit.GetComponent<NavMeshAgent>();
                Animator animator = unit.GetComponent<Animator>();
                foreach (AnimatorControllerParameter controller in animator.parameters){
                    animator.SetBool(controller.name,false);
                }
                if (agent!=null){
                    agent.SetDestination(walkablePosition); 
                    animator.SetBool("Moving",true);               
                }
                unit.target = null;
            }   
        }else{
            Debug.Log("Local Inválido!");
        }
    }
    
}

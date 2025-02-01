using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityEditor;
public enum BuildingState {

    inConstruction,
    busy,
    ghost
}
public class Building : UnitData
{
    public GameObject construction;
    
    public GameObject MainBuilding;
    
    public float BuildingPoints = 10;
    private MeshFilter currentMesh;
    private MeshRenderer buildingRenderer;
    private float constructionMeshRange;
    private UnitData unitData;
    private BuildingState _buildingState;
    public BuildingState buildingState {get => _buildingState; set{
            
            
            
            _buildingState = value;
            
            
        } 
    }
    override public void Start()
    {
        
        buildingRenderer = MainBuilding.GetComponent<MeshRenderer>();
        currentMesh = MainBuilding.GetComponent<MeshFilter>();
        //divide a quantidade de HP para cada mesh 
       /* if(constructionMesh.Count==0){
            constructionMesh.Add(currentMesh.mesh);
        }*/

        if (buildingState == BuildingState.inConstruction){
                CanDie = false;
                construction.SetActive(true);
                CurrentHP = 0;
                GetComponent<NavMeshObstacle>().enabled = true;
                buildingRenderer.material.SetFloat("_ClipHeight",0.1f);
                 

            // Itera sobre as propriedades
                
                //currentMesh.mesh = constructionMesh.First<Mesh>();
        }
        //constructionMeshRange = MaxHP / constructionMesh.Count;
       // buildingState = BuildingState.inConstruction;
    }
    
    // Update is called once per frame
    /*void Update()
    {
        
        if( buildingState==BuildingState.inConstruction){
            //changeMesh();
             
        }    
    } */

    public void Build(){
        if (buildingState == BuildingState.inConstruction){
            CurrentHP += (int)BuildingPoints;
            //vai suar a quantidade de HP para dividir pelo HP atual e vai retornar o index do mesh que deve ficar
            //se naõ tiver mesh por etapa, vai ficar o mesh 0 mesmo
            
            if (CanDie==false){
                CanDie = true;
                gameObject.layer = LayerMask.NameToLayer("Units");
            }
                
            if (CurrentHP>= MaxHP){
                buildingState = BuildingState.busy;
                CurrentHP= MaxHP;
                construction.SetActive(false);
                
            }else {
                //colocado no else para não dar outofindex quando o HP passar do max
                //currentMesh.mesh = constructionMesh[(int)Mathf.Floor(CurrentHP/constructionMeshRange)];
                buildingRenderer.material.SetFloat("_ClipHeight",(float)CurrentHP/(float)MaxHP * currentMesh.mesh.bounds.size.y);
            }
        }
    }


}

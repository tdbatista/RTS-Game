
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using UnityEngine.UI;

public class HumanControl : MonoBehaviour
{
    
    public Player player;
    public List<GameObject> selectedUnits = new List<GameObject>();

    public RectTransform selectionBox;

    public RectTransform buildingMenu;
    public Button buildingButton;
    public Button disponibleBuilding;
    public List<Button> disponibleBuildingButtons = new List<Button>();
    private Vector2 boxStartPos;
    private GameObject buildingGhost;
    // Start is called before the first frame update
    public UnityEvent buildingButtonClick = new UnityEvent(); 

    void Start()
    {
        
        buildingButton.onClick.AddListener(delegate
            {
                buildingMenu.gameObject.SetActive( !buildingMenu.gameObject.activeSelf);
                buildingButton.gameObject.SetActive(false);
                loadBuildingButtons();
                
            });
    }

    // Update is called once per frame
    void Update()
    {
        if (buildingGhost!=null){
            placeBuildingPosition();
            
        }

        MouseSelection();
        MouseMovement();
        BoxSelection();
    }

    void MouseSelection(){
        if(Input.GetMouseButtonDown(0)){
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Input.GetKey(KeyCode.LeftShift)||Input.GetKey(KeyCode.RightShift)){
                            //adiciona normal
            }else{
                DeselectAll();
            }
            if (Physics.Raycast(ray,out hit, Mathf.Infinity,LayerMask.GetMask("Units"))){
                
                UnitData unitData = hit.transform.GetComponent<UnitData>();                
                if (unitData!=null){
                    if (unitData.Owner==player){       
                       
                        unitData.Select();

                        
                    }                    
                }
            }
        }
    }

    void placeBuildingPosition(){
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray,out hit,Mathf.Infinity, LayerMask.GetMask("Ground"))){
            buildingGhost.transform.position = hit.point;
            Collider[] colliders = Physics.OverlapBox(
                                                        hit.point,
                                                        buildingGhost.GetComponent<BoxCollider>().size,
                                                        Quaternion.identity,
                                                        LayerMask.GetMask("Units"));
            if (colliders.Length>0){
                buildingGhost.GetComponentInChildren<MeshRenderer>().material.color = Color.red;
            }else {
                buildingGhost.GetComponentInChildren<MeshRenderer>().material.color = Color.green;
                if (Input.GetMouseButton(0)){
                    buildingGhost.GetComponent<Building>().buildingState = BuildingState.inConstruction;
                    buildingGhost.GetComponent<UnitData>().CurrentHP = 0;
                    player.placeConstruction(buildingGhost,new Vector2(hit.point.x, hit.point.z));
                    Destroy(buildingGhost);
                    buildingGhost = null;
                    Debug.Log("Colocado!");
                    buildingButton.gameObject.SetActive(true);
                }
            }           
        }  
    }
    


    void DeselectAll(){
        //Inspector.Instance.Unspect();
         
        foreach(UnitData unit in player.units){
            if(unit!=null){    
                if (unit.selected){
                    unit.Select(); 
                }
            }
        }
        Inspector.Instance.Unspect(); 
             
    }  

    void MouseMovement(){
        if(Input.GetMouseButtonDown(1)){
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Input.GetKey(KeyCode.LeftShift)||Input.GetKey(KeyCode.RightShift)){
                            //adiciona normal
            }else{
                //DeselectAll();
            }
            if (Physics.Raycast(ray,out hit, Mathf.Infinity,LayerMask.GetMask("Units","Ground"))){
                
                UnitData unitData = hit.transform.GetComponent<UnitData>();  
                
                if (unitData!=null){
                    player.MoveUnitsToTarget(unitData);
                }else {
                    player.MoveUnitsToPoint(hit.point);
                }
            }            
        }        
    } 

    void BoxSelection(){
        if (Input.GetMouseButtonDown(0)){
            selectionBox.gameObject.SetActive(true);
            boxStartPos = Input.mousePosition; 
        }
        if (Input.GetMouseButton(0)){
            float width =  Input.mousePosition.x - boxStartPos.x;
            float height = Input.mousePosition.y - boxStartPos.y;
            selectionBox.sizeDelta = new Vector2(Mathf.Abs(width),Mathf.Abs(height));
            selectionBox.anchoredPosition = boxStartPos + new Vector2( width/2, height/2);


        }

        if (Input.GetMouseButtonUp(0)){
            
            Vector2 min = selectionBox.anchoredPosition - selectionBox.sizeDelta/2;
            Vector2 max = selectionBox.anchoredPosition + selectionBox.sizeDelta/2;
            selectionBox.gameObject.SetActive(false);
            foreach(UnitData unit in player.units){
                if (unit!=null){
                    Vector3 screenPos = GetComponent<SpectatorMovement>().camera.WorldToScreenPoint(unit.transform.position);
                    if (screenPos.x > min.x &&  screenPos.x < max.x && screenPos.y > min.y && screenPos.y < max.y){
                        if(!unit.selected)
                            unit.Select();
                    }

                }
            }

        }
    }

    void loadBuildingButtons(){
        foreach(Button bt in disponibleBuildingButtons){
            Destroy(bt);
        }
        disponibleBuildingButtons.Clear();
        if(player!=null){
            
            foreach(GameObject buildings in player.AvaliableBuildings){
            
                Button newBt= Instantiate(disponibleBuilding,buildingMenu.transform);
                if (disponibleBuildingButtons.Count>0){
                    float atualPos = newBt.GetComponent<RectTransform>().position.x;
                    float size = newBt.GetComponent<RectTransform>().sizeDelta.x;
                    newBt.GetComponent<RectTransform>().position =new Vector3(atualPos+size,0,0);
                    newBt.GetComponent<RectTransform>().pivot = new Vector2(0.0f,0.0f);                                        

                }
                newBt.GetComponentInChildren<Text>().text = buildings.GetComponent<UnitData>().name;
                disponibleBuildingButtons.Add(newBt);
                //disponibleBuilding.onClick.RemoveAllListeners();
                newBt.onClick.AddListener(delegate{ 
                        GameObject gameObject = buildings;
                        buildingMenu.gameObject.SetActive(false);
                        chooseLocalToPlaceConstruction(gameObject);
                    }
                );
                
            }
        }
    }

    void chooseLocalToPlaceConstruction(GameObject building){
        Debug.Log(building.name);
        
        buildingGhost = Instantiate(building);
        buildingGhost.GetComponent<NavMeshObstacle>().enabled = false;
        buildingGhost.gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
        GameServer.changeLayer(buildingGhost, LayerMask.NameToLayer("Ignore Raycast"));
        buildingGhost.GetComponent<Building>().construction.SetActive(false);
    }
}

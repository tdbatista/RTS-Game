using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Inspector : MonoBehaviour
{
    
    public static Inspector Instance;
    private UnitData inspected;

    public GameObject inspectPanel;

    HumanControl playerControl;

    
    // Start is called before the first frame update
    void Start()
    {
        playerControl = GetComponent<HumanControl>();
    }
    
     private void Awake()
    {
        // Verifica se já existe uma instância
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // Destroi o novo objeto para garantir uma única instância
            return;
        }

        // Define a instância atual
        Instance = this;

        // Opcional: Impede que o objeto seja destruído ao trocar de cena
        DontDestroyOnLoad(gameObject);
    }
    // Update is called once per frame
    void Update()
    {   
        
        if (playerControl.selectedUnits.Count>0){
            Inspect(playerControl.selectedUnits[0].GetComponent<UnitData>());
        }

        if (Input.GetMouseButtonDown(0)){
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray,out hit, Mathf.Infinity,LayerMask.GetMask("Units"))){
                    
                    UnitData unitData = hit.transform.GetComponent<UnitData>();                
                    if (unitData!=null){
                        Inspect(unitData);                    
                    }
                }

        }



        if (inspectPanel.activeSelf){
         //   if(inspected==null)                       
         //       return;
             UnitStatus unitStatus = inspectPanel.GetComponent<UnitStatus>();
             unitStatus.unitName.text = inspected.unitClass.ToString();
             if (inspected.Owner!=null){
                unitStatus.playerOwner.text = inspected.Owner.name;
             }else{
                unitStatus.playerOwner.text = "Nature";
             }
             
             unitStatus.healthBar.value = (float)inspected.CurrentHP / (float)inspected.MaxHP;
            /*Vector3 screenPosition = Camera.main.WorldToScreenPoint(inspected.transform.position); 
            if (screenPosition.x >= 0 && screenPosition.x <= Screen.width &&
                screenPosition.y >= 0 && screenPosition.y <= Screen.height)
            {
                RectTransform panelRectTransform = unitStatus.hintBox;
                  screenPosition.z = 0;
                panelRectTransform.anchoredPosition = screenPosition;
    // Atualize a posição do HintBox
            }        
                   // Atualize a posição do painel no Canvas */
            
        }


    }

    public void Inspect(UnitData unitData){
        Unspect();
        inspected = unitData;
        unitData.Cursor.SetActive(true);
        inspectPanel.SetActive(true);
       
       
        //unitData.outline.OutlineMode = AssetOutline.Outline.Mode.OutlineVisible;
        //unitData.outline.enabled = true;

    }

    public void Unspect(){
        inspectPanel.SetActive(false);
        if (inspected!=null ){
            inspected.Cursor.SetActive(false);
            //if (!inspected.selected)
                //inspected.outline.enabled=false;
           //inspected.outline.OutlineMode = AssetOutline.Outline.Mode.OutlineHidden;
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.AI;



public enum UnitType
{
    Target,
    Resource,
    Unit,
    Building
}

public enum UnitClass
{
    None,
    Villager,
    Infantry,
    Cavalary,
    Artillary,
    Ship,
    Cleric,
    Fortification,
    FoodResource,
    WoodResource,
    GoldResource,
    TradePoint,
    ExternalTravelPoint,
    Buiding
}


public class UnitData : MonoBehaviour
{
    public UnitType unitTipe;
    public UnitClass unitClass;

    public bool CanDie;
    private bool showHelthBar;
    //public AssetOutline.Outline outline;
    public GameObject dieUnit;
    
    //public Slider healthBar; 
    private float hpRatio =1;
    public bool selected;
    [SerializeField]
    private Player _owner;
    public Player Owner{get => _owner; set {
        _owner = value;
        value.units.Add(this);
        if (Cursor!= null){
            Cursor.GetComponent<MeshRenderer>().material.color = value.color;
        }
    }}

    public GameObject Cursor;

    [SerializeField]
    private int _unitTeam;
    public int unitTeam { get => _unitTeam; set {
        _unitTeam = value;
       
    }}
    
    public int MaxHP;
     [SerializeField]
    private int _CurrentHP;
    public int CurrentHP { get =>_CurrentHP; set {
        hpRatio = (float)_CurrentHP/(float)MaxHP;
        //_CurrentHP = value;
    /*    if (value <1){
            healthBar.enabled = true;
        } */
      /*  if(value<=0){
            if(CanDie){
                Die();
            }
            
        }*/

        _CurrentHP = value;
    }}

    public UnitData target;

    private bool IsCloseTarget;
    public float LOS;
    
    public float agentRadius;
    private NavMeshAgent agent;
    
    // Start is called before the first frame update
    public virtual void Start()
    {
        CurrentHP = MaxHP;
        Owner = _owner;
       
        //gameObject.layer = LayerMask.NameToLayer("Units");
        //outline = gameObject.GetComponent<AssetOutline.Outline>();
        //outline.OutlineMode =  AssetOutline.Outline.Mode.OutlineHidden;
        if (unitTipe == UnitType.Unit){
            gameObject.layer = LayerMask.NameToLayer("Units");
        }
        agent = GetComponent<NavMeshAgent>();
        agentRadius = agent.radius +agent.stoppingDistance;
        
    }

    // Update is called once per frame
    public virtual void Update()
    {
        if(agent) //a unidade estando fora do lugar que pode andar, é deletado
            if (agent.navMeshOwner==null){
                
                Owner.units.Remove(this);
                Destroy(gameObject);
            }
        // healthBar.gameObject.SetActive(showHelthBar && ((hpRatio< 1) || selected));
   //     if (healthBar.enabled ){
   //        showHPBar();
//
   //     }
            

    }

    void OnBecameInvisible(){
   //     showHelthBar = false;
        
        
    }

    void OnBecameVisible(){
        
        
  //      showHelthBar = true;
    }
    public void showHPBar(){
        BoxCollider boxCollider = GetComponent<BoxCollider>();
        Vector3 screenPosition = Camera.main.WorldToScreenPoint(transform.position + boxCollider.bounds.extents);
        if (screenPosition.x < 0 || screenPosition.x>Screen.width || screenPosition.y <0 || screenPosition.y >Screen.height){
            //healthBar.enabled = false;
            
            return;            
        }else{
            if (hpRatio==1){
               // healthBar.enabled = true;
            }

           
        }

        //RectTransform rectTransform = healthBar.GetComponent<RectTransform>();
        //rectTransform.anchoredPosition = screenPosition;
        //healthBar.value = hpRatio;

    }
    public virtual void TakeDamage(UnitData owner, float damage){
        if (target==null){
            target = owner;
        }
        CurrentHP-= (int)Mathf.Floor(damage);
        if (CurrentHP<=0)
            {
                //a implementar estado de morrer
                Die();
            } 
        
    }

    void OnMouseEnter(){
      //  Inspector.Instance.Inspect(this);
//        outline.enabled = true;
    }

    void OnMouseExit(){
        
         //   Inspector.Instance.Unspect();
    }

    void Die(){
        if (Owner!=null){
            //remove da lista de unidades selecionadas
            if (Owner.UnitsSelected.Contains(this))
                Owner.UnitsSelected.Remove(this);
            //remove da lista de unidades
            if (Owner.units.Contains(this))
                Owner.units.Remove(this);
        }
        Destroy(gameObject);
    }

    public bool detectTargetIsClose(){
        Collider[] hits = Physics.OverlapSphere(transform.position, agentRadius, LayerMask.GetMask("Units"));
        foreach (Collider hit in hits){
            UnitData dataTarget = hit.gameObject.GetComponent<UnitData>();
            if(dataTarget==target){
                return true;
            }
        }
        
        return false;

        
    }

    public void Select(){
        if (Owner){
            if (Owner.UnitsSelected.Contains(this)){
                selected = false;
                Owner.UnitsSelected.Remove(this);
            }else {
                selected = true;
                Owner.UnitsSelected.Add(this);
            }
            Cursor.SetActive(selected);
        }

    }
    
    
}



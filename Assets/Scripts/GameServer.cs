using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AssetOutline;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;



public class GameServer : MonoBehaviour
{
    public const int LAYER_NATURE =3;
    public GameObject unitBlue;
    public GameObject unitRed;
    public int UNITS_QTD;
     
    
    float range = 100;
    // Start is called before the first frame update   
    public static GameServer Instance { get; private set; }

    [SerializeField]
    public List<Player> players = new List<Player>();
    public ConcurrentQueue<Action> detectionThreadQueue = new ConcurrentQueue<Action>();
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

    void Start()
    {
       
        range =  Terrain.activeTerrain.terrainData.size.x;
        
        
        
        /*
        for(int i=0;i<UNITS_QTD;i++){
            
            Vector2 random_pos = new Vector2(Random.Range(0, range), Random.Range(0, range/2));
            GameObject new_unit = SpawUnit(unitBlue,random_pos , 1);
            player1Units.Add(new_unit);

        }
        for(int i=0;i<UNITS_QTD;i++){
            
            Vector2 random_pos = new Vector2(Random.Range(0, range), Random.Range( range/2, range));
            GameObject new_unit = SpawUnit(unitRed,random_pos ,2);
            player2Units.Add(new_unit);
           
        }*/

       
    }

    
    // Update is called once per frame
    void Update()
    {
        
        for(int i = 0; i<100; i++){
            if (UNITS_QTD<0) return;
            range =  Terrain.activeTerrain.terrainData.size.x;
            UNITS_QTD-=1;
            Vector2 random_pos = new Vector2(Random.Range(0, range), Random.Range(0, range));
            Player player = players[Random.Range(0,2)];

            SpawUnit(unitBlue,random_pos ,player);
            random_pos = new Vector2(Random.Range(0, range), Random.Range(0, range));
            SpawUnit(unitRed,random_pos ,player);
        }

        

    }

    

    public GameObject SpawUnit(GameObject unit, Vector2 local,Player player = null){
        Vector3 position = MapPosition(local);
        if (position == Vector3.negativeInfinity){
            Debug.Log("Local Inválido.");
            return null;
        }
        GameObject new_unit = Instantiate(unit,MapPosition(local),Quaternion.identity);
        if (new_unit!=null){
            if (player!=null){
                UnitData unitData = new_unit.GetComponent<UnitData>();
                if (unitData){
                    new_unit.GetComponent<UnitData>().Owner = player;  
                    new_unit.GetComponent<UnitData>().unitTeam = player.team;
                    new_unit.GetComponent<UnitData>().Cursor.GetComponent<MeshRenderer>().material.color = player.color;
                    //new_unit.GetComponent<UnitData>().outline.OutlineColor = player.color; 
                    player.units.Add(new_unit.GetComponent<UnitData>());

                }else{
                    Debug.Log("Não há UnitData como componente, a unidade não poderá ser utilizada ou selecionada");
                }
                
                
            }else {
                Debug.Log("A unidade não tem um player, será como Nature.");
                //new_unit.GetComponent<UnitData>().outline.OutlineColor = Color.white;  
            }
            
            
            
        }else {
            Debug.Log("Erro ao criar nova unidade");
        }
        return new_unit;
    }

    public static Vector3 MapPosition(Vector2 local){
        Vector3 local3d = new Vector3(local.x,0,local.y);
        local3d.y = Terrain.activeTerrain.SampleHeight(local3d);

        NavMeshHit navMeshHit;
        if (NavMesh.SamplePosition(local3d,out navMeshHit,10.0f,NavMesh.AllAreas)){
            return navMeshHit.position;
        }else{
            return Vector3.negativeInfinity;
        }
        
    }

    public static void changeLayer(GameObject gameobject, int layer){
        gameobject.layer = layer;
        foreach (Transform child in gameobject.transform){
            child.gameObject.layer = layer;
        }
    }

    public void MoveUnitToPoint(GameObject unit, Vector2 local){
        //var unitCtrl = unit.GetComponent<Unit>();
        //unitCtrl.MoveToPoint(MapPosition(local));
    }

    public void MoveToTarget(GameObject unit, GameObject target){
       // var unitCtrl = unit.GetComponent<Unit>();        
        //unitCtrl.MoveToTarget(target);
        
    }

    
}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class LightCreator : MonoBehaviour {

	float RAYLENGTH = 25f;
	public int RAYCOUNT;
	public float RAYMAXSPACING;
	public float RAYMAXANGLE;
	public float SKIN = .1f;
	
	public bool canLight;	
	
	public LayerMask Wall;
	public LayerMask Switch;
	
	public bool CORRECT;

	Vector3[] startPoints;
	Vector3[] endPoints;
	Dictionary<int, Dictionary<int, Vector3>> postReflectPoints = new Dictionary<int, Dictionary<int, Vector3>>();

	public GameObject pontinho;
	public GameObject pontinho2;

	float MaxDistPoints;

	// Use this for initialization
	void Start () {
		MaxDistPoints = RAYLENGTH * RAYMAXSPACING/6;
	}
	
	// Update is called once per frame
	void Update () {
		CreateMesh();
		CheckSwitches();
	}
	
	void CheckSwitches(){
		int i, j;
		Dictionary<int, Dictionary<int, Vector3>> reflectionPoints = new Dictionary<int, Dictionary<int, Vector3>>();;	
		j=0;
		
		if(canLight){

			//Add startPoints
			reflectionPoints[0] = new Dictionary<int, Vector3>();
			for(i=0; i<RAYCOUNT; i++){
				reflectionPoints[0][i] = startPoints[i];
			}
	
			//Add endPoints
			reflectionPoints[1] = new Dictionary<int, Vector3>();
			for(i=0; i < endPoints.Length; i++){
				reflectionPoints[1][i] = endPoints[i];
			}			

			//Add reflectPoints
			for(j=0; j < postReflectPoints.Count; j++){
				reflectionPoints[j+2] = new Dictionary<int, Vector3>();
				for(i=0; i<RAYCOUNT; i++){
					if(postReflectPoints[j].ContainsKey(i))
						reflectionPoints[j+2][i] = postReflectPoints[j][i];
				}
			}
	
			Ray r;		
			RaycastHit[] hs;
			
			//Find switches
			for(j=0; j < reflectionPoints.Count-1; j++){
				for(i=0; i<RAYCOUNT; i++){
					if(reflectionPoints[j].ContainsKey(i) && reflectionPoints[j+1].ContainsKey(i) ){
						r = new Ray(transform.position + transform.right * reflectionPoints[j][i].x + transform.up * reflectionPoints[j][i].y , 
						            Quaternion.Euler(0,0,transform.rotation.eulerAngles.z) * (reflectionPoints[j+1][i] - reflectionPoints[j][i]).normalized);
					
							
						Vector3 spoint = reflectionPoints[j][i];				
						Vector3 vec3Aux;

						hs = Physics.RaycastAll(r, Mathf.Abs(Vector3.Distance(spoint ,reflectionPoints[j+1][i])), Switch);						

						for(int k=0; k < hs.Length; k++){
							hs[k].collider.gameObject.GetComponent<Switch>().React();
						}

					}

				}
			}
		}	
	}
	
	void CreateMesh(){
		int i, j;
		
		//Comecar desenho
		MeshFilter mf = GetComponent<MeshFilter>();
		Mesh m = new Mesh();
		mf.mesh = m;
		GetComponent<MeshCollider>().sharedMesh = m;
		
		if(canLight){
			//========== ENCONTRAR PONTOS!!
			//Criar pontos de partida
			startPoints = new Vector3[RAYCOUNT];
			float spacePerRay = 2*RAYMAXSPACING/(RAYCOUNT-1);
			float rotPerRay = 2*RAYMAXANGLE/(RAYCOUNT-1);
			//Dividir espaço
			startPoints[0] =  Vector3.up * RAYMAXSPACING;
			for(i=0; i<RAYCOUNT; i++)
				startPoints[i] =  Vector3.up * RAYMAXSPACING - Vector3.up * spacePerRay * i ;
			
			//=== Encontrar pontos de colisao
			Ray r;		
			RaycastHit h;
			int numReflecs = -1;
			float angle = (transform.rotation.eulerAngles.z) * Mathf.PI/180;		
			float rotInRay;
			Vector3 mirrorNormal;
			endPoints = new Vector3[RAYCOUNT];
			postReflectPoints = new Dictionary<int, Dictionary<int, Vector3>>();

			//Para cada raio do feixe de luz
			for(i=0; i<RAYCOUNT; i++){
				//Setando informações pro raycast
				//De local pra global
				rotInRay = RAYMAXANGLE - rotPerRay * i;
				r = new Ray(transform.position + transform.right * startPoints[i].x + transform.up * startPoints[i].y , 
				            transform.right * Mathf.Cos(Mathf.Deg2Rad * (rotInRay)) + transform.up * Mathf.Sin(Mathf.Deg2Rad * (rotInRay)));
				h = new RaycastHit();
				//Instantiate(pontinho2, r.origin, Quaternion.identity);//Pontinhos na origem

				//Testar Raycast no raio atual
				if(Physics.Raycast(r, out h, Mathf.Abs(RAYLENGTH), Wall)){
					//De global pra local
					endPoints[i] = new Vector3(Mathf.Cos(-angle),Mathf.Sin(-angle),0) * (h.point.x - transform.position.x) + new Vector3(Mathf.Cos(-angle + Mathf.PI/2),Mathf.Sin(-angle + Mathf.PI/2),0) * (h.point.y - transform.position.y) ;
					//Instantiate(pontinho, h.point, Quaternion.identity);//Pontinho na reflexao
					if(h.collider.gameObject.name == "Prism")
						h.collider.gameObject.GetComponent<Prism>().React();
					//Se pegou num espelho, ...
					if(h.collider.gameObject.tag == "Mirror"){
						//Entrar na funcao recursiva
						//Debug.Log("Ta ao menos triggando?");

						mirrorNormal = new Vector3();
						foreach(Transform t2 in h.collider.gameObject.GetComponentInChildren<Transform>())
							if(t2.gameObject.name == "Normal")
								mirrorNormal = t2.right;

						numReflecs = Math.Max(RecursiveReflection(0, i,  endPoints[i], 
						                                          transform.right * Mathf.Cos(Mathf.Deg2Rad * (rotInRay)) + transform.up * Mathf.Sin(Mathf.Deg2Rad * (rotInRay)),
						                                          mirrorNormal, postReflectPoints), numReflecs);
					
					}
				}else{
					endPoints[i] = startPoints[i] + Vector3.right * RAYLENGTH;
				}			
				//Debug.DrawRay(transform.position + transform.right * startPoints[i].x + transform.up * startPoints[i].y,
				  //            (transform.right * Mathf.Cos(Mathf.Deg2Rad * (rotInRay)) + transform.up * Mathf.Sin(Mathf.Deg2Rad * (rotInRay))) * Vector3.Distance(startPoints[i],endPoints[i]));

			}

			
			//========== DESENHAR!!
			
			//Vertices
			List<Vector3> vertices = new List<Vector3>();//new Vector3[2*RAYCOUNT];
			
			for(i=0; i<RAYCOUNT; i++){
				vertices.Add(startPoints[i]);					
				vertices.Add (endPoints[i]);
			}
	
			
			//Triangulos
			List<int> tri = new  List<int>();//new int[((2*RAYCOUNT)-2)*3];
			j=0;
			
			for(i=0; i<(2*RAYCOUNT)-2; i++){
				if(i%2==0){
					//Par
					tri.Add (i);
					tri.Add (i+1);
					tri.Add (i+2);
					
				}else{
					//Impar
					tri.Add (i);
					tri.Add (i+2);
					tri.Add (i+1);
				}
			}
			
			//Vetores Normais
			List<Vector3> normals = new  List<Vector3>();//new Vector3[2*RAYCOUNT];
			
			for(i=0; i<2*RAYCOUNT; i++)
				normals.Add(-Vector3.forward);
	
			
			//UV
			
			//ADICIONAR MESHES DAS REFLEXOES
			
			Dictionary<int, Vector3> reflecLayer;
			int[] previousInVert = new int[RAYCOUNT];
			//loop de reflexao
			for(i=0; i< RAYCOUNT; i++){
				previousInVert[i] = (i * 2) + 1;
			}
			
			for(i=0; i<=numReflecs; i++ ){
				reflecLayer = postReflectPoints[i] as Dictionary<int, Vector3>;
				//Loop dos raios
				for(j=0; j<RAYCOUNT; j++){
					if(reflecLayer.ContainsKey(j)){

						//Adicionar a lista de vertices
						vertices.Add((Vector3)reflecLayer[j]);
						normals.Add(-Vector3.forward);
					
				
						if(j!=0 && reflecLayer.ContainsKey(j-1) && 
						   Vector3.Distance(vertices[previousInVert[j-1]],vertices[previousInVert[j]]) < MaxDistPoints){
							//Para que dois pontos no mesmo nivel de reflexao mas incompativeis nao se conectem
	
							if(i%2==0){
								//1o tri
								tri.Add(previousInVert[j-1]);//endpoint[i-1]
								tri.Add(previousInVert[j]);//endpoint[i]
								tri.Add(vertices.Count - 2);//reflec[i-1]
			
								//2o tri
								tri.Add(vertices.Count - 2);//reflec[i-1]
								tri.Add(previousInVert[j]);//endpoint[i]
								tri.Add(vertices.Count - 1);//reflec[i]
							}else{

								//1o tri
								tri.Add(previousInVert[j-1]);//endpoint[i-1]
								tri.Add(vertices.Count - 2);//reflec[i-1]
								tri.Add(previousInVert[j]);//endpoint[i]
								
								//2o tri
								tri.Add(vertices.Count - 2);//reflec[i-1]
								tri.Add(vertices.Count - 1);//reflec[i]
								tri.Add(previousInVert[j]);//endpoint[i]
							}
							
							previousInVert[j-1] = vertices.Count - 2;
							if((j == RAYCOUNT-1) || (j != RAYCOUNT-1 && !reflecLayer.ContainsKey(j+1)))
								previousInVert[j] = vertices.Count - 1;	
	
						}
					}
				}
			}
			
			//Converter para vetores normais
			Vector3[] v = new Vector3[vertices.Count];
			int[] t = new int[tri.Count];
			Vector3[] n = new Vector3[normals.Count];
			
			
			ListToArray<Vector3>(vertices, v);
			ListToArray<int>(tri, t);
			ListToArray<Vector3>(normals, n);
			
			//Juntar tudo
			m.vertices = v;
			m.triangles = t;
			m.normals = n;	
			
			;
		}
	}
	
	//Funcao recursiva de reflexao em espelhos
	int RecursiveReflection(int reflectionLevel, int rayIndex,  Vector3 incidentPoint, Vector3 incidentDir, Vector3 normal, Dictionary<int, Dictionary<int, Vector3>> reflectionPoints){
		Ray r;
		RaycastHit h;
		Vector3 newDirection = Vector3.Reflect(incidentDir,normal);
		Vector3 nextPoint;
		Vector3 mirrorNormal;
		int numReflecs = reflectionLevel;
		float angle = transform.rotation.eulerAngles.z;
		angle *= Mathf.PI/180;		
		
		//Preparar Ray e RaycastHit pro Raycast
		r = new Ray(transform.position + transform.right * incidentPoint.x + transform.up * incidentPoint.y , 
		            newDirection);
		h = new RaycastHit();
		
		//Instantiate(pontinho2, r.origin, Quaternion.identity);
		//Debug.Log("Raio " + rayIndex);

		if(Physics.Raycast(r, out h, Mathf.Abs(RAYLENGTH), Wall)){
			//Debug.Log("Acertou Wall");
			nextPoint = new Vector3(Mathf.Cos(-angle),Mathf.Sin(-angle),0) * (h.point.x - transform.position.x) + new Vector3(Mathf.Cos(-angle + Mathf.PI/2),Mathf.Sin(-angle + Mathf.PI/2),0) * (h.point.y - transform.position.y) ;
			//Instantiate(pontinho, h.point, Quaternion.identity);//Pontinho na colisao
			if(h.collider.gameObject.tag == "Mirror" && reflectionLevel < 15){
				//Entrar noutra recursao
				mirrorNormal = new Vector3();
				foreach(Transform t2 in h.collider.gameObject.GetComponentInChildren<Transform>())
					if(t2.gameObject.name == "Normal")
						mirrorNormal = t2.right;

				numReflecs = Mathf.Max(RecursiveReflection(reflectionLevel+1, rayIndex,  nextPoint, newDirection, mirrorNormal, reflectionPoints), numReflecs);
				
			}
		}else{
			//Debug.Log("Errou Wall");
			nextPoint = incidentPoint + newDirection * RAYLENGTH;
		}

		//Debug.DrawRay(transform.position + transform.right * incidentPoint.x + transform.up * incidentPoint.y,
		  //           newDirection * Vector3.Distance(incidentPoint,nextPoint));
		
		if(!reflectionPoints.ContainsKey(reflectionLevel)){
			reflectionPoints[reflectionLevel] = new Dictionary<int, Vector3>();
		}
		Dictionary<int, Vector3> reflecLayer = reflectionPoints[reflectionLevel] as Dictionary<int, Vector3>;
		reflecLayer.Add(rayIndex, nextPoint);
		
		return numReflecs;
	}
	
	
	void ListToArray <T> (List<T> arrList, T[] arr){
		int i;
		for(i=0; i<arrList.Count; i++){
			arr[i] = arrList[i];
		}
	}

}

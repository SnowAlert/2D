using UnityEngine;
using System.Collections;

public class GeneratorKarti : MonoBehaviour
{
	public float PossX = 20;
	public float PossY = 20;
	public float PossZ = 20;
	public Transform pref1;
	public Transform pref2;
	public Transform pref3;
	public Transform pref4;
	public Transform pref5;
    public Transform in_group;

    private string Line;
	private byte counter = 0;
	private int[,] Map = new int[64, 64];
	// Массив с значением спрайтев для создания на карте

	// Если стена угловая
	//1-стена
	//2-стена угл 	■
	// 				■ ■

	//3-стена угл 	■ ■
	// 				■

	//5-стена угл 	  ■
	// 				■ ■

	//4-стена угл 	■ ■
	// 				  ■


	// Use this for initialization
	void Start ()
	{
		//Читаем файл и записываем ешл в массив
		System.IO.StreamReader file =
			new System.IO.StreamReader (@"D:\Мастерская\Unity\Unitty 5\2D\Assets\Map.txt");

		while ((Line = file.ReadLine ()) != null) 
		{
			for (int i = 0; i < Line.Length; i++) 
			{
				Map [counter, i] = Line [i];
			}
			counter++;
		}
		file.Close ();

		//проверяем весь массив на повороты стен------------------------------
		for (int g = 0; g < 63; g++)
			for (int v = 0; v < 63; v++) 
			//нашли стену
				if ((g > 0) && ((Map [v, g - 1] == '1') && (Map [v, g] >= '1') && (Map [v, g] <= '5')) & !
				    //  ?
				    //? ■ ■ 
				    //  ?
					((v > 0) && (Map [v - 1, g - 1] >= '1') && (Map [v - 1, g - 1] <= '5') && (Map [v + 1, g - 1] >= '1') && (Map [v + 1, g - 1] <= '5')) & !
				    //  ■ 
				    //? ■ ■
				    //  ■
					((v > 0) && (Map [v - 1, g] >= '1') && (Map [v - 1, g] <= '5') && (Map [v + 1, g] >= '1') && (Map [v + 1, g] <= '5')))
					//    ■ 
					//? ■ ■
					//    ■
				{

					if (v > 0 & g > 1 && (((Map [v - 1, g - 1] >= '1') && (Map [v - 1, g - 1] <= '5')) & !((Map [v, g - 2] >= '1') && (Map [v, g - 2] <= '5')))) { 
						//  ■
						//0 ■ ■ 
						//  ?

						Map [v, g - 1] = '2';
					} else if ((v > 0) && (g <= 1) && (Map [v - 1, g - 1] >= '1') && (Map [v - 1, g - 1] <= '5')) {
						// ■
						// ■ ■ 
						// ?

						Map [v, g - 1] = '2';
					}

					if (((v >= 1) && (Map [v - 1, g] >= '1') && (Map [v - 1, g] <= '5')) & !(v >= 1 && (Map [v, g + 1] >= '1') && (Map [v, g + 1] <= '5'))) {
						//    ■
						//? ■ ■ 0
						//    ?

						Map [v, g] = '5';
					}

					if (((g == 1) && (Map [v + 1, g - 1] >= '1') && (Map [v + 1, g - 1] <= '5'))
					    || ((g > 1) && ((Map [v + 1, g - 1] >= '1') && (Map [v + 1, g - 1] <= '5')) & !((Map [v, g - 2] >= '1') && (Map [v, g - 2] <= '5')))) {	
						//  ?
						//0 ■ ■
						//  ■ 

						Map [v, g - 1] = '3';
					}

					if (((Map [v + 1, g] >= '1') && (Map [v + 1, g] <= '5')) & !((Map [v, g + 1] >= '1') && (Map [v, g + 1] <= '5'))) {	//работает
						//  ?
						//? ■ ■
						//  ? ■ 

						Map [v, g] = '4';
					}
				}
	
		//После всех проверок создаем спрайты на карте---------------------------------

		for (int i = 0; i < 64; i++) {
			for (int j = 0; j < 64; j++) {

                //if (Map [j, i] == 0) {}
                if (Map[j, i] == '1')
                {
                    Transform t = Instantiate(pref1, transform.position + new Vector3(PossX * i, -PossZ * j, PossY), transform.rotation) as Transform;
                    t.parent = in_group; // group the instance under the spawner
                    t.gameObject.name = "Brick1_j" + j + ",i" + i;
                }

                else if (Map[j, i] == '2')
                {
                    Transform t = Instantiate(pref2, transform.position + new Vector3(PossX * i, -PossZ * j, PossY), transform.rotation) as Transform;
                    t.parent = in_group; // group the instance under the spawner
                    t.gameObject.name = "Brick2_j" + j+",i"+i;
                }

                else if (Map[j, i] == '3')
                {
                    Transform t = Instantiate(pref3, transform.position + new Vector3(PossX * i, -PossZ * j, PossY), transform.rotation) as Transform;
                    t.parent = in_group; // group the instance under the spawner
                    t.gameObject.name = "Brick3_j" + j + ",i" + i;
                }

                else if (Map[j, i] == '4')
                {
                    Transform t = Instantiate(pref4, transform.position + new Vector3(PossX * i, -PossZ * j, PossY), transform.rotation) as Transform;
                    t.parent = in_group; // group the instance under the spawner
                    t.gameObject.name = "Brick4_j" + j + ",i" + i;
                }

                else if (Map[j, i] == '5')
                {
                    Transform t = Instantiate(pref5, transform.position + new Vector3(PossX * i, -PossZ * j, PossY), transform.rotation) as Transform;
                    t.parent = in_group; // group the instance under the spawner
                    t.gameObject.name = "Brick5_j" + j + ",i" + i;
                }
            }
		}
	}
}

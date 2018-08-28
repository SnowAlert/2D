using UnityEngine;
using System.Collections;

public class GeneratorKartiOld : MonoBehaviour
{
	public float PossX = 20;
	public float PossY = 20;
	public float PossZ = 20;
	public Transform pref1;
    public Transform in_group;
    string local_Path;

    private string Line;
	private byte counter = 0;


	void Start ()
    {
        local_Path = GetDataPath();

        //Читаем файл и записываем ешл в массив
        System.IO.StreamReader file =
            new System.IO.StreamReader(local_Path + @"\Grass.txt");
        while ((Line = file.ReadLine()) != null)
        {
            for (int i = 0; i < Line.Length; i++)
            {
                if (Line[i] == '1')
                {

                    Transform t = Instantiate(pref1, transform.position + new Vector3(PossX * i - PossX * Line.Length / 2 + 8, -PossZ * counter + PossZ * Line.Length / 2 - 8, PossY), transform.rotation) as Transform; // instantiate prefab and get its transform
                    t.parent = in_group; // group the instance under the spawner
                    t.gameObject.name = "Grass_" + i;
                }
            }
            counter++;
        }
        file.Close();
    }

    private static string GetDataPath()
    {
        return Application.dataPath;
    }
}


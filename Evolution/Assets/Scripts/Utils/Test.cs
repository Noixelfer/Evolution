using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Test : MonoBehaviour
{
	// Start is called before the first frame update
	private Task t;
	private float time = 1f;
	private bool cancel = false;
    void Start()
    {
		Debug.Log(Time.time);
		SearchForPathCoroutine();
		Debug.Log(Time.time);
    }

    // Update is called once per frame
    void Update()
    {
    }

	async void SearchForPathCoroutine()
	{
		t = Task.Run(() => { Debug.Log("haha "); while (true) { var x = 3; if (cancel) break; } });
		await t;
		Debug.Log("Done");
	}

	private void OnApplicationQuit()
	{
		cancel = true;
	}
}

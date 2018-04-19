using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CircularProgressAsyncLoadScene : MonoBehaviour {

	//获得圆形进度条和文本
	public Image progress;
	public Text progressText;

	//进度条平滑加载速度
	private float smoothSpeed = 0.5f;

	//目标加载值和当前加载值
	private float targetValue;
	private float currentValue;

	//异步加载操作符
	private AsyncOperation asyncOperation;


	// Use this for initialization
	void Start () {
		currentValue = 0.0f;

		StartCoroutine (AsyncLoadScene());
	}

	IEnumerator AsyncLoadScene(){

		//异步加载 02Scene 场景
		asyncOperation = SceneManager.LoadSceneAsync ("02Scene");

		//异步加载未完成前，置为假，使得场景在加载未完成前不跳转
		asyncOperation.allowSceneActivation = false;

		yield return asyncOperation;
	}

	// Update is called once per frame
	void Update () {

		//目标加载值等于异步加载操作符的值	
		targetValue = asyncOperation.progress;

		//operation.progress的值最大为0.9 ，当为0.9时置目标加载值为1.0
		if(targetValue >= 0.9f) {
			targetValue = 1.0f;
		}

		//当前加载值小于目标值时，进入分支，增加当前加载值
		if(currentValue < targetValue){
			currentValue = Mathf.Lerp (currentValue, targetValue, smoothSpeed * Time.deltaTime);

			//当当前加载值等于大于目标值时，令当前加载值等于目标加载值
			if(Mathf.Abs (currentValue - targetValue) <= 0.01f){
				currentValue = targetValue;
			}

			//圆形进度条与文本显示对应更新
			progress.fillAmount = currentValue;
			progressText.text = (int)(currentValue * 100) + "%";

			print ("currentValue :"+currentValue);
		}

		//当前之等于1表明加载完毕，因为浮点数不能相等，故转为整数比较
		if((int)(currentValue * 100) == 100){

			//异步加载完成后，置为真，方可跳转场景
			asyncOperation.allowSceneActivation = true;
		}


	}
}

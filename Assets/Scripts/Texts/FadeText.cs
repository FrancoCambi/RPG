using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class FadeText : MonoBehaviour
{

	[SerializeField]
	private float timeToLive = 0.5f;

	[SerializeField]
	private float floatSpeed = 50f;

	[SerializeField]
	private Vector3 floatDirection = new Vector3(0, 1, 0);

	[SerializeField]
	private Text text;

	[SerializeField]
	private bool moveText;
	
	[SerializeField]
	private bool hasParent = false;
	private bool critText = false;
	
	private RectTransform rTransform;
	private Color startingColor;

	private float timeElapsed = 0.0f;
	
	private bool startFading = false;
	

	// Start is called before the first frame update
	void Start()
	{
		rTransform = GetComponent<RectTransform>();
		startingColor = text.color;
	}

	// Update is called once per frame
	void Update()
	{
		if (!startFading) 
		{
			
			Invoke(nameof(StartFade), 1f);
		}
		else
		{
			timeElapsed += Time.deltaTime;

			text.color = new Color(startingColor.r, startingColor.g, startingColor.b, 1 - (timeElapsed / timeToLive));

			if (timeElapsed > timeToLive && !hasParent)
			{
				Destroy(gameObject);
			}
			else if (timeElapsed > timeToLive && hasParent) 
			{
				Destroy(transform.parent.gameObject);
			}
		}
		if (rTransform != null && moveText == true)
		{
			rTransform.position += floatSpeed * Time.deltaTime * floatDirection;
		}
	}
	
	private void StartFade() 
	{
		startFading = true;
	}
	
}

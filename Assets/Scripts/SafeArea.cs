using UnityEngine;

public class SafeArea : MonoBehaviour
{
	private RectTransform rectTranform;
	private Rect safeArea;
	private Vector2 minAnchor, maxAnchor;

	private void Awake()
	{
		rectTranform = GetComponent<RectTransform>();
		safeArea = Screen.safeArea;
		minAnchor = safeArea.position;
		maxAnchor = minAnchor + safeArea.size;

		minAnchor.x /= Screen.width;
		minAnchor.y /= Screen.height;
		maxAnchor.x /= Screen.width;
		maxAnchor.y /= Screen.height;

		rectTranform.anchorMin = minAnchor;
		rectTranform.anchorMax = maxAnchor;
	}
}

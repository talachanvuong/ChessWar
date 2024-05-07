using UnityEngine;

public class DRMenuGameplay : MonoBehaviour
{
	private RectTransform rectTransform;

	private void Awake()
	{
		rectTransform = GetComponent<RectTransform>();

		float realSize = (float)(Screen.height - Screen.width) / 2;

		float size = rectTransform.rect.height / realSize;

		if (realSize < rectTransform.rect.height)
		{
			size = 1 / size;
		}

		rectTransform.localScale = new(size, size, size);
	}
}
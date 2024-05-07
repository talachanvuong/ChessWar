using UnityEngine;

public class DRMenuWaiting : MonoBehaviour
{
	private RectTransform rectTransform;

	private void Awake()
	{
		rectTransform = GetComponent<RectTransform>();

		float realSize = (float)(Screen.height * rectTransform.rect.height) / 1920;

		float size = realSize / rectTransform.rect.height;

		if (realSize < rectTransform.rect.height)
		{
			size = 1 / size;
		}

		rectTransform.localScale = new(size, size, size);
	}
}
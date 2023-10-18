using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToUIElem : MonoBehaviour {

    public RectTransform _target;
    public float _moveSpeed = 10;
    public Vector3 _targetScale = Vector3.one;
    public Color _targetColor = Color.white;

	// Use this for initialization
	void Start () {
        GetComponent<Animator>().enabled = false;
        transform.localScale = _targetScale;
        GetComponent<SpriteRenderer>().color = _targetColor;
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 worldCoords = _target.TransformPoint(new Vector3(_target.rect.center.x, _target.rect.center.y, 0));
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(worldCoords);
        transform.position = Vector3.MoveTowards(transform.position, worldPos, _moveSpeed * Time.deltaTime);
        // Destroy if target is reached
        float EPS = 1e-1f;
        if ((transform.position - worldPos).sqrMagnitude < EPS)
            Destroy(gameObject);
	}

}

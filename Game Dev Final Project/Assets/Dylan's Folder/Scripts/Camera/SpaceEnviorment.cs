using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceEnviorment : MonoBehaviour
{
    public Rigidbody2D player;
    public SpriteRenderer topLayer, bottomLayer;

    public float topLayerSpeed, bottomLayerSpeed;

    float topX, topY, bottomX, bottomY;
    Material topLayerMaterial;
    Material bottomLayerMaterial;

    public void Awake()
    {
        topLayerMaterial = topLayer.material;
        bottomLayerMaterial = bottomLayer.material;
    }

    void ScrollTopLayer()
    {
        topX += topLayerSpeed * (player.velocity.x * Time.deltaTime);
        topY += topLayerSpeed * (player.velocity.y * Time.deltaTime);
        topLayerMaterial.mainTextureOffset = new Vector2(topX / 10.0f, topY / 10.0f);
    }

    void ScrollBottomLayer()
    {
        bottomX += bottomLayerSpeed * (player.velocity.x * Time.deltaTime);
        bottomY += bottomLayerSpeed * (player.velocity.y * Time.deltaTime);
        bottomLayerMaterial.mainTextureOffset = new Vector2(bottomX / 10.0f, bottomY / 10.0f);
    }

    private void Update()
    {
        ScrollTopLayer();
        ScrollBottomLayer();
    }
}

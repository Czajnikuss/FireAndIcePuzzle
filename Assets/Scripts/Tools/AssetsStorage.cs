using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssetsStorage : MonoBehaviour
{
   
#region SINGLETON PATTERN
 public static AssetsStorage _instance;
 public static AssetsStorage Instance
 {
     get {
         if (_instance == null)
         {
             _instance = GameObject.FindObjectOfType<AssetsStorage>();
             
             if (_instance == null)
             {
                 GameObject container = new GameObject("AssetsStorage");
                 _instance = container.AddComponent<AssetsStorage>();
             }
         }
     
         return _instance;
     }
 }
 #endregion
    [Header("Materials")]
    public Material fireFadeInMaterial;
    public Material iceFadeInMaterial;
    [Space]
    [Header("Animations")]
    [Space]
    public AnimationClip fadeInAnim;

    public RuntimeAnimatorController gridCellAnimController;
    

    [Space]
    [Header("ParticleSystems")]
    [Space]
    public ParticleSystem Spark08;
    [Space]
    [Header("Sprites")]
    [Space]
    public Sprite[] spritesMain;
    public Sprite[] spritesShower;
    public Sprite ingYangSprite;
    public Sprite emptySprite;
    public Sprite fireFullyFilledSprite;
    public Sprite iceFullyFilledSprite;
    public Sprite[] meltAminSprites;
    public Sprite[] extinguishAminSprites;


    private void Awake() {
        spritesMain = new Sprite[3];
        spritesMain[0] = emptySprite;
        spritesMain[1] = fireFullyFilledSprite;
        spritesMain[2] = iceFullyFilledSprite;

        spritesShower = new Sprite[3];
        spritesShower[0] = null;
        spritesShower[1] = fireFullyFilledSprite;
        spritesShower[2] = iceFullyFilledSprite;
    }

}

using System;
using System.Collections.Generic;
using System.Reflection;
using Characters.Abilities.Customs;
using Characters.Gear;
using Characters.Operations.Attack;
using GameResources;
using Hardmode.Darktech;
using HarmonyLib;
using Level;
using Services;
using Singletons;
using UnityEngine;

namespace BeelzWheelz;

[HarmonyPatch(typeof(OffensiveWheel), "Initialize")]
public class WheelPatch
{
    static readonly Sprite sprite = MakeSprite();
    static void Postfix(ref OffensiveWheel __instance)
    {
        ref var self = ref __instance;

        foreach (var component in self._operations._components)
        {
            if (component is FireProjectile)
            {
                // Clone the original wheel and set it as the cloning source for the pool
                // This is done this way because the pool is being cleared when changing stages, erasing the Face
                // Alternatively, we could inject this on projectile launch, which is an awful way of doing it.
                var projectile = component as FireProjectile;
                var poolObj = projectile._projectile.reusable;
                var original = UnityEngine.Object.Instantiate<PoolObject>(poolObj);
                UnityEngine.Object.DontDestroyOnLoad(original);
                poolObj._keepOriginal = true;
                projectile._projectile._reusable = poolObj.pool._original;
                var wheel = poolObj.pool._original.gameObject;

                original.gameObject.SetActive(true);


                // Create a clone of the existing sprite renderer for the wheel
                var wheelRenderer = wheel.transform.Find("Body").gameObject.GetComponent<SpriteRenderer>();
                var renderer = UnityEngine.Object.Instantiate<SpriteRenderer>(wheelRenderer);
                GameObject faceObj = renderer.gameObject;

                faceObj.name = "Face";
                UnityEngine.Object.DontDestroyOnLoad(faceObj);
                faceObj.transform.parent = wheel.transform;

                // Add the face sprite to the new renderer
                renderer.sprite = sprite;
                renderer.sortingOrder = 1;

                // Fix Wheel rotation
                GameObject sourceObj = new("WheelRotationFixer");
                UnityEngine.Object.DontDestroyOnLoad(sourceObj);
                var constraint = faceObj.AddComponent<UnityEngine.Animations.RotationConstraint>();
                constraint.locked = false;

                UnityEngine.Animations.ConstraintSource constraintSource = new();
                constraintSource.sourceTransform = sourceObj.transform;
                constraintSource.weight = 1.0f;
                constraint.AddSource(constraintSource);

                constraint.constraintActive = true;
                constraint.enabled = true;
                constraint.locked = true;

                // Hide the wheel again, as it's just a source for cloning
                original.gameObject.SetActive(false);

            }
        }

    }

    static Sprite MakeSprite()
    {
        var assembly = typeof(WheelPatch).Assembly;
        var resource = assembly.GetManifestResourceStream("BeelzWheelz.WheelFace.png");

        byte[] buf = new byte[resource.Length];
        resource.Read(buf, 0, (int)resource.Length);

        Texture2D texture = new(2, 2);
        texture.LoadImage(buf);
        texture.filterMode = FilterMode.Bilinear;
        return Sprite.Create(texture, new Rect(0.0f, 0.0f, texture.width, texture.height), new Vector2(0.5f, 0.5f), 125f);
    }
}

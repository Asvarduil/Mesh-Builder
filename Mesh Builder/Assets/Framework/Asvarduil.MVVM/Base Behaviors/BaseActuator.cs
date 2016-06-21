using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseActuator<T> : DebuggableBehavior
{
    #region Methods

    public abstract void RealizeModel(T model);

    public abstract void ResetActuator(T model);

    protected virtual GameObject RealizeMeshDetails(MeshDetail meshDetail)
    {
        GameObject result = RealizeMesh(meshDetail);
        RealizeMeshRenderer(meshDetail);

        return result;
    }

    protected virtual void RealizeMeshCollider(MeshDetail meshDetail, GameObject target = null)
    {
        if (string.IsNullOrEmpty(meshDetail.MeshPath))
            return;

        Mesh mesh = Resources.Load<Mesh>(meshDetail.MeshPath);
        if (mesh == null)
            throw new ApplicationException("Could not find a mesh object at path " + meshDetail.MeshPath);

        MeshCollider collider = GetMeshCollider(target);

        Mesh instance = Instantiate(mesh);
        collider.sharedMesh = instance;
    }

    protected virtual GameObject RealizeMesh(MeshDetail meshDetail)
    {
        if (string.IsNullOrEmpty(meshDetail.MeshPath))
            return null;

        gameObject.transform.localScale = meshDetail.ObjectScale;

        GameObject meshObject = Resources.Load<GameObject>(meshDetail.MeshPath);
        if (meshObject == null)
            throw new ApplicationException("Could not find a mesh object at path " + meshDetail.MeshPath);

        GameObject instance = (GameObject)Instantiate(meshObject, transform.position, transform.rotation);
        instance.name = "Mesh";
        instance.transform.position = transform.position + meshDetail.MeshOffset;
        instance.transform.rotation = Quaternion.Euler(meshDetail.MeshRotation);
        instance.transform.localScale = meshDetail.MeshScale;
        instance.transform.parent = gameObject.transform;

        Animator animator = instance.GetComponent<Animator>();
        if (animator == null)
            return instance;

        RuntimeAnimatorController controller = Resources.Load<RuntimeAnimatorController>(meshDetail.AnimationControllerPath);
        animator.runtimeAnimatorController = controller;

        return instance;
    }

    protected virtual void RealizeMeshRenderer(MeshDetail meshDetail)
    {
        if (string.IsNullOrEmpty(meshDetail.MeshPath))
            return;

        Renderer renderer = GetRendererInChildren();
        if (renderer == null)
            throw new InvalidOperationException("Could not find a MeshRenderer in any children of game object " + gameObject.name);

        if (meshDetail.MaterialDetails.IsNullOrEmpty())
            return;

        List<Material> materials = new List<Material>();
        for (int i = 0; i < meshDetail.MaterialDetails.Count; i++)
        {
            MaterialDetail current = meshDetail.MaterialDetails[i];
            Material newMaterial = ParseMaterial(current);
            materials.Add(newMaterial);
        }

        renderer.materials = materials.ToArray();
    }

    protected virtual Renderer GetRendererInChildren()
    {
        MeshRenderer meshRenderer = GetComponentInChildren<MeshRenderer>();
        if (meshRenderer != null)
            return meshRenderer;

        SkinnedMeshRenderer skinnedMeshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
        if (skinnedMeshRenderer != null)
            return skinnedMeshRenderer;

        return null;
    }

    protected virtual MeshCollider GetMeshCollider(GameObject target = null)
    {
        MeshCollider collider = null;

        if (target == null)
        {
            collider = GetComponentInChildren<MeshCollider>();
            if (collider == null)
            {
                throw new ApplicationException("Could not find a MeshCollider in any children of game object " + gameObject.name);
            }
        }
        else
        {
            collider = target.GetComponent<MeshCollider>();
            if (collider == null)
            {
                collider = target.AddComponent<MeshCollider>();
            }
        }

        return collider;
    }

    protected virtual Material ParseMaterial(MaterialDetail detail)
    {
        Material originalMaterial = Resources.Load<Material>(detail.MaterialPath);

        Material material = Instantiate(originalMaterial);
        if (material == null)
            throw new ApplicationException("Could not find a material at path " + detail.MaterialPath);

        material.name = detail.Name;

        for(int i = 0; i < detail.TextureDetails.Count; i++)
        {
            TextureDetail textureDetail = detail.TextureDetails[i];
            RealizeTextureOnMaterial(material, textureDetail);
        }

        return material;
    }

    protected virtual void RealizeTextureOnMaterial(Material material, TextureDetail textureDetail)
    {
        if (material == null)
            throw new InvalidOperationException("Cannot realize textures on a null material.");

        switch (textureDetail.DetailType)
        {
            case TextureDetailType.Texture:
                string texturePath = textureDetail.DetailValue.ToString();
                if (string.IsNullOrEmpty(texturePath))
                    throw new ApplicationException("Cannot load texture from a null/empty path!");

                texturePath = texturePath.Trim('"');

                Texture2D texture = Resources.Load<Texture2D>(texturePath);
                if (texture == null)
                    DebugMessage("Could not find a texture at path " + texturePath);

                material.SetTexture(textureDetail.DetailKey, texture);
                break;

            case TextureDetailType.Tint:
                Color color = (Color) textureDetail.DetailValue;
                material.SetColor(textureDetail.DetailKey, color);
                break;

            case TextureDetailType.Int:
                int integer = (int) textureDetail.DetailValue;
                material.SetInt(textureDetail.DetailKey, integer);
                break;

            case TextureDetailType.Float:
                float number = (float)textureDetail.DetailValue;
                material.SetFloat(textureDetail.DetailKey, number);
                break;

            case TextureDetailType.Offset:
                Vector2 offset = (Vector2) textureDetail.DetailValue;
                material.SetTextureOffset(textureDetail.DetailKey, offset);
                break;

            default:
                throw new ApplicationException("Unexpected Texture Detail Type: " + textureDetail.DetailType);
        }
    }

    #endregion Methods
}

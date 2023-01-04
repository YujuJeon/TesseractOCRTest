using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.SceneManagement;

/// <summary>
/// Create new type for addressable ref
/// </summary>
[System.Serializable]
public class AssetReferenceAudioClip : AssetReferenceT<AudioClip>
{
    public AssetReferenceAudioClip(string guid) : base(guid) { }
}

public class SpawnObjectAddressable : MonoBehaviour
{
    [SerializeField] private AssetReference assetReference;
    [SerializeField] private AssetLabelReference assetLabelRef;
    //There's more type 
    [SerializeField] private AssetReferenceGameObject assetRefGameObj;
    [SerializeField] private AssetReferenceTexture2D assetRefTexture;

    //Created type
    [SerializeField] private AssetReferenceAudioClip assetRefAudioClip;

    [SerializeField] private GameObject spawnedGameObject;


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            assetRefGameObj.InstantiateAsync().Completed += (asyncOperation) => spawnedGameObject = asyncOperation.Result;

            //Load addressable folder should be same type ex) gameobject types, texture types, sprite types, etc...
            //Addressables.LoadAssetsAsync<Texture2D>(assetLabelRef, (texture)=> 
            //{
            //    Debug.Log(texture);
            //});            

            //The Simplest way
            //assetRefGameObj.InstantiateAsync();

            ////////
            //using asset Reference Method
            //assetReference.LoadAssetAsync<GameObject>().Completed +=

            //using asset label reference method
              //Addressables.LoadAssetAsync<GameObject>(assetLabelRef).Completed +=
              //  (asyncOperationHandle) =>
              //  {
              //      if (asyncOperationHandle.Status == AsyncOperationStatus.Succeeded)
              //      {
              //          Instantiate(asyncOperationHandle.Result);
              //      }
              //      else
              //      {
              //          Debug.Log("FAILED TO LOAD");
              //      }
              //  };               
        }

        //remove
        if (Input.GetKeyDown(KeyCode.U))
        {
            assetRefGameObj.ReleaseInstance(spawnedGameObject);
            SceneManager.LoadScene("AddressableTest");
        }
    }
}

using UnityEngine;
using System;

/// <summary>
/// This is a template for a singleton class.
/// Properties:
/// - Ensures that only one instance of the class exists.
/// - Ensures that the instance is accessible globally.
/// - Class.Instance is the preferred way to access the instance.
/// - Class.Exists can be used to check if an instance exists.
/// - Class.Instance can be used instead of GetComponent<Class>() so we don't have to drag and drop the script
///     in the inspector / worry about moving the script to a different gameobject.
/// </summary>
public class SingletonTemplate : MonoBehaviour
{
    //Singleton pattern

    #region Singleton pattern
    private static SingletonTemplate _instance;

    public static bool Exists => _instance != null;

    public static SingletonTemplate Instance
    {
        get
        {
            if (_instance == null)
                throw new Exception("Instance is NULL");
            return _instance;
        }
    }
    #endregion
    
    private void Awake()
    {
        if (_instance != null)
            throw new Exception("Instance already exists");
        _instance = this;
    }

    private void OnDestroy()
    {
        _instance = null;
    }
    
}
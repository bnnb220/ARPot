using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Firebase;
using Firebase.Database;


public class FBManager : MonoBehaviour
{

    private readonly string uri = "https://arpot-c7044-default-rtdb.firebaseio.com/";

    private DatabaseReference reference;

    public IDictionary iTargetDataDict;


    void Awake()
    {
        AppOptions options = new AppOptions();

        options.DatabaseUrl = new System.Uri(uri);

        FirebaseApp app = FirebaseApp.Create(options);
    }

    IEnumerator Start()
    {
        reference = FirebaseDatabase.DefaultInstance.GetReference("Info");

        yield return new WaitForSeconds(1.0f);
    }



    public void LoadData(string code)
    {
        Debug.Log("Load data On" + code);
        
        Query targetQuery = reference.OrderByChild("code").EqualTo(code);

        Debug.Log("After Load data On");
        targetQuery.ValueChanged += OnDataLoaded;
    }

    void OnDataLoaded(object sender, ValueChangedEventArgs args)
    {
        DataSnapshot snapshot = args.Snapshot;
        foreach(var data in snapshot.Children)
        {
            iTargetDataDict = (IDictionary)data.Value;
        }
        Debug.Log("Data Loaded");
    }
}
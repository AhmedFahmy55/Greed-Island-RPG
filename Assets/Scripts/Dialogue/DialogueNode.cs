using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

[System.Serializable]
public class DialogueNode : ScriptableObject
{

    [SerializeField] bool isPlayerSpeaking = false ;
    [SerializeField] string nodeText;
    [SerializeField] AudioClip voice = null;
    [SerializeField] List<string> ChildNodes = new List<string>();
    [SerializeField] Rect rect = new Rect(0,0,200,100);
    [SerializeField] string nodeEvent;
    
    // geters
    public List<string> GetChilds()
    {
        return ChildNodes;
    }

    public Rect GetRect()
    {
        return rect;
    }

    public string GetText()
    {
        return nodeText ;
    }

    public string GetEvent()
    {
        return nodeEvent ;
    }

    public bool IsPlayerSpeaking()
    {
        return isPlayerSpeaking;
    }




    #if UNITY_EDITOR
    public void SetRect(Vector2 position)
    {
        Undo.RecordObject(this,"Update position");
        rect.position = position;
        EditorUtility.SetDirty(this);
    }
    public void AddChild(string childID)
    {
        Undo.RecordObject(this,"Link Node");
        ChildNodes.Add(childID);
        EditorUtility.SetDirty(this);
    }
    public void RemoveChild(string childID)
    {
        Undo.RecordObject(this,"UnlinkNode node");
        ChildNodes.Remove(childID);
        EditorUtility.SetDirty(this);
    }

    public void SetText(string newText)
    {
        if(newText != nodeText)
        {
            Undo.RecordObject(this, "Change Dialogue Text");
            nodeText = newText;
            // Reminder: undo setting dirty automatic on the high level not the supp level so we need to mark them dirty
            EditorUtility.SetDirty(this);
        }
    }

    public void SetSpeaker(bool newSpeaker)
    {
        Undo.RecordObject(this, "Update Speaker");
        isPlayerSpeaking = newSpeaker;
        EditorUtility.SetDirty(this);

    }

#endif
}



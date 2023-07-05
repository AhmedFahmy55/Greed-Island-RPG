using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif


[CreateAssetMenu(fileName = "Dialogue", menuName = "RPG/Dialogue", order = 0)]
public class Dialogue : ScriptableObject , ISerializationCallbackReceiver
{

    [SerializeField] List<DialogueNode> nodes = new List<DialogueNode>();

    Dictionary<string,DialogueNode> nodesLookup = new Dictionary<string, DialogueNode>();



    private void OnValidate() 
    {
        foreach (var node in nodes)
        {
            if(!string.IsNullOrEmpty(node.name))
            nodesLookup[node.name] = node;
        }
        
    }
    
    private void Awake() 
    {
        OnValidate();
       
    }
   

    public IEnumerable<DialogueNode> GetNodes()
    {
        return nodes;
    }
    public DialogueNode GetRootNode()
    {
        return nodes[0];
    }

    public IEnumerable<DialogueNode> GetChildNodes(DialogueNode node)
    {
        foreach (var childNodeID in node.GetChilds())
        {
            if(nodesLookup.ContainsKey(childNodeID))
            {
                yield return nodesLookup[childNodeID];
            }
        }
    }
    public IEnumerable<DialogueNode> GetPlayerNodes(DialogueNode node)
    {
        foreach (var item in GetChildNodes(node))
        {
            if(item.IsPlayerSpeaking())
            yield return item ;
        }
    }
    public IEnumerable<DialogueNode> GetNPCNodes(DialogueNode node)
    {
        foreach (var item in GetChildNodes(node))
        {
            if(!item.IsPlayerSpeaking())
            yield return item ;
        }
    }

    #if UNITY_EDITOR

    public void CreatNewNode(DialogueNode parentNode)
    {
        DialogueNode node = CreatNode(parentNode);
        Undo.RecordObject(this, "Adding Node");
        Undo.RegisterCreatedObjectUndo(node, "Creating new Node");
        nodes.Add(node);
        OnValidate();

    }

    private  DialogueNode CreatNode(DialogueNode parentNode)
    {
        DialogueNode node = CreateInstance<DialogueNode>();
        node.name = Guid.NewGuid().ToString();
        if (parentNode != null)
        {
            parentNode.AddChild(node.name);
            Vector2 offset = new Vector2(parentNode.GetRect().xMax, parentNode.GetRect().center.y);
            offset *= .2f;
            node.SetRect(parentNode.GetRect().position + offset);
            node.SetSpeaker(!parentNode.IsPlayerSpeaking());
        }

        return node;
    }

    public void DeleteNode(DialogueNode removedNode)
    {
        Undo.RecordObject(this, "deleting Node");
        nodes.Remove(removedNode);
        
        foreach (var node in nodes)
        {
            node.RemoveChild(removedNode.name);
        }
        foreach (DialogueNode node in GetChildNodes(removedNode))
        {
            if(node != removedNode) // just garding against nodes linked to itself normally this shouldnt happen
            DeleteNode(node);
        }

        Undo.DestroyObjectImmediate(removedNode);
        OnValidate();
    }
#endif
    public void OnBeforeSerialize()
    {
#if UNITY_EDITOR
        if (nodes.Count == 0)
        {

            DialogueNode node = CreatNode(null);
            nodes.Add(node);
        }
      
        if(AssetDatabase.GetAssetPath(this) != "")
        {
            foreach (var node in nodes)
            {
                if(AssetDatabase.GetAssetPath(node) == "")
                {
                    AssetDatabase.AddObjectToAsset(node,this);
                }
            }
        }
#endif

    }

    public void OnAfterDeserialize()
    {
        
    }

}



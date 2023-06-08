
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using TMPro;
using System;

public class DialogueEditor : EditorWindow
{
    
    [NonSerialized] DialogueNode DraggingNode ;
    [NonSerialized] DialogueNode creatingNode = null;
    [NonSerialized] DialogueNode deletedNode = null;
    [NonSerialized] DialogueNode linkingNode = null;
    [NonSerialized] Vector2 positionOffset;
    [NonSerialized] GUIStyle nodeStyle = new GUIStyle();
    [NonSerialized] GUIStyle playerNodeStyle = new GUIStyle();
    static Dialogue SelecteDialogue = null ;
    bool isDraggingCanvas = false ;
    const float canvasSize = 5000;

    Vector2 scrolPosition;
    Vector2 scrollOffset;
    Vector3 damp  ;

    void OnEnable() 
    {
        Selection.selectionChanged += OnSelectionChange;
        nodeStyle.normal.background = EditorGUIUtility.Load("node0") as Texture2D; 
        nodeStyle.padding = new RectOffset(20,20,20,20);
        nodeStyle.border = new RectOffset(12,12,12,12);

        playerNodeStyle.normal.background = EditorGUIUtility.Load("node1") as Texture2D; 
        playerNodeStyle.padding = new RectOffset(20,20,20,20);
        playerNodeStyle.border = new RectOffset(12,12,12,12);
    }

    [MenuItem("Window/Dialogue Editor")]
    static void ShowDialogue()
    {
        DialogueEditor window = (DialogueEditor)GetWindow(typeof(DialogueEditor),false,"DialogueEditor",true);
    }




    [OnOpenAsset(1)]
    static bool onOpenAsset(int assetID,int line)
    {
        Dialogue opendDialogue = EditorUtility.InstanceIDToObject(assetID) as  Dialogue ;
        if(opendDialogue != null)
        {
            ShowDialogue();
            SelecteDialogue = opendDialogue;
            return true ;
        }
        return false ;
    }

    private void OnSelectionChange()
    {
        Dialogue newSelectedDialogue  = Selection.activeObject as Dialogue ;
        if(newSelectedDialogue != null)
        {
            SelecteDialogue = newSelectedDialogue ;
            Repaint();
        }
    }

    private void OnGUI()
    {
        if (SelecteDialogue == null) return;
        scrolPosition = GUILayout.BeginScrollView(scrolPosition);

        Rect canvas = GUILayoutUtility.GetRect(canvasSize,canvasSize);
        Texture2D texture = Resources.Load<Texture2D>("CanvasText");
        Rect textureScale = new Rect(0,0,canvasSize / 20f,canvasSize / 20f);
        GUI.DrawTextureWithTexCoords(canvas,texture,textureScale);
        ProcessEvents();
        foreach (var node in SelecteDialogue.GetNodes())
        {
            DrawConnections(node);
        }

        foreach (var node in SelecteDialogue.GetNodes())
        {
            DrawNode(node);
        }
        GUILayout.EndScrollView();

        if (creatingNode != null)
        {
            SelecteDialogue.CreatNewNode(creatingNode);
            creatingNode = null;
        }
        if (deletedNode != null)
        {
            SelecteDialogue.DeleteNode(deletedNode);
            deletedNode = null;
        }

    }



    private void ProcessEvents()
    {
        if(Event.current.type == EventType.MouseDown && DraggingNode == null)
        {
            DraggingNode = GetSelectedNode();
            if(DraggingNode != null)
            {
                Selection.activeObject = DraggingNode ;
                positionOffset = DraggingNode.GetRect().position - Event.current.mousePosition;
            }
            else 
            {
                isDraggingCanvas = true;
                scrollOffset = Event.current.mousePosition;
                Selection.activeObject = SelecteDialogue;
            }
        }
        else if (Event.current.type == EventType.MouseDrag && DraggingNode != null)
        {
           DraggingNode.SetRect(Event.current.mousePosition + positionOffset) ;
           GUI.changed = true ;
        }
        else if (Event.current.type == EventType.MouseDrag && isDraggingCanvas == true)
        {
            //TODO bug  jiteer screoll
            
        //    Vector2 direction = scrollOffset - Event.current.mousePosition;
        //    Debug.Log(direction);
        //    if(direction.x > 0 )
        //    {
        //         scrolPosition.x += 10f;
        //    }
        //    else
        //    {
        //         scrolPosition.x -= 10f;
        //    }
        //    if(direction.y > 0 )
        //    {
        //         scrolPosition.y += 10f;
        //    }
        //    else
        //    {
        //         scrolPosition.y -= 10f;
        //    }

            scrolPosition =  Event.current.mousePosition - scrollOffset  ;
            GUI.changed = true ; 

        }
        else if (Event.current.type == EventType.MouseUp && isDraggingCanvas == true)
        {
            isDraggingCanvas = false ;
        }
        else if(Event.current.type == EventType.MouseUp && DraggingNode != null)
        {
            DraggingNode = null ;
        }
    }

    

    private void DrawNode(DialogueNode node)
    {
        GUIStyle style = node.IsPlayerSpeaking()? playerNodeStyle:nodeStyle ;
        GUILayout.BeginArea(node.GetRect(), style);
        node.SetText(EditorGUILayout.TextField(node.GetText()));
        GUILayout.BeginHorizontal();
        CreatNodeButtons(node);
        GUILayout.EndHorizontal();
        GUILayout.EndArea();
    }

    private void CreatNodeButtons(DialogueNode node)
    {
        if (GUILayout.Button("x"))
        {
            deletedNode = node;
        }
        if (linkingNode == null)
        {
            if (GUILayout.Button("Link"))
            {
                linkingNode = node;
            }
        }
        else if (linkingNode == node)
        {
            if (GUILayout.Button("Cancel"))
            {
                linkingNode = null;
            }
        }
        else if (linkingNode.GetChilds().Contains(node.name))
        {
            if (GUILayout.Button("Unlink"))
            {
                linkingNode.RemoveChild(node.name);
                linkingNode = null;
            }
        }
        else 
        {
            if (GUILayout.Button("Child"))
            {
                linkingNode.AddChild(node.name);
                linkingNode = null;
            }
        }
        if (GUILayout.Button("+"))
        {
            creatingNode = node;
        }
    }

    private void DrawConnections(DialogueNode node)
    {
        Vector3 startPosition = new Vector2(node.GetRect().xMax,node.GetRect().center.y);
        foreach (var child in SelecteDialogue.GetChildNodes(node))
        {
            Vector3 endPosition = new Vector2(child.GetRect().xMin,child.GetRect().center.y);
            Vector3 offset = endPosition - startPosition ;
            offset.y = 0 ;
            offset *= .8f ;
            Handles.DrawBezier(startPosition,endPosition,startPosition + offset , endPosition - offset ,Color.blue,null,4f);
        }
        
    }
    private DialogueNode GetSelectedNode()
    {
        DialogueNode frontNode = null ;
        foreach (var node in SelecteDialogue.GetNodes())
        {
            if(node.GetRect().Contains(Event.current.mousePosition ))
            {
                frontNode = node;
            }
        }
        return frontNode;
        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Events;

namespace RoseHFSM
{
    /// <summary>
    /// A class that contains the data for a node in the node editor.
    /// </summary>
    public class Node
    {
        

        private State state;
        public State NodeState
        {
          get { return state; }
            set { state = value; }
        }

        private Rect rect;
        public Rect NodeRect
        {
            get { return rect; }
            set { rect = value;  }
        }

        private GUIStyle styleDefault;
        private GUIStyle styleSelected;
        private GUIStyle styleStart;
        private GUIStyle styleStartSelected;
        private GUIStyle styleSimulation;

        public bool IsDragged;
        public bool IsSelected;
        public bool IsStart;
        public bool IsSimulation;

        private UnityAction<Node> contextAction;
        private UnityAction<Node> selectAction;


        /// <summary>
        /// Nodes that connect to this node.
        /// </summary>
        private List<Connection> fromConnections = new List<Connection>();
        public List<Connection> FromConnections
        {
            get { return fromConnections; }
        }
        /// <summary>
        /// Nodes this node connects to.
        /// </summary>
        private List<Connection> toConnections = new List<Connection>();
        public List<Connection> ToConnections
        {
            get { return toConnections; }
        }

        public Node(State state, Rect rect, UnityAction<Node> selectAction, UnityAction<Node> contextAction)
        {
            this.state = state;
            this.rect = rect;
            this.selectAction = selectAction;
            this.contextAction = contextAction;
        }

        /// <summary>
        /// 
        /// </summary>
        public void SetStyles(GUIStyle styleDefault, GUIStyle styleSelected, GUIStyle styleStart, GUIStyle styleStartSelected, GUIStyle styleSimulation)
        {

            this.styleDefault = styleDefault;
            this.styleSelected = styleSelected;
            this.styleStart = styleStart;
            this.styleStartSelected = styleStartSelected;
            this.styleSimulation = styleSimulation;
        }

        public void AddToConnection(Connection connection)
        {
            toConnections.Add(connection);
        }

        public void AddFromConnection(Connection connection)
        {
            fromConnections.Add(connection);
        }

        public void Drag(Vector2 delta)
        {
            rect.position += delta;
            if(NodeState != null)
            {
                NodeState.nodeEditorLoc = rect.position;
            }
        }

        public void Draw()
        {
            string name = "Node";
            if (state != null)
                name = state.StateName;
            if(IsSimulation)
            {
                GUI.Box(rect, name, styleSimulation);
            }
            else if(IsSelected && IsStart)
            {
                GUI.Box(rect, name, styleStartSelected);
            }
            else if (IsSelected && !IsStart)
            {
                GUI.Box(rect, name, styleSelected);
            }
            else if (!IsSelected && IsStart)
            {
                GUI.Box(rect, name, styleStart);
            }
            else
            {
                GUI.Box(rect, name, styleDefault);
            }

        }

        public bool ProcessEvents(Event e)
        {
            switch (e.type)
            {
                case EventType.MouseDown:
                    if(e.button == 0)
                    {
                        if (rect.Contains(e.mousePosition))
                        {
                            IsDragged = true;
                            GUI.changed = true;
                            IsSelected = true;
                        }
                        else
                        {
                            GUI.changed = true;
                            IsSelected = false;
                        }
                    }
                    break;
                case EventType.MouseUp:
                    IsDragged = false;
                    if (e.button == 1)
                    {
                        if (rect.Contains(e.mousePosition))
                        {
                            if (contextAction != null)
                                contextAction.Invoke(this);
                            e.Use();
                        }
                    }
                    else if(e.button == 0)
                    {
                        if (rect.Contains(e.mousePosition))
                        {
                            if (selectAction != null)
                                selectAction.Invoke(this);
                            e.Use();
                        }
                    }

                    break;
                case EventType.MouseDrag:
                    if (e.button == 0 && IsDragged)
                    {
                        Drag(e.delta);
                        e.Use();
                        return true;
                    }
                    break;
            }
            return false;
        }
    }
}
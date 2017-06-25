using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace RoseHFSM
{
    public class NodeEditor : EditorWindow
    {
        enum NodeEditorMode
        {
            Editing, NewTransition, Empty, Simulation
        }

        private List<Node> nodes = new List<Node>();
        private List<HFSM> hfsms = new List<HFSM>();
        private List<Connection> connections = new List<Connection>();

        private Vector2 offset;
        private Vector2 drag;

        private Stack<HFSM> parentHFSM = new Stack<HFSM>();
        private HFSM currentHFSM;
        private int selectedTab = 0;
        private Node selectedNode;

        private float inspectHeight = 200;
        private Vector2 inspectScroll = Vector2.zero;

        NodeEditorMode currentMode = NodeEditorMode.Empty;

        /// <summary>
        /// Starting point of a new transition.
        /// </summary>
        private int newTransitionNodeId;

        /*
        [MenuItem("Window/Node editor")]
        public static void ShowEditor()
        {
            NodeEditor editor = EditorWindow.GetWindow<NodeEditor>();
        }
        */

        void OnSelectionChange()
        {
            UpdateNodes();
        }

        /// <summary>
        /// Fills the editor with updates nodes.
        /// </summary>
        public void UpdateNodes()
        {
            parentHFSM.Clear();

            if (Selection.activeTransform == null) {
                currentMode = NodeEditorMode.Empty;
                return;
            }

            Behaviour bc = Selection.activeTransform.gameObject.GetComponent<Behaviour>();
            
            //Stop if no active transform or behaviour on active object.
            if (bc == null)
            {
                currentMode = NodeEditorMode.Empty;
                return;
            }

            //Set to editing mode.
            if (currentMode == NodeEditorMode.Empty)
                currentMode = NodeEditorMode.Editing;
            // bc.GetHFSM().Reset();
            currentHFSM = null;
            //currentHFSM = bc.GetHFSM();
            if (bc.TopHFSM == null)
                bc.TopHFSM = CreateInstance<HFSM>();
            FillFromHFSM(bc.TopHFSM);
            Repaint();
        }

        void OnGUI()
        {
            //Draw Grid
            DrawGrid(20, 0.2f, Color.white);
            DrawGrid(100, 0.4f, Color.white);

            //Don't bother if no object to work with
            if (currentMode == NodeEditorMode.Empty)
                return;

            //Setup Inspect rect
            Rect inspect = new Rect(0, position.height - inspectHeight, position.width, inspectHeight);

            //Draw HFSM selector
            string[] hfsmTabs = new string[hfsms.Count];
            for (int i = 0; i < hfsms.Count; i++)
            {
                string tabName = hfsms[i].hfsmName;
                if (hfsms[i] == Selection.activeGameObject.GetComponent<Behaviour>().TopHFSM)
                    tabName = "Base";
                if (parentHFSM.Count != 0 && hfsms[i] == parentHFSM.Peek())
                    tabName = "Go Back";
                hfsmTabs[i] = tabName;
            }

            int chosenTab = GUILayout.Toolbar(selectedTab, hfsmTabs, GUILayout.Width(100*hfsmTabs.Length));
            if (chosenTab != selectedTab)
            {
                selectedTab = chosenTab;
                selectedNode = null;
                FillFromHFSM(hfsms[selectedTab]);
            }

            foreach(Connection c in connections)
            {
                //Find start position
                int i = c.fromNode.ToConnections.IndexOf(c);
                Vector2 startPos = new Vector2(c.fromNode.NodeRect.x + c.fromNode.NodeRect.width,
                      c.fromNode.NodeRect.y + i * c.fromNode.NodeRect.height / c.fromNode.ToConnections.Count +
                      c.fromNode.NodeRect.height / c.fromNode.ToConnections.Count / 2);

                //Find end position
                i = c.toNode.FromConnections.IndexOf(c);
                Vector2 endPos = new Vector2(c.toNode.NodeRect.x,
                    c.toNode.NodeRect.y + i * c.toNode.NodeRect.height / c.toNode.FromConnections.Count +
                    c.toNode.NodeRect.height / c.toNode.FromConnections.Count / 2);

                DrawCurve(startPos, endPos);

                DrawDirectionMarker(endPos);
            }
      
            Event e = Event.current;

            //Check for Drag and Drop states
            //if (e.type == EventType.MouseUp && e.button == 0)
            foreach (Object o in DragAndDrop.objectReferences)
            {
                if(o is MonoScript && (((MonoScript)o).GetClass().IsSubclassOf(typeof(State)) ||
                    ((MonoScript)o).GetClass() == typeof(State)))
                {
                    DragAndDrop.visualMode = DragAndDropVisualMode.Copy;

                    if(e.type == EventType.DragPerform)
                    {
                        State state = CreateInstance(((MonoScript)o).GetClass()) as State;
                        state.nodeEditorLoc.x = e.mousePosition.x;
                        state.nodeEditorLoc.y = e.mousePosition.y;
                        AddNode(state);

                        //Hack to add HFSM
                        if (state is ParentState)
                            hfsms.Add(((ParentState)state).StateHFSM);

                        DragAndDrop.AcceptDrag();
                            e.Use();
                        }

                    }
                }



            //Check for delete button.
            if (e.type == EventType.KeyUp && e.keyCode == KeyCode.Delete && selectedNode != null)
            {
                DeleteNode(selectedNode);
                e.Use();
            }

            //Draw new transition curve
            if (currentMode == NodeEditorMode.NewTransition)
            {
                //Click to window if found
                bool foundWindow = false;
                for (int i = 0; i < nodes.Count; i++)
                {
                    if (i == newTransitionNodeId)
                        continue;
                    if (nodes[i].NodeRect.Contains(Event.current.mousePosition))
                    {
                        DrawNodeCurve(nodes[newTransitionNodeId].NodeRect, nodes[i].NodeRect);
                        foundWindow = true;
                        break;
                    }
                }

                if (!foundWindow)
                    DrawNewTransitionCurve(nodes[newTransitionNodeId].NodeRect, e.mousePosition);
            }

            
            BeginWindows();
            //GUILayout.BeginArea(new Rect(0, position.height, position.width, position.height - 200));
            for (int i = 0; i < nodes.Count; i++)
            {
                if (e.type == EventType.MouseUp && e.button == 0 && !nodes[i].NodeRect.Contains(e.mousePosition) &&
                    !inspect.Contains(e.mousePosition))
                    selectedNode = null;

                nodes[i].NodeRect = GUI.Window(i, nodes[i].NodeRect, DrawNodeWindow, nodes[i].NodeState.StateName);
                /*
                SerializedObject nodeSerial = new SerializedObject(nodes[i].NodeState);
                SerializedProperty eventProperty = nodeSerial.FindProperty("task");
                EditorGUILayout.PropertyField(eventProperty);
                nodeSerial.ApplyModifiedProperties();
                */
            }
            //GUILayout.EndArea();

            if (selectedNode != null)
            {
                // create a style based on the default label style
                GUIStyle inspectStyle = new GUIStyle(GUI.skin.label);
                // do whatever you want with this style, e.g.:
                inspectStyle.margin = new RectOffset(20, 20, 20, 20);
                GUILayout.BeginArea(inspect);
                inspectScroll = GUILayout.BeginScrollView(inspectScroll, inspectStyle);

                SerializedObject serializedObject = new SerializedObject(selectedNode.NodeState);

                SerializedProperty property = serializedObject.GetIterator();
                property.Next(true);
                bool children = false;

                
                while (property.NextVisible(children))
                {
                    //serializedObject.Update();
                    if (property.name == "nodeEditorLoc" ||
                        property.name == "transitions")
                        continue;

                    EditorGUILayout.PropertyField(property);
                    
                }

                serializedObject.ApplyModifiedProperties();

                GUILayout.EndScrollView();
                GUILayout.EndArea();
            }

            switch (currentMode)
            {
                case (NodeEditorMode.Editing):
                    if (e.type == EventType.ContextClick)
                    {
                        GenericMenu menu = new GenericMenu();
                        menu.AddItem(new GUIContent("New State"), false, new GenericMenu.MenuFunction2(AddNodeCallBack),
                            e.mousePosition);
                        menu.AddItem(new GUIContent("New HFSM State"), false, new GenericMenu.MenuFunction2(AddHFSMCallBack),
                            e.mousePosition);
                        menu.ShowAsContext();
                        e.Use();

                    }
                    break;
                case (NodeEditorMode.NewTransition):
                    if (e.type == EventType.MouseUp && e.button == 0)
                    {
                        for (int i = 0; i < nodes.Count; i++)
                        {
                            if (newTransitionNodeId == i)
                                continue;

                            if (nodes[i].NodeRect.Contains(e.mousePosition))
                            {
                                AddTransition(newTransitionNodeId, i);
                                CancelTransitioning();
                                break;
                            }
                        }

                        if (currentMode == NodeEditorMode.Editing)
                            break;

                        GenericMenu menu = new GenericMenu();
                        menu.AddItem(new GUIContent("To New State"), false,
                            new GenericMenu.MenuFunction2(NewStateViaTransitionCallBack),
                            e.mousePosition);
                        menu.AddItem(new GUIContent("Cancel"), false, new GenericMenu.MenuFunction(CancelTransitioning));
                        menu.ShowAsContext();
                        e.Use();

                    }
                    else if (e.type == EventType.MouseUp && e.button == 1)
                    {
                        CancelTransitioning();
                        e.Use();
                    }
                    break;
            }


            EndWindows();
        }

        void Update()
        {
            if (EditorWindow.focusedWindow == this &&
                EditorWindow.mouseOverWindow == this &&
                currentMode == NodeEditorMode.NewTransition)
            {
                Repaint();
            }
        }

        /// <summary>
        /// Fill the node editor from a behaviour Game Object.
        /// </summary>
        void FillFromHFSM(HFSM hfsm)
        {
            //Clear the space for new behaviour.
            Clear();

            if (hfsm != Selection.activeGameObject.GetComponent<Behaviour>().TopHFSM)
            {
                selectedTab += 1;
                hfsms.Add(Selection.activeGameObject.GetComponent<Behaviour>().TopHFSM);
                if (currentHFSM == Selection.activeGameObject.GetComponent<Behaviour>().TopHFSM)
                    currentHFSM = null;
                if (parentHFSM.Count != 0 && hfsm == parentHFSM.Peek())
                {
                    parentHFSM.Pop();
                    currentHFSM = parentHFSM.Peek();
                }
                else {
                    parentHFSM.Push(currentHFSM);
                }
                if (currentHFSM != null)
                {
                    hfsms.Add(currentHFSM);
                    selectedTab += 1;
                }
            }
            else
            {
                parentHFSM.Clear();
            }

            hfsms.Add(hfsm);
            currentHFSM = hfsm;
            //Make nodes from HFSM states.
            for (int i = 0; i < hfsm.States.Count; i++)
            {
                Vector2 stateLocation = hfsm.States[i].nodeEditorLoc;

                Rect rect = new Rect(stateLocation.x, stateLocation.y, 100, 100);

                Node node = new Node(hfsm.States[i], rect);

                if (hfsm.States[i] is ParentState)
                    hfsms.Add(((ParentState)hfsm.States[i]).StateHFSM);

                nodes.Add(node);
            }

            //Make connections between each node;
            for (int i = 0; i < nodes.Count; i++)
            {
                for (int j = 0; j < nodes[i].NodeState.Transitions.Count; j++)
                {
                    Node toNode = nodes.Find(obj =>obj.NodeState == nodes[i].NodeState.Transitions[j].ToState);
                    if (toNode == null)
                    {
                        Debug.LogError("Unlinked transition.");
                        continue;
                    }

                    AddConnection(nodes[i], toNode, nodes[i].NodeState.Transitions[j]);
                    
                }
            }

        }    
        
        private Connection AddConnection(Node fromNode, Node toNode, Transition transition)
        {
            Connection connection = new Connection(fromNode, toNode, transition);
            fromNode.ToConnections.Add(connection);
            toNode.FromConnections.Add(connection);
            connections.Add(connection);
            return connection;
        }    

        /// <summary>
        /// Complete new transition.
        /// </summary>
        /// <param name="fromId">Start of the transition.</param>
        /// <param name="toId">End of the transition.</param>
        void AddTransition(int fromId, int toId)                                                                        
        {
            Transition transition = CreateInstance<Transition>();
            transition.hideFlags = HideFlags.HideInInspector;
            transition.ToState = nodes[toId].NodeState;

            nodes[fromId].NodeState.Transitions.Add(transition);
            currentHFSM.Transitions.Add(transition);

            AddConnection(nodes[fromId], nodes[toId], transition);
        }

        /// <summary>
        /// Stop making new transition.
        /// </summary>
        void CancelTransitioning()
        {
            currentMode = NodeEditorMode.Editing;
        }

        void AddNode(State state)
        {
            state.StateName = "New " + state.GetType().Name;
            if (nodes.Count == 0)
                currentHFSM.StartState = state;

            currentHFSM.States.Add(state);
            Rect rect = new Rect(state.nodeEditorLoc.x, state.nodeEditorLoc.y, 100, 100);

            nodes.Add(new Node(state, rect));
        }

        /// <summary>
        /// Clear the nodes.
        /// </summary>
        void Clear()
        {
            selectedTab = 0;
            hfsms.Clear();
            nodes.Clear();
            connections.Clear();
        }

        /// <summary>
        /// Deletes a node and all of it's related game objects.
        /// </summary>
        /// <param name="forDeletion"></param>
        void DeleteNode(Node forDeletion)
        {
            if (selectedNode == forDeletion)
                selectedNode = null;

            foreach (Connection c in forDeletion.FromConnections)
            {
                c.fromNode.ToConnections.Remove(c);
                currentHFSM.Transitions.Remove(c.transition);
                DestroyImmediate(c.transition);
                connections.Remove(c);
            }

            foreach (Connection c in forDeletion.ToConnections)
            {
                c.toNode.FromConnections.Remove(c);
                currentHFSM.Transitions.Remove(c.transition);
                DestroyImmediate(c.transition);
                connections.Remove(c);
            }

            //If state is parent, delete its HFSM.
            if (forDeletion.NodeState is ParentState)
            {
                DestroyHFSM(((ParentState)forDeletion.NodeState).StateHFSM);
            }

            currentHFSM.States.Remove(forDeletion.NodeState);
            DestroyImmediate(forDeletion.NodeState);
            nodes.Remove(forDeletion);
        }

        #region[CallBack Methods]
        /// <summary>
        /// Add a new state.
        /// </summary>
        /// <param name="sender"></param>
        void AddNodeCallBack(object pos)
        {
            if (Selection.activeTransform == false)
                return;

            Vector2 position = (Vector2)pos;
            State state = CreateInstance<State>();
            state.hideFlags = HideFlags.HideInInspector;
            state.nodeEditorLoc = position;

            AddNode(state);
        }

        /// <summary>
        /// Adding a parent HFSM node.
        /// </summary>
        /// <param name="sender"></param>
        void AddHFSMCallBack(object pos)
        {
            if (Selection.activeTransform == false)
                return;

            Vector2 position = (Vector2)pos;
            ParentState state = CreateInstance<ParentState>();
            state.hideFlags = HideFlags.HideInInspector;
            state.StateHFSM = CreateInstance<HFSM>();
            state.StateHFSM.hideFlags = HideFlags.HideInInspector;
            state.StateHFSM.hfsmName = state.StateName;
            hfsms.Add(state.StateHFSM);
            state.nodeEditorLoc = position;

            AddNode(state);
        }

        /// <summary>
        /// Create a new state while making a new transition and connect them.
        /// </summary>
        /// <param name="sender">Id of new state.</param>
        void NewStateViaTransitionCallBack(object id)
        {
            AddNodeCallBack(id);
            AddTransition(newTransitionNodeId, nodes.Count - 1);
            CancelTransitioning();
        }

        /// <summary>
        /// Start creating a new transition.
        /// </summary>
        /// <param name="id">Id of window.</param>
        void NewTransitionCallBack(object id)
        {

            newTransitionNodeId = (int)id;
            currentMode = NodeEditorMode.NewTransition;
        }

        void DeleteNodeCallBack(object id)
        {
            Node forDeletion = nodes[(int)id];
            DeleteNode(forDeletion);
        }
        #endregion

        void DestroyHFSM(HFSM hfsm)
        {
            foreach(State s in hfsm.States)
            {
                if (s is ParentState)
                    DestroyHFSM(((ParentState)s).StateHFSM);
                DestroyImmediate(s);
            }
            foreach (Transition t in hfsm.Transitions)
            {
                DestroyImmediate(t);
            }
            hfsms.Remove(hfsm);
            DestroyImmediate(hfsm);
        }

        #region[DrawingMethods]
        /// <summary>
        /// Window functions.
        /// </summary>
        /// <param name="id">Id of window.</param>
        void DrawNodeWindow(int id)
        {
            if (currentMode != NodeEditorMode.Editing)
                return;

            //Right-click menu
            Event e = Event.current;
            
            if (e.type == EventType.mouseUp)
            {
                selectedNode = nodes[id];
            }
            
            if (e.type == EventType.mouseUp && e.button == 1)
            {
                GenericMenu menu = new GenericMenu();
                menu.AddItem(new GUIContent("New Transition"), false, new GenericMenu.MenuFunction2(NewTransitionCallBack),
                    id);
                menu.AddItem(new GUIContent("Set As Start State"), false, new GenericMenu.MenuFunction2(NewTransitionCallBack),
                    id);
                menu.AddItem(new GUIContent("Delete"), false, new GenericMenu.MenuFunction2(DeleteNodeCallBack),
                    id);
                menu.ShowAsContext();
                e.Use();

            }

            //Update position
            if (e.type == EventType.mouseUp && e.button == 0)
            {
                nodes[id].NodeState.nodeEditorLoc.x = nodes[id].NodeRect.x;
                nodes[id].NodeState.nodeEditorLoc.y = nodes[id].NodeRect.y;
            }

            GUI.DragWindow();
        }

        /// <summary>
        /// Draw curve between two rects.
        /// </summary>
        /// <param name="start">Starting rect.</param>
        /// <param name="end">End rect.</param>
        void DrawNodeCurve(Rect start, Rect end)
        {
            Vector3 startPos = new Vector3(start.x + start.width, start.y + start.height / 2, 0);
            Vector3 endPos = new Vector3(end.x, end.y + end.height / 2, 0);
            DrawCurve(startPos, endPos);
        }

        /// <summary>
        /// Draw curve for drawing new transitions.
        /// </summary>
        /// <param name="start">Starting rect.</param>
        /// <param name="mousePos">Mouse position.</param>
        void DrawNewTransitionCurve(Rect start, Vector2 mousePos)
        {
            Vector3 startPos = new Vector3(start.x + start.width, start.y + start.height / 2, 0);
            DrawCurve(startPos, mousePos);
        }

        /// <summary>
        /// Draw curve representing transition.
        /// </summary>
        /// <param name="startPos">Start position of curve.</param>
        /// <param name="endPos">End position.</param>
        void DrawCurve(Vector3 startPos, Vector3 endPos)
        {
            Vector3 startTan = startPos + Vector3.right * 50;
            Vector3 endTan = endPos + Vector3.left * 50;
            Color shadowCol = new Color(0, 0, 0, 0.06f);

            for (int i = 0; i < 3; i++)
            {// Draw a shadow
                Handles.DrawBezier(startPos, endPos, startTan, endTan, shadowCol, null, (i + 1) * 5);
            }

            Handles.DrawBezier(startPos, endPos, startTan, endTan, Color.black, null, 1);
        }

        /// <summary>
        /// Draw marker to show direction of transitions.
        /// </summary>
        /// <param name="window">The window transitions too.</param>
        private void DrawDirectionMarker(Vector2 pos)
        {
            //Handles.BeginGUI();
            Handles.color = Color.black;
            Handles.ConeCap(0, pos, Quaternion.identity, 15);
            //Handles.EndGUI();
            //Repaint();
        }
        
        /// <summary>
        /// Draw background grid.
        /// </summary>
        /// <param name="gridSpacing"></param>
        /// <param name="gridOpacity"></param>
        /// <param name="gridColor"></param>
        private void DrawGrid(float gridSpacing, float gridOpacity, Color gridColor)
        {
            int widthDivs = Mathf.CeilToInt(position.width / gridSpacing);
            int heightDivs = Mathf.CeilToInt(position.height / gridSpacing);

            Handles.BeginGUI();
            Handles.color = new Color(gridColor.r, gridColor.g, gridColor.b, gridOpacity);

            offset += drag * 0.5f;
            Vector3 newOffset = new Vector3(offset.x % gridSpacing, offset.y % gridSpacing, 0);

            for (int i = 0; i < widthDivs; i++)
            {
                Handles.DrawLine(new Vector3(gridSpacing * i, -gridSpacing, 0) + newOffset, new Vector3(gridSpacing * i, position.height, 0f) + newOffset);
            }

            for (int j = 0; j < heightDivs; j++)
            {
                Handles.DrawLine(new Vector3(-gridSpacing, gridSpacing * j, 0) + newOffset, new Vector3(position.width, gridSpacing * j, 0f) + newOffset);
            }

            Handles.color = Color.white;
            Handles.EndGUI();
        }
        #endregion
    }
}                              
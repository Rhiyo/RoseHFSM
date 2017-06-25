using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

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

        public Node(State _state, Rect _rect)
        {
            state = _state;
            rect = _rect;
        }

        public void AddToConnection(Connection connection)
        {
            toConnections.Add(connection);
        }

        public void AddFromConnection(Connection connection)
        {
            fromConnections.Add(connection);
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace RoseHFSM {
    /// <summary>
    /// Transition between two nodes.
    /// </summary>
    public class Connection {
        public Node toNode;
        public Node fromNode;
        public Transition transition;

        public Connection(Node _fromNode, Node _toNode, Transition _transition)
        {
            toNode = _toNode;
            fromNode = _fromNode;
            transition = _transition;
        }
    }
}

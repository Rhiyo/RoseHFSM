using System;

namespace RoseHFSM
{
    [AttributeUsage(AttributeTargets.Field)]
    public class NodeFieldAttribute : Attribute
    {
        private bool children;

        public NodeFieldAttribute()
        {

        }

        /// <summary>
        /// Allow to find child fields to display.
        /// </summary>
        /// <param name="children"></param>
        public NodeFieldAttribute(bool children)
        {
            this.children = children;
        }
    }
}
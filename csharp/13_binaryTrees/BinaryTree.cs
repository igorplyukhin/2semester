using System;
using System.Collections;
using System.Collections.Generic;

namespace BinaryTrees
{
    public class TreeNode<T> where T : IComparable
    {
        public T Value { get; }
        public TreeNode<T> LeftChild { get; set; }
        public TreeNode<T> RightChild { get; set; }
        public TreeNode<T> Parent { get; }
        public int SubTreeCount { get; set; }

        public TreeNode(T value)
        {
            Value = value;
        }

        public TreeNode(T value, TreeNode<T> parent)
        {
            Value = value;
            Parent = parent;
        }
    }

    public class BinaryTree<T> : IEnumerable<T> where T : IComparable
    {
        public T this[int i]
        {
            get
            {
                if (i < 0 || RootNode == null)
                    throw new IndexOutOfRangeException();
                return GetIndex(RootNode, RootNode.SubTreeCount, i, RootNode.Value);
            }
        }
        private TreeNode<T> RootNode { get; set; }

        private T GetIndex(TreeNode<T> root, int currentIndex, int i, T f)
        {
            if (currentIndex > i)
                f = GetIndex(root.LeftChild, root.LeftChild.SubTreeCount, i, f);
            else if (currentIndex < i)
                f = GetIndex(root.RightChild,root.SubTreeCount - root.RightChild.SubTreeCount - 1, i, f);
            else 
                return root.Value;
            return f;
        }

        public void Add(T val)
        {
            if (RootNode == null)
            {
                RootNode = new TreeNode<T>(val);
                return;
            }

            var currentNode = RootNode;
            while (true)
                if (val.CompareTo(currentNode.Value) < 0)
                {
                    if (currentNode.LeftChild == null)
                    {
                        currentNode.LeftChild = new TreeNode<T>(val, currentNode);
                        var child = currentNode.LeftChild;
                        var parent = currentNode;
                        while (parent != null)
                        {
                            if (parent.LeftChild == child)
                                parent.SubTreeCount++;
                            child = parent;
                            parent = parent.Parent;
                        }
                        break;
                    }

                    currentNode = currentNode.LeftChild;
                }
                else
                {
                    if (currentNode.RightChild == null)
                    {
                        currentNode.RightChild = new TreeNode<T>(val, currentNode);
                        break;
                    }

                    currentNode = currentNode.RightChild;
                }
        }

        public bool Contains(T val)
        {
            var currentNode = RootNode;
            while (true)
            {
                if (currentNode == null)
                    return false;
                if (currentNode.Value.Equals(val))
                    return true;
                currentNode = val.CompareTo(currentNode.Value) < 0
                    ? currentNode.LeftChild
                    : currentNode.RightChild;
            }
        }

        private static IEnumerable<T> GetNextNode(TreeNode<T> root)
        {
            if (root != null)
            {
                foreach (var e in GetNextNode(root.LeftChild))
                    yield return e;

                yield return root.Value;
                foreach (var e in GetNextNode(root.RightChild))
                    yield return e;
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            return GetNextNode(RootNode).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
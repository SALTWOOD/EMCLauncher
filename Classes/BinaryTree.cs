//二叉树节点类
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace EMCL.Classes
{
    public class Tree<T> : IEnumerable<T> where T : IComparable<T>
    {
        // 左子树
        public Tree<T>? left;
        // 右子树
        public Tree<T>? right;
        // 数据
        public TreeNodeItem<T> data;

        public Tree()
        {
            // 初始化数据
            this.data = new TreeNodeItem<T>();
            // 初始化左子树
            this.left = null;
            // 初始化右子树
            this.right = null;
        }

        public Tree(T data)
        {
            // 初始化数据
            this.data = new TreeNodeItem<T>(data);
        }

        // 插入数据
        public void Insert(T data)
        {
            // 如果没有数据，则直接设置数据
            if (!this.data.HasItem)
            {
                this.data.SetData(data);
                return;
            }
            // 如果数据不为空，则比较数据和当前数据
            if (this.data.Value != null)
            {
                if (this.data.Value.CompareTo(data) > 0)
                {
                    // 如果左子树不为空，则插入
                    if (this.left == null)
                    {
                        this.left = new Tree<T>(data);
                    }
                    else
                    {
                        this.left.Insert(data);
                    }
                }
                else
                {
                    // 如果右子树不为空，则插入
                    if (this.right == null)
                    {
                        this.right = new Tree<T>(data);
                    }
                    else
                    {
                        this.right.Insert(data);
                    }
                }
            }
        }

        public List<T> WalkTree()
        {
            List<T> list = new List<T>();
            if (this.left != null)
            {
                list = list.Concat(this.left.WalkTree()).ToList();
            }
            if (this.data.Value != null)
            {
                list.Add(this.data.Value);
            }
            if (this.right != null)
            {
                list = list.Concat(this.right.WalkTree()).ToList();
            }
            return list;
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            List<T> list;
            list = this.WalkTree();
            foreach (T node in list)
            {
                yield return node;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            foreach (T node in this)
            {
                yield return (object)node;
            }
        }
    }

    public class TreeNodeItem<T> where T : IComparable<T>
    {
        // 数据
        private T? data;
        private bool hasItem = false;

        public T? Value { get { return this.data; } }

        public bool HasItem
        {
            get => this.hasItem;
        }

        public TreeNodeItem()
        {
            // 初始化数据
            this.data = default(T);
            this.hasItem = false;
        }

        public TreeNodeItem(T data)
        {
            // 初始化数据
            this.data = data;
            this.hasItem = true;
        }

        public void SetData(T data)
        {
            // 设置数据
            this.data = data;
            this.hasItem = true;
        }

        public T? GetData()
        {
            if (this.HasItem)
            {
                return this.Value!;
            }
            else
            {
                return default(T);
            }
        }

        public void Clear()
        {
            this.data = default(T);
            this.hasItem = false;
        }
    }
}
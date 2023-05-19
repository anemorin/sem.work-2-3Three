using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace TaskTwoThreeTree;

public class TwoThreeTree<T> : ITwoThreeTree<T> where T : IComparable<T>
{
    /// Корень дерева.
    public Node<T> RootNode { get; set; }
    public int IterationsCountOfAdding { get; set; }
    public int IterationsCountOfFinding { get; set; }
    public int IterationsCountOfRemoving { get; set; }
    public int TempForRemove = 0;
    /// Пустое дерево или нет.
    public bool IsEmpty = true;
    
    /// Высота дерева.
    private int _height = 0;
    public bool Add(T data)
    {
        if (Contains(data) || data.Equals(default(T)))
        {
            IterationsCountOfAdding++;
            return false;
        }
        else if (IsEmpty)
        {
            IterationsCountOfAdding++;
            RootNode = new Node<T>(data);
            IsEmpty = false;
            _height++;
        }
        else
        {
            var parent = RootNode;
            var height = 1;
            while (height != _height)
            {
                IterationsCountOfAdding++;
                if (parent.DataLeft.CompareTo(data) > 0)
                {
                    parent = parent.Left;
                }
                else if ((parent.DataRight == null && parent.DataLeft.CompareTo(data) < 0) || parent.DataRight?.CompareTo(data) < 0 )
                {
                    parent = parent.Right;
                }
                else if (parent.DataLeft.CompareTo(data) < 0 && parent.DataRight?.CompareTo(data) > 0)
                {
                    parent = parent.MiddleRight;
                }
                height++;
            }

            if (parent.DataRight.Equals(default(T)))
            {
                if (parent.DataLeft.CompareTo(data) > 0)
                {
                    (parent.DataRight, parent.DataLeft) = (parent.DataLeft, data);
                }
                else
                {
                    parent.DataRight = data;
                }
            }
            else if (parent.DataRight.CompareTo(data) > 0)
            {
                if (parent.DataLeft.CompareTo(data) > 0)
                {
                    (parent.DataMiddle, parent.DataLeft) = (parent.DataLeft, data);
                }
                else
                { 
                    parent.DataMiddle = data;
                }
            }
            else
            {
                (parent.DataMiddle, parent.DataRight) = (parent.DataRight, data);
            }
            while (!parent.DataMiddle.Equals(default(T)))
            {
                IterationsCountOfAdding++;
                if (IsRightParent(parent))
                {
                    if (parent.Parent.DataRight.Equals(default(T)))
                    {
                        parent.Parent.DataRight = parent.DataMiddle;
                        parent.DataMiddle = default(T);
                        var left = new Node<T>(parent.DataLeft);
                        var right = new Node<T>(parent.DataRight);
                        left.Parent = parent.Parent;
                        right.Parent = parent.Parent;
                        left.Left = parent.Left;
                        left.Right = parent.MiddleLeft;
                        right.Left = parent.MiddleRight;
                        right.Right = parent.Right;
                        GetParent(parent, left, right);
                        parent.Parent.MiddleRight = left;
                        parent.Parent.Right = right;
                        if (parent.Parent != null)
                        {
                            parent = parent.Parent;
                        }
                    }
                    else
                    {
                        (parent.Parent.DataMiddle, parent.Parent.DataRight) = (parent.Parent.DataRight, parent.DataMiddle);
                        parent.DataMiddle = default(T);
                        var left = new Node<T>(parent.DataLeft);
                        var right = new Node<T>(parent.DataRight);
                        left.Parent = parent.Parent;
                        right.Parent = parent.Parent;
                        left.Left = parent.Left;
                        left.Right = parent.MiddleLeft;
                        right.Left = parent.MiddleRight;
                        right.Right = parent.Right;
                        GetParent(parent, left, right);
                        parent.Parent.MiddleLeft = parent.Parent.MiddleRight;
                        parent.Parent.MiddleRight = left;
                        parent.Parent.Right = right;
                        if (parent.Parent != null)
                        {
                            parent = parent.Parent;
                        }
                    }
                }
                else if (IsLeftParent(parent))
                {
                    if (parent.Parent.DataRight.Equals(default(T)))
                    {
                        (parent.Parent.DataRight, parent.Parent.DataLeft) = (parent.Parent.DataLeft, parent.DataMiddle);
                        parent.DataMiddle = default(T);
                        var left = new Node<T>(parent.DataLeft);
                        var right = new Node<T>(parent.DataRight);
                        left.Parent = parent.Parent;
                        right.Parent = parent.Parent;
                        left.Left = parent.Left;
                        left.Right = parent.MiddleLeft;
                        right.Left = parent.MiddleRight;
                        right.Right = parent.Right;
                        GetParent(parent, left, right);
                        parent.Parent.Left = left;
                        parent.Parent.MiddleRight = right;
                        if (parent.Parent != null)
                        {
                            parent = parent.Parent;
                        }
                    }
                    else
                    {
                        (parent.Parent.DataMiddle, parent.Parent.DataLeft) = (parent.Parent.DataLeft, parent.DataMiddle);
                        parent.DataMiddle = default(T);
                        var left = new Node<T>(parent.DataLeft);
                        var right = new Node<T>(parent.DataRight);
                        left.Parent = parent.Parent;
                        right.Parent = parent.Parent;
                        left.Left = parent.Left;
                        left.Right = parent.MiddleLeft;
                        right.Left = parent.MiddleRight;
                        right.Right = parent.Right;
                        GetParent(parent, left, right);
                        parent.Parent.Left = left;
                        parent.Parent.MiddleLeft = right;
                        if (parent.Parent != null)
                        {
                            parent = parent.Parent;
                        }
                    }
                }
                else if (IsMiddleRightParent(parent))
                {
                    parent.Parent.DataMiddle = parent.DataMiddle;
                    parent.DataMiddle = default(T);
                    var left = new Node<T>(parent.DataLeft);
                    var right = new Node<T>(parent.DataRight);
                    left.Parent = parent.Parent;
                    right.Parent = parent.Parent;
                    left.Left = parent.Left;
                    left.Right = parent.MiddleLeft;
                    right.Left = parent.MiddleRight;
                    right.Right = parent.Right;
                    GetParent(parent, left, right);
                    parent.Parent.MiddleLeft = left;
                    parent.Parent.MiddleRight = right;
                    if (parent.Parent != null)
                    {
                        parent = parent.Parent;
                    }
                }
                else if (!IsParent(parent))
                {
                    var left = new Node<T>(parent.DataLeft);
                    var right = new Node<T>(parent.DataRight);
                    var nodeP = new Node<T>(parent.DataMiddle);
                    parent.DataMiddle = default(T);
                    left.Parent = nodeP;
                    right.Parent = nodeP;
                    left.Left = parent.Left;
                    left.Right = parent.MiddleLeft;
                    right.Left = parent.MiddleRight;
                    right.Right = parent.Right;
                    GetParent(parent, left, right);
                    nodeP.Right = right;
                    nodeP.Left = left;
                    RootNode = nodeP;
                    _height++;
                }
            }
        }
        return true;
    }

    /// <summary>
    /// Данный метод, присваивает родителя.
    /// Выполняется за O(1).
    private void GetParent(Node<T>? parent, Node<T> left, Node<T> right)
    {
        if (IsLeftParent(parent.Left))
        {
            parent.Left.Parent = left;
        }
        if (IsMiddleLeftParent(parent.MiddleLeft))
        {
            parent.MiddleLeft.Parent = left;
        }
        if (IsMiddleRightParent(parent.MiddleRight))
        {
            parent.MiddleRight.Parent = right;
        }
        if (IsRightParent(parent.Right))
        {
            parent.Right.Parent = right;
        }
    }

    /// <summary>
    /// Проверка содержится ли данный элемент в дереве.
    /// Данный метод выполняется за O(logn).

    /// true - данный элемент содержится в дереве.
    /// false - данный элемент не содержится в дереве.

    public bool Contains(T data)
    {
        var parent = RootNode;
        if (parent == null || data.Equals(default(T)))
        {
            return false;
        }
        if (RootNode.DataLeft.Equals(data) || RootNode.DataRight.Equals(data))
        {
            return true;
        }
        var height = 1;
        while (parent != null && height != _height + 1)
        {
            if (parent.DataLeft.Equals(data) || parent.DataRight.Equals(data))
            {
                return true;
            }
            if (parent.DataLeft.CompareTo(data) > 0)
            {
                parent = parent.Left;
            }
            else if ((parent.DataRight == null && parent.DataLeft.CompareTo(data) < 0) || parent.DataRight?.CompareTo(data) < 0)
            {
                parent = parent.Right;
            }
            else if (parent.DataLeft.CompareTo(data) < 0 && parent.DataRight?.CompareTo(data) > 0)
            {
                parent = parent.MiddleRight;
            }
            height++;
        }
        return false;
    }
    
    /// Метод, который ищет узел с данным элементом.
    /// Данный метод выполняется за O(logn).
    
    /// Узел с данным элементом.
    public Node<T> Find(T data)
    {
        TempForRemove = 0;
        var parent = RootNode;
        if (data.Equals(default(T)))
        {
            TempForRemove++;
            IterationsCountOfFinding++;
            return null;
        }
        if (RootNode.DataLeft.Equals(data) || RootNode.DataRight.Equals(data))
        {
            TempForRemove++;
            IterationsCountOfFinding++;
            return RootNode;
        }
        var height = 1;
        while (parent != null && height != _height + 1)
        {
            TempForRemove++;
            IterationsCountOfFinding++;
            if (parent.DataLeft.Equals(data) || parent.DataRight.Equals(data))
            {
                return parent;
            }
            if (parent.DataLeft.CompareTo(data) > 0)
            {
                parent = parent.Left;
            }
            else if ((parent.DataRight == null && parent.DataLeft.CompareTo(data) < 0) || parent.DataRight?.CompareTo(data) < 0)
            {
                parent = parent.Right;
            }
            else if (parent.DataLeft.CompareTo(data) < 0 && parent.DataRight?.CompareTo(data) > 0)
            {
                parent = parent.MiddleRight;
            }
            height++;
        }
        return null;
    }


    /// Этот метод удаляет элемент из дерева.
    /// Данный метод выполняется за O(logn).
    public bool Remove(T data)
    {
        var node = Find(data);
        IterationsCountOfRemoving += TempForRemove;
        if (node == null || data.Equals(default(T)))
        {
            IterationsCountOfRemoving++;
            return false;
        }
        if (node.DataLeft.Equals(data))
        {
            if (node.Left != null)
            {
                var parent = node.Left;
                while (parent.Right != null)
                {
                    IterationsCountOfRemoving++;
                    parent = parent.Right;
                }
                if (!parent.DataRight.Equals(default(T)))
                {
                    node.DataLeft = parent.DataRight;
                    parent.DataRight = default(T);
                }
                else if (parent.Equals(node.Left))
                {
                    node.DataLeft = parent.DataLeft;
                    parent.DataLeft = default(T);
                    if (parent.Left == null)
                    {
                        Sheet(parent);
                    }
                    else if (!parent.Parent.DataRight.Equals(default(T)))
                    {
                        if (!parent.Parent.MiddleRight.DataRight.Equals(default(T)))
                        {
                            parent.DataLeft = parent.Parent.DataLeft;
                            parent.Parent.DataLeft = parent.Parent.MiddleRight.DataLeft;
                            parent.Parent.MiddleRight.DataLeft = parent.Parent.MiddleRight.DataRight;
                            parent.Parent.MiddleRight.DataRight = default(T);
                        }
                        else
                        {
                            parent.DataLeft = parent.Parent.DataLeft;
                            parent.DataRight = parent.Parent.MiddleRight.DataLeft;
                            parent.Parent.DataLeft = parent.Parent.DataRight;
                            parent.Parent.DataRight = default(T);
                            parent.Parent.MiddleRight = null;
                        }
                    }
                    else
                    {
                        if (parent.Parent.Right.DataRight.Equals(default(T)))
                        {
                            parent.DataLeft = parent.Parent.DataLeft;
                            parent.DataRight = parent.Parent.Right.DataLeft;
                            parent.Parent.DataLeft = default(T);
                            parent.Parent.Right = null;
                            parent = parent.Parent;
                            if (IsLeftParent(parent))
                            {
                                FlippingLeft(parent);
                            }
                            else if (IsRightParent(parent))
                            {
                                FlippingRight(parent);
                            }
                            else if (IsMiddleRightParent(parent))
                            {
                                FlippingMiddleRight(parent);
                            }
                            else
                            {
                                RootNode = parent.Left;
                                parent.Left.Parent = null;
                                _height--;
                            }
                        }
                        else
                        {
                            parent.DataLeft = parent.Parent.DataLeft;
                            parent.Parent.DataLeft = parent.Parent.Right.DataLeft;
                            parent.Parent.Right.DataLeft = parent.Parent.Right.DataRight;
                            parent.Parent.Right.DataRight = default(T);
                        }
                    }
                }
                else
                {
                    node.DataLeft = parent.DataLeft;
                    parent.DataLeft = default(T);
                    if (!parent.Parent.DataRight.Equals(default(T)))
                    {
                        if (!parent.Parent.MiddleRight.DataRight.Equals(default(T)))
                        {
                            parent.DataLeft = parent.Parent.DataRight;
                            parent.Parent.DataRight = parent.Parent.MiddleRight.DataRight;
                            parent.Parent.MiddleRight.DataRight = default(T);
                        }
                        else
                        {
                            parent.DataLeft = parent.Parent.MiddleRight.DataLeft;
                            parent.DataRight = parent.Parent.DataRight;
                            parent.Parent.DataRight = default(T);
                            parent.Parent.MiddleRight = null;
                        }
                    }
                    else
                    {
                        if (parent.Parent.Left.DataRight.Equals(default(T)))
                        {
                            parent.Parent.Left.DataRight = parent.Parent.DataLeft;
                            parent.Parent.DataLeft = default(T);
                            parent = parent.Parent;
                            parent.Right = null;
                            if (IsLeftParent(parent))
                            {
                                FlippingLeft(parent);
                            }
                            else if (IsRightParent(parent))
                            {
                                FlippingRight(parent);
                            }
                            else if (IsMiddleRightParent(parent))
                            {
                                FlippingMiddleRight(parent);
                            }
                            else
                            {
                                RootNode = parent.Left;
                                parent.Left.Parent = null;
                                _height--;
                            }
                        }
                        else
                        {
                            parent.DataLeft = parent.Parent.DataLeft;
                            parent.Parent.DataLeft = parent.Parent.Left.DataRight;
                            parent.Parent.Left.DataRight = default(T);
                        }
                    }
                }
            }
            else
            {
                var parent = node;
                parent.DataLeft = default(T);
                if (!parent.DataRight.Equals(default(T)))
                {
                    parent.DataLeft = parent.DataRight;
                    parent.DataRight = default(T);
                }
                else if (IsLeftParent(parent))
                {
                    if (!parent.Parent.DataRight.Equals(default(T)))
                    {
                        if (!parent.Parent.MiddleRight.DataRight.Equals(default(T)))
                        {
                            parent.DataLeft = parent.Parent.DataLeft;
                            parent.Parent.DataLeft = parent.Parent.MiddleRight.DataLeft;
                            parent.Parent.MiddleRight.DataLeft = parent.Parent.MiddleRight.DataRight;
                            parent.Parent.MiddleRight.DataRight = default(T);
                        }
                        else
                        {
                            parent.DataLeft = parent.Parent.DataLeft;
                            parent.DataRight = parent.Parent.MiddleRight.DataLeft;
                            parent.Parent.DataLeft = parent.Parent.DataRight;
                            parent.Parent.DataRight = default(T);
                            parent.Parent.MiddleRight = null;
                        }
                    }
                    else
                    {
                        if (parent.Parent.Right.DataRight.Equals(default(T)))
                        {
                            parent.DataLeft = parent.Parent.DataLeft;
                            parent.DataRight = parent.Parent.Right.DataLeft;
                            parent.Parent.DataLeft = default(T);
                            parent.Parent.Right = null;
                            parent = parent.Parent;
                            if (IsLeftParent(parent))
                            {
                                FlippingLeft(parent);
                            }
                            else if (IsRightParent(parent))
                            {
                                FlippingRight(parent);
                            }
                            else if (IsMiddleRightParent(parent))
                            {
                                FlippingMiddleRight(parent);
                            }
                            else
                            {
                                RootNode = parent.Left;
                                parent.Left.Parent = null;
                                _height--;
                            }
                        }
                        else
                        {
                            parent.DataLeft = parent.Parent.DataLeft;
                            parent.Parent.DataLeft = parent.Parent.Right.DataLeft;
                            parent.Parent.Right.DataLeft = parent.Parent.Right.DataRight;
                            parent.Parent.Right.DataRight = default(T);
                        }
                    }
                }
                else if (IsRightParent(parent))
                {
                    if (!parent.Parent.DataRight.Equals(default(T)))
                    {
                        if (!parent.Parent.MiddleRight.DataRight.Equals(default(T)))
                        {
                            parent.DataLeft = parent.Parent.DataRight;
                            parent.Parent.DataRight = parent.Parent.MiddleRight.DataRight;
                            parent.Parent.MiddleRight.DataRight = default(T);
                        }
                        else
                        {
                            parent.DataLeft = parent.Parent.MiddleRight.DataLeft;
                            parent.DataRight = parent.Parent.DataRight;
                            parent.Parent.DataRight = default(T);
                            parent.Parent.MiddleRight = null;
                        }
                    }
                    else
                    {
                        if (parent.Parent.Left.DataRight.Equals(default(T)))
                        {
                            parent.Parent.Left.DataRight = parent.Parent.DataLeft;
                            parent.Parent.DataLeft = default(T);
                            parent = parent.Parent;
                            parent.Right = null;
                            if (IsLeftParent(parent))
                            {
                                FlippingLeft(parent);
                            }
                            else if (IsRightParent(parent))
                            {
                                FlippingRight(parent);
                            }
                            else if (IsMiddleRightParent(parent))
                            {
                                FlippingMiddleRight(parent);
                            }
                            else
                            {
                                RootNode = parent.Left;
                                parent.Left.Parent = null;
                                _height--;
                            }
                        }
                        else
                        {
                            parent.DataLeft = parent.Parent.DataLeft;
                            parent.Parent.DataLeft = parent.Parent.Left.DataRight;
                            parent.Parent.Left.DataRight = default(T);
                        }
                    }
                }
                else if (IsMiddleRightParent(parent))
                {
                    if (!parent.Parent.Left.DataRight.Equals(default(T)))
                    {
                        parent.DataLeft = parent.Parent.DataLeft;
                        parent.Parent.DataLeft = parent.Parent.Left.DataRight;
                        parent.Parent.Left.DataRight = default(T);
                    }
                    else
                    {
                        parent.Parent.Left.DataRight = parent.Parent.DataLeft;
                        parent.Parent.DataLeft = parent.Parent.DataRight;
                        parent.Parent.DataRight = default(T);
                        parent.Parent.MiddleRight = null;
                    }
                }
            }
        }
        else
        {
            if (node.Right != null)
            {
                var parent = node.Right;
                while (parent.Left != null)
                {
                    IterationsCountOfRemoving++;
                    parent = parent.Left;
                }
                if (!parent.DataRight.Equals(default(T)))
                {
                    node.DataRight = parent.DataLeft;
                    parent.DataLeft = parent.DataRight;
                    parent.DataRight = default(T);
                }
                else if (parent.Equals(node.Right))
                {
                    node.DataRight = parent.DataLeft;
                    parent.DataLeft = default(T);
                    if (parent.Left == null)
                    {
                        Sheet(parent);
                    }
                    else if (!parent.Parent.DataRight.Equals(default(T)))
                    {
                        if (!parent.Parent.MiddleRight.DataRight.Equals(default(T)))
                        {
                            parent.DataLeft = parent.Parent.DataRight;
                            parent.Parent.DataRight = parent.Parent.MiddleRight.DataRight;
                            parent.Parent.MiddleRight.DataRight = default(T);
                        }
                        else
                        {
                            parent.DataLeft = parent.Parent.MiddleRight.DataLeft;
                            parent.DataRight = parent.Parent.DataRight;
                            parent.Parent.DataRight = default(T);
                            parent.Parent.MiddleRight = null;
                        }
                    }
                    else
                    {
                        if (parent.Parent.Left.DataRight.Equals(default(T)))
                        {
                            parent.Parent.Left.DataRight = parent.Parent.DataLeft;
                            parent = parent.Parent;
                            parent.Right = null;
                            if (IsLeftParent(parent))
                            {
                                FlippingLeft(parent);
                            }
                            else if (IsRightParent(parent))
                            {
                                FlippingRight(parent);
                            }
                            else if (IsMiddleRightParent(parent))
                            {
                                FlippingMiddleRight(parent);
                            }
                            else
                            {
                                RootNode = parent.Left;
                                parent.Left.Parent = null;
                                _height--;
                            }
                        }
                        else
                        {
                            parent.DataLeft = parent.Parent.DataLeft;
                            parent.Parent.DataLeft = parent.Parent.Left.DataRight;
                            parent.Parent.Left.DataRight = default(T);
                        }
                    }
                }
                else
                {
                    node.DataRight = parent.DataLeft;
                    parent.DataLeft = default(T);
                    if (!parent.Parent.DataRight.Equals(default(T)))
                    {
                        if (!parent.Parent.MiddleRight.DataRight.Equals(default(T)))
                        {
                            parent.DataLeft = parent.Parent.DataLeft;
                            parent.Parent.DataLeft = parent.Parent.MiddleRight.DataLeft;
                            parent.Parent.MiddleRight.DataLeft = parent.Parent.MiddleRight.DataRight;
                            parent.Parent.MiddleRight.DataRight = default(T);
                        }
                        else
                        {
                            parent.DataLeft = parent.Parent.DataLeft;
                            parent.DataRight = parent.Parent.MiddleRight.DataLeft;
                            parent.Parent.DataLeft = parent.Parent.DataRight;
                            parent.Parent.DataRight = default(T);
                            parent.Parent.MiddleRight = null;
                        }
                    }
                    else
                    {
                        if (parent.Parent.Right.DataRight.Equals(default(T)))
                        {
                            parent.DataLeft = parent.Parent.DataLeft;
                            parent.DataRight = parent.Parent.Right.DataLeft;
                            parent.Parent.DataLeft = default(T);
                            parent.Parent.Right = null;
                            parent = parent.Parent;
                            if (IsLeftParent(parent))
                            {
                                FlippingLeft(parent);
                            }
                            else if (IsRightParent(parent))
                            {
                                FlippingRight(parent);
                            }
                            else if (IsMiddleRightParent(parent))
                            {
                                FlippingMiddleRight(parent);
                            }
                            else
                            {
                                RootNode = parent.Left;
                                parent.Left.Parent = null;
                                _height--;
                            }
                        }
                        else
                        {
                            parent.DataLeft = parent.Parent.DataLeft;
                            parent.Parent.DataLeft = parent.Parent.Right.DataLeft;
                            parent.Parent.Right.DataLeft = parent.Parent.Right.DataRight;
                            parent.Parent.Right.DataRight = default(T);
                        }
                    }
                }
            }
            else
            {
                node.DataRight = default(T);
            }
        }
        return true;
    }
    
    /// Этот метод балансирует поддерево.
    /// Выполняется за O(1).
    private void FlippingMiddleRight(Node<T> parent)
    {
        if (parent.Parent.Left.DataRight.Equals(default(T)))
        {
            parent.Parent.Left.DataRight = parent.Parent.DataLeft;
            parent.Parent.DataLeft = parent.Parent.DataRight;
            parent.Parent.DataRight = default(T);
            parent.Parent.Left.MiddleRight = parent.Parent.Left.Right;
            parent.Parent.Left.Right = parent.Left;
            parent.Left.Parent = parent.Parent.Left;
            parent.Parent.MiddleRight = null;
        }
        else
        {
            parent.DataLeft = parent.Parent.DataLeft;
            parent.Parent.DataLeft = parent.Parent.Left.DataRight;
            parent.Parent.Left.DataRight = default(T);
            parent.Right = parent.Left;
            parent.Left = parent.Parent.Left.Right;
            parent.Parent.Left.Right.Parent = parent;
            parent.Parent.Left.Right = parent.Parent.Left.MiddleRight;
            parent.Parent.Left.MiddleRight = null;
        }
    }
    
    /// Этот метод балансирует поддерево.
    /// Выполняется за O(1), а если очень повезёт, то за O(logn).
    private void FlippingLeft(Node<T> parent)
    {
        IterationsCountOfRemoving++;
        if (parent.Parent.DataRight.Equals(default(T)))
        {
            if (parent.Parent.Right.DataRight.Equals(default(T)))
            {
                parent.DataLeft = parent.Parent.DataLeft;
                parent.Parent.DataLeft = default(T);
                parent.DataRight = parent.Parent.Right.DataLeft;
                parent.MiddleRight = parent.Parent.Right.Left;
                parent.Parent.Right.Left.Parent = parent;
                parent.Right = parent.Parent.Right.Right;
                parent.Parent.Right.Right.Parent = parent;
                parent.Parent.Right = null;
                parent = parent.Parent;
                if (IsLeftParent(parent))
                {
                    FlippingLeft(parent);
                }
                else if (IsRightParent(parent))
                {
                    FlippingRight(parent);
                }
                else if (IsMiddleRightParent(parent))
                {
                    FlippingMiddleRight(parent);
                }
                else
                {
                    RootNode = parent.Left;
                    parent.Left.Parent = null;
                    _height--;
                }
            }
            else
            {
                parent.Right = parent.Parent.Right.Left;
                parent.Parent.Right.Left.Parent = parent;
                parent.DataLeft = parent.Parent.DataLeft;
                parent.Parent.DataLeft = parent.Parent.Right.DataLeft;
                parent.Parent.Right.DataLeft = parent.Parent.Right.DataRight;
                parent.Parent.Right.DataRight = default(T);
                parent.Parent.Right.Left = parent.Parent.Right.MiddleRight;
                parent.Parent.Right.MiddleRight = null;
            }
        }
        else
        {
            if (parent.Parent.MiddleRight.DataRight.Equals(default(T)))
            {
                parent.DataLeft = parent.Parent.DataLeft;
                parent.Parent.DataLeft = parent.Parent.DataRight;
                parent.Parent.DataRight = default(T);
                parent.DataRight = parent.Parent.MiddleRight.DataLeft;
                parent.MiddleRight = parent.Parent.MiddleRight.Left;
                parent.Parent.MiddleRight.Left.Parent = parent;
                parent.Right = parent.Parent.MiddleRight.Right;
                parent.Parent.MiddleRight.Right.Parent = parent;
                parent.Parent.MiddleRight = null;
            }
            else
            {
                parent.DataLeft = parent.Parent.DataLeft;
                parent.Parent.DataLeft = parent.Parent.MiddleRight.DataLeft;
                parent.Parent.MiddleRight.DataLeft = parent.Parent.MiddleRight.DataRight;
                parent.Parent.MiddleRight.DataRight = default(T);
                parent.Right = parent.Parent.MiddleRight.Left;
                parent.Parent.MiddleRight.Left.Parent = parent;
                parent.Parent.MiddleRight.Left = parent.Parent.MiddleRight.MiddleRight;
                parent.Parent.MiddleRight.MiddleRight = null;
            }
        }
    }
    
    /// Этот метод балансирует поддерево.
    /// Выполняется за O(1), а если очень повезёт, то за O(logn).
    private void FlippingRight(Node<T> parent)
    {
        IterationsCountOfRemoving++;
        if (parent.Parent.DataRight.Equals(default(T)))
        {
            if (parent.Parent.Left.DataRight.Equals(default(T)))
            {
                parent.DataLeft = parent.Parent.Left.DataLeft;
                parent.DataRight = parent.Parent.DataLeft;
                parent.Parent.DataLeft = default(T);
                parent.MiddleRight = parent.Parent.Left.Right;
                parent.Parent.Left.Right.Parent = parent;
                parent.Right = parent.Left;
                parent.Left = parent.Parent.Left.Left;
                parent.Parent.Left.Left.Parent = parent;
                parent.Parent.Left = parent.Parent.Right;
                parent.Parent.Right = null;
                parent = parent.Parent;
                if (IsLeftParent(parent))
                {
                    FlippingLeft(parent);
                }
                else if (IsRightParent(parent))
                {
                    FlippingRight(parent);
                }
                else if (IsMiddleRightParent(parent))
                {
                    FlippingMiddleRight(parent);
                }
                else
                {
                    RootNode = parent.Left;
                    parent.Left.Parent = null;
                    _height--;
                }
            }
            else
            {
                parent.Right = parent.Left;
                parent.Left = parent.Parent.Left.Right;
                parent.Parent.Left.Right.Parent = parent;
                parent.DataLeft = parent.Parent.DataLeft;
                parent.Parent.DataLeft = parent.Parent.Left.DataRight;
                parent.Parent.Left.DataRight = default(T);
                parent.Parent.Left.Right = parent.Parent.Left.MiddleRight;
                parent.Parent.Left.MiddleRight = null;
            }
        }
        else
        {
            if (parent.Parent.MiddleRight.DataRight.Equals(default(T)))
            {
                parent.DataLeft = parent.Parent.MiddleRight.DataLeft;
                parent.DataRight = parent.Parent.DataRight;
                parent.Right = parent.Left;
                parent.Left = parent.Parent.MiddleRight.Left;
                parent.Parent.MiddleRight.Left.Parent = parent;
                parent.MiddleRight = parent.Parent.MiddleRight.Right;
                parent.Parent.MiddleRight.Right.Parent = parent;
                parent.Parent.DataRight = default(T);
                parent.Parent.MiddleRight = null;
            }
            else
            {
                parent.DataLeft = parent.Parent.DataRight;
                parent.Parent.DataRight = parent.Parent.MiddleRight.DataRight;
                parent.Parent.MiddleRight.DataRight = default(T);
                parent.Right = parent.Left;
                parent.Left = parent.Parent.MiddleRight.Right;
                parent.Parent.MiddleRight.Right.Parent = parent;
                parent.Parent.MiddleRight.Right = parent.Parent.MiddleRight.MiddleRight;
                parent.Parent.MiddleRight.MiddleRight = null;
            }
        }
    }
    
    /// Балансирует дерево, начиная с листа.
    private void Sheet(Node<T> parent)
    {
        IterationsCountOfRemoving++;
        if (!parent.DataRight.Equals(default(T)))
        {
            parent.DataLeft = parent.DataRight;
            parent.DataRight = default(T);
        }
        else if (IsLeftParent(parent))
        {
            if (!parent.Parent.DataRight.Equals(default(T)))
            {
                if (!parent.Parent.MiddleRight.DataRight.Equals(default(T)))
                {
                    parent.DataLeft = parent.Parent.DataLeft;
                    parent.Parent.DataLeft = parent.Parent.MiddleRight.DataLeft;
                    parent.Parent.MiddleRight.DataLeft = parent.Parent.MiddleRight.DataRight;
                    parent.Parent.MiddleRight.DataRight = default(T);
                }
                else
                {
                    parent.DataLeft = parent.Parent.DataLeft;
                    parent.DataRight = parent.Parent.MiddleRight.DataLeft;
                    parent.Parent.DataLeft = parent.Parent.DataRight;
                    parent.Parent.DataRight = default(T);
                    parent.Parent.MiddleRight = null;
                }
            }
            else
            {
                if (parent.Parent.Right.DataRight.Equals(default(T)))
                {
                    parent.DataLeft = parent.Parent.DataLeft;
                    parent.DataRight = parent.Parent.Right.DataLeft;
                    parent.Parent.DataLeft = default(T);
                    parent.Parent.Right = null;
                    parent = parent.Parent;
                    if (IsLeftParent(parent))
                    {
                        FlippingLeft(parent);
                    }
                    else if (IsRightParent(parent))
                    {
                        FlippingRight(parent);
                    }
                    else if (IsMiddleRightParent(parent))
                    {
                        FlippingMiddleRight(parent);
                    }
                    else
                    {
                        RootNode = parent.Left;
                        parent.Left.Parent = null;
                        _height--;
                    }
                }
                else
                {
                    parent.DataLeft = parent.Parent.DataLeft;
                    parent.Parent.DataLeft = parent.Parent.Right.DataLeft;
                    parent.Parent.Right.DataLeft = parent.Parent.Right.DataRight;
                    parent.Parent.Right.DataRight = default(T);
                }
            }
        }
        else if (IsRightParent(parent))
        {
            if (!parent.Parent.DataRight.Equals(default(T)))
            {
                if (!parent.Parent.MiddleRight.DataRight.Equals(default(T)))
                {
                    parent.DataLeft = parent.Parent.DataRight;
                    parent.Parent.DataRight = parent.Parent.MiddleRight.DataRight;
                    parent.Parent.MiddleRight.DataRight = default(T);
                }
                else
                {
                    parent.DataLeft = parent.Parent.MiddleRight.DataLeft;
                    parent.DataRight = parent.Parent.DataRight;
                    parent.Parent.DataRight = default(T);
                    parent.Parent.MiddleRight = null;
                }
            }
            else
            {
                if (parent.Parent.Left.DataRight.Equals(default(T)))
                {
                    parent.Parent.Left.DataRight = parent.Parent.DataLeft;
                    parent.Parent.DataLeft = default(T);
                    parent = parent.Parent;
                    parent.Right = null;
                    if (IsLeftParent(parent))
                    {
                        FlippingLeft(parent);
                    }
                    else if (IsRightParent(parent))
                    {
                        FlippingRight(parent);
                    }
                    else if (IsMiddleRightParent(parent))
                    {
                        FlippingMiddleRight(parent);
                    }
                    else
                    {
                        RootNode = parent.Left;
                        parent.Left.Parent = null;
                        _height--;
                    }
                }
                else
                {
                    parent.DataLeft = parent.Parent.DataLeft;
                    parent.Parent.DataLeft = parent.Parent.Left.DataRight;
                    parent.Parent.Left.DataRight = default(T);
                }
            }
        }
        else if (IsMiddleRightParent(parent))
        {
            if (!parent.Parent.Left.DataRight.Equals(default(T)))
            {
                parent.DataLeft = parent.Parent.DataLeft;
                parent.Parent.DataLeft = parent.Parent.Left.DataRight;
                parent.Parent.Left.DataRight = default(T);
            }
            else
            {
                parent.Parent.Left.DataRight = parent.Parent.DataLeft;
                parent.Parent.DataLeft = parent.Parent.DataRight;
                parent.Parent.DataRight = default(T);
                parent.Parent.MiddleRight = null;
            }
        }
    }


    /// Проверка на левый потомок.
    private bool IsLeftParent(Node<T> node)
    {
        IterationsCountOfRemoving++;
        if (IsParent(node) && node.Parent!.Left!.Equals(node))
        {
            return true;
        }
        return false;
    }

    /// Проверка на правый потомок.
    private bool IsRightParent(Node<T> node)
    {
        IterationsCountOfRemoving++;
        if (IsParent(node) && node.Parent!.Right!.Equals(node))
        {
            return true;
        }
        return false;
    }


    /// Проверка на средний правый потомок.
    private bool IsMiddleRightParent(Node<T> node)
    {
        IterationsCountOfRemoving++;
        if (IsParent(node) && node.Parent!.MiddleRight!.Equals(node))
        {
            return true;
        }
        return false;
    }


    /// Проверка на средний левый потомок.
    private bool IsMiddleLeftParent(Node<T> node)
    {
        IterationsCountOfRemoving++;
        if (IsParent(node) && node.Parent!.MiddleLeft!.Equals(node))
        {
            return true;
        }
        return false;
    }


    /// Проверка на существование родителя.
    private bool IsParent(Node<T> node)
    {
        if (node == null || node.Parent == null)
        {
            return false;
        }
        return true;
    }
}

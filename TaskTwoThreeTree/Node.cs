using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskTwoThreeTree;

public class Node<T> where T : IComparable<T>
{
    public Node(T data)
    {
        DataLeft = data;
    }

    public Node<T>? Left { get; set; }
    public Node<T>? MiddleLeft { get; set; }
    public Node<T>? MiddleRight { get; set; }
    public Node<T>? Right { get; set; }
    public Node<T>? Parent { get; set; }
    public T? DataLeft { get; set; }
    public T? DataMiddle { get; set; }
    public T? DataRight { get; set; }
}

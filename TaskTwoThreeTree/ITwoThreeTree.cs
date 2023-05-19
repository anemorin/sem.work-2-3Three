using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskTwoThreeTree;

public interface ITwoThreeTree<T> where T : IComparable<T>
{
    public Node<T> RootNode { get; set; }
    bool Add(T data);
    Node<T> Find(T data);
    bool Remove(T data);
    bool Contains(T data);
}

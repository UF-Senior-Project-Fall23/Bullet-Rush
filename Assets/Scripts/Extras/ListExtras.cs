using System.Collections.Generic;
using System.Linq;

// Utility and extension methods for lists
public static class ListExtra
{
    // Resizes a list to the given size. The last parameter indicates what to fill the list with if it becomes larger.
    public static void Resize<T>(this List<T> list, int sz, T c)
    {
        int cur = list.Count;
        if (sz < cur)
            list.RemoveRange(sz, cur - sz);
        else if (sz > cur)
        {
            if (sz > list.Capacity)//this bit is purely an optimisation, to avoid multiple automatic capacity changes.
                list.Capacity = sz;
            list.AddRange(Enumerable.Repeat(c, sz - cur));
        }
    }
    
    // Resizes a list to the given size, passing a default-valued input as the fill value.
    public static void Resize<T>(this List<T> list, int sz) where T : new()
    {
        Resize(list, sz, new T());
    }
}
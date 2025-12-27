using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

// Object that controls the spell during runtime, essentialy the structure of it
// Stack Convention:
// First Pop = first argument (top of stack)
public class SpellRuntime : MonoBehaviour
{
    // Privates here
    private Stack<object> stack = new Stack<object>();
    private T Pop<T>()
    {
        if (stack.Count == 0){throw new System.InvalidOperationException("Stack Underflow");}
        
        return (T)stack.Pop();
    }
    
    private void Push(object value)
    {
        stack.Push(value);
    }

    // Miscelannious publics needed

    public void ClearStack()
    {
        stack.Clear();
    }

    public string StackToString()
    {
        // Print bottom → top
        return string.Join(", ", stack);
    }

    // Numbers Under, they add numbers, 1 through 9
    // figure out the amount to add when reading it in by parsing.
    public void PushNumber(float value)
    {
        stack.Push(value);
    }

    // Basic Math Operations Under, none should have returns (+-*/)
    
    public void Add()
    {
        float a = Pop<float>();
        float b = Pop<float>();
        Push(a+b);
    }

    public void Subtract()
    {
        float a = Pop<float>();
        float b = Pop<float>();
        Push(a-b);
    }

    public void Multiply()
    {
        float a = Pop<float>();
        float b = Pop<float>();
        Push(a*b);
    }

    public void Divide()
    {
        float a = Pop<float>();
        float b = Pop<float>();

        if (b == 0f){throw new InvalidOperationException("Division by zero");}

        Push(a/b);
    }

    // Unary Operations (kinda useful)
    public void Negate()
    {
        float a = Pop<float>();
        Push(-a);
    }

    public void Abs()
    {
        float a = Pop<float>();
        Push(MathF.Abs(a));
    }

    public void Sqrt()
    {
        float a = Pop<float>();

        if (a < 0f){throw new InvalidOperationException("Sqrt of negative value");}

        Push(MathF.Sqrt(a));
    }

    public void Ceil()
    {
        float a = Pop<float>();
        Push(MathF.Ceiling(a));
    }

    public void Floor()
    {
        float a = Pop<float>();
        Push(MathF.Floor(a));
    }

    // Power and Modulo

    public void Power()
    {
        float a = Pop<float>();
        float b = Pop<float>();
        Push(MathF.Pow(a,b));
    }

    public void Mod()
    {
        float a = Pop<float>();
        float b = Pop<float>();
        Push(a % b);
    }

    // Trig

    public void Sin()
    {
        float a = Pop<float>();
        Push(Mathf.Sin(a));
    }

    public void Cos()
    {
        float a = Pop<float>();
        Push(Mathf.Cos(a));
    }

    public void Tan()
    {
        float a = Pop<float>();
        Push(Mathf.Tan(a));
    }

    // Min and Max
    public void Min()
    {
        float a = Pop<float>();
        float b = Pop<float>();
        Push(Mathf.Min(a, b));
    }

    public void Max()
    {
        float a = Pop<float>();
        float b = Pop<float>();
        Push(Mathf.Max(a, b));
    }

    // Entropy
    public void Entropy()
    {
        Push(UnityEngine.Random.value);
    }

    ///
    /// Stack Manipulation
    /// 
    
    public void CopyVal()
    {
        int amount = (int)Pop<float>();
        if (amount < 0 || amount > stack.Count) {throw new InvalidOperationException("Invalid CopyVal amount");}

        // Grab top n elements
        object[] temp = stack.ToArray();

        for (int i = amount - 1; i >= 0; i--)
        {
            Push(temp[i]);
        }
    }

    public void Package()
    {
        float x = Pop<float>();
        float y = Pop<float>();

        Push(new UnityEngine.Vector2(x,y));
    }

    public void Unpackage()
    {
        UnityEngine.Vector2 v = Pop<UnityEngine.Vector2>();

        Push(v.y);
        Push(v.x);
    }

    public void RotationalTouch(int n)
    {
        if (n <= 1 || n > stack.Count)
            throw new InvalidOperationException("Invalid n");

        var temp = new List<object>(stack.ToArray());

        object target = temp[n-1];

        temp.RemoveAt(n-1);
        temp.Insert(0,target);

        stack.Clear();

        for (int i = temp.Count - 1; i >= 0; i--)
            stack.Push(temp[i]);
    }

    public void RotTouch2() => RotationalTouch(2);
    public void RotTouch3() => RotationalTouch(3);
    public void RotTouch4() => RotationalTouch(4);

    public void Size()
    {
        stack.Push((float)stack.Count);
    }

    public void Move()
    {
        int n = Pop<int>();

        if (stack.Count == 0)
            throw new InvalidOperationException("Empty stack");

        var temp = new List<object>(stack.ToArray()); // top → bottom

        if (n > 0)
        {
            if (n >= temp.Count)
                throw new InvalidOperationException("Move out of range");

            var val = temp[n];
            temp.RemoveAt(n);
            temp.Insert(0, val);
        }
        else if (n < 0)
        {
            int depth = -n;
            if (depth >= temp.Count)
                throw new InvalidOperationException("Move out of range");

            var val = temp[0];
            temp.RemoveAt(0);
            temp.Insert(depth, val);
        }

        stack.Clear();
        for (int i = temp.Count - 1; i >= 0; i--)
            stack.Push(temp[i]);
    }


}

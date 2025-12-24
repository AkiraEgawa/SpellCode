using UnityEngine;
using System;
using System.Collections.Generic;

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
    
    private void Push(float value)
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

}

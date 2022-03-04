using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

public class StrFor
{
    List<string> Usings = new List<string>();
    public StringBuilder _BB;
    public string _Base = "";
    StreamWriter _W;
    public StrFor(StreamWriter W)
    {
        _BB = new StringBuilder();
        _Base = "";
        _W = W;
    }

    public void NL(string New)
    {
        if (_W != null)
            _W.WriteLine(_Base + New);
        _BB.AppendLine(_Base + New);
    }
    public void AddUsing(string Using)
    {
        if(Usings.Contains(Using))
        {
            return;
        }
        else
        {
            Usings.Add(Using);
            NL("using " + Using + ";");
        }
    }
    public StrFor AddIndent()
    {
        _Base += "    ";
        return this;
    }
    public StrFor AddIndent2()
    {
        _Base += "....";
        return this;
    }
    public StrFor RemoveIndent()
    {
        _Base = _Base.Substring(0, _Base.Length - 4);
        return this;
    }


    public StrFor Clear()
    {
        _Base = "";
        return this;
    }

    public void OpenFunction()
    {
        NL("{");
        AddIndent();
    }

    public void CloseFunction()
    {
        if (_Base.Length >= 4)
        {
            RemoveIndent();
            NL("}");
        }
        //else
        //{
        //    UnityEngine.Debug.Log("indent imposible");
        //}
    }
    public void CloseFunctionPeriod()
    {
        RemoveIndent();
        NL("};");
    }
    public void Else()
    {
        NL("else");
    }

    public override string ToString()
    {
        return _BB.ToString();
    }
}
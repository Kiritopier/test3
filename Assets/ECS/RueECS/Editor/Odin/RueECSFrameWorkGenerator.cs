using System.Collections.Generic;

public abstract class RueECSFrameWorkGenerator : RueECSFrameWorkGenerator.Start, RueECSFrameWorkGenerator.Main, RueECSFrameWorkGenerator.DeclareEntity, RueECSFrameWorkGenerator.DeclareCollection, RueECSFrameWorkGenerator.DeclareComponent
{
    public interface Start
    {
        Main __Start ();
    }
    public abstract Main __Start ();
    public interface Main
    {
        DeclareComponent _CreateComponent (string Name);
        DeclareCollection _CreateCollection (string Name);
        DeclareEntity _CreateEntity (string EntityName);
    }
    public abstract DeclareComponent _CreateComponent (string Name);
    public abstract DeclareCollection _CreateCollection (string Name);
    public abstract DeclareEntity _CreateEntity (string EntityName);
    public interface DeclareEntity
    {
        DeclareEntity _AddComponent (string EntityName);
        Main _FinishEntity ();
    }
    public abstract DeclareEntity _AddComponent (string EntityName);
    public abstract Main _FinishEntity ();
    public interface DeclareCollection
    {
        DeclareCollection _AddComponentToCollection (string ComponentType);
        DeclareCollection _AddExceptionToCollection (string ComponentType);
        Main _FinishCollection ();
    }
    public abstract DeclareCollection _AddComponentToCollection (string ComponentType);
    public abstract DeclareCollection _AddExceptionToCollection (string ComponentType);
    public abstract Main _FinishCollection ();
    public interface DeclareComponent
    {
        Main _FinishComponent ();
        DeclareComponent _AddFieldToComponent (string VarType, string FieldName);
        DeclareComponent _AddFieldWithListenerToComponent (string VarType, string FieldName);
        DeclareComponent _AddFieldWithPrevNowListenerToComponent (string VarType, string FieldName, bool IsHidden);
    }
    public abstract Main _FinishComponent ();
    public abstract DeclareComponent _AddFieldToComponent (string VarType, string FieldName);
    public abstract DeclareComponent _AddFieldWithListenerToComponent (string VarType, string FieldName);
    public abstract DeclareComponent _AddFieldWithPrevNowListenerToComponent (string VarType, string FieldName, bool IsHidden);
}

public static class ExtFrameworkECS
{
    public static RueECSFrameWorkGenerator.Main FullComponent(this RueECSFrameWorkGenerator.Main e, string ComponentName, params (string, string, bool)[] Arguments)
    {
        var arg = e._CreateComponent(ComponentName);
        for (int i = 0; i < Arguments.Length; i ++)
        {
            arg._AddFieldWithPrevNowListenerToComponent(Arguments[i].Item1, Arguments[i].Item2,Arguments[i].Item3);
        }

        return arg._FinishComponent();
    }
    public static RueECSFrameWorkGenerator.Main FullCollection(this RueECSFrameWorkGenerator.Main e, string CollectionName, params string[] Components)
    {
        var c = e._CreateCollection(CollectionName);
        for (int i = 0; i < Components.Length; i++)
        {
            c._AddComponentToCollection(Components[i]);
        }
        return c._FinishCollection();
    }
    public static RueECSFrameWorkGenerator.Main FullEntity(this RueECSFrameWorkGenerator.Main e, string EntityName, params string[] Components)
    {
        var en = e._CreateEntity(EntityName);
        List<string> Parsed = new List<string>(Components.Length);

        for (int i = 0; i < Components.Length; i++)
        {
            if (!Parsed.Contains(Components[i]))
            {
                Parsed.Add(Components[i]);
            }
        }

        for (int i = 0; i < Parsed.Count; i++)
        {
            en._AddComponent(Parsed[i]);
        }
        return en._FinishEntity();
    }

    public static RueECSFrameWorkGenerator.Main FullEntity(this RueECSFrameWorkGenerator.Main e, string EntityName, string[] Bundle, params string[] Components)
    {
        var en = e._CreateEntity(EntityName);

        List<string> Parsed = new List<string>(Components.Length + Bundle.Length);

        for (int i = 0; i < Components.Length; i++)
        {
            if (!Parsed.Contains(Components[i]))
            {
                Parsed.Add(Components[i]);
            }
        }
        for (int i = 0; i < Bundle.Length; i++)
        {
            if (!Parsed.Contains(Bundle[i]))
            {
                Parsed.Add(Bundle[i]);
            }
        }

        for (int i = 0; i < Parsed.Count; i++)
        {
            en._AddComponent(Parsed[i]);
        }
        return en._FinishEntity();
    }

}

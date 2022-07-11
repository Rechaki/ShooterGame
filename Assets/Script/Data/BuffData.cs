using System.Collections;
using System.Collections.Generic;

public class BuffData
{
    public string id;
    public string name;
    public float durationTime;                                                              //
    public Actor owner;                                                                     //所有者
    public BuffAttributes attributes = BuffAttributes.None;
    public List<PropertyData> modifiedProperties = new List<PropertyData>();                //
}


public class PropertyData
{
    public ModifiedProperty property = ModifiedProperty.None;
    public ValueSource source = ValueSource.Set;
    public ModifiedProperty sourceType = ModifiedProperty.None;
    public float value;
    public int actualValue;
}
using System.Collections;
using System.Collections.Generic;

public delegate void Callback();

public class EventData
{
    public List<Callback> callbacks = new List<Callback>();
    public List<Callback> temp = new List<Callback>();
    public bool isInvoking;
}

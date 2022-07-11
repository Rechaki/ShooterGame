public class BaseData
{
    public delegate void EventDataHandler<T>(T t) where T : BaseData;
}

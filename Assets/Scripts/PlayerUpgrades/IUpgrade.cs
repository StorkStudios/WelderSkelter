public interface IUpgrade<T> where T : class, new()
{
    public void ApplyModifier(T modifier);
}

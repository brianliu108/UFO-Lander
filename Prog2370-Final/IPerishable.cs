namespace Prog2370_Final {
    /// <summary>
    /// For when an object can go bad at any arbitrary time. If <c>Perished</c> is set to false, the object is still
    /// good and can keep being used. If the object has perished though, it should be appropriately thrown out. 
    /// </summary>
    public interface IPerishable {
        bool Perished { get; }
    }
}
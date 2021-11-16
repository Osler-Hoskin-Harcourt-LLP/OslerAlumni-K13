namespace OslerAlumni.Core.Kentico.Models
{
    /// <summary>
    /// Any Page Type that implements this interface can be made publicly available.
    /// See OslerAuthorizeAttribute for Usage.
    /// </summary>
    public interface IPublicType
    {
        bool IsPublic { get; set; }
    }
}

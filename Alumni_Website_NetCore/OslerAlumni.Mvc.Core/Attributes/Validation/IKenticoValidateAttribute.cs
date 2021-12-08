namespace OslerAlumni.Mvc.Core.Attributes.Validation
{
    public interface IKenticoValidateAttribute
    {
        string FormatErrorMessage(string name);
    }
}
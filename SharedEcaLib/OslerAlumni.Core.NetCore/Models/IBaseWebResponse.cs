
using OslerAlumni.Core.Definitions;

namespace OslerAlumni.Core.Models
{
    public interface IBaseWebResponse<T>
    {
        T Result { get; set; }

        WebResponseStatus Status { get; set; }

        bool RefreshOnSuccess { get; set; }

    }
}

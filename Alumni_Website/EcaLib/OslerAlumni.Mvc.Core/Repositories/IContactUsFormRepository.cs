using System.Web;
using ECA.Core.Repositories;
using OslerAlumni.Mvc.Core.Kentico.Models.Forms;

namespace OslerAlumni.Mvc.Core.Repositories
{
    public interface IContactUsFormRepository 
        : IRepository
    {
        int? InsertContactUsFormItem(
            IContactUsFormItem contactUsFormItem, 
            HttpPostedFileBase uploadedFile = null);
    }
}

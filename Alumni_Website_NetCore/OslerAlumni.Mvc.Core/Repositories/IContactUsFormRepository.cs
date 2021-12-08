using System.Web;
using ECA.Core.Repositories;
using Microsoft.AspNetCore.Http;
using OslerAlumni.Mvc.Core.Kentico.Models.Forms;

namespace OslerAlumni.Mvc.Core.Repositories
{
    public interface IContactUsFormRepository 
        : IRepository
    {
        int? InsertContactUsFormItem(
            IContactUsFormItem contactUsFormItem, 
            IFormFile uploadedFile = null);
    }
}

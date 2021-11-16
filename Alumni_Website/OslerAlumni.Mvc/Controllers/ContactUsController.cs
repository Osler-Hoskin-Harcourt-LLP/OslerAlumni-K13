using System.Web.Mvc;
using ECA.Core.Models;
using ECA.PageURL.Services;
using Kentico.Content.Web.Mvc;
using Kentico.Content.Web.Mvc.Routing;
using OslerAlumni.Core.Definitions;
using OslerAlumni.Core.Kentico.Models;
using OslerAlumni.Core.Models;
using OslerAlumni.Core.Repositories;
using OslerAlumni.Mvc.Controllers;
using OslerAlumni.Mvc.Core.Attributes.ActionFilters;
using OslerAlumni.Mvc.Core.Controllers;
using OslerAlumni.Mvc.Core.Definitions;
using OslerAlumni.Mvc.Core.Kentico.Models;
using OslerAlumni.Mvc.Core.Repositories;
using OslerAlumni.Mvc.Core.Services;
using OslerAlumni.Mvc.Models;

[assembly: RegisterPageRoute(PageType_Page.CLASS_NAME, typeof(ContactUsController), Path = "/Contact-Us", ActionName = nameof(ContactUsController.Index))]

namespace OslerAlumni.Mvc.Controllers
{
    public class ContactUsController 
        : BaseController
    {
        private readonly IContactUsFormRepository _contactUsFormRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMvcEmailService _emailService;

        public ContactUsController(
            IContactUsFormRepository contactUsFormRepository, 
            IUserRepository userRepository,
            IMvcEmailService emailService,
            IPageUrlService pageUrlService,
            ContextConfig context,
            IPageDataContextRetriever dataRetriever)
            : base(pageUrlService, context, dataRetriever)
        {
            _contactUsFormRepository = contactUsFormRepository;
            _userRepository = userRepository;
            _emailService = emailService;
        }

        [HttpGet]
        public ActionResult Index()
        {
            var page = _dataRetriever.Retrieve<PageType_Page>().Page;

            var currentUser = _userRepository.CurrentUser;

            var isAuthenticated = currentUser != null;

            var contactUsPageViewModel = new ContactUsPageViewModel(page)
            {
                IsAuthenticated = isAuthenticated,
                FormPostModel = new ContactUsPostModel
                {
                    ReasonForContactingUsEnum = ReasonForContactingUs.GeneralInquiry,
                    IsAuthenticated = isAuthenticated
                }
            };
            
            var contactUsPostModel = contactUsPageViewModel.FormPostModel;

            //Don't prepulate for form for OslerNetwork Logins
            if (isAuthenticated && !_userRepository.IsSystemUser(currentUser.UserName))
            {
                contactUsPostModel.FirstName = currentUser.FirstName;
                contactUsPostModel.LastName = currentUser.LastName;
                contactUsPostModel.Email = currentUser.Email;
                contactUsPostModel.CompanyName = currentUser.Company;
            }

            return View(contactUsPageViewModel);
        }

        [HttpPost]
        [ValidateHeaderAntiForgeryToken(Order = 1)]
        [ValidateModel(Order = 2)]
        //Reason why we don't validate for html injections is because we don't check for them on any of AJAX post forms.
        //Therefore to keep it consistent with all forms this is needed.
        //Note even though this form is AJAX post, since it has a file upload, it still goes through the typical form post back process
        //which is why we need this.
        [ValidateInput(false)]
        public ActionResult Index(
            ContactUsPostModel model)
        {
            var contactUsId = _contactUsFormRepository
                .InsertContactUsFormItem(
                    model, 
                    model.FileUpload);

            if (contactUsId.HasValue)
            {
                model.ContactUsId = contactUsId;

                _emailService.SendContactUsNotificationExternalEmail(model);
                _emailService.SendContactUsNotificationInternalEmail(model);
            }

            return JsonContent(new BaseWebResponse<object>
            {
                Status = contactUsId.HasValue 
                    ? WebResponseStatus.Success
                    : WebResponseStatus.Error
            });
        }
    }
}

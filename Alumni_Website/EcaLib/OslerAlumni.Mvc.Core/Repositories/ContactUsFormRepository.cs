using System;
using System.Web;
using ECA.Core.Repositories;
using OslerAlumni.Core.Repositories;
using OslerAlumni.Mvc.Core.Definitions;
using OslerAlumni.Mvc.Core.Kentico.Models.Forms;
using OslerAlumni.Mvc.Core.Services;

namespace OslerAlumni.Mvc.Core.Repositories
{
    public class ContactUsFormRepository 
        : IContactUsFormRepository
    {
        private readonly IEventLogRepository _eventLogRepository;
        private readonly IUserRepository _userRepository;
        private readonly IFileService _fileService;

        public ContactUsFormRepository(
            IEventLogRepository eventLogRepository, 
            IUserRepository userRepository, 
            IFileService fileService)
        {
            _eventLogRepository = eventLogRepository;
            _userRepository = userRepository;
            _fileService = fileService;
        }

        public int? InsertContactUsFormItem(
            IContactUsFormItem contactUsFormItem, 
            HttpPostedFileBase uploadedFile = null)
        {
            try
            {
                if (uploadedFile != null)
                {
                    var attachmentName = _fileService.SaveFileForFormAttachment(uploadedFile);

                    if (string.IsNullOrWhiteSpace(attachmentName))
                    {
                        throw new Exception();
                    }

                    contactUsFormItem.Attachment = attachmentName;
                }

                var currentUser = _userRepository.CurrentUser;

                if (_userRepository.IsSystemUser(currentUser?.UserName))
                {
                    //Don't save user info for OslerNetwork Logins
                    currentUser = null;
                }

                var item = new ContactUsItem
                {
                    ReasonForContactingUsEnum = 
                        contactUsFormItem.ReasonForContactingUsEnum,
                    FirstName = contactUsFormItem.FirstName,
                    LastName = contactUsFormItem.LastName,
                    CompanyName = contactUsFormItem.CompanyName,
                    Message = contactUsFormItem.Message,
                    PhoneNumber = contactUsFormItem.PhoneNumber,
                    Subject = contactUsFormItem.Subject,
                    Email = contactUsFormItem.Email,
                    Attachment = contactUsFormItem.Attachment,
                    UserName = currentUser?.UserName,
                    UserGuid = currentUser?.UserGUID,
                };

                if (contactUsFormItem.ReasonForContactingUsEnum == ReasonForContactingUs.PostAnOpportunity)
                {
                    item.OpportunityType = contactUsFormItem.OpportunityType;
                }

                item.Insert();
                
                return (item.ItemID > 0) ? item.ItemID : (int?) null;
            }
            catch (Exception ex)
            {
                _eventLogRepository.LogError(
                    GetType(),
                    nameof(InsertContactUsFormItem),
                    ex);

                return null;
            }
        }
    }
}

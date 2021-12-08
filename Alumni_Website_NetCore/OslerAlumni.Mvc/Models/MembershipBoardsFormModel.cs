using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using OslerAlumni.Mvc.Core.Attributes.Validation;
using OslerAlumni.Mvc.Core.Definitions;
using OslerAlumni.Mvc.Core.Models;

namespace OslerAlumni.Mvc.Models
{
    public class MembershipBoardsFormModel
    {
        public Guid? UserGuid { get; set; }

        [Display(Name = Constants.ResourceStrings.Form.MembershipBoards.BoardMemberships)]
        public List<string> BoardMembershipList { get; set; }

        [Display(Name = Constants.ResourceStrings.Form.MembershipBoards.NewBoard)]
        [Required(ErrorMessage = Constants.ResourceStrings.Form.MembershipBoards.NewBoardRequired)]
        [StringNotContainsValidation(";", ErrorMessage = Constants.ResourceStrings.Form.MembershipBoards.NewBoardIllegalCharacter)]
        [LocalizedMaxLength(150, ErrorMessage = Constants.ResourceStrings.Form.MembershipBoards.NewBoardMaxLengthError)]
        public string NewBoard { get; set; }
    }
}

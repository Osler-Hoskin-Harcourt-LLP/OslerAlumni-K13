using System;
using System.Collections.Generic;
using ECA.Core.Services;
using OslerAlumni.OnePlace.Models;

namespace OslerAlumni.OnePlace.Services
{
    public interface IOnePlaceFunctionInviteeService
        : IService
    {
        List<OnePlaceFunctionInvitee> GetAttendeesForOneplaceFunction(string oneplaceFunctionId);
    }
}

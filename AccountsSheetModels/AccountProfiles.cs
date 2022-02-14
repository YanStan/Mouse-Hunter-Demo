using System;
using System.Collections.Generic;
using System.Linq;

namespace Mouse_Hunter.AccountsSheetModels
{
    public class AccountProfilesAnalyser
    {
        IEnumerable<AccountProfile> Profiles;
        public AccountProfilesAnalyser(IEnumerable<AccountProfile> accountProfiles) => 
            Profiles = accountProfiles;
        public IEnumerable<AccountProfile> GetAllActiveProfiles(string checkString)
        {
            var activeProfiles = Profiles.ToArray().Where(x => x.ActivityState == checkString);
            return activeProfiles;
        }
    }
}

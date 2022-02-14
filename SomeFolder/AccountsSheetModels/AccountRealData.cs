using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mouse_Hunter.AccountsSheetModels
{
    public class AccountRealData
    {
        //TODO Migrate all project to .Net 5 and set  { get; init; }
        public string Surname;
        public readonly string Name;
        public readonly string PatroName;
        public readonly string BirthYear;
        public readonly string BirthMonth;
        public readonly string BirthDay;
        public readonly string City;
        public readonly string Street;
        public readonly string Building;

        public AccountRealData(string accountData) 
        {
            var accountDataArr = accountData.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            Surname = accountDataArr[0];
            Name = accountDataArr[1];
            PatroName = accountDataArr[2];
            var dateArr = accountDataArr[3].Split('-');
            BirthYear = dateArr[0];
            BirthMonth = dateArr[1];
            BirthDay = dateArr[2];
            City = accountDataArr[4];
            Street = accountDataArr[5];
            Building = accountDataArr[6];
        }
    }
}

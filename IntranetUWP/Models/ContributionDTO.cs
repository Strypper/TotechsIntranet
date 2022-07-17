using System;
using System.Globalization;

namespace IntranetUWP.Models
{
    public class ContributionDTO : BaseDTO
    {
        public UserDTO        Contributor { get; set; }
        public decimal        Amount      { get; set; }
        public DateTime       DonateOn    { get; set; }
        public PaymentTypeDTO PaymentType { get; set; }

        private bool isApproved;
        public bool IsApproved
        {
            get { return isApproved; }
            set 
            { 
                isApproved = value;
                OnPropertyChanged();
            }
        }
    }

    public enum PaymentTypeDTO
    {
        Cash, Momo, Bank
    }
}

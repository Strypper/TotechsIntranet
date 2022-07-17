using IntranetUWP.Ultils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace IntranetUWP.Models
{
    public class UserDTO : UserIdentityDTO
    {
        public string CardPic             { get; set; } = String.Empty;
        public string Bio                 { get; set; }
        public string Former              { get; set; } = String.Empty;
        public string Hobby               { get; set; } = String.Empty;
        public string SpecialAward        { get; set; }
        public string Relationship        { get; set; } = String.Empty;
        public string SignalRConnectionId { get; set; } = String.Empty;
        public string ProfilePic          { get; set; } = String.Empty;
        public int?   Like                { get; set; }
        public int?   Friendly            { get; set; }
        public int?   Funny               { get; set; }
        public int?   Enthusiastic        { get; set; }



        public ICollection<SkillDTO> Skills { get; set; } = new ObservableCollection<SkillDTO>();

    }

    public class UserIdentityDTO
    {
        public string    Guid          { get; set; }
        public string    UserName      { get; set; } = string.Empty;
        public string    FirstName     { get; set; } = string.Empty;
        public string    MiddleName    { get; set; } = string.Empty;
        public string    LastName      { get; set; } = string.Empty;
        public string    FullName                    => FirstName + MiddleName + LastName;
        public string    Email         { get; set; } = string.Empty;
        public string    PhoneNumber   { get; set; } = string.Empty;
        public string    Password      { get; set; } = string.Empty;
        public DateTime? DateOfBirth   { get; set; }
        public int       Age           { get => DateOfBirth.GetAge();}

        //public CountryDTO Country { get; set; }
        //public ICollection<string> Roles { get; set; } = Array.Empty<string>();
    }

    public class IdentityResultDTO
    {
        public DateTime        RequestAt    { get; set; }
        //public int             ExpiresIn    { get; set; }
        public string          AccessToken  { get; set; }
        public string          RefreshToken { get; set; }
        public UserIdentityDTO UserInfo     { get; set; }
    }

    public class RegistingModel : UserDTO
    {
        public bool Gender { get; set; }
    }

    public static class UserExtensions
    {
        public static void FillUserIdentityInfo(this UserDTO user, UserIdentityDTO identityInfo)
        {
            user.Guid          = identityInfo.Guid;
            user.UserName      = identityInfo.UserName;
            user.FirstName     = identityInfo.FirstName;
            user.MiddleName    = identityInfo.MiddleName;
            user.LastName      = identityInfo.LastName;
            user.DateOfBirth   = identityInfo.DateOfBirth;
            user.PhoneNumber   = identityInfo.PhoneNumber;
        }


        public static void FillUserIntranetInfo(this UserDTO user, UserDTO intranetUserInfo)
        {
            user.Bio                 = intranetUserInfo.Bio;
            user.Former              = intranetUserInfo.Former;
            user.Relationship        = intranetUserInfo.Relationship;
            user.Like                = intranetUserInfo.Like;
            user.Friendly            = intranetUserInfo.Friendly;
            user.Enthusiastic        = intranetUserInfo.Enthusiastic;
            user.Funny               = intranetUserInfo.Funny;
            user.ProfilePic          = intranetUserInfo.ProfilePic;
            user.Hobby               = intranetUserInfo.Hobby;
            user.SpecialAward        = intranetUserInfo.SpecialAward;
            user.SignalRConnectionId = intranetUserInfo.SignalRConnectionId;
        }
    }
}

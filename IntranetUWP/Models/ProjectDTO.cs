using System;
using System.Collections.ObjectModel;

namespace IntranetUWP.Models
{
    public class ProjectDTO : BaseDTO
    {
        public string ProjectName         { get; set; }
        public string ProjectLogo         { get; set; } = string.Empty;
        public string ProjectBackground   { get; set; } = string.Empty;
        public string Clients             { get; set; }
        public string About               { get; set; }
        public string GithubLink          { get; set; } = string.Empty;
        public string FigmaLink           { get; set; } = string.Empty;
        public string MicrosoftStoreLink  { get; set; } = string.Empty;
        public string GooglePlayLink      { get; set; } = string.Empty;
        public string AppStoreLink        { get; set; } = string.Empty;

        public DateTime  StartTime        { get; set; }
        public DateTime? Deadline         { get; set; }
        public string    TechLeadGuid     { get; set; } = string.Empty;
        public ObservableCollection<UserDTO> Members { get; set; } = new ObservableCollection<UserDTO>();
    }
}

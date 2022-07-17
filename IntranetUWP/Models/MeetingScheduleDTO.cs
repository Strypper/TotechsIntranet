using IntranetUWP.RefitInterfaces;
using Refit;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace IntranetUWP.Models
{
    public class MeetingScheduleDTO : BaseDTO
    {
        public DateTime                   MeetingDate  { get; set; }
        public DateTime                   EndTime      { get; set; }
        public UserDTO                    Planner      { get; set; }
        public MeetingInfoDTO             MeetingInfo  { get; set; }
        public ICollection<TodoTaskDTO>   MustDoneTask { get; set; } = new ObservableCollection<TodoTaskDTO>();
        public ICollection<AttendanceDTO> Attendances  { get; set; } = new ObservableCollection<AttendanceDTO>();
    }

    public class MeetingInfoDTO : BaseDTO
    {
        public string          Description     { get; set; }
        public string          Location        { get; set; }
        public ImportanceLevel ImportanceLevel { get; set; }
    }

    public class AttendanceDTO : BaseDTO
    {
        public DateTime Attend           { get; set; }
        public DateTime Leave            { get; set; }
        public UserDTO  AttendanceInfo   { get; set; }
        public decimal  ContributeAmount { get; set; }
        public bool     IsApproved       { get; set; }
    }

    public class TodoTaskDTO : BaseDTO
    {
        public DateTime   DateCreated  { get; set; }
        public string     Title        { get; set; }
        public string     Description  { get; set; }
        public string     ImageTaskUrl { get; set; }
        public TaskStatus Status       { get; set; }
        public UserDTO    Author       { get; set; }
    }

    public class MeetingActivity : BaseDTO
    {
        public UserDTO User     { get; set; }
        public string  Activity { get; set; }
        public string  Icon     { get; set; }
    }

    public enum TaskStatus
    {
        Suggestion, Todo, Doing, Testing, Done
    }

    public enum ImportanceLevel
    {
        Chill, Medium, High, Extreme
    }
}

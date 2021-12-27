using IntranetUWP.Models;
using System.Collections.ObjectModel;
using Windows.UI.Xaml.Controls;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace IntranetUWP.UserControls
{
    public sealed partial class ChannelChatUserControl : UserControl
    {
        private ObservableCollection<ChannelDTO> DataSource = new ObservableCollection<ChannelDTO>();
        public ChannelChatUserControl()
        {
            this.InitializeComponent();
            DataSource = GetData();
        }
        private ObservableCollection<ChannelDTO> GetData()
        {
            var list = new ObservableCollection<ChannelDTO>();
            ChannelDTO folder1 = new ChannelDTO()
            {
                Name = "Announcements",
                IconProp = "\uE789",
                Color = "#ffc200",
                Type = ChannelDTO.ChannelDTOType.Folder,
                Children = new ObservableCollection<ChannelDTO>()
                {
                    new ChannelDTO()
                    {
                        Name = "Health Care",
                        IconProp = "\uE95E",
                        Color = "#ea1c37",
                        Type = ChannelDTO.ChannelDTOType.Folder
                    },
                    new ChannelDTO()
                    {
                        Name = "An Phu Building",
                        IconProp = "\uEC07",
                        Type = ChannelDTO.ChannelDTOType.File,
                    },
                    new ChannelDTO()
                    {
                        Name = "English Class",
                        IconProp = "\uEC87",
                        Type = ChannelDTO.ChannelDTOType.File,
                    }
                },
                IsVisisbleAddButton = true
            };
            ChannelDTO developerZone = new ChannelDTO()
            {
                Name = "Developer Zone",
                IconProp = "\uE943",
                Type = ChannelDTO.ChannelDTOType.Folder,
                Children = new ObservableCollection<ChannelDTO>()
                {
                    new ChannelDTO()
                    {
                        Name = "Design Zone",
                        IconProp = "\uE2B1"
                    },
                    new ChannelDTO()
                    {
                        Name = "Front-end Zone",
                        IconProp = "\uE8FC",
                    },
                    new ChannelDTO()
                    {
                        Name = "Back-end Zone",
                        IconProp = "\uEDA2",
                    },
                    new ChannelDTO()
                    {
                        Name = "DevOp Zone",
                        IconProp = "\uEBD2",
                    }
                },
            };

            ChannelDTO folder2 = new ChannelDTO()
            {
                Name = "HR Help",
                IconProp = "\uE902"
            };

            ChannelDTO folder3 = new ChannelDTO()
            {
                Name = "IT Helpdesk",
                IconProp = "\uEC4E",
            };

            list.Add(folder1);
            list.Add(developerZone);
            list.Add(folder2);
            list.Add(folder3);
            return list;
        }
    }
}

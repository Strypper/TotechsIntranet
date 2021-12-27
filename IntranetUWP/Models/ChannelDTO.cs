using System.Collections.ObjectModel;
using System.ComponentModel;

namespace IntranetUWP.Models
{
    public class ChannelDTO : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public enum ChannelDTOType { Folder, File };
        public string Name { get; set; }
        public string IconProp { get; set; }
        public string Color { get; set; } = "#f1f1f1";
        public ChannelDTOType Type { get; set; }
        private ObservableCollection<ChannelDTO> m_children;
        public ObservableCollection<ChannelDTO> Children
        {
            get
            {
                if (m_children is null) m_children = new ObservableCollection<ChannelDTO>();
                return m_children;
            }
            set => m_children = value;
        }

        private bool m_isExpanded;
        public bool IsExpanded
        {
            get { return m_isExpanded; }
            set
            {
                if (m_isExpanded != value)
                {
                    m_isExpanded = value;
                    NotifyPropertyChanged("IsExpanded");
                }
            }
        }

        private bool isVisisbleAddButton;
        public bool IsVisisbleAddButton
        {
            get { return isVisisbleAddButton; }

            set
            {
                if (isVisisbleAddButton != value)
                {
                    isVisisbleAddButton = value;
                    NotifyPropertyChanged("IsVisisbleAddButton");
                }
            }

        }

        private void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaaMedW.ViewModel
{
    public class VmDocumentType : INotifyPropertyChanged
    {
        private DocumentType m_object;
        public event PropertyChangedEventHandler PropertyChanged;
        public VmDocumentType()
        {
            m_object = new DocumentType();
        }
        public VmDocumentType(DocumentType users)
        {
            m_object = users;
        }
        public DocumentType users
        {
            get => m_object;
        }
        public int Id
        {
            get => m_object.Id;
            set
            {
                m_object.Id = value;
                OnPropertyChanged("Id");
            }
        }
        public string Name
        {
            get => m_object.Name;
            set
            {
                m_object.Name = value;
                OnPropertyChanged("Name");
            }
        }
        private void OnPropertyChanged(string PropertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(PropertyName));
        }
    }
}

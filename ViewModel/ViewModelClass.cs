using System.ComponentModel;

namespace ViewModel
{
    public class ViewModelClass : INotifyPropertyChanged
    {
        private string title;
        private string description;
        private string image;
        private string videoId;

        public ViewModelClass()
        {

        }

        public string Title
        {
            get
            {
                return title;
            }
            set
            {
                title = value;
                RaisePropertyChanged("Title");
            }
        }


        public string Description
        {
            get
            {
                return description;
            }
            set
            {
                description = value;
                RaisePropertyChanged("Description");
            }
        }
        public string VideoId
        {
            get
            {
                return videoId;
            }
            set
            {
                videoId = value;
                RaisePropertyChanged("VideoId");
            }
        }
        
        public string Image
        {
            get
            {
                return image;
            }
            set
            {
                image = value;
                RaisePropertyChanged("Image");
            }
        }
      
        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;

            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}

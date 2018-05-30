using ChatClient.Infrastructure;
using ContractClient;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ChatClient.ViewModel
{
    class UserEditViewModel : ViewModelBase
    {
        UserExt _user;
        public UserExt User
        {
            get
            {
                return _user;
            }
            set
            {
                if (_user != value)
                {
                    _user = value;
                    OnPropertyChanged();
                }
            }
        }


        public string Name
        {
            get
            {
                return _user.Name;
            }
            set
            {
                if (_user.Name != value)
                {
                    _user.Name = value;
                    OnPropertyChanged();
                }
            }
        }
        private String _imagePath;
        public string ImagePath
        {
            get
            {
                return _imagePath;
            }
            set
            {
                if (_imagePath != value)
                {
                    _imagePath = value;
                    OnPropertyChanged();
                    if (value != null)
                    {
                        User.Icon = File.ReadAllBytes(value);
                       // IconImg = GEtImageA(User.Icon);
                    }

                }
            }
        }

        public DateTime? BirthDate
        {
            get
            {
                return User.BirthDate;
            }
            set
            {
                if (User.BirthDate != value)
                {
                    User.BirthDate = value;
                    OnPropertyChanged();
                }
            }
        }

        BitmapImage _icon;
        public BitmapImage IconImg
        {
            get
            {
                if (_icon==null&& User.Icon != null&&User.Icon.Length>100)
                {

                    _icon = LoadImage(User.Icon);
                }
                return _icon;
            }
            set
            {
                if (_icon != value)
                {
                    _icon = value;
                    OnPropertyChanged();
                }
            }
        }
        private  BitmapImage LoadImage(byte[] imageData)
        {
            if (imageData == null || imageData.Length == 0) return null;
            var image = new BitmapImage();
            using (var mem = new MemoryStream(imageData))
            {
                mem.Position = 0;
                image.BeginInit();
                image.CreateOptions = BitmapCreateOptions.PreservePixelFormat;
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.UriSource = null;
                image.StreamSource = mem;
                image.EndInit();
            }
            image.Freeze();
            return image;
        }
        private BitmapImage GEtImageA(byte[] byteArrayIn)
        {
            try
            {
                using (var ms = new MemoryStream(byteArrayIn))
                {

                    ms.Seek(0, SeekOrigin.Begin);
                    BitmapImage bi = new BitmapImage();
                    bi.BeginInit();
                    bi.StreamSource = ms;
                    bi.EndInit();
                    return bi;

                }
            }
            catch (Exception e)
            {


            }
            return new BitmapImage();
        }
        private String TrySetIcon(String path)
        {
            if (File.Exists(path))
            {
                try
                {
                    Bitmap image1 = (Bitmap)Image.FromFile(path);
                }
                catch (Exception e)
                {
                    return "Error";
                }
                return String.Empty;
            }
            return "File not Exists";
        }

        /*Validation*/
        public string this[string columnName]
        {
            get
            {
                string error = String.Empty;
                switch (columnName)
                {
                    case "Name":
                        if (!Regex.IsMatch(Name, "^[а-яА-ЯёЁa-zA-Z0-9 ]+${4,50}"))
                        {
                            return "Numbers, Cyrylic, latin, space. Length 4-50";
                        }
                        break;
                    case "ImagePath":
                        return TrySetIcon(ImagePath);
                }
                return error;
            }
        }

        RelayCommand _OpenFile;
        public ICommand OpenFileCommand
        {
            get
            {
                if (_OpenFile == null)
                {
                    _OpenFile = new RelayCommand(ExecuteOpenFileCommand);
                }
                return _OpenFile;
            }
        }

        private void ExecuteOpenFileCommand(object parametr)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            if (fileDialog.ShowDialog() == true)
            {
                if (TrySetIcon(fileDialog.FileName) == String.Empty)
                {
                    ImagePath = fileDialog.FileName;
                     
                    User.Icon = File.ReadAllBytes(ImagePath);
                    IconImg = LoadImage(User.Icon);

                }
                else
                {
                    MessageBox.Show("Load image error. Choose another file");
                }
            }
         
        }




    }
}

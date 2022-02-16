using Service;
using System;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using Unity;
using UniversityWPF.Commands;

namespace UniversityWPF.ViewModels
{
    public class InputsViewModel : INotifyPropertyChanged
    {
        private IStudentService _studentService;
        private ITeacherService _teacherService;
        private UnityContainer _unityContainer;
        public InputsViewModel(IStudentService studentService,
                               ITeacherService teacherService)
        {
            _studentService = studentService;
            _teacherService = teacherService;
        }

        // Teacher Properties
        private string _teacherName, _teacherEmail, _teacherRank;
        public string TeacherName
        {
            get { return _teacherName; }
            set { _teacherName = value; NotifyPropertyChanged(); }
        }

        public string TeacherEmail
        {
            get { return _teacherEmail; }
            set { _teacherEmail = value; NotifyPropertyChanged(); }
        }

        public string TeacherRank
        {
            get { return _teacherRank; }
            set { _teacherRank = value; NotifyPropertyChanged(); }
        }

        // Student Properties
        private string _studentName, _studentEmail, _studentSpeciality, _studentCourse;
        public string StudentName
        {
            get { return _studentName; }
            set { _studentName = value; NotifyPropertyChanged(); }
        }

        public string StudentEmail
        {
            get { return _studentEmail; }
            set { _studentEmail = value; NotifyPropertyChanged(); }
        }

        public string StudentSpeciality
        {
            get { return _studentSpeciality; }
            set { _studentSpeciality = value; NotifyPropertyChanged(); }
        }

        public string StudentCourse
        {
            get { return _studentCourse; }
            set { _studentCourse = value; NotifyPropertyChanged(); }
        }

        // Commands
        private ICommand _addStudentCommand;
        private ICommand _addTeacherCommand;
        public ICommand AddStudentCommand
        {
            get { return _addStudentCommand ?? (_addStudentCommand = new RelayCommand(AddStudentClicked)); }
        }
        public ICommand AddTeacherCommand
        {
            get { return _addTeacherCommand ?? (_addTeacherCommand = new RelayCommand(AddTeacherClicked)); }
        }

        private void AddStudentClicked(object obj)
        {
            if (StudentSpeciality != null && StudentCourse != null)
            {
                StudentSpeciality = ComboBoxValueExtractor(StudentSpeciality);
                StudentCourse = ComboBoxValueExtractor(StudentCourse);
            }

            try
            {
                _unityContainer = (UnityContainer)Application.Current.Resources["IoC"];
                _studentService = (StudentService)_unityContainer.Resolve<IStudentService>();
                _studentService.AddStudent(StudentName, StudentEmail, StudentSpeciality, StudentCourse);
                MessageBox.Show("Student Successfully Added!");
            }
            catch (ArgumentException aex)
            {

                MessageBox.Show(aex.Message);
            }
        }

        private void AddTeacherClicked(object obj)
        {
            if (TeacherRank != null)
            {
                TeacherRank = ComboBoxValueExtractor(TeacherRank);
            }

            try
            {
                _unityContainer = (UnityContainer)Application.Current.Resources["IoC"];
                _teacherService = (TeacherService)_unityContainer.Resolve<ITeacherService>();
                _teacherService.AddTeacher(TeacherName, TeacherEmail, TeacherRank);
                MessageBox.Show("Teacher Successfully Added!");
            }
            catch (ArgumentException aex)
            {
                MessageBox.Show(aex.Message);
            }
        }

        private string ComboBoxValueExtractor(string item)
        {
            return item.Split(' ').Skip(1).FirstOrDefault();
        }

        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}

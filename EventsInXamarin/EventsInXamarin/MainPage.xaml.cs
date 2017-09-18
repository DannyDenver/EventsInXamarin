using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using EventsInXamarin.Model;

namespace EventsInXamarin
{
    public partial class MainPage : ContentPage
    {
        public List<Job> _jobList { get; set; }
        public List<Employee> _employeesList { get; set; }

        public MainPage()
        {
            InitializeComponent();
            InstantiateJobsAndEmployees();

            Mediator.GetInstance().JobChanged += (s, e) =>
            {
                BindJobDescriptionData(e.Job);
            };

            Mediator.GetInstance().JobChanged += (s, e) =>
             {
                 BindEmployees(e.Job.ID);
             };
            
            picker.ItemsSource = _jobList.Select(x => x.Title).ToList();

            employeesOnJob.ItemsSource = null;
        }

        void OnPickerSelectedIndexChanged(object sender, EventArgs e)
        {
            var picker = (Picker)sender;
            int selectedIndex = picker.SelectedIndex;

            Mediator.GetInstance().OnJobChanged(this, _jobList[selectedIndex]);      
        }

        private void BindJobDescriptionData(Job job)
        {
            this.startDate.Text = job.StartDate.ToString();
            this.endDate.Text = job.EndDate.ToString();
        }

        private void BindEmployees(int jobId)
        {
            this.employeesOnJob.ItemsSource = _employeesList.Where(x => x.Jobs.Any(j => j.ID == jobId)).Select(x => x.Name);
        }
        private void InstantiateJobsAndEmployees()
        {

            _jobList = new List<Job>
            {
                new Job{ Title= "SalesMan", ID=1, EndDate= new DateTime(2014,2,3), StartDate=new DateTime(2016,4,5)},
                new Job{ Title= "Journalist", ID=2, EndDate= new DateTime(2014,2,3), StartDate=new DateTime(2016,4,5)},
                new Job{ Title= "Accountant", ID=3, EndDate= new DateTime(2014,2,3), StartDate=new DateTime(2016,4,5)},
                new Job{ Title= "Lawyer", ID=4, EndDate= new DateTime(2014,2,3), StartDate=new DateTime(2016,4,5)},
                new Job{ Title= "Post Office Worker", ID=5, EndDate= new DateTime(2014,2,3), StartDate=new DateTime(2016,4,5)}
            };

            _employeesList = new List<Employee>
            {
                new Employee { ID=1, Name="John Smith", Jobs = new List<Job>
                {
                    new Job{ ID = 1, Title = "Go Door to door" },
                    new Job{ ID = 3, Title = "Go Door to door" },
                    new Job{ ID = 4, Title = "Go Door to door" },
                }
                },

                new Employee { ID = 2, Name= "Dan Brown", Jobs = new List<Job>
                {
                    new Job{ ID = 5, Title = "Go Door to door" },
                    new Job{ ID = 4, Title = "Go Door to door" },
                    new Job{ ID = 2, Title = "Go Door to door" },
                }
                },
               new Employee { ID = 3, Name= "Mike Taylor", Jobs = new List<Job>
                {
                    new Job{ ID = 3, Title = "Go Door to door" },
                },
                }
            };
        }

    }

    
}

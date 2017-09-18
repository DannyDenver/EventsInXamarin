using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Xamarin.Forms;
using EventsInXamarin.Model;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace EventsInXamarin
{
    public partial class MainPage : ContentPage
    {
        public List<Job> _jobList { get; set; }
        public List<Employee> _employeesList { get; set; }
        delegate void ShowProgressDelegate(int val);
        delegate void StartProcessDelegate(int val);

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

        private void StartButton_Click(object sender, EventArgs e)
        {
            var progDel = new StartProcessDelegate(StartProcess);
            progDel.BeginInvoke(10, null, null);  ///invoce asyncronously 
            
        }

        private async void StartProcess(int max)
        {
            ShowProgress(0);
            for(int i = 0; i <= max; i++)
            {
                await Task.Delay(1000);
                // lblOutput.Text = i.ToString();
                // this.pbStatus.Value = i;

                ShowProgress(i);
            }
        }

        private void ShowProgress(int i)
        {
            var del = new ShowProgressDelegate(ShowProgress);
            Device.BeginInvokeOnMainThread(()=>  // get into GUI thread
            {
                progressText.Text = (i * 10).ToString() + "%";
                progressBar.Progress = (i / 10.0);
            });        
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

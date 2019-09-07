﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace test
{
    public class myFd:INotifyPropertyChanged
    {
        private int _empno;

        private string _ename;

        private string _job;




        public event PropertyChangedEventHandler PropertyChanged;



        public int Empno

        {

            get { return _empno; }

            set { _empno = value; OnPropertyChanged("Empno"); }

        }




        public string Ename

        {

            get { return _ename; }

            set { _ename = value; OnPropertyChanged("Ename"); }

        }




        public string Job

        {

            get { return _job; }

            set { _job = value; OnPropertyChanged("Job"); }

        }




        public void OnPropertyChanged(string PName)

        {

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(PName));

        }
    }
}

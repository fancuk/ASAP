using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace test
{
    public class myFds : ObservableCollection<myFd>
    {
        myFds()
        {
            Add(new myFd() { Empno = 1, Ename = "김길동" });

            Add(new myFd() { Empno = 2, Ename = "박길동"});

            Add(new myFd() { Empno = 3, Ename = "정길동"});

            Add(new myFd() { Empno = 4, Ename = "남길동"});

            Add(new myFd() { Empno = 5, Ename = "황길동"});

            Add(new myFd() { Empno = 6, Ename = "홍길동" });
            
        }
    }
}

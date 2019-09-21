using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace test
{
    public class Emps : ObservableCollection<Emp>
    {
        public Emps()

        {

            Add(new Emp() {Ename = "김길동", Job = "Salesman" });

            Add(new Emp() {Ename = "박길동", Job = "Clerk" });

            Add(new Emp() {Ename = "정길동", Job = "Clerk" });

            Add(new Emp() {Ename = "남길동", Job = "Clerk" });

            Add(new Emp() {Ename = "황길동", Job = "Salesman" });

            Add(new Emp() {Ename = "홍길동", Job = "Manager" });


        }

        public Emps(int i)

        {

            Add(new Emp() { Ename = "김길동", Job = "Salesman" });

            Add(new Emp() { Ename = "박길동", Job = "Clerk" });

            Add(new Emp() { Ename = "정길동", Job = "Clerk" });

            Add(new Emp() { Ename = "남길동", Job = "Clerk" });

            Add(new Emp() { Ename = "황길동", Job = "Salesman" });

            Add(new Emp() { Ename = "홍길동", Job = "Manager" });

            Add(new Emp() { Ename = "홍길동", Job = "Manager" });

            Add(new Emp() { Ename = "홍길동", Job = "Manager" });

            Add(new Emp() { Ename = "홍길동", Job = "Manager" });

            Add(new Emp() { Ename = "홍길동", Job = "Manager" });

            Add(new Emp() { Ename = "홍길동", Job = "Manager" });

            Add(new Emp() { Ename = "홍길동", Job = "Manager" });

            Add(new Emp() { Ename = "홍길동", Job = "Manager" });

            Add(new Emp() { Ename = "홍길동", Job = "Manager" });

            Add(new Emp() { Ename = "홍길동", Job = "Manager" });


        }
    }
}

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

/// <summary>
/// Window1.xaml에 대한 상호 작용 논리
/// </summary>
namespace test

{
    public partial class Window1 : Window
    {
        public ListCollectionView MyCollectionView;


 

        public Emp emp;

        public Window1()
        {
            InitializeComponent();
        }
        public void DCChange(object sender, DependencyPropertyChangedEventArgs args)

        {

            // StackPanel의 DataContext로 지정된 emps 컬렉션을 소스로 해서 ListCollectionView 생성

            // 이를 이용하여 정렬, 탐색, 필터링 기능 등을 구현한다.

            MyCollectionView = (ListCollectionView)CollectionViewSource.GetDefaultView(rootElement.DataContext);

        }


        /*

        // 리스트박스 상단의 정렬 기능 처리

        private void OnClick(object sender, RoutedEventArgs e)

        {

            var b = sender as Button;




            MyCollectionView.SortDescriptions.Clear();




            switch (b.Name)

            {

                case "BtnEname":

                    MyCollectionView.SortDescriptions.Add(new SortDescription("Ename", ListSortDirection.Ascending));

                    break;

                case "BtnJob":

                    MyCollectionView.SortDescriptions.Add(new SortDescription("Job", ListSortDirection.Ascending));

                    break;

            }

        }




        //Prevous, Next 버튼 처리

        private void OnMove(object sender, RoutedEventArgs e)

        {

            var b = sender as Button;

            switch (b.Name)

            {

                case "Previous":

                    if (MyCollectionView.MoveCurrentToPrevious())

                        emp = MyCollectionView.CurrentAddItem as Emp;

                    else

                        MyCollectionView.MoveCurrentToFirst();

                    break;

                case "Next":

                    if (MyCollectionView.MoveCurrentToNext())

                        emp = MyCollectionView.CurrentAddItem as Emp;

                    else

                        MyCollectionView.MoveCurrentToLast();

                    break;

            }

        }




        // 필터링 기능, 관리자만 또는 관리자가 아닌 사원 리스트 출력

        private void OnFilter(object sender, RoutedEventArgs e)

        {

            var b = sender as Button;




            //토글 기능 구현

            switch (MyCollectionView.Filter)

            {

                case null: MyCollectionView.Filter = IsManager; break;

                default: MyCollectionView.Filter = null; break;

            }

        }

        


        private bool IsManager(object o)

        {

            var e = o as Emp;

            return e?.Job == "Manager";

        }
        */
    }
}



using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TelerikWpfApp3.ViewModel.Command;

namespace TelerikWpfApp3.ViewModel
{
    public class MessageViewModel
    {
        public string MessageText{get;set;}
        public Command.MessageCommand DisplayCommand { get; private set; }
        public MessageViewModel()
        {
            DisplayCommand = new Command.MessageCommand(DisplayMessage);
        }

        public void DisplayMessage()
        {
            MessageBox.Show(MessageText);
        }
    }
}

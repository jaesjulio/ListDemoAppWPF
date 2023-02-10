using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Xml.Linq;

namespace ListDemoApp
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        public MainWindowViewModel()
        {
        }

        public ObservableCollection<string> Items { get; } = new ObservableCollection<string>();

        private string _itemSelected { get; set; }
        public string ItemSelected { 
            get { return _itemSelected; }
            set {
                    _itemSelected= value;
                OnPropertyChanged(_itemSelected);
                } 
            }

        private string _itemAdding { get; set; }
        public string ItemAdding
        {
            get { return _itemAdding; }
            set
            {
                _itemAdding = value;
                OnPropertyChanged(_itemAdding);
            }
        }

        private ICommand _addCommand;
        public ICommand AddCommand
        {
            get
            {
                return _addCommand ?? (_addCommand = new CommandHandler(() => AddItem(), () => CanExecute));
            }
        }

        private ICommand _removeCommand;
        public ICommand RemoveCommand
        {
            get
            {
                return _removeCommand ?? (_removeCommand = new CommandHandler(() => RemoveItem(), () => CanExecute));
            }
        }
        public bool CanExecute
        {
            get
            {
                return true;
            }
        }

        public void AddItem()
        {
            if(!string.IsNullOrEmpty(ItemAdding)) {
                Items.Add(ItemAdding);
                ItemAdding = string.Empty;
            }

        }

        public void RemoveItem()
        {
            Items.Remove(ItemSelected);
        }

       
        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            var changed = PropertyChanged;
            if (changed == null)
                return;

            changed.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
    public class CommandHandler : ICommand
    {
        private Action _action;
        private Func<bool> _canExecute;

        public CommandHandler(Action action, Func<bool> canExecute)
        {
            _action = action;
            _canExecute = canExecute;
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object parameter)
        {
            return _canExecute.Invoke();
        }

        public void Execute(object parameter)
        {
            _action();
        }
    }
}

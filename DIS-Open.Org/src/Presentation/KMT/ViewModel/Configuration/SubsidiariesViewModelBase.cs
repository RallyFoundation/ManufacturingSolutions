using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using DIS.Business.Operation;
using DIS.Business.Proxy;
using DIS.Data.DataContract;
using DIS.Presentation.KMT.Commands;
using DIS.Presentation.KMT.Properties;
using DIS.Presentation.KMT.View.Configuration;

namespace DIS.Presentation.KMT.ViewModel
{
    public abstract class SubsidiariesViewModelBase : ViewModelBase
    {
        private ISubsidiaryProxy ssProxy;

        private Subsidiary selectedSubsidiary;
        private ObservableCollection<Subsidiary> subsidiaries;
        private bool isRemovingSubsidiary = false;

        private DelegateCommand addSubsidiaryCommand;
        private DelegateCommand editSubsidiaryCommand;
        private DelegateCommand removeSubsidiaryCommand;

        public SubsidiariesViewModelBase(ISubsidiaryProxy ssProxy)
        {
            if (ssProxy == null)
                this.ssProxy = new SubsidiaryProxy();
            else
                this.ssProxy = ssProxy;

            LoadSubsidiaries();
        }

        /// <summary>
        /// Contains the Subsidiary detail
        /// </summary>
        public Subsidiary SelectedSubsidiary
        {
            get
            {
                return selectedSubsidiary;
            }
            set
            {
                selectedSubsidiary = value;
                RaisePropertyChanged("SelectedSubsidiary");
            }
        }

        /// <summary>
        /// Contains list of FactoryType i.e TPICorp or FactoryFloor
        /// </summary>
        public ObservableCollection<Subsidiary> Subsidiaries
        {
            get { return subsidiaries; }
            set
            {
                subsidiaries = value;
                RaisePropertyChanged("Subsidiaries");
            }
        }

        /// <summary>
        /// Command used on clicking Add button
        /// </summary>
        public ICommand AddSubsidiaryCommand
        {
            get
            {
                if (addSubsidiaryCommand == null)
                {
                    addSubsidiaryCommand = new DelegateCommand(AddSubsidiary);
                }
                return addSubsidiaryCommand;
            }
        }

        /// <summary>
        /// Command used on clicking Edit button
        /// </summary>
        public ICommand EditSubsidiaryCommand
        {
            get
            {
                if (editSubsidiaryCommand == null)
                {
                    editSubsidiaryCommand = new DelegateCommand(
                        () => { EditSubsidiary(); }, () => { return SelectedSubsidiary != null; });
                }
                return editSubsidiaryCommand;
            }
        }

        /// <summary>
        /// Command used on clicking Remove button
        /// </summary>
        public ICommand RemoveSubsidiaryCommand
        {
            get
            {
                if (removeSubsidiaryCommand == null)
                {
                    removeSubsidiaryCommand = new DelegateCommand(
                            () => { RemoveSubsidiary(); }, () => { return (SelectedSubsidiary != null) && !isRemovingSubsidiary; });
                }
                return removeSubsidiaryCommand;
            }
        }

        private void LoadSubsidiaries()
        {
            Subsidiaries = new ObservableCollection<Subsidiary>(ssProxy.GetSubsidiaries());
        }

        /// <summary>
        /// Add or edit Subsidiary
        /// </summary>
        private void AddSubsidiary()
        {
            SubsidiaryEditor editor = new SubsidiaryEditor();
            editor.Owner = App.Current.MainWindow;
            editor.ShowDialog();
            if (editor.VM.IsSaved)
                LoadSubsidiaries();
        }

        /// <summary>
        /// Edit the selected Subsidiary from list
        /// </summary>
        private void EditSubsidiary()
        {
            SubsidiaryEditor editor = new SubsidiaryEditor(SelectedSubsidiary);
            editor.Owner = App.Current.MainWindow;
            editor.ShowDialog();
            if (editor.VM.IsSaved)
                LoadSubsidiaries();
        }

        /// <summary>
        /// Remove the selected Subsidiary from list
        /// </summary>
        private void RemoveSubsidiary()
        {
            isRemovingSubsidiary = true;
            WorkInBackground((s, e) =>
            {
                try
                {
                    ssProxy.DeleteSubsidiary(SelectedSubsidiary.SSID);
                    MessageLogger.LogOperation(Constant.LoginUser.LoginId, 
                        string.Format("Subsidiary {0} was been removed.", SelectedSubsidiary.CustomerNumber));
                    LoadSubsidiaries();
                    isRemovingSubsidiary = false;
                }
                catch (Exception ex)
                {
                    isRemovingSubsidiary = false;
                    ex.ShowDialog(Resources.ConfigViewModel_DeleteSubSidiary);
                    ExceptionHandler.HandleException(ex);
                }
            });
        }
    }
}

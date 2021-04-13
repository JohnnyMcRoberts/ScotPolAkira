using ElectionDataParser;
using ScotPolWpfApp.Models;

namespace ScotPolWpfApp.ViewModels
{
    using System;
    using System.Windows.Input;

    /// <summary>
    /// This class will open the main results files and process the data from the CSV files.
    /// </summary>
    public class ResultsImporterViewModel : Caliburn.Micro.PropertyChangedBase
    {
        #region Constants

        private readonly string[] _importersList = new[]
        {
            "Import Notes",
            "Import Constituencies",
            "Import Regional Lists"
        };

        private string _notesFile;

        private string _constituenciesFile;

        private string _regionalListsFile;

        private bool _hasNotes;

        #endregion

        #region Private Data

        /// <summary>
        /// The load notes command.
        /// </summary>
        private ICommand _loadNotesCommand;

        #endregion

        #region Properties

        /// <summary>
        /// Gets the load notes text.
        /// </summary>
        public string LoadNotesText => _importersList[0];

        /// <summary>
        /// Gets the load notes text.
        /// </summary>
        public string LoadConstituencyResultsText => _importersList[1];

        /// <summary>
        /// Gets the load notes text.
        /// </summary>
        public string LoadListResultsText => _importersList[2];

        public string NotesFile
        {
            get => _notesFile;
            set { _notesFile = value; NotifyOfPropertyChange(() => NotesFile); }
        }
        public string ConstituenciesFile
        {
            get => _constituenciesFile;
            set { _constituenciesFile = value; NotifyOfPropertyChange(() => ConstituenciesFile); }
        }
        public string RegionalListsFile
        {
            get => _regionalListsFile;
            set { _regionalListsFile = value; NotifyOfPropertyChange(() => RegionalListsFile); }
        }

        public bool HasNotes
        {
            get => _hasNotes;
            set { _hasNotes = value; NotifyOfPropertyChange(() => HasNotes); }

        }

        #endregion

        #region Commands

        /// <summary>
        /// Load the notes command.
        /// </summary>
        public ICommand LoadNotesCommand =>
            _loadNotesCommand ?? (_loadNotesCommand = new CommandHandler(LoadNotesCommandAction, true));

        /// <summary>
        /// Load the notes command.
        /// </summary>
        public ICommand LoadConstituencyResultsCommand =>
            _loadNotesCommand ?? (_loadNotesCommand = new CommandHandler(LoadNotesCommandAction, true));

        /// <summary>
        /// Load the notes command.
        /// </summary>
        public ICommand LoadListResultsCommand =>
            _loadNotesCommand ?? (_loadNotesCommand = new CommandHandler(LoadNotesCommandAction, true));

        #endregion

        #region Command handlers

        /// <summary>
        /// The load notes file command action.
        /// </summary>
        private void LoadNotesCommandAction()
        {
            Console.WriteLine("LoadNotesCommandAction");
         
            NotesFileParser parser = 
                new NotesFileParser(ConfigurationSettings.DatabaseSettings.ResultsDirectory);

            NotesFile = parser.FilePath;
            HasNotes = parser.ReadSuccessfully;
        }

        #endregion

        #region Public Methods

        #endregion

        public ResultsImporterViewModel()
        {
            _notesFile = string.Empty;
            _constituenciesFile = string.Empty;
            _regionalListsFile = string.Empty;

            _hasNotes = false;
        }
    }
}

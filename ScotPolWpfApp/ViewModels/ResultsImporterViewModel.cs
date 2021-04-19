using System.Collections.ObjectModel;
using System.Linq;
using ElectionDataParser;
using ElectionDataTypes;
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

        private readonly string[] _importersList =
        {
            "Import Notes",
            "Import Constituencies",
            "Import Regional Lists"
        };

        private string _notesFile;

        private string _constituenciesFile;

        private string _regionalListsFile;

        private bool _hasNotes;

        private bool _hasConstituencies;

        private bool _hasRegionalList;

        #endregion

        #region Private Data

        /// <summary>
        /// The load notes command.
        /// </summary>
        private ICommand _loadNotesCommand;

        /// <summary>
        /// The load constituency results command.
        /// </summary>
        private ICommand _loadConstituencyResultsCommand;

        /// <summary>
        /// The load list results command.
        /// </summary>
        private ICommand _loadListResultsCommand;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the election results for a year.
        /// </summary>
        public ElectionResult ElectionResults { get; set; }

        /// <summary>
        /// Gets the load notes text.
        /// </summary>
        public string LoadNotesText => _importersList[0];

        /// <summary>
        /// Gets the load constituency results text.
        /// </summary>
        public string LoadConstituencyResultsText => _importersList[1];

        /// <summary>
        /// Gets the load list results text.
        /// </summary>
        public string LoadListResultsText => _importersList[2];

        /// <summary>
        /// Gets or sets the notes file name.
        /// </summary>
        public string NotesFile
        {
            get => _notesFile;
            set { _notesFile = value; NotifyOfPropertyChange(() => NotesFile); }
        }

        /// <summary>
        /// Gets or sets the constituency results file name.
        /// </summary>
        public string ConstituenciesFile
        {
            get => _constituenciesFile;
            set { _constituenciesFile = value; NotifyOfPropertyChange(() => ConstituenciesFile); }
        }

        /// <summary>
        /// Gets or sets the list results file name.
        /// </summary>
        public string RegionalListsFile
        {
            get => _regionalListsFile;
            set { _regionalListsFile = value; NotifyOfPropertyChange(() => RegionalListsFile); }
        }

        /// <summary>
        /// Gets or sets if the the data model has notes.
        /// </summary>
        public bool HasNotes
        {
            get => _hasNotes;
            set
            {
                _hasNotes = value;
                NotifyOfPropertyChange(() => HasNotes);
                NotifyOfPropertyChange(() => HasConstituencies);
            }
        }

        /// <summary>
        /// Gets or sets if the the data model has constituencies results.
        /// </summary>
        public bool HasConstituencies
        {
            get => _hasConstituencies;
            set
            {
                _hasConstituencies = value;
                NotifyOfPropertyChange(() => HasConstituencies);
                NotifyOfPropertyChange(() => HasNotesAndConstituencies);
            }
        }

        /// <summary>
        /// Gets or sets if the the data model has constituencies results.
        /// </summary>
        public bool HasNotesAndConstituencies => _hasConstituencies && _hasNotes;

        /// <summary>
        /// Gets or sets if the the data model has constituencies results.
        /// </summary>
        public bool HasRegionalList
        {
            get => _hasRegionalList;
            set { _hasRegionalList = value; NotifyOfPropertyChange(() => HasRegionalList); }
        }

        /// <summary>
        /// Gets the party notes list observable for the data grid display.
        /// </summary>
        public ObservableCollection<PartyNote> PartyNotesList { get; }

        /// <summary>
        /// Gets the constituency result list observable for the data grid display.
        /// </summary>
        public ObservableCollection<ConstituencyResult> ConstituencyResultsList { get; }

        /// <summary>
        /// Gets the regional results list observable for the data grid display.
        /// </summary>
        public ObservableCollection<ConstituencyResult> RegionalResultsList { get; }

        #endregion

        #region Commands

        /// <summary>
        /// Load the notes command.
        /// </summary>
        public ICommand LoadNotesCommand =>
            _loadNotesCommand ??
                (_loadNotesCommand = new CommandHandler(LoadNotesCommandAction, true));

        /// <summary>
        /// Load the constituency results file command.
        /// </summary>
        public ICommand LoadConstituencyResultsCommand =>
            _loadConstituencyResultsCommand ??
                (_loadConstituencyResultsCommand = new CommandHandler(LoadConstituencyResultsCommandAction, true));

        /// <summary>
        /// Load the list results command.
        /// </summary>
        public ICommand LoadListResultsCommand =>
            _loadListResultsCommand ??
                (_loadListResultsCommand = new CommandHandler(LoadListResultsCommandAction, true));

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

            if (HasNotes)
            {
                ElectionResults.Parties = parser.PartiesProvider;
                PartyNotesList.Clear();

                foreach (string abbreviation in parser.PartiesProvider.PartyAbbreviations.OrderBy(x => x))
                {
                    PartyNotesList.Add(new PartyNote { Abbreviation = abbreviation, FullName = parser.PartiesProvider.PartiesByAbbreviation[abbreviation] });
                }
            }
        }

        /// <summary>
        /// The load constituency results file command action.
        /// </summary>
        private void LoadConstituencyResultsCommandAction()
        {
            Console.WriteLine("LoadConstituencyResultsCommandAction");

            ConstituencyResultsFileParser parser =
                new ConstituencyResultsFileParser(ConfigurationSettings.DatabaseSettings.ResultsDirectory);

            ConstituenciesFile = parser.FilePath;
            HasConstituencies = parser.ReadSuccessfully;

            if (HasConstituencies)
            {
                ElectionResults.FirstVotes = parser.ConstituencyResultProvider;
                ConstituencyResultsList.Clear();

                foreach (string name in parser.ConstituencyResultProvider.ConstituencyNames.OrderBy(x => x))
                {
                    ConstituencyResultsList.Add(parser.ConstituencyResultProvider.ResultsByName[name]);
                }
            }
        }

        /// <summary>
        /// The load list results file command action.
        /// </summary>
        private void LoadListResultsCommandAction()
        {
            Console.WriteLine("LoadListResultsCommandAction");

            ListResultsFileParser parser =
                new ListResultsFileParser(ConfigurationSettings.DatabaseSettings.ResultsDirectory);

            RegionalListsFile = parser.FilePath;
            HasRegionalList = parser.ReadSuccessfully;

            if (HasNotes)
            {
                ElectionResults.SecondVotes = parser.ConstituencyResultProvider;
                RegionalResultsList.Clear();

                foreach (string name in parser.ConstituencyResultProvider.ConstituencyNames.OrderBy(x => x))
                {
                    RegionalResultsList.Add(parser.ConstituencyResultProvider.ResultsByName[name]);
                }
            }
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
            _hasConstituencies = false;
            _hasRegionalList = false;

            PartyNotesList = new ObservableCollection<PartyNote>();
            ConstituencyResultsList = new ObservableCollection<ConstituencyResult>();
            RegionalResultsList = new ObservableCollection<ConstituencyResult>();
        }
    }
}

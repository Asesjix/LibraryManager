using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.VisualStudio.Text;
using Microsoft.Web.LibraryManager.Contracts;
using Microsoft.Web.LibraryManager.Vsix.Resources;
using Microsoft.Web.LibraryManager.Vsix.UI.Models;

namespace Microsoft.Web.LibraryManager.Vsix.UI
{
    internal partial class InstallDialog
    {
        private readonly IDependencies _deps;
        private readonly string _folder;
        private readonly string _configFileName;
        private string _helpText;

        public InstallDialog(IDependencies dependencies, string configFileName, string folder)
        {
            if (!String.IsNullOrWhiteSpace(folder))
            {
                InstallationFolder.DestinationFolder = Path.GetFileName(folder.TrimEnd('\\')) + "/";
            }

            InitializeComponent();

            _deps = dependencies;
            _folder = folder;
            _configFileName = configFileName;

            LostKeyboardFocus += InstallDialog_LostKeyboardFocus;
            Loaded += OnLoaded;
        }

        private void InstallDialog_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            if (!IsKeyboardFocusWithin && !(e.NewFocus is ListBoxItem))
            {
                TraversalRequest request = new TraversalRequest(FocusNavigationDirection.Next);
                MoveFocus(request);
            }
        }

        internal InstallDialogViewModel ViewModel
        {
            get { return DataContext as InstallDialogViewModel; }
            set { DataContext = value; }
        }

        public Task<CompletionSet> PerformSearch(string searchText, int caretPosition)
        {
            return ViewModel.SelectedProvider.GetCatalog().GetLibraryCompletionSetAsync(searchText, caretPosition);
        }

        public Task<CompletionSet> TargetLocationSearch(string searchText, int caretPosition)
        {
            var dependencies = Dependencies.FromConfigFile(_configFileName);
            string cwd = dependencies?.GetHostInteractions().WorkingDirectory;

            IEnumerable<Tuple<string, string>> completions = GetCompletions(cwd, searchText, caretPosition, out Span textSpan);

            var span = new CompletionSet
            {
                Start = 0,
                Length = searchText.Length
            };

            var completionItems = new List<CompletionItem>();

            foreach (Tuple<string, string> completion in completions)
            {
                var completionItem = new CompletionItem
                {
                    DisplayText = completion.Item1,
                    InsertionText =completion.Item2,
                };

                completionItems.Add(completionItem);
            }

            span.Completions = completionItems;

            return Task.FromResult(span);
        }

        public string HelpText
        {
            get { return _helpText; }
            set
            {
                if (String.Equals(value, "cdnjs"))
                {
                    _helpText = Text.CdnjsLibraryIdHintText;
                }
                else if (String.Equals(value, "filesystem"))
                {
                    _helpText = Text.FileSystemLibraryIdHintText;
                }
            }
        }

        private IEnumerable<Tuple<string, string>> GetCompletions(string cwd, string value, int caretPosition, out Span span)
        {
            span = new Span(0, value.Length);
            var list = new List<Tuple<string, string>>();

            int index = value.Length >= caretPosition - 1 ? value.LastIndexOf('/', Math.Max(caretPosition - 1, 0)) : value.Length;
            string prefix = "";

            if (index > 0)
            {
                prefix = value.Substring(0, index + 1);
                cwd = Path.Combine(cwd, prefix);
                span = new Span(index + 1, value.Length - index - 1);
            }

            var dir = new DirectoryInfo(cwd);

            if (dir.Exists)
            {
                foreach (FileSystemInfo item in dir.EnumerateDirectories())
                {
                    list.Add(Tuple.Create(item.Name + "/", prefix + item.Name + "/"));
                }
            }

            return list;
        }

        private void CloseDialog(bool res)
        {
            try
            {
                DialogResult = res;
            }
            catch { }
            Close();
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            ViewModel = new InstallDialogViewModel(Dispatcher, _configFileName, _deps, _folder, CloseDialog);

            FocusManager.SetFocusedElement(cbName, cbName);
        }

        private void ThemedWindow_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (!cbName.IsMouseOver && !cbName.IsMouseOverFlyout)
            {
                TraversalRequest request = new TraversalRequest(FocusNavigationDirection.Next);
                MoveFocus(request);
            }
        }
    }
}

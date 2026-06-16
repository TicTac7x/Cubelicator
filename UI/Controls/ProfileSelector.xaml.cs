using Cubelicator.Services;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using Windows.System;

namespace Cubelicator.UI.Controls
{
    public partial class ProfileSelector : UserControl
    {
        private readonly ProfileManager profileManager;
        private readonly GamecubeController gamecubeController;
        private readonly bool advanced;

        private bool isRenaming = false;

        public ProfileSelector(ProfileManager profileManager, GamecubeController gamecubeController, bool advanced)
        {
            this.profileManager = profileManager;
            this.gamecubeController = gamecubeController;
            this.advanced = advanced;

            InitializeComponent();
            InitializeUI();
            SetupEvents();
        }

        private void InitializeUI()
        {
            SelectedProfileText.Text = gamecubeController.Profile.Name;
            Element_SelectedProfile.IsEnabled = profileManager.Profiles.Count > 1;
            Element_DeleteProfile.IsEnabled = profileManager.Profiles.Count > 1;
            GenerateProfiles();

            if (advanced)
            {
                ProfileText.Visibility = Visibility.Visible;
                ProfileManagerButton.Visibility = Visibility.Visible;
            }
        }

        private void SetupEvents()
        {
            gamecubeController.OnProfileChanged += (_) =>
            {
                InitializeUI();
            };

            profileManager.OnProfilesChanged += (profiles) =>
            {
                InitializeUI();
            };
        }

        private void GenerateProfiles()
        {
            RootProfiles.Items.Clear();
            foreach (var profile in profileManager.Profiles)
            {
                if (profile.Name == gamecubeController.Profile.Name) continue;

                var menuItem = new MenuFlyoutItem
                {
                    Text = profile.Name,
                };
                menuItem.Click += (_, _) =>
                {
                    gamecubeController.Profile = profile;
                };

                RootProfiles.Items.Add(menuItem);
            }
        }

        private void OnNewProfile(object sender, RoutedEventArgs args)
        {
            var profile = profileManager.NewProfile();
            gamecubeController.Profile = profile;
        }

        private void OnDeleteProfile(object sender, RoutedEventArgs args)
        {
            gamecubeController.Profile.Delete();
        }

        private void OnRenameProfile(object sender, RoutedEventArgs e)
        {
            isRenaming = true;

            SelectedProfileEdit.Text = gamecubeController.Profile.Name;

            Element_SelectedProfile.Visibility = Visibility.Collapsed;
            SelectedProfileEdit.Visibility = Visibility.Visible;

            SelectedProfileEdit.Focus(FocusState.Programmatic);
            SelectedProfileEdit.SelectAll();
        }

        private void OnRenameKeyDown(object sender, KeyRoutedEventArgs args)
        {
            if (args.Key == VirtualKey.Enter)
            {
                CommitRename();
            }
            else if (args.Key == VirtualKey.Escape)
            {
                CancelRename();
            }
        }

        private void OnRenameLostFocus(object sender, RoutedEventArgs args)
        {
            if (isRenaming)
            {
                CommitRename();
            }
        }

        private void CancelRename()
        {
            SelectedProfileEdit.Text = gamecubeController.Profile.Name;
            ExitRenameMode();
        }

        private void CommitRename()
        {
            string newName = SelectedProfileEdit.Text.Trim();

            if (string.IsNullOrWhiteSpace(newName))
            {
                CancelRename();
                return;
            }

            // Prevent duplicates
            if (profileManager.GetProfile(newName) != null &&
                newName != gamecubeController.Profile.Name)
            {
                CancelRename();
                return;
            }

            gamecubeController.Profile.Name = newName;
            SelectedProfileText.Text = newName;

            ExitRenameMode();
        }

        private void ExitRenameMode()
        {
            isRenaming = false;
            SelectedProfileEdit.Visibility = Visibility.Collapsed;
            Element_SelectedProfile.Visibility = Visibility.Visible;
        }
    }
}

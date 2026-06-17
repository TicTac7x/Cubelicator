using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using Windows.System;

namespace Cubelicator
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
            Element_SelectedProfileText.Text = gamecubeController.Profile.Name;
            Element_SelectedProfile.IsEnabled = profileManager.Profiles.Count > 1;
            Element_DeleteProfile.IsEnabled = profileManager.Profiles.Count > 1;
            Element_ToggleRumble.Text = gamecubeController.Profile.Rumble ? Strings.ProfileSelector_DisableRumble : Strings.ProfileSelector_EnableRumble;
            GenerateProfiles();

            if (advanced)
            {
                Element_ProfileText.Visibility = Visibility.Visible;
                Element_ProfileManagerButton.Visibility = Visibility.Visible;
            }
        }

        private void SetupEvents()
        {
            gamecubeController.Event_ProfileChanged += (_) =>
            {
                InitializeUI();
            };

            gamecubeController.Profile.Event_Changed += () =>
            {
                InitializeUI();
            };

            profileManager.Event_ProfilesChanged += (profiles) =>
            {
                InitializeUI();
            };
        }

        private void GenerateProfiles()
        {
            Element_Profiles.Items.Clear();
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

                Element_Profiles.Items.Add(menuItem);
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

        private void OnToggleRumble(object sender, RoutedEventArgs args)
        {
            App.DebugOutput(gamecubeController.Profile.Rumble);
            gamecubeController.Profile.SetRumble(!gamecubeController.Profile.Rumble);
        }

        private void OnRenameProfile(object sender, RoutedEventArgs e)
        {
            isRenaming = true;

            Element_SelectedProfileEdit.Text = gamecubeController.Profile.Name;

            Element_SelectedProfile.Visibility = Visibility.Collapsed;
            Element_SelectedProfileEdit.Visibility = Visibility.Visible;

            Element_SelectedProfileEdit.Focus(FocusState.Programmatic);
            Element_SelectedProfileEdit.SelectAll();
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
            Element_SelectedProfileEdit.Text = gamecubeController.Profile.Name;
            ExitRenameMode();
        }

        private void CommitRename()
        {
            string newName = Element_SelectedProfileEdit.Text.Trim();

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
            Element_SelectedProfileText.Text = newName;

            ExitRenameMode();
        }

        private void ExitRenameMode()
        {
            isRenaming = false;
            Element_SelectedProfileEdit.Visibility = Visibility.Collapsed;
            Element_SelectedProfile.Visibility = Visibility.Visible;
        }
    }
}

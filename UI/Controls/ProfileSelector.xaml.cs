using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using System.Text.RegularExpressions;
using Windows.System;

namespace Cubelicator
{
    public partial class ProfileSelector : UserControl
    {
        private readonly AdapterPort port;
        private readonly Settings settings;
        private readonly ProfileManager profileManager;
        private readonly ViewsManager viewsManager;
        private readonly GamecubeController gamecubeController;
        private readonly bool advanced;

        private bool initialized = false;
        private bool isRenaming = false;

        public ProfileSelector(AdapterPort port, Settings settings, ViewsManager viewsManager, ProfileManager profileManager, GamecubeController gamecubeController, bool advanced)
        {
            this.port = port;
            this.settings = settings;
            this.viewsManager = viewsManager;
            this.profileManager = profileManager;
            this.gamecubeController = gamecubeController;
            this.advanced = advanced;

            InitializeComponent();
            InitializeUI();
            SetupEvents();

            // Populate list of controller colors.
            Element_ChangeColor.Items.Clear();
            foreach (var controllerColor in ControllerColors.Map.Keys)
            {
                var item = new MenuFlyoutItem
                {
                    Text = Regex.Replace(
                    controllerColor.ToString(),
                    "(\\B[A-Z])",
                    " $1"),
                    Tag = controllerColor.ToString()
                };

                item.Click += UIEvent_ChangeColor;

                Element_ChangeColor.Items.Add(item);
            }

            initialized = true;
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

        private void UIEvent_ChangeColor(object sender, RoutedEventArgs e)
        {
            if (!initialized) return;
            if (sender is not MenuFlyoutItem item)
                return;

            if (item.Tag is not string colorName)
                return;

            if (!Enum.TryParse<ControllerColor>(colorName, out var colorEnum))
                return;

            settings.SetControllerColor(port, colorEnum);
        }
        private void UIEvent_NewProfile(object sender, RoutedEventArgs args)
        {
            if (!initialized) return;
            var profile = profileManager.NewProfile();
            gamecubeController.Profile = profile;
        }

        private void UIEvent_DeleteProfile(object sender, RoutedEventArgs args)
        {
            if (!initialized) return;
            gamecubeController.Profile.Delete();
        }

        private void UIEvent_ToggleRumble(object sender, RoutedEventArgs args)
        {
            if (!initialized) return;
            gamecubeController.Profile.SetRumble(!gamecubeController.Profile.Rumble);
        }

        private void UIEvent_Calibrate(object sender, RoutedEventArgs args)
        {
            if (!initialized) return;
            viewsManager.ShowCalibration(port, gamecubeController);
        }

        private void UIEvent_RenameProfile(object sender, RoutedEventArgs e)
        {
            if (!initialized) return;
            isRenaming = true;

            Element_SelectedProfileEdit.Text = gamecubeController.Profile.Name;

            Element_SelectedProfile.Visibility = Visibility.Collapsed;
            Element_SelectedProfileEdit.Visibility = Visibility.Visible;

            Element_SelectedProfileEdit.Focus(FocusState.Programmatic);
            Element_SelectedProfileEdit.SelectAll();
        }

        private void UIEvent_KeyDown(object sender, KeyRoutedEventArgs args)
        {
            if (!initialized) return;
            if (args.Key == VirtualKey.Enter)
            {
                CommitRename();
            }
            else if (args.Key == VirtualKey.Escape)
            {
                CancelRename();
            }
        }

        private void UIEvent_LostFocus(object sender, RoutedEventArgs args)
        {
            if (!initialized) return;
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

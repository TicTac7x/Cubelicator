using Cubelicator.Services;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace Cubelicator.UI.Controls
{
    public partial class ProfileSelector : UserControl
    {
        private readonly ProfileManager profileManager;
        private readonly GamecubeController gamecubeController;
        private readonly bool advanced;

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
            SelectedProfile.Text = gamecubeController.Profile.Name;
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
    }
}

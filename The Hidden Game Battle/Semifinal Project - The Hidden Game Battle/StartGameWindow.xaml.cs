namespace Semifinal_Project___The_Hidden_Game_Battle {
    using System;
    using System.Collections.Generic;
    using System.Media;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Media;
    using System.Windows.Media.Animation;
    using System.Windows.Media.Imaging;
    using System.Windows.Shapes;
    using System.Windows.Threading;
    using Semifinal_Project___The_Hidden_Game_Battle.Classes;
    /// <summary>
    /// Interaction logic for StartGameWindow.xaml
    /// </summary>
    public partial class StartGameWindow : Window {
        
        public StartGameWindow() {
            InitializeComponent();

            SetPlayersInfo();
            PlaceButtonsToGrid();
            SetGameObjects();

            playerListView = _player1ListView;
            playerObject = player1ObjectsList;

            // set dinosaur sprite location
            for (int i = 1; i <= 8; i++) {
                dinoSpriteRunList.Add("../../Images/Run/Run (" + i.ToString() + ").png");
            }

            _transitionCanvas.Height = 700;
            _transitionCanvas.Width = 1350;
            _strategyModeLabel.Height = 200;
            _strategyModeLabel.Width = 1350;

            transitionTimer = new DispatcherTimer();
            transitionTimer.Interval = TimeSpan.FromSeconds(0.06);
            transitionTimer.Tick += new EventHandler(AnimateDinosaur);
            transitionTimer.Tick += new EventHandler(MoveTrain);
            transitionTimer.Start();
            trainMusic.PlayLooping();
#if false
            _player1PowerProgressBar.Value = 20;
            _player2PowerProgressBar.Value = 20;
#endif
#if false
            _transitionCanvas.Height = 0;
            _transitionCanvas.Width = 0;
            _strategyModeLabel.Height = 0;
            _strategyModeLabel.Width = 0;
            transitionTimer.Stop();
            trainMusic.Stop();
#endif
            timeTimer = new DispatcherTimer();
            timeTimer.Interval = TimeSpan.FromSeconds(1);
            timeTimer.Tick += new EventHandler(StartTimer);
            

            rainObjectTimer = new DispatcherTimer();
            rainObjectTimer.Interval = TimeSpan.FromSeconds(.05);
            rainObjectTimer.Tick += new EventHandler(RainObjects);
            rainObjectTimer.Tick += new EventHandler(MoveBackgroundImage);
            rainObjectTimer.Start();

            moveBackgroundTimer = new DispatcherTimer();
            moveBackgroundTimer.Interval = TimeSpan.FromSeconds(.05);
            moveBackgroundTimer.Tick += new EventHandler(MoveBackgroundImage);
            moveBackgroundTimer.Start();

            winnerTimer = new DispatcherTimer();
            winnerTimer.Interval = TimeSpan.FromSeconds(0.1);
            winnerTimer.Tick += new EventHandler(MoveWinnerText);
        }

        private Random random = new Random();

        private SoundPlayer trainMusic = new SoundPlayer("../../Images/Sound/beep.wav");
        private SoundPlayer battleMusic = new SoundPlayer("../../Images/Sound/unravel.wav");
        private SoundPlayer youWinMusic = new SoundPlayer("../../Images/Sound/you win.wav");
        private SoundPlayer yeheyWinMusic = new SoundPlayer("../../Images/Sound/yehey.wav");

        // class for threading updating UI
        private DispatcherTimer transitionTimer = new DispatcherTimer();
        private DispatcherTimer timeTimer = new DispatcherTimer();
        private DispatcherTimer rainObjectTimer = new DispatcherTimer();
        private DispatcherTimer moveBackgroundTimer = new DispatcherTimer();
        private DispatcherTimer winnerTimer = new DispatcherTimer();

        private List<string> dinoSpriteRunList = new List<string>();
        private List<Animals> player1ObjectsList = new List<Animals>();
        private List<Animals> player2ObjectsList = new List<Animals>();
        private List<Button> player1ButtonSelectedList = new List<Button>();
        private List<Button> player2ButtonSelectedList = new List<Button>();
        private List<Button> copyPlayer1ButtonSelectedList = new List<Button>();
        private List<Button> copyPlayer2ButtonSelectedList = new List<Button>();
        private List<Button> buttonList = new List<Button>();
        private List<Line> linesList = new List<Line>();

        private ListView playerListView = new ListView();
        private List<Animals> playerObject = new List<Animals>();

        private Button otherButton = new Button();
        private Button firstButtonClick = new Button();
        private Button secondButtonClick = new Button();
        private Line line = new Line();

        private const int gridLength = 15;
        private const int playerObjectSize = 24;

        private int index = 0;
        private int count = 0;
        private int button1Column = -1;
        private int button1Row = -1;
        private int button2Column = -1;
        private int button2Row = -1;
        private int timeSeconds = 0;
        private int player1CountWin = 0;
        private int player2CountWin = 0;
 
        private bool ifBattleMode = false;
        private bool winnerTextGoingUP = false;

        private void CheckIfPowerUPCanBeUsed() {
            // button start blinking
            ColorAnimation animation = new ColorAnimation(Colors.Green, new Duration(TimeSpan.FromSeconds(1)));
            animation.RepeatBehavior = RepeatBehavior.Forever;
            
            // player 1 power up buttons
            if (_player1PowerProgressBar.Value >= 10 && _player1BorderButton.Background == Brushes.Red) {
                _player1PowerUP1Button.Background = new SolidColorBrush(Colors.Yellow);
                _player1PowerUP1Button.Background.BeginAnimation(SolidColorBrush.ColorProperty, animation);
            }
            else {
                _player1PowerUP1Button.Background = Brushes.Transparent;
            } 
            if (_player1PowerProgressBar.Value >= 12 && _player1BorderButton.Background == Brushes.Red) {
                _player1PowerUP2Button.Background = new SolidColorBrush(Colors.Yellow);
                _player1PowerUP2Button.Background.BeginAnimation(SolidColorBrush.ColorProperty, animation);
            }
            else {
                _player1PowerUP2Button.Background = Brushes.Transparent;
            }
            if (_player1PowerProgressBar.Value >= 15 && _player1BorderButton.Background == Brushes.Red) {
                _player1PowerUP3Button.Background = new SolidColorBrush(Colors.Yellow);
                _player1PowerUP3Button.Background.BeginAnimation(SolidColorBrush.ColorProperty, animation);
            }
            else {
                _player1PowerUP3Button.Background = Brushes.Transparent;
            }
            if (_player1PowerProgressBar.Value >= 17 && _player1BorderButton.Background == Brushes.Red && buttonList.Count > 50) {
                _player1PowerUP4Button.Background = new SolidColorBrush(Colors.Yellow);
                _player1PowerUP4Button.Background.BeginAnimation(SolidColorBrush.ColorProperty, animation);
            }
            else {
                _player1PowerUP4Button.Background = Brushes.Transparent;
            }
            if (_player1PowerProgressBar.Value >= 20 && _player1BorderButton.Background == Brushes.Red) {
                _player1PowerUP5Button.Background = new SolidColorBrush(Colors.Yellow);
                _player1PowerUP5Button.Background.BeginAnimation(SolidColorBrush.ColorProperty, animation);
            }
            else {
                _player1PowerUP5Button.Background = Brushes.Transparent;
            }

            // player 2 power up buttons
            if (_player2PowerProgressBar.Value >= 10 && _player2BorderButton.Background == Brushes.Red) {
                _player2PowerUP1Button.Background = new SolidColorBrush(Colors.Yellow);
                _player2PowerUP1Button.Background.BeginAnimation(SolidColorBrush.ColorProperty, animation);
            }
            else {
                _player2PowerUP1Button.Background = Brushes.Transparent;
            }
            if (_player2PowerProgressBar.Value >= 12 && _player2BorderButton.Background == Brushes.Red) {
                _player2PowerUP2Button.Background = new SolidColorBrush(Colors.Yellow);
                _player2PowerUP2Button.Background.BeginAnimation(SolidColorBrush.ColorProperty, animation);
            }
            else {
                _player2PowerUP2Button.Background = Brushes.Transparent;
            }
            if (_player2PowerProgressBar.Value >= 15 && _player2BorderButton.Background == Brushes.Red) {
                _player2PowerUP3Button.Background = new SolidColorBrush(Colors.Yellow);
                _player2PowerUP3Button.Background.BeginAnimation(SolidColorBrush.ColorProperty, animation);
            }
            else {
                _player2PowerUP3Button.Background = Brushes.Transparent;
            }
            if (_player2PowerProgressBar.Value >= 17 && _player2BorderButton.Background == Brushes.Red && buttonList.Count > 50) {
                _player2PowerUP4Button.Background = new SolidColorBrush(Colors.Yellow);
                _player2PowerUP4Button.Background.BeginAnimation(SolidColorBrush.ColorProperty, animation);
            }
            else {
                _player2PowerUP4Button.Background = Brushes.Transparent;
            }
            if (_player2PowerProgressBar.Value >= 20 && _player2BorderButton.Background == Brushes.Red) {
                _player2PowerUP5Button.Background = new SolidColorBrush(Colors.Yellow);
                _player2PowerUP5Button.Background.BeginAnimation(SolidColorBrush.ColorProperty, animation);
            }
            else {
                _player2PowerUP5Button.Background = Brushes.Transparent;
            }
        }
        private void SetVisibility() {
            // hide listvie and button
            _player1ListView.Visibility = Visibility.Hidden;
            _player1FinishButton.Visibility = Visibility.Hidden;
            _player1FinishButton.IsEnabled = false;

            _player2ListView.Visibility = Visibility.Hidden;
            _player2FinishButton.Visibility = Visibility.Hidden;
            _player2FinishButton.IsEnabled = false;

            // visible power up and health
            _player1LifeProgressBar.Visibility = Visibility.Visible;
            _player1LifeTextBlock.Visibility = Visibility.Visible;
            _player1PowerProgressBar.Visibility = Visibility.Visible;
            _player1PowerTextBlock.Visibility = Visibility.Visible;
            _player1LifeLabelTextBlock.Visibility = Visibility.Visible;
            _player1PowerLabelTextBlock.Visibility = Visibility.Visible;

            _player2LifeProgressBar.Visibility = Visibility.Visible;
            _player2LifeTextBlock.Visibility = Visibility.Visible;
            _player2PowerProgressBar.Visibility = Visibility.Visible;
            _player2PowerTextBlock.Visibility = Visibility.Visible;
            _player2LifeLabelTextBlock.Visibility = Visibility.Visible;
            _player2PowerLabelTextBlock.Visibility = Visibility.Visible;

            // visible power up buttons
            _player1PowerUP1Button.Visibility = Visibility.Visible;
            _player1PowerUP2Button.Visibility = Visibility.Visible;
            _player1PowerUP3Button.Visibility = Visibility.Visible;
            _player1PowerUP4Button.Visibility = Visibility.Visible;
            _player1PowerUP5Button.Visibility = Visibility.Visible;

            _player2PowerUP1Button.Visibility = Visibility.Visible;
            _player2PowerUP2Button.Visibility = Visibility.Visible;
            _player2PowerUP3Button.Visibility = Visibility.Visible;
            _player2PowerUP4Button.Visibility = Visibility.Visible;
            _player2PowerUP5Button.Visibility = Visibility.Visible;
        }
        private void EnableButtonGrid() {
            player1ButtonSelectedList = new List<Button>(copyPlayer1ButtonSelectedList);
            player2ButtonSelectedList = new List<Button>(copyPlayer2ButtonSelectedList);
            foreach (Button button in _buttonGrid.Children) {
                button.IsEnabled = true;
                button.Visibility = Visibility.Visible;
            }
        }
        private void CheckWinner() {
            if(_player2LifeProgressBar.Value == 0) {
                player1CountWin++;
                if(player1CountWin == 1) {
                    player1CountScoreLabel1.Background = Brushes.Yellow;
                    _player2LifeProgressBar.Value = 100;
                    _player1LifeProgressBar.Value = 100;
                    _player2PowerProgressBar.Value = 0;
                    _player1PowerProgressBar.Value = 0;
                    
                    _winnerMessageTextBlock.Text = _player1NameLabel.Content + " Win this round!";
                    winnerTimer.Start();
                    youWinMusic.Play();
                }
                else if(player1CountWin == 2) {
                    player1CountScoreLabel2.Background = Brushes.Yellow;
                }
            }
            else if (_player1LifeProgressBar.Value == 0) {
                player2CountWin++;
                if (player2CountWin == 1) {
                    player2CountScoreLabel1.Background = Brushes.Yellow;
                    _player1LifeProgressBar.Value = 100;
                    _player2LifeProgressBar.Value = 100;
                    _player2PowerProgressBar.Value = 0;
                    _player1PowerProgressBar.Value = 0;

                    _winnerMessageTextBlock.Text = _player2NameLabel.Content + " Win this round!";
                    winnerTimer.Start();
                    youWinMusic.Play(); 
                }
                else if (player2CountWin == 2) {
                    player2CountScoreLabel2.Background = Brushes.Yellow;
                }
            }
            if(player1CountWin == 2) {
                _winnerMessageTextBlock.Text = _player1NameLabel.Content + " Won the match!";
                player1CountWin = 2;
                yeheyWinMusic.Play();
                winnerTimer.Start();
            }
            else if(player2CountWin == 2) {
                _winnerMessageTextBlock.Text = _player2NameLabel.Content + " Won the match!";
                player2CountWin = 2;
                yeheyWinMusic.Play();
                winnerTimer.Start();
            }
        }
        private bool CheckIntersect() {
            foreach (var item in linesList) {
                xPoint p1 = new xPoint();
                xPoint p2 = new xPoint();
                xPoint p3 = new xPoint();
                xPoint p4 = new xPoint();

                p1.x = item.X1;
                p1.y = item.Y1;
                p2.x = item.X2;
                p2.y = item.Y2;


                p3.x = line.X1;
                p3.y = line.Y1;
                p4.x = line.X2;
                p4.y = line.Y2;

                Intersection collision = new Intersection();
                if (collision.doIntersect(p1, p2, p3, p4)) {
                    return true;
                }
            }
            return false;
        }
        private bool IfButtonClickIsAllied(Button btn, List<Button> playerButtonsSelected) {
            foreach (var buttons in playerButtonsSelected) {
                if (btn == buttons) {
                    return true;
                }
            }
            return false;
        }
        private bool IfButtonClickIsEnemy(Button btn, List<Button> playersButtonsSelected) {
            foreach (var buttons in playersButtonsSelected) {
                if (btn == buttons) {
                    return true;
                }
            }
            return false;
        }
        private void SetPlayersInfo() {
            MainWindow window = Application.Current.Windows[0] as MainWindow;
            _player1AvatarImage.Source = window.avatars[window._player1ComboBox.SelectedIndex].image;
            _player2AvatarImage.Source = window.avatars[window._player2ComboBox.SelectedIndex].image;
            _player1BorderButton.Background = Brushes.Red;

            if (window._player1MaleRadioButton.IsChecked == true) {
                _player1NameLabel.Content = "(M) " + window._player1NameTextBox.Text;
            }
            else if (window._player1FemaleRadioButton.IsChecked == true) {
                _player1NameLabel.Content = "(F) " + window._player1NameTextBox.Text;
            }
            if (window._player2FemaleRadioButton.IsChecked == true) {
                _player2NameLabel.Content = "(F) " + window._player2NameTextBox.Text;
            }
            else if (window._player2MaleRadioButton.IsChecked == true) {
                _player2NameLabel.Content = "(M) " + window._player2NameTextBox.Text;
            }
        }
        private void SetGameObjects() {
#if false
            player1ObjectsList.Add(new Animals {
                objects = new BitmapImage(new Uri("../../Images/Objects/Object1.png", UriKind.Relative)),
                blockOccupied = 6
            });
            player2ObjectsList.Add(new Animals {
                objects = new BitmapImage(new Uri("../../Images/Objects/Object12.png", UriKind.Relative)),
                blockOccupied = 6
            });
#else
            int[] arr = new int[] { 6, 4, 4, 3, 3, 2, 2 };
            for (int i = 1, j = 0; i <= 7; i++, j++) {
                player1ObjectsList.Add(new Animals {
                    objects = new BitmapImage(new Uri("../../Images/Objects/Object" + i.ToString() + ".png", UriKind.Relative)),
                    blockOccupied = arr[j]
                });
            }
            arr = new int[] { 2, 2, 3, 3, 6, 4, 4 };
            for (int i = 8, j = 0; i <= 14; i++, j++) {
                player2ObjectsList.Add(new Animals {
                    objects = new BitmapImage(new Uri("../../Images/Objects/Object" + i.ToString() + ".png", UriKind.Relative)),
                    blockOccupied = arr[j]
                });
            }
#endif
            _player1ListView.ItemsSource = player1ObjectsList;
            _player2ListView.ItemsSource = player2ObjectsList;
        }
        private void PlaceButtonsToGrid() {
            for (int i = 0; i < gridLength; i++) {
                for (int j = 0; j < gridLength; j++) {
                    Button button = new Button();
                    button.Background = Brushes.Transparent;
                    button.Name = "_btn" + i.ToString() + j.ToString();
                    button.Click += new RoutedEventHandler(IfButtonGridIsClick);
                    button.PreviewMouseMove += new MouseEventHandler(ButtonGridIfMouseMove);
                    button.MouseEnter += new MouseEventHandler(ButtonGridIfMouseEnter);
                    Grid.SetRow(button, i);
                    Grid.SetColumn(button, j);

                    _buttonGrid.Children.Add(button);
                    buttonList.Add(button);
                }
            }
        }
        private void SetImage() {
            Image image = new Image();
            bool x1 = false;
            bool x2 = false;
            int c = 0;
           
            if (Grid.GetRow(firstButtonClick) != Grid.GetRow(secondButtonClick)) { // vertical
                foreach (Button buttons in _buttonGrid.Children) {
                    if (buttons == firstButtonClick && x2 == false) {
                        x1 = true;
                        buttons.Visibility = Visibility.Hidden;
                        Grid.SetColumn(image, Grid.GetColumn(buttons));
                        Grid.SetRow(image, Grid.GetRow(buttons));
                        continue;
                    }
                    if (buttons == secondButtonClick && x1 == false) {
                        x2 = true;
                        buttons.Visibility = Visibility.Hidden;
                        Grid.SetColumn(image, Grid.GetColumn(buttons));
                        Grid.SetRow(image, Grid.GetRow(buttons));
                        continue;
                    }
                    if (x1) {
                        if (Grid.GetColumn(buttons) == Grid.GetColumn(firstButtonClick)) {
                            buttons.IsEnabled = false;
                            buttons.Visibility = Visibility.Hidden;
                            image.Source = playerObject[playerListView.SelectedIndex].objects;
                            image.Stretch = Stretch.Fill;
                            c++;
                        }
                        if (buttons == secondButtonClick) {
                            Grid.SetRowSpan(image, c + 1);
                            buttons.Visibility = Visibility.Hidden;
                            _objectGrid.Children.Add(image);
                            break;
                        }
                    }
                    else if (x2) {
                        if (Grid.GetColumn(buttons) == Grid.GetColumn(secondButtonClick)) {
                            buttons.IsEnabled = false;
                            buttons.Visibility = Visibility.Hidden;
                            image.Source = playerObject[playerListView.SelectedIndex].objects;
                            image.Stretch = Stretch.Fill;
                            c++;
                        }
                        if (buttons == firstButtonClick) {
                            Grid.SetRowSpan(image, c + 1);
                            buttons.Visibility = Visibility.Hidden;
                            _objectGrid.Children.Add(image);
                            break;
                        }
                    }
                }
            }
            else {
                foreach (Button buttons in _buttonGrid.Children) {    // horizontal
                    if (buttons == firstButtonClick && x2 == false) {
                        x1 = true;
                        buttons.Visibility = Visibility.Hidden;
                        Grid.SetColumn(image, Grid.GetColumn(buttons));
                        Grid.SetRow(image, Grid.GetRow(buttons));
                        continue;
                    }
                    if (buttons == secondButtonClick && x1 == false) {
                        x2 = true;
                        buttons.Visibility = Visibility.Hidden;
                        Grid.SetColumn(image, Grid.GetColumn(buttons));
                        Grid.SetRow(image, Grid.GetRow(buttons));
                        continue;
                    }
                    if (x1) {
                        buttons.IsEnabled = false;
                        buttons.Visibility = Visibility.Hidden;
                        image.Source = playerObject[playerListView.SelectedIndex].objects;
                        image.Stretch = Stretch.Fill;
                        c++;
                        if (buttons == secondButtonClick) {
                            Grid.SetColumnSpan(image, c + 1);
                            buttons.Visibility = Visibility.Hidden;                          
                            _objectGrid.Children.Add(image);
                            break;
                        }
                    }
                    else if (x2) {
                        buttons.IsEnabled = false;
                        buttons.Visibility = Visibility.Hidden;
                        image.Source = playerObject[playerListView.SelectedIndex].objects;
                        image.Stretch = Stretch.Fill;
                        c++;
                        if (buttons == firstButtonClick) {
                            Grid.SetColumnSpan(image, c + 1);
                            buttons.Visibility = Visibility.Hidden;
                            _objectGrid.Children.Add(image);
                            break;
                        }
                    }
                }
            }
        }
        private void UpdateListView() {
            if(playerListView == _player1ListView) {
                player1ObjectsList.RemoveAt(playerListView.SelectedIndex);

                _player1ListView.ItemsSource = null;
                _player1ListView.ItemsSource = player1ObjectsList;

                if(_player1ListView.Items.Count > 0) {
                    playerListView = _player1ListView;
                    playerObject = player1ObjectsList;
                }
                else {
                    playerListView = _player2ListView;
                    playerObject = player2ObjectsList;

                    // button start blinking
                    ColorAnimation animation = new ColorAnimation(Colors.Green, new Duration(TimeSpan.FromSeconds(1)));
                    animation.RepeatBehavior = RepeatBehavior.Forever;
                    _player1FinishButton.Background = new SolidColorBrush(Colors.Yellow);
                    _player1FinishButton.Background.BeginAnimation(SolidColorBrush.ColorProperty, animation);
                }
            }
            else if(playerListView == _player2ListView) {
                player2ObjectsList.RemoveAt(playerListView.SelectedIndex);
                
                _player2ListView.ItemsSource = null;
                _player2ListView.ItemsSource = player2ObjectsList;

                if(_player2ListView.Items.Count > 0) {
                    playerListView = _player2ListView;
                    playerObject = player2ObjectsList;
                }
                else {
                    // button start blinking
                    ColorAnimation animation = new ColorAnimation(Colors.Green, new Duration(TimeSpan.FromSeconds(1)));
                    animation.RepeatBehavior = RepeatBehavior.Forever;
                    _player2FinishButton.Background = new SolidColorBrush(Colors.Yellow);
                    _player2FinishButton.Background.BeginAnimation(SolidColorBrush.ColorProperty, animation);
                }
            }
        }
        private void CheckLineLength() {
            if (Grid.GetRow(otherButton) == Grid.GetRow(firstButtonClick)) {
                int c1 = 0;
                int c2 = 0;
                bool x1 = false;
                bool x2 = false;
                for (int i = 0; i < _buttonGrid.Children.Count; i++) {
                    if (_buttonGrid.Children[i] as Button == firstButtonClick && x1 == false) {
                        x1 = true;

                    }
                    if ((_buttonGrid.Children[i] as Button == otherButton && x2 == false)) {
                        x2 = true;
                    }
                    if (x1) {
                        if (_buttonGrid.Children[i] as Button == otherButton) {
                            c1++;
                            if (c1 > playerObject[playerListView.SelectedIndex].blockOccupied || c1 < playerObject[playerListView.SelectedIndex].blockOccupied) {
                                line.Stroke = Brushes.Red;
                                break;
                            }
                        }
                        c1++;
                    }
                    if (x2) {
                        if (_buttonGrid.Children[i] as Button == firstButtonClick) {
                            c2++;
                            if (c2 > playerObject[playerListView.SelectedIndex].blockOccupied || c2 < playerObject[playerListView.SelectedIndex].blockOccupied) {
                                line.Stroke = Brushes.Red;
                                break;
                            }
                        }
                        c2++;
                    }
                }
            }
            else if (Grid.GetColumn(otherButton) == Grid.GetColumn(firstButtonClick)) {
                int c1 = 0;
                int c2 = 0;
                bool x1 = false;
                bool x2 = false;
                for (int i = 0; i < _buttonGrid.Children.Count; i++) {
                    if (_buttonGrid.Children[i] as Button == firstButtonClick && x1 == false) {
                        x1 = true;

                    }
                    if ((_buttonGrid.Children[i] as Button == otherButton && x2 == false)) {
                        x2 = true;
                    }
                    if (x1) {
                        if (Grid.GetColumn(_buttonGrid.Children[i] as Button) == Grid.GetColumn(firstButtonClick)) {
                            if (_buttonGrid.Children[i] as Button == otherButton) {
                                c1++;
                                if (c1 > playerObject[playerListView.SelectedIndex].blockOccupied || c1 < playerObject[playerListView.SelectedIndex].blockOccupied) {
                                    line.Stroke = Brushes.Red;
                                    break;
                                }
                            }
                            c1++;
                        }
                    }
                    if (x2) {
                        if (Grid.GetColumn(_buttonGrid.Children[i] as Button) == Grid.GetColumn(firstButtonClick)) {
                            if (_buttonGrid.Children[i] as Button == firstButtonClick) {
                                c2++;
                                if (c2 > playerObject[playerListView.SelectedIndex].blockOccupied || c2 < playerObject[playerListView.SelectedIndex].blockOccupied) {
                                    line.Stroke = Brushes.Red;
                                    break;
                                }
                            }
                            c2++;
                        }

                    }
                }
            }
        }
        private void CreateLine(Button button) {
            Point mousePoint = Mouse.GetPosition(_mainCanvas);
            if (count == 1) {
                button1Column = Grid.GetColumn(button);
                button1Row = Grid.GetRow(button);

                line.Width = 10;
                line.Height = 10;
                line = new Line();
                line.Stroke = Brushes.Green;
                line.StrokeThickness = 5;
                line.X1 = mousePoint.X;
                line.Y1 = mousePoint.Y;
                line.X2 = mousePoint.X;
                line.Y2 = mousePoint.Y;
                Panel.SetZIndex(line, -1);
                _mainCanvas.Children.Add(line);
                firstButtonClick = button;

            }
            else if (count == 2 && line.Stroke == Brushes.Red) {
                count = 1;
                button.IsEnabled = true;
                return;
            }
            else if (count == 2) {
                linesList.Add(line);
                line.Visibility = Visibility.Hidden;
                count = 0;
                Panel.SetZIndex(line, 1);
                secondButtonClick = button;
               
                SetImage();
                UpdateListView();

                firstButtonClick.IsEnabled = false;
                secondButtonClick.IsEnabled = false;
                firstButtonClick.Visibility = Visibility.Hidden;
                secondButtonClick.Visibility = Visibility.Hidden;
                firstButtonClick = new Button();
                secondButtonClick = new Button();
            }
        }
        private void PlayerStartAttack(Button btn) {
            if(_player1BorderButton.Background == Brushes.Red) {
                bool successAttack = false;
                if (!IfButtonClickIsAllied(btn, player1ButtonSelectedList) && !IfButtonClickIsEnemy(btn, player2ButtonSelectedList)) {
                    btn.IsEnabled = false;
                    btn.Visibility = Visibility.Hidden;

                    _player1BorderButton.Background = Brushes.White;
                    _player2BorderButton.Background = Brushes.Red;

                }
                else if (IfButtonClickIsEnemy(btn, player2ButtonSelectedList)) { // success to get enemy's object
                    btn.IsEnabled = false;
                    btn.Visibility = Visibility.Hidden;
                    player2ButtonSelectedList.Remove(btn);
                    // update player 1 lifebar
                    _player2LifeProgressBar.Value -= (100.0d / Convert.ToDouble(playerObjectSize));
                    successAttack = true;
                }
                else {
                    _player1BorderButton.Background = Brushes.White;
                    _player2BorderButton.Background = Brushes.Red;

                }
                timeSeconds = -1;

                // update player 1 powerbar
                if (successAttack) {
                    _player1PowerProgressBar.Value += 2;
                }
                else {
                    _player1PowerProgressBar.Value += 1;
                }
            }
            else if (_player2BorderButton.Background == Brushes.Red) {
                bool successAttack = false;
                if (!IfButtonClickIsAllied(btn, player2ButtonSelectedList) && !IfButtonClickIsEnemy(btn, player1ButtonSelectedList)) {
                    btn.IsEnabled = false;
                    btn.Visibility = Visibility.Hidden;

                    _player2BorderButton.Background = Brushes.White;
                    _player1BorderButton.Background = Brushes.Red;
                }
                else if (IfButtonClickIsEnemy(btn, player1ButtonSelectedList)) { // success to get enemy's object
                    btn.IsEnabled = false;
                    btn.Visibility = Visibility.Hidden;
                    player1ButtonSelectedList.Remove(btn);

                    // update player 1 lifebar
                    _player1LifeProgressBar.Value -= (100.0d / Convert.ToDouble(playerObjectSize));

                    successAttack = true;
                }
                else {
                    _player2BorderButton.Background = Brushes.White;
                    _player1BorderButton.Background = Brushes.Red;
                }
                timeSeconds = -1;
                // update player 1 powerbar
                if (successAttack) {
                    _player2PowerProgressBar.Value += 2;
                }
                else {
                    _player2PowerProgressBar.Value += 1;
                }
            }
        }
        private void StartTimer(object sender, EventArgs e) {
            CheckIfPowerUPCanBeUsed();
            if(timeSeconds == 10) {
                if(_player1BorderButton.Background == Brushes.Red) {
                    _player1BorderButton.Background = Brushes.White;
                    _player2BorderButton.Background = Brushes.Red;
                }
                else if (_player2BorderButton.Background == Brushes.Red) {
                    _player2BorderButton.Background = Brushes.White;
                    _player1BorderButton.Background = Brushes.Red;
                }
                timeSeconds = -1;
            }
            timeSeconds++;

            string str = string.Empty;
            if(timeSeconds < 10) {
                str = "0";
            }
            else {
                str = string.Empty;
            }
            if(timeSeconds > 6) {
                _timerLabel.Foreground = Brushes.Red;
            }
            else {
                _timerLabel.Foreground = Brushes.Black;
            }
            _timerLabel.Content = "00" + ":" + str + timeSeconds;
        }
        private void IfButtonGridIsClick(object sender, RoutedEventArgs e) {
            Button button = sender as Button;
            buttonList.Remove(button);
            if (ifBattleMode) {
                PlayerStartAttack(button);
                CheckWinner();
                return;
            }
            if ((!(playerListView.SelectedIndex < playerObject.Count && playerListView.SelectedIndex >= 0)) && (playerObject.Count != 0 && playerListView == _player1ListView)) {
                MessageBox.Show("Please select an object to deploy!", "", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            else if ((!(playerListView.SelectedIndex < playerObject.Count && playerListView.SelectedIndex >= 0)) && (playerObject.Count != 0 && playerListView == _player2ListView)) {
                MessageBox.Show("Please select an object to deploy!", "", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            else if(_player1ListView.Items.Count == 0 && _player2ListView.Items.Count == 0) {
                return;
            }

            count++;
            CreateLine(button);
        }
        public  void ButtonGridIfMouseEnter(object sender, MouseEventArgs e) {
#if false
            _player1PowerProgressBar.Value = 20;
            _player2PowerProgressBar.Value = 20;
#endif
            Button button = sender as Button;
            otherButton = button;
            button2Column = Grid.GetColumn(button);
            button2Row = Grid.GetRow(button);
        }  
        private void ButtonGridIfMouseMove(object sender, MouseEventArgs e) {
            if (line != null && count == 1) {
                if (button1Row != button2Row && button1Column != button2Column) {
                    line.Stroke = Brushes.Red;
                }
                else if (CheckIntersect()) { // check intersection between two lines
                    line.Stroke = Brushes.Red;
                }
                else {   
                    line.Stroke = Brushes.Green;
                    CheckLineLength();
                }
                
                // follow the Line to mouse cursor
                Point mousePoint = Mouse.GetPosition(_mainCanvas);  
                line.Y2 = mousePoint.Y;
                line.X2 = mousePoint.X;
            }
        }
        private void StartGameWindow_PreviewKeyDown(object sender, KeyEventArgs e) {
            if(e.Key == Key.Escape) {
                _mainCanvas.Children.Remove(line);
                line = new Line();
                count = 0;
                playerListView.SelectedIndex = -1;
                
            }
        }
        private void AnimateDinosaur(object sender, EventArgs e) {
            if (index >= dinoSpriteRunList.Count) {
                index = 0;
            }
            _dinoRec.Fill = new ImageBrush {
                ImageSource = new BitmapImage(new Uri(dinoSpriteRunList[index], UriKind.Relative))
            };
            _dinoRec.LayoutTransform = new ScaleTransform(-1, 1, 0, 0);
            index++;
        }
        private void MoveTrain(object sender, EventArgs e) {
            timeSeconds = -1;
            if (Canvas.GetLeft(_trainRec) >= -500) {
                _transitionCanvas.Height = 700;
                _transitionCanvas.Width = 1390;

                Canvas.SetLeft(_trainRec, Canvas.GetLeft(_trainRec) - 30);
                Canvas.SetLeft(_dinoRec, Canvas.GetLeft(_dinoRec) - 30);
            }
            else {
                _transitionCanvas.Height = 0;
                _transitionCanvas.Width = 0;
                _strategyModeLabel.Height = 0;
                _strategyModeLabel.Width = 0;
                _battleModeLabel.Height = 0;
                _battleModeLabel.Width = 0;

                Canvas.SetLeft(_trainRec, 1500);
                Canvas.SetLeft(_dinoRec, 1420);
                transitionTimer.Stop();
                trainMusic.Stop();
                trainMusic.Dispose();

                if (ifBattleMode) {
                    battleMusic.PlayLooping();
                }
            }
        }
        private void RainObjects(object sender, EventArgs e) { 
            int num = random.Next(400);
            Ellipse objects = new Ellipse();
            objects.Width = 5;
            objects.Height = 5;
            objects.Fill = new ImageBrush {
                ImageSource = new BitmapImage(new Uri("../../Images/Pictures/Snow.png", UriKind.Relative))
            };
            Canvas.SetTop(objects, 0);
            Canvas.SetLeft(objects, random.Next(1350));
            objects.LayoutTransform = new RotateTransform(-35);
            if (num == 25) {
                objects.LayoutTransform = new RotateTransform(random.Next(360));
                objects.Width = 130;
                objects.Height = 130;
                objects.Fill = new ImageBrush {
                    ImageSource = new BitmapImage(new Uri("../../Images/Pictures/karl.png", UriKind.Relative))
                };
            }
            _meteorCanvas.Children.Add(objects);

            // remove snow UI Element
            for (int index = _meteorCanvas.Children.Count - 1; index >= 0; index--) {
                if (_meteorCanvas.Children[index] is Ellipse) {
                    if (Canvas.GetTop(_meteorCanvas.Children[index]) >= _meteorCanvas.ActualHeight + 100) {
                        _meteorCanvas.Children.RemoveAt(index);
                    }
                }
            }

            // drop snow UI Element
            foreach (Ellipse snows in _meteorCanvas.Children) {
                Canvas.SetTop(snows, Canvas.GetTop(snows) + 10);
            }
        }
        private void MoveBackgroundImage(object sender, EventArgs e) {
            // transition imageBackground
            if (Canvas.GetLeft(_backGroundImageCanvas2) <= -(_backGroundImageCanvas1.Width)) {
                Canvas.SetLeft(_backGroundImageCanvas2, _backGroundImageCanvas2.Width - 10);
            }
            if (Canvas.GetLeft(_backGroundImageCanvas1) <= -(_backGroundImageCanvas2.Width)) {
                Canvas.SetLeft(_backGroundImageCanvas1, _backGroundImageCanvas1.Width - 10);
            }
            Canvas.SetLeft(_backGroundImageCanvas1, Canvas.GetLeft(_backGroundImageCanvas1) - 3);
            Canvas.SetLeft(_backGroundImageCanvas2, Canvas.GetLeft(_backGroundImageCanvas2) - 3);
        }
        private void MoveWinnerText(object sender, EventArgs e) {
            timeSeconds = -1;
            if (!winnerTextGoingUP) {
                Canvas.SetTop(_winnerGrid, Canvas.GetTop(_winnerGrid) + 100);
                if (Canvas.GetTop(_winnerGrid) == 0) {
                    winnerTimer.Stop();
                }
            }
            else if (winnerTextGoingUP) {
                Canvas.SetTop(_winnerGrid, Canvas.GetTop(_winnerGrid) - 100);
                if (Canvas.GetTop(_winnerGrid) == -700) {
                    winnerTextGoingUP = false;
                    battleMusic.Play();
                    winnerTimer.Stop();
                }
            }
        }
      
        private void Player1FinishButton_Click(object sender, RoutedEventArgs e) {
            if (player1ObjectsList.Count == 0) {
                foreach(Button button in _buttonGrid.Children) {    // add button to list player 1
                    if(button.IsEnabled == false) {
                        player1ButtonSelectedList.Add(button);
                        copyPlayer1ButtonSelectedList.Add(button);
                    }
                }

                _player2ListView.IsEnabled = true;
                _player1FinishButton.Background = Brushes.White;
                _player1BorderButton.Background = Brushes.White;
                _player2BorderButton.Background = Brushes.Red;
            }
        }
        private void Player2FinishButton_Click(object sender, RoutedEventArgs e) {
            if (player2ObjectsList.Count == 0) {
                foreach (Button button in _buttonGrid.Children) {   // add button to list player 1
                    if (button.IsEnabled == false) {
                        if (player1ButtonSelectedList.Contains(button)) {
                            continue;
                        }
                        player2ButtonSelectedList.Add(button);
                        copyPlayer2ButtonSelectedList.Add(button);
                    }
                }
                SetVisibility();
                
                ifBattleMode = true;
                _player2FinishButton.Background = Brushes.White;
                _player2BorderButton.Background = Brushes.White;
                _player1BorderButton.Background = Brushes.Red;

                foreach(Button button in _buttonGrid.Children) {
                    button.IsEnabled = true;
                    button.Background = Brushes.Gray;
                    button.Visibility = Visibility.Visible;
                }
                Panel.SetZIndex(_objectGrid, -1);

#if true
                _transitionCanvas.Height = 700;
                _transitionCanvas.Width = 1350;
                _battleModeLabel.Height = 200;
                _battleModeLabel.Width = 1350;

                transitionTimer.Start();
                trainMusic.PlayLooping();

#endif
                timeSeconds = -1;
                timeTimer.Start();
            }
        }
        private void ExitButton_Click(object sender, RoutedEventArgs e) {
            this.Close();
        }
        private void OKButton_Click(object sender, RoutedEventArgs e) {  
            if (player1CountWin == 2 || player2CountWin == 2) {
                this.Close();
            }
            else {
                winnerTimer.Start();
                winnerTextGoingUP = true;
                EnableButtonGrid();
            }
        }
        private void Player1PowerUPButtons_Click(object sender, RoutedEventArgs e) {
            Button button = sender as Button;
            // 10 12 15 17 20
            if (_player1PowerProgressBar.Value >= 10 && _player1BorderButton.Background == Brushes.Red) {
                Power_ups power_up = new Power_ups(ref _buttonGrid, ref player2ButtonSelectedList, ref buttonList, ref _player2LifeProgressBar, ref player1ButtonSelectedList, Colors.Green, Colors.Yellow);
                if (button.Name == _player1PowerUP1Button.Name) {
                    _player1PowerProgressBar.Value -= 10;
                    power_up.PowerUP(1); 
                }
                else if (button.Name == _player1PowerUP2Button.Name) {
                    _player1PowerProgressBar.Value -= 12;
                    power_up.PowerUP(2);
                }
                else if (button.Name == _player1PowerUP3Button.Name) {
                    _player1PowerProgressBar.Value -= 15;
                    power_up.PowerUP(3);
                }
                else if (button.Name == _player1PowerUP4Button.Name && buttonList.Count > 4) {
                    _player1PowerProgressBar.Value -= 17;
                    power_up.PowerUP(4);
                }
                else if (button.Name == _player1PowerUP5Button.Name) {
                    _player1PowerProgressBar.Value -= 20;
                    power_up.PowerUP(5);
                }
                _player1BorderButton.Background = Brushes.White;
                _player2BorderButton.Background = Brushes.Red;
                timeSeconds = -1;
                CheckWinner();
            }
        }
        private void Player2PowerUPButtons_Click(object sender, RoutedEventArgs e) {
            Button button = sender as Button;
            // 10 12 15 17 20
            if (_player2PowerProgressBar.Value >= 10 && _player2BorderButton.Background == Brushes.Red) {
                Power_ups power_up = new Power_ups(ref _buttonGrid, ref player1ButtonSelectedList, ref buttonList, ref _player1LifeProgressBar, ref player2ButtonSelectedList, Colors.Blue, Colors.Orange);
                if (button.Name == _player2PowerUP1Button.Name) {
                    _player2PowerProgressBar.Value -= 10;
                    power_up.PowerUP(1);
                }
                else if (button.Name == _player2PowerUP2Button.Name) {
                    _player2PowerProgressBar.Value -= 12;
                    power_up.PowerUP(2);
                }
                else if (button.Name == _player2PowerUP3Button.Name) {
                    _player2PowerProgressBar.Value -= 15;
                    power_up.PowerUP(3);
                }
                else if (button.Name == _player2PowerUP4Button.Name && buttonList.Count > 4) {
                    _player2PowerProgressBar.Value -= 17;
                    power_up.PowerUP(4);
                }
                else if (button.Name == _player2PowerUP5Button.Name) {
                    _player2PowerProgressBar.Value -= 20;
                    power_up.PowerUP(5);
                }
                _player1BorderButton.Background = Brushes.Red;
                _player2BorderButton.Background = Brushes.White;
                timeSeconds = -1;
                CheckWinner();
            }
        }

        private void Player1PowerUpButtons_MouseEnter(object sender, MouseEventArgs e) {
            Button button = sender as Button;
            if(button.Name == "_player1PowerUP1Button") {
                _player1PowerUP1.Visibility = Visibility.Visible;
            }
            else if (button.Name == "_player1PowerUP2Button") {
                _player1PowerUP2.Visibility = Visibility.Visible;
            }
            else if (button.Name == "_player1PowerUP3Button") {
                _player1PowerUP3.Visibility = Visibility.Visible;
            }
            else if (button.Name == "_player1PowerUP4Button") {
                _player1PowerUP4.Visibility = Visibility.Visible;
            }
            else if (button.Name == "_player1PowerUP5Button") {
                _player1PowerUP5.Visibility = Visibility.Visible;
            }
        }
        private void Player1PowerUpButtons_MouseLeave(object sender, MouseEventArgs e) {
            Button button = sender as Button;
            if (button.Name == "_player1PowerUP1Button") {
                _player1PowerUP1.Visibility = Visibility.Hidden;
            }
            else if (button.Name == "_player1PowerUP2Button") {
                _player1PowerUP2.Visibility = Visibility.Hidden;
            }
            else if (button.Name == "_player1PowerUP3Button") {
                _player1PowerUP3.Visibility = Visibility.Hidden;
            }
            else if (button.Name == "_player1PowerUP4Button") {
                _player1PowerUP4.Visibility = Visibility.Hidden;
            }
            else if (button.Name == "_player1PowerUP5Button") {
                _player1PowerUP5.Visibility = Visibility.Hidden;
            }
        }

        private void Player2PowerUpButtons_MouseEnter(object sender, MouseEventArgs e) {
            Button button = sender as Button;
            if (button.Name == "_player2PowerUP1Button") {
                _player2PowerUP1.Visibility = Visibility.Visible;
            }
            else if (button.Name == "_player2PowerUP2Button") {
                _player2PowerUP2.Visibility = Visibility.Visible;
            }
            else if (button.Name == "_player2PowerUP3Button") {
                _player2PowerUP3.Visibility = Visibility.Visible;
            }
            else if (button.Name == "_player2PowerUP4Button") {
                _player2PowerUP4.Visibility = Visibility.Visible;
            }
            else if (button.Name == "_player2PowerUP5Button") {
                _player2PowerUP5.Visibility = Visibility.Visible;
            }
        }
        private void Player2PowerUpButtons_MouseLeave(object sender, MouseEventArgs e) {
            Button button = sender as Button;
            if (button.Name == "_player2PowerUP1Button") {
                _player2PowerUP1.Visibility = Visibility.Hidden;
            }
            else if (button.Name == "_player2PowerUP2Button") {
                _player2PowerUP2.Visibility = Visibility.Hidden;
            }
            else if (button.Name == "_player2PowerUP3Button") {
                _player2PowerUP3.Visibility = Visibility.Hidden;
            }
            else if (button.Name == "_player2PowerUP4Button") {
                _player2PowerUP4.Visibility = Visibility.Hidden;
            }
            else if (button.Name == "_player2PowerUP5Button") {
                _player2PowerUP5.Visibility = Visibility.Hidden;
            }
        }
    }
}

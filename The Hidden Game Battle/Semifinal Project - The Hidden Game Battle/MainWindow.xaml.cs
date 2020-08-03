namespace Semifinal_Project___The_Hidden_Game_Battle {
    using System;
    using System.Collections.Generic;
    using System.Media;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;
    using System.Windows.Shapes;
    using System.Windows.Threading;
    using Semifinal_Project___The_Hidden_Game_Battle.Classes;
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        public MainWindow() {
            InitializeComponent();

            // set dinosaur sprite location
            for (int i = 1; i <= 8; i++) {
                dinoSpriteRun.Add("../../Images/Run/Run (" + i.ToString() + ").png");
            }

            // sets chants && avatars
            SetChants();
            SetAvatars();



#if true
            // play background music
            backgroundMusic.PlayLooping();
#else
            _gameTitleImage1.Opacity = 1;
            backgroundMusic.Stop();
#endif

            // start threading
            timer1.Interval = TimeSpan.FromSeconds(1);
            timer1.Tick += new EventHandler(MoveGameTitle);
            timer1.Tick += new EventHandler(AnimateDinosaur);
            timer1.Tick += new EventHandler(RainSnow);
            timer1.Tick += new EventHandler(MoveShips);
            timer1.Start();

            timer2.Interval = TimeSpan.FromSeconds(0.1);
            timer2.Tick += new EventHandler(MovePreperationCanvas);
            timer2.Tick += new EventHandler(MoveAboutCanvas);
            timer2.Start();

            timer3.Interval = TimeSpan.FromSeconds(0.1);
            timer3.Tick += new EventHandler(MoveHelpCanvas);

            _videoMedia.Play();
            _videoMedia.Pause();
        }
 
        private Random random = new Random();
        private SoundPlayer backgroundMusic = new SoundPlayer("../../Images/Sound/Sound1.wav");

        public List<Avatars> avatars = new List<Avatars>();
        public List<Chant> chants = new List<Chant>();
        private List<string> dinoSpriteRun = new List<string>();
        
        private DispatcherTimer timer1 = new DispatcherTimer();
        private DispatcherTimer timer2 = new DispatcherTimer();
        private DispatcherTimer timer3 = new DispatcherTimer();

        private double max = 1;
        private int count = 0;
        private int angleRotate = -1;

        private bool buttonsOpacity = false;
        private bool movePreperationCanvas = false;
        private bool moveAboutCanvas = false;

        private void SetAvatars() {
            // set avatars
            for (int i = 1; i <= 12; i++) {
                avatars.Add(new Avatars {
                    image = new BitmapImage(new Uri("../../Images/Avatars/avatar" + i.ToString() + ".jpg", UriKind.Relative))
                });
            }
            // bind data
            _player1ComboBox.ItemsSource = avatars;
            _player2ComboBox.ItemsSource = avatars;


            _player1ComboBox.SelectedIndex = 5; // player1 default avatar
            _player2ComboBox.SelectedIndex = 11; // player2 default avatar
        }
        private void SetChants() {
            chants.Add(new Chant {
                trashtalk = "Well Played"
            });
            chants.Add(new Chant {
                trashtalk = "Nice Attack"
            });
            chants.Add(new Chant {
                trashtalk = "Sorry I got you!"
            });
            chants.Add(new Chant {
                trashtalk = "Opps!"
            });

            // default gender
            _player1MaleRadioButton.IsChecked = true;
            _player2FemaleRadioButton.IsChecked = true;
        }
        private void UnHideButtons() {
            _button1.IsEnabled = true;
            _button2.IsEnabled = true;
            _button3.IsEnabled = true;
            _button4.IsEnabled = true;

            _button1.Visibility = Visibility.Visible;
            _button2.Visibility = Visibility.Visible;
            _button3.Visibility = Visibility.Visible;
            _button4.Visibility = Visibility.Visible;
            movePreperationCanvas = false;
            Canvas.SetLeft(_preperationCanvas, 1000);
        }
        private void HideButtons() {
            _button1.IsEnabled = false;
            _button2.IsEnabled = false;
            _button3.IsEnabled = false;
            _button4.IsEnabled = false;

            _button1.Visibility = Visibility.Hidden;
            _button2.Visibility = Visibility.Hidden;
            _button3.Visibility = Visibility.Hidden;
            _button4.Visibility = Visibility.Hidden;

        }
       
        private void MoveShips(object sender, EventArgs e) {
            if (_gameTitleImage1.Opacity >= max) {
                _oceanRec.Opacity = 1;
                Canvas.SetLeft(_ship1Rec, Canvas.GetLeft(_ship1Rec) - 10);
                Canvas.SetLeft(_ship2Rec, Canvas.GetLeft(_ship2Rec) - 6);
                if (Canvas.GetLeft(_ship1Rec) <= -_gameTitleImage1.ActualWidth) {
                    Canvas.SetLeft(_ship1Rec, _gameTitleImage1.ActualWidth);
                }
                if (Canvas.GetLeft(_ship2Rec) <= -_gameTitleImage1.ActualWidth) {
                    Canvas.SetLeft(_ship2Rec, _gameTitleImage1.ActualWidth);
                }
            } 
        }
        private void MoveGameTitle(object sender, EventArgs e) {        
            if (_gameTitleImage1.Opacity <= max) {
                _gameTitleImage1.Opacity += 0.05;
            }
            if(_gameTitleImage1.Opacity >= max) {
                if (!buttonsOpacity) {
                    _dinoRec.Opacity = 1;
                    _button1.Opacity = 1;
                    _button2.Opacity = 1;
                    _button3.Opacity = 1;
                    _button4.Opacity = 1;
                    buttonsOpacity = true;
                }
                timer1.Interval = TimeSpan.FromSeconds(.1);
                Canvas.SetLeft(_gameTitleImage1, Canvas.GetLeft(_gameTitleImage1) - 20);
                Canvas.SetLeft(_dinoRec, Canvas.GetLeft(_dinoRec) - 20);
                if (Canvas.GetLeft(_gameTitleImage1) <= -_gameTitleImage1.ActualWidth) {
                    Canvas.SetLeft(_gameTitleImage1, _gameTitleImage1.ActualWidth);
                }
                if (Canvas.GetLeft(_dinoRec) <= -_gameTitleImage1.ActualWidth) {
                    Canvas.SetLeft(_dinoRec, _gameTitleImage1.ActualWidth);
                }
            }
        }
        private void AnimateDinosaur(object sender, EventArgs e) {
            if (count >= dinoSpriteRun.Count) {
                count = 0;
            }
            _dinoRec.Fill = new ImageBrush {
                ImageSource = new BitmapImage(new Uri(dinoSpriteRun[count], UriKind.Relative))
            };
            _dinoRec.LayoutTransform = new ScaleTransform(-1, 1, 0, 0);
            count++;
        }
        private void RainSnow(object sender, EventArgs e) {
            if (_gameTitleImage1.Opacity >= max) {
                Ellipse snow = new Ellipse();
                snow.Width = 5;
                snow.Height = 5;
                snow.Fill = new ImageBrush {
                    ImageSource = new BitmapImage(new Uri("../../Images/Pictures/Snow.png", UriKind.Relative))
                };

                Canvas.SetTop(snow, 0);
                Canvas.SetLeft(snow, random.Next(1000));
                _snowCanvas.Children.Add(snow);

                // remove snow UI Element
                for (int index = _snowCanvas.Children.Count - 1; index >= 0; index--) {
                    if (_snowCanvas.Children[index] is Ellipse) {
                        if (Canvas.GetTop(_snowCanvas.Children[index]) >= _snowCanvas.ActualHeight + 100) {
                            _snowCanvas.Children.RemoveAt(index);
                        }
                    }
                }

                // drop snow UI Element
                foreach (Ellipse snows in _snowCanvas.Children) {
                    Canvas.SetTop(snows, Canvas.GetTop(snows) + 10);
                }

            }
        }
        private void MovePreperationCanvas(object sender, EventArgs e) {
            if (movePreperationCanvas) {
                if (Canvas.GetLeft(_preperationCanvas) > (_preperationCanvas.ActualWidth / 2) + 10) {
                    Canvas.SetLeft(_preperationCanvas, Canvas.GetLeft(_preperationCanvas) - 20);
                }
            }
        }
        private void MoveHelpCanvas(object sender, EventArgs e) {
            if (!videoBack) {
                Canvas.SetLeft(_helpCanvas1, Canvas.GetLeft(_helpCanvas1) + 100);
                if (Canvas.GetLeft(_helpCanvas1) >= 0) {
                    timer3.Stop();
                }
            }
            else if (videoBack) {
                Canvas.SetLeft(_helpCanvas1, Canvas.GetLeft(_helpCanvas1) - 100);
                if (Canvas.GetLeft(_helpCanvas1) <= -1000) {
                    videoBack = false;
                    UnHideButtons();
                    backgroundMusic.Play();
                    timer3.Stop();
                }
            }
        }
        private bool videoBack = false;
        private void MoveAboutCanvas(object sender, EventArgs e) {
            if (moveAboutCanvas) {
                angleRotate -= 8;
                if(angleRotate <= -360) {
                    angleRotate = -1; 
                }
                _myPicEllipse.RenderTransform = new RotateTransform(angleRotate);
                if (Canvas.GetLeft(_aboutCanvas) > -_aboutCanvas.ActualWidth) {
                    Canvas.SetLeft(_aboutCanvas, Canvas.GetLeft(_aboutCanvas) - 20);
                }
                if (Canvas.GetLeft(_aboutCanvas) <= -_aboutCanvas.ActualWidth) {
                    Canvas.SetLeft(_aboutCanvas, 1000);
                    moveAboutCanvas = false;
                    UnHideButtons();
                }
            }   
        }

        // mother canvas buttons
        private void StartGameButton_Click(object sender, RoutedEventArgs e) {
            HideButtons();
            movePreperationCanvas = true;
        }
        private void HelpButton_Click(object sender, RoutedEventArgs e) {
            backgroundMusic.Stop();
            HideButtons();
            _videoMedia.ScrubbingEnabled = true;
            timer3.Start();
        }
        private void AboutButton_Click(object sender, RoutedEventArgs e) {
            HideButtons();
            moveAboutCanvas = true;
        }
        private void ExitButton_Click(object sender, RoutedEventArgs e) {
            Application.Current.Shutdown();
        }

        // preperation canvas buttons
        private void StartNowButton_Click(object sender, RoutedEventArgs e) {
            backgroundMusic.Stop();
            backgroundMusic.Dispose();
            timer1.Stop();

            this.Hide();
            StartGameWindow startGameWindow = new StartGameWindow();
            startGameWindow.ShowDialog();
            backgroundMusic.PlayLooping();

            UnHideButtons();
            

            timer1.Start();
            this.ShowDialog();
        }
        private void CancelButton_Click(object sender, RoutedEventArgs e) {
            UnHideButtons();
        }

        private void PlayVideoButton_Click(object sender, RoutedEventArgs e) {
            _videoMedia.Play(); 
        }

        private void PauseVideoButton_Click(object sender, RoutedEventArgs e) {
            _videoMedia.Pause();
        }
        private void BackVideoButton_Click(object sender, RoutedEventArgs e) {
            _videoMedia.Stop();
            videoBack = true;
            timer3.Start();
        }

        private void Media_Ended(object sender, RoutedEventArgs e) {
            _videoMedia.Position = TimeSpan.Zero;
            _videoMedia.Stop();
        }
    }
}

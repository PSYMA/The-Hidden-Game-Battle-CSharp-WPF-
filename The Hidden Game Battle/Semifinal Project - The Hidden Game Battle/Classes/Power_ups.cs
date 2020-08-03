namespace Semifinal_Project___The_Hidden_Game_Battle.Classes {
    using System;
    using System.Collections.Generic;
    using System.Windows.Controls;
    using System.Windows;
    using System.Windows.Media.Animation;
    using System.Windows.Media;
    using System.Windows.Threading;

    public class Power_ups {
        private Grid buttonGrid = new Grid();
        private Random random = new Random();
        private ProgressBar playerLife = new ProgressBar();
        private List<Button> currentPlayerButtons = new List<Button>();
        private List<Button> buttonList = new List<Button>();
        private List<Button> otherPlayerButtons = new List<Button>();
        private Color color1;
        private Color color2;
        public Power_ups(ref Grid buttonGrid, ref List<Button> otherPlayerButtons, ref List<Button> buttonList, ref ProgressBar playerLife, ref List<Button> currentPlayerButtons, Color color1, Color color2) {
            this.buttonGrid = buttonGrid;
            this.otherPlayerButtons = otherPlayerButtons;
            this.buttonList = buttonList;
            this.playerLife = playerLife;
            this.currentPlayerButtons = currentPlayerButtons;
            this.color1 = color1;
            this.color2 = color2;
        }
        public void PowerUP(int num) {
            if(num == 1) {
                PowerOne();
            }
            else if (num == 2) {
                PowerTwo();
            }
            else if (num == 3) {
                PowerThree();
            }
            else if (num == 4) {
                PowerFour();
            }
            else if (num == 5) {
                PowerFive();
            }
        }

        private void PowerOne() {
            // reveal one part of enemy's object's
            try {
                Button revealButton = otherPlayerButtons[random.Next(otherPlayerButtons.Count)];
                foreach (Button button in buttonGrid.Children) {
                    if (revealButton == button && button.IsEnabled == true) {
                        button.IsEnabled = false;
                        button.Visibility = Visibility.Hidden;
#if true
                        playerLife.Value -= (100.0d / 24.0d);
#else
                        playerLife.Value -= (100.0d / 6.0d);
#endif
                        otherPlayerButtons.Remove(button);
                        break;
                    }
                }
            }catch(Exception) {  }
        }
        private void PowerTwo() {
            try {
                // hint 3 buttons for 5 seconds
                List<Button> hintButtonList = new List<Button>();
                List<Button> copyOtherPlayerButtons = new List<Button>(otherPlayerButtons);
                for (int i = 0; i < 3; i++) {
                    Button hintButton = copyOtherPlayerButtons[random.Next(copyOtherPlayerButtons.Count)];
                    foreach (Button button in buttonGrid.Children) {
                        if (hintButton == button && button.IsEnabled == true) {
                            hintButtonList.Add(button);
                            copyOtherPlayerButtons.Remove(button);
                        }
                    }
                }
                // start blinking
                foreach (var items in hintButtonList) {
                    ColorAnimation animation = new ColorAnimation(color1, new Duration(TimeSpan.FromSeconds(1)));
                    animation.RepeatBehavior = RepeatBehavior.Forever;
                    items.Background = new SolidColorBrush(color2);
                    items.Background.BeginAnimation(SolidColorBrush.ColorProperty, animation);
                }
                // stop blinking
                var timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(5) };
                timer.Start();
                timer.Tick += (sender, args) => {
                    timer.Stop();
                    foreach (var items in hintButtonList) {
                        items.Background = Brushes.Gray;
                    }
                };
            } catch (Exception) { }
        }
        private void PowerThree() {
            try {
                // hint 6 buttons for 8 seconds
                List<Button> hintButtonList = new List<Button>();
                List<Button> copyOtherPlayerButtons = new List<Button>(otherPlayerButtons);
                for (int i = 0; i < 6; i++) {
                    Button hintButton = copyOtherPlayerButtons[random.Next(copyOtherPlayerButtons.Count)];
                    foreach (Button button in buttonGrid.Children) {
                        if (hintButton == button && button.IsEnabled == true) {
                            hintButtonList.Add(button);
                            copyOtherPlayerButtons.Remove(button);
                        }
                    }
                }
                // start blinking
                foreach (var items in hintButtonList) {
                    ColorAnimation animation = new ColorAnimation(color1, new Duration(TimeSpan.FromSeconds(1)));
                    animation.RepeatBehavior = RepeatBehavior.Forever;
                    items.Background = new SolidColorBrush(color2);
                    items.Background.BeginAnimation(SolidColorBrush.ColorProperty, animation);
                }
                // stop blinking
                var timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(8) };
                timer.Start();
                timer.Tick += (sender, args) => {
                    timer.Stop();
                    foreach (var items in hintButtonList) {
                        items.Background = Brushes.Gray;
                    }
                };
            } catch (Exception) { }
        }
        private void PowerFour() {
            // reveal four random buttons
            try {
                int x = 0;
                for (int i = 0; i < 4; i++) {
                    Button revealButton = buttonList[random.Next(buttonList.Count)];
                    foreach(Button button in buttonGrid.Children) {
                        if(button == revealButton && button.IsEnabled == true) {
                            if (currentPlayerButtons.Contains(button)) {
                                i--;
                                break;
                            }
                            if (otherPlayerButtons.Contains(button)) {
                                playerLife.Value -= (100.0d / 24.0d);
                            }
                            button.IsEnabled = false;
                            button.Visibility = Visibility.Hidden;
                            buttonList.Remove(button);
                            x++;
                        }
                    }
                }
            }catch(Exception) {  }
        }
        private void PowerFive() {
            // reveal two part of enemy's object
            try {
                Button firstButton = new Button();
                for (int i = 0; i < 2; i++) {
                    List<Button> copyPlayerButton = otherPlayerButtons;
                    Button revealButton = copyPlayerButton[random.Next(copyPlayerButton.Count)];
                    copyPlayerButton.Remove(revealButton);
                    foreach (Button button in buttonGrid.Children) {
                        if (firstButton == button) {
                            continue;
                        }
                        if (revealButton == button && button.IsEnabled == true) {
                            button.IsEnabled = false;
                            button.Visibility = Visibility.Hidden;
                            firstButton = button;
#if true
                            playerLife.Value -= (100.0d / 24.0d);
#else
                            playerLife.Value -= (100.0d / 6.0d);
#endif
                            otherPlayerButtons.Remove(button);
                            break;
                        }
                    }
                }
            } catch (Exception) { }
        }
    }
}

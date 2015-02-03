using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Backgammon
{
    /// <summary>
    /// Interaction logic for HowTo.xaml
    /// </summary>
    public partial class HowTo : Window
    {
        int i = 0;

        public HowTo()
        {
            InitializeComponent();
        }

        private void btn_next1(object sender, RoutedEventArgs e)
        {
            txt_1.FontFamily = new System.Windows.Media.FontFamily( "TimesNewRomanBold" );
            txt_1.FontSize = 14;
            i += 1;
            if (i == 1)
            {
                txt_1.Text = ("The object of the game is move all your checkers into your own home board and then bear them off. The first player to bear off all of their checkers wins the game.");
                Uri uri = new Uri(@"Grafik/bild1.gif", UriKind.Relative);
                ImageSource imgSource = new BitmapImage(uri);
                pic_1.Source = imgSource;
                //pic_1.Margin = new Thickness("";)
            }



            if (i == 2)
            {
                txt_1.Text = ("To start the game, each player throws a single dice. This determines both the player to go first and the numbers to be played. If equal numbers come up, then both players roll again until they roll different numbers. The player throwing the higher number now moves his checkers according to the numbers showing on both dice. After the first roll, the players throw two dice and alternate turns.The roll of the dice indicates how many points, or pips, the player is to move his checkers. The checkers are always moved forward, to a lower-numbered point. The following rules apply :A checker may be moved only to an open point, one that is not occupied by two or more opposing checkers. The numbers on the two dice constitute separate moves. For example, if a player rolls 5 and 3, he may move one checker five spaces to an open point and another checker three spaces to an open point, or he may move the one checker a total of eight spaces to an open point, but only if the intermediate point (either three or five spaces from the starting point) is also open.");
                Uri uri = new Uri(@"Grafik/bild2.gif", UriKind.Relative);
                ImageSource imgSource = new BitmapImage(uri);
                pic_1.Source = imgSource;
                //pic_1.Margin = new Thickness("";)
            }

            if (i == 3)
            {
                txt_1.Text = "A point occupied by a single checker of either color is called a blot. If an opposing checker lands on a blot, the blot is hit and placed on the bar.Any time a player has one or more checkers on the bar, his first obligation is to enter those checker(s) into the opposing home board. A checker is entered by moving it to an open point corresponding to one of the numbers on the rolled dice.For example, if a player rolls 4 and 6, he may enter a checker onto either the opponent’s four point or six point, so long as the prospective point is not occupied by two or more of the opponent’s checkers. If neither of the points is open, the player loses his turn. If a player is able to enter some but not all of his checkers, he must enter as many as he can and then forfeit the remainder of his turn.After the last of a player’s checkers has been entered, any unused numbers on the dice must be played, by moving either the checker that was entered or a different checker. ";
                Uri uri = new Uri(@"Grafik/bild3.gif", UriKind.Relative);
                ImageSource imgSource = new BitmapImage(uri);
                pic_1.Source = imgSource;
            }


            if (i == 4)
            {
                txt_1.Text = ("Once a player has moved all of his fifteen checkers into his home board, he may commence bearing off. A player bears off a checker by rolling a number that corresponds to the point on which the checker resides, and then removing that checker from the board. Thus, rolling a 6 permits the player to remove a checker from the six point.If there is no checker on the point indicated by the roll, the player must make a legal move using a checker on a higher-numbered point. If there are no checkers on higher-numbered points, the player is permitted (and required) to remove a checker from the highest point on which one of his checkers resides. A player is under no obligation to bear off if he can make an otherwise legal move. A player must have all of his active checkers in his home board in order to bear off. If a checker is hit during the bear-off process, the player must bring that checker back to his home board before continuing to bear off. The first player to bear off all fifteen checkers wins the game");
                Uri uri = new Uri(@"Grafik/bild4.gif", UriKind.Relative);
                ImageSource imgSource = new BitmapImage(uri);
                pic_1.Source = imgSource;
                pic_1.Width = 400;
                btn_nextSlide.Opacity = 0;
                pic_1.Height = 400;
                btn_gotIt.Opacity = 1;
              

            }

            if (i==5)
            {
                this.Close();
       
            }
        }
    }
}

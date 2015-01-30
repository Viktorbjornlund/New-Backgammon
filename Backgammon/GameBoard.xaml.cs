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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Media.Effects;
using System.Windows.Media.Animation;

namespace Backgammon
{
    public partial class GameBoard : UserControl
    {
        Ellipse _pieceSelected = null;
        Point _posOfMouseOnHit;
        Point _posOfEllipseOnHit;
        Model model = new Model();
        Player black = new Player();
        Player white = new Player();
        Player activePlayer, inactivePlayer;
        BlurEffect blur = new BlurEffect();
        Polygon[] polygons;
        Grid homeGrid;

        private int dice1, dice2, start;
        private int d1, d2, d3;
        int _totalChildren = 0;

        TextBlock numberOfPieces;
        System.Media.SoundPlayer music = new System.Media.SoundPlayer(@"Ljud\musik.wav");
        bool soundOn;

        ImageBrush piece_light = new ImageBrush();
        ImageBrush piece_dark = new ImageBrush();
        ImageBrush polygon_light = new ImageBrush();
        ImageBrush polygon_dark = new ImageBrush();
        ImageBrush home_image = new ImageBrush();

        void Active(Object sender, EventArgs args)
        {
        } // Active

        void InActive(Object sender, EventArgs args)
        {
            theCanvas.Opacity = 1;
            theCanvas.IsEnabled = false;
            theCanvas.IsHitTestVisible = false;
            theCanvas.Effect = blur;
            menuGrid.IsHitTestVisible = true;
            menuGrid.IsEnabled = true;
            menuGrid.Opacity = 1;
            if (_pieceSelected != null)
            {
                Canvas.SetLeft(_pieceSelected, _posOfEllipseOnHit.X);
                Canvas.SetTop(_pieceSelected, _posOfEllipseOnHit.Y);
                _pieceSelected.Effect = null;
                _pieceSelected.Stroke = Brushes.Gray;
                _pieceSelected = null;
            }
        } // DeActive

        public GameBoard()
        {
            App.Current.Activated += Active;
            App.Current.Deactivated += InActive;

            polygon_light.ImageSource = new BitmapImage( new Uri( @"Grafik/metal-light.jpg", UriKind.Relative ) );
            polygon_dark.ImageSource = new BitmapImage( new Uri( @"Grafik/metal-dark.jpg", UriKind.Relative ) );
            piece_light.ImageSource = new BitmapImage( new Uri( @"Grafik/piece-white.jpg", UriKind.Relative ) );
            piece_dark.ImageSource = new BitmapImage( new Uri( @"Grafik/piece-black.jpg", UriKind.Relative ) );
            home_image.ImageSource = new BitmapImage( new Uri( @"Grafik/wood-border2.jpg", UriKind.Relative ) );
            playMusic();
            InitializeComponent();
            menuGrid.Opacity = 1;
            menuGrid.IsEnabled = true;
            theCanvas.Opacity = 1;
            theCanvas.IsEnabled = false;
            theCanvas.Effect = blur;
            DiceView1.Opacity = 0;
            DiceView2.Opacity = 0;
            blackTurnArrow.Opacity = 0;
            whiteTurnArrow.Opacity = 0;
            polygons = new Polygon[] { p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12, p13, p14, p15, p16, p17, p18, p19, p20, p21, p22, p23 }; 
        } // GAMEBOARD

        private void lightup_pieces()
        {
            lightdown_pieces();

            if (activePlayer._out == 0)
            {
                for (int i = 0; i < 24; i++)
                {
                    if (activePlayer._laces[i] > 0 && DiceRoll.IsEnabled == false)
                    {
                        if (activePlayer == black)
                        {
                            d1 = i - dice1;
                            d2 = i - dice2;
                            d3 = i - dice1 - dice2;
                        }
                        else
                        {
                            d1 = i + dice1;
                            d2 = i + dice2;
                            d3 = i + dice1 + dice2;
                        }
                        if ((d1 >= 0 && d1 <= 23 && inactivePlayer._laces[d1] <= 1 && dice1 != 0) || (d2 >= 0 && d2 <= 23 && inactivePlayer._laces[d2] <= 1 && dice2 != 0) || (d3 >= 0 && d3 <= 23 && inactivePlayer._laces[d3] <= 1 && dice1 != 0 && dice2 != 0))
                        {
                            Point p;
                            if (activePlayer._laces[i] > 5)
                            {
                                if (i < 12)
                                    p = new Point(model.pointX[i] + 12, 24 * 5 - 12);
                                else
                                    p = new Point(model.pointX[i] + 12, 320 - 24 * 5 + 12);
                            }
                            else
                            {
                                if (i < 12)
                                    p = new Point(model.pointX[i] + 12, activePlayer._laces[i] * 24 - 12);
                                else
                                    p = new Point(model.pointX[i] + 12, 320 - activePlayer._laces[i] * 24 + 12);
                            }
                            HitTestResult hr = VisualTreeHelper.HitTest(theCanvas, p);
                            Object obj = hr.VisualHit;
                            if (obj is Ellipse)
                            {
                                Ellipse el = (Ellipse)obj;
                                el.Stroke = Brushes.Red;
                            }
                        }
                    }
                }
            }
            else if (activePlayer._out > 0)
            {
                Point p;
                if (activePlayer == black)
                    p = new Point( 195, 240 );
                else
                    p = new Point( 195, 80 );
                HitTestResult hr = VisualTreeHelper.HitTest(theCanvas, p);
                Object obj = hr.VisualHit;
                if(obj is Ellipse )
                {
                    Ellipse el = (Ellipse)obj;
                    el.Stroke = Brushes.Red;
                }
            }
        } // lightup_pieces

        private void lightdown_pieces()
        {
            _totalChildren = theCanvas.Children.Count - 1;
            for (int i = _totalChildren; i > 0; i--)
            {
                if (theCanvas.Children[i].GetType() == typeof( Ellipse ))
                {
                    Ellipse el = (Ellipse)theCanvas.Children[i];
                    el.Stroke = Brushes.Gray;
                }
            }
        } // lightdown_pieces

        private void lightup_polygons( int i )
        {
            if (activePlayer == black)
            {
                d1 = i - dice1;
                d2 = i - dice2;
                d3 = i - dice1 - dice2;
            }
            else
            {
                d1 = i + dice1;
                d2 = i + dice2;
                d3 = i + dice1 + dice2;
            }
            if (d1 >= 0 && d1 <= 23 && dice1 != 0 && inactivePlayer._laces[d1] <= 1)
                polygons[d1].Fill = Brushes.Yellow;
            if (d2 >= 0 && d2 <= 23 && dice2 != 0 && inactivePlayer._laces[d2] <= 1)
                polygons[d2].Fill = Brushes.Yellow;
            if (d3 >= 0 && d3 <= 23 && inactivePlayer._laces[d3] <= 1)
                polygons[d3].Fill = Brushes.Yellow;
            if (activePlayer._goalReady && (d1 == -1 || d1 < -1 || d2 == -1 || d2 < -1 || d3 == -1 || d3 < -1))
                homeGrid.Background = Brushes.Red;
        } // lightup_polygons

        private void lightdown_polygons()
        {
            for (int i = 0; i < 24; i++)
            {
                if (polygons[i].Fill == Brushes.Yellow)
                {
                    if (i % 2 == 0)
                        polygons[i].Fill = polygon_dark;
                    else
                        polygons[i].Fill = polygon_light;
                }
            }
        } // lightdown_polygons

        private void lightup_out()
        {
            bool possibleMove = false;
            if (activePlayer == black)
            {
                d1 = 24 - dice1;
                d2 = 24 - dice2;
                d3 = 24 - dice1 - dice2;
            }
            else
            {
                d1 = dice1 - 1;
                d2 = dice2 - 1;
                d3 = dice1 + dice2 - 1;
            }
            if (d1 >= 0 && d1 <= 23 && dice1 != 0 && inactivePlayer._laces[d1] <= 1)
            {
                polygons[d1].Fill = Brushes.Yellow;
                possibleMove = true;
            }
            if (d2 >= 0 && d2 <= 23 && dice2 != 0 && inactivePlayer._laces[d2] <= 1)
            {
                polygons[d2].Fill = Brushes.Yellow;
                possibleMove = true;
            }
            if (d3 >= 0 && d3 <= 23 && dice1 != 0 && dice2 != 0 && inactivePlayer._laces[d3] <= 1)
            {
                polygons[d3].Fill = Brushes.Yellow;
                possibleMove = true;
            }
            if (!possibleMove)
            {
                changePlayer();
            }
        } // lightup_out

        private void select_piece( Object obj )
        {
            Ellipse el = (Ellipse)obj;
            if (el.Stroke == Brushes.Red)
            {
                _pieceSelected = el; 
                _posOfEllipseOnHit.X = Canvas.GetLeft( _pieceSelected );
                _posOfEllipseOnHit.Y = Canvas.GetTop( _pieceSelected );
                theCanvas.Children.Remove( _pieceSelected );
                theCanvas.Children.Add( _pieceSelected );

                if (activePlayer._out == 0)
                {
                    for (int i = 0; i < 24; i++)
                    {
                        if (i < 12 && _posOfEllipseOnHit.X == model.pointX[i] && _posOfEllipseOnHit.Y < 160)
                        {
                            start = i;
                        }
                        else if (i > 11 && _posOfEllipseOnHit.X == model.pointX[i] && _posOfEllipseOnHit.Y > 160)
                        {
                            start = i;
                        }
                    }
                    lightdown_pieces();
                    _pieceSelected.Stroke = Brushes.Red;
                    lightup_polygons( start );
                }
                else
                {
                    lightup_out();
                }
            }
        } // select_piece

        private void moveToPolygon( int move )
        {
            if (polygons[move].Fill == Brushes.Yellow)
            {
                if (activePlayer._out == 0)
                {
                    activePlayer._laces[start]--;
                    activePlayer._laces[move]++;
                }
                else
                {
                    activePlayer._out--;
                    activePlayer._laces[move]++;
                }

                if (move == d1 && dice1 != 0)
                {
                    dice1 = 0;
                    DiceView1.Effect = blur;
                }
                else if (move == d2 && dice2 != 0)
                {
                    dice2 = 0;
                    DiceView2.Effect = blur;
                }
                else if (move == d3 && dice1 != 0 && dice2 != 0)
                {
                    dice1 = 0;
                    dice2 = 0;
                    DiceView1.Effect = blur;
                    DiceView2.Effect = blur;
                }
                //Canvas.SetLeft( _pieceSelected, model.pointX[move] );
                double spacing = 24.0;

                if (activePlayer._laces[move] > 5)
                {
                    if (move < 12)
                    {
                        //Canvas.SetTop( _pieceSelected, spacing * 4 );
                        MovePiece( _pieceSelected, model.pointX[move], spacing * 4 );
                        
                        //_pieceSelected.SetValue(Canvas.TopProperty, model.pointX[move]);
                    }
                    else
                        //Canvas.SetTop( _pieceSelected, 320 - spacing * 5 );
                        MovePiece( _pieceSelected, model.pointX[move], 320 - spacing * 5 );

                    numberOfPieces = new TextBlock();
                    Panel panel = (Panel)numberOfPieces.Parent;
                    theCanvas.Children.Remove( numberOfPieces );
                    theCanvas.Children.Add( numberOfPieces );
                    numberOfPieces.Height = 24;
                    numberOfPieces.Width = 24;
                    numberOfPieces.TextAlignment = TextAlignment.Center;
                    numberOfPieces.IsHitTestVisible = false;
                    numberOfPieces.Foreground = Brushes.Red;
                    numberOfPieces.Text = activePlayer._laces[move].ToString();
                    Canvas.SetLeft( numberOfPieces, Canvas.GetLeft( _pieceSelected ) );
                    Canvas.SetTop( numberOfPieces, Canvas.GetTop( _pieceSelected ) );
                    
                }
                else
                {
                    if (move < 12)
                    {
                        //Canvas.SetTop( _pieceSelected, spacing * (activePlayer._laces[move] - 1) );
                        MovePiece( _pieceSelected, model.pointX[move], spacing * (activePlayer._laces[move] - 1) );
                    }
                    else
                        //Canvas.SetTop( _pieceSelected, 320 - spacing * activePlayer._laces[move] );
                        MovePiece( _pieceSelected, model.pointX[move], 320 - spacing * activePlayer._laces[move] );
                }
                homeGrid.Background = home_image;
            }
        } // moveToPolygon

        private void putOut( int i )
        {
            if (black._laces[i] == 1 && white._laces[i] == 1)
            {
                _totalChildren = theCanvas.Children.Count - 1;
                for (int j = _totalChildren; j > 0; j--)
                {
                    if (theCanvas.Children[j].GetType() == typeof( Ellipse ))
                    {
                        Ellipse el = (Ellipse)theCanvas.Children[j];
                        if (i < 12 && Canvas.GetLeft( el ) == model.pointX[i] && Canvas.GetTop( el ) == 0)
                        {
                            if (activePlayer == black && el.Fill == piece_light)
                            {
                                //Canvas.SetLeft( el, 183 );
                                //Canvas.SetTop( el, 228 );
                                MovePiece( el, 183, 228 );
                                white._laces[i]--;
                                white._out++;
                                el.Stroke = Brushes.Gray;
                            }
                            else if (activePlayer == white && el.Fill == piece_dark)
                            {
                                //Canvas.SetLeft( el, 183 );
                                //Canvas.SetTop( el, 68 );
                                MovePiece( el, 183, 68 );
                                black._laces[i]--;
                                black._out++;
                                el.Stroke = Brushes.Gray;
                            }
                        }
                        else if (i > 11 && Canvas.GetLeft( el ) == model.pointX[i] && Canvas.GetTop( el ) == 296)
                        {
                            if (activePlayer == black && el.Fill == piece_light)
                            {
                                //Canvas.SetLeft( el, 183 );
                                //Canvas.SetTop( el, 228 );
                                MovePiece( el, 183, 228 );
                                white._laces[i]--;
                                white._out++;
                                el.Stroke = Brushes.Gray;
                            }
                            else if (activePlayer == white && el.Fill == piece_dark)
                            {
                                //Canvas.SetLeft( el, 183 );
                                //Canvas.SetTop( el, 68 );
                                MovePiece( el, 183, 68 );
                                black._laces[i]--;
                                black._out++;
                                el.Stroke = Brushes.Gray;
                            }
                        }
                    }
                }
            }
        } // putOut

        private void moveHome()
        {
            if (blackHome.Background == Brushes.Red || whiteHome.Background == Brushes.Red)
            {
                int distance = 0;
                if (activePlayer == black)
                    distance = start - (-1);
                else
                    distance = 24 - start;
                if (distance <= dice1 + dice2 && distance > dice1 && distance > dice2)
                {
                    dice1 = 0;
                    dice2 = 0;
                    DiceView1.Effect = blur;
                    DiceView2.Effect = blur;
                }
                if (dice1 != dice2)
                {
                    if (distance == dice1)
                    {
                        dice1 = 0;
                        DiceView1.Effect = blur;
                    }
                    if (distance == dice2)
                    {
                        dice2 = 0;
                        DiceView2.Effect = blur;
                    }

                    if (distance < dice1 && distance < dice2)
                    {
                        if (dice1 > dice2)
                        {
                            dice1 = 0;
                            DiceView1.Effect = blur;
                        }
                        else
                        {
                            dice2 = 0;
                            DiceView2.Effect = blur;
                        }
                    }
                }
                else
                {
                    dice1 = 0;
                    DiceView1.Effect = blur;
                }
                activePlayer._laces[start]--;
                activePlayer._bricksAmount--;
            }
        } // moveHome

        private bool moveToFinish()
        {
            int[] pos;
            if (activePlayer == black)
            {
                pos = new int[] { 0, 1, 2, 3, 4, 5 };
            }
            else
            {
                pos = new int[] { 23, 22, 21, 20, 19, 18 };
            }
            for (int i = 0; i < 24; i++)
            {
                if (activePlayer._laces[i] != 0 && i != pos[0] && i != pos[1] && i != pos[2] && i != pos[3] && i != pos[4] && i != pos[5])
                {
                    return false;
                }
            }
            return true;
        } // moveToFinish

        private void Canvas_MouseDown_1( object sender, MouseButtonEventArgs e )
        {
            Point pt = e.GetPosition( theCanvas );
            
            if (moveToFinish())
                activePlayer._goalReady = true;
            else
                activePlayer._goalReady = false;

            if (_pieceSelected == null)
            {
                start = 0;
                HitTestResult hr = VisualTreeHelper.HitTest( theCanvas, pt );
                Object obj = hr.VisualHit;

                if (obj is Ellipse)
                {
                    select_piece( obj );
                }
            }
            else
            {
                int move = 0;
                _posOfMouseOnHit = pt;
                for (int i = 0; i < 24; i++)
                {
                    if (i < 12 && _posOfMouseOnHit.X >= model.pointX[i] - 3 && _posOfMouseOnHit.X < model.pointX[i] + 27 && _posOfMouseOnHit.Y < 160)
                    {
                        move = i;
                    }
                    else if (i > 11 && _posOfMouseOnHit.X >= model.pointX[i] - 3 && _posOfMouseOnHit.X < model.pointX[i] + 27 && _posOfMouseOnHit.Y > 160)
                    {
                        move = i;
                    }
                }
                moveToPolygon( move );
                putOut(move);
                moveHome();
                homeGrid.Background = home_image;

                if(activePlayer._bricksAmount==0)
                {
                //win
                }

                if (dice1 == 0 && dice2 == 0)
                {
                    changePlayer();
                }
                theCanvas.Children.Remove( _pieceSelected );
                theCanvas.Children.Add( _pieceSelected );
                _pieceSelected.Stroke = Brushes.Gray;
                

                lightdown_polygons();
                lightdown_pieces();
                lightup_pieces();
                _pieceSelected = null;
            }
        } // Mouse Down

        private void diceRoll(object sender, RoutedEventArgs e)
        {
            dice1 = model.dice();
            dice2 = model.dice();
            DiceView1.Source = new BitmapImage(new Uri(@"Grafik\Dice" + dice1.ToString() + ".png", UriKind.Relative));
            DiceView2.Source = new BitmapImage(new Uri(@"Grafik\Dice" + dice2.ToString() + ".png", UriKind.Relative));
            DiceView1.Opacity = 1;
            DiceView2.Opacity = 1;
            DiceView1.Effect = null;
            DiceView2.Effect = null;
            DiceRoll.Opacity = 0;
            DiceRoll.IsEnabled = false;
            lightup_pieces();
            //System.Media.SoundPlayer player = new System.Media.SoundPlayer(@"Ljud\roll.wav");
            //player.Play();
        } // diceRoll

        private void changePlayer()
        {
            if (activePlayer == white)
            {
                activePlayer = black;
                inactivePlayer = white;
                blackTurnArrow.Opacity = 1;
                whiteTurnArrow.Opacity = 0;
                homeGrid = blackHome;
            }
            else
            {
                activePlayer = white;
                inactivePlayer = black;
                blackTurnArrow.Opacity = 0;
                whiteTurnArrow.Opacity = 1;
                homeGrid = whiteHome;
            }
            dice1 = 0;
            dice2 = 0;
            DiceRoll.IsEnabled = true;
            DiceRoll.Opacity = 1;
            DiceView1.Opacity = 0;
            DiceView2.Opacity = 0;
        } // changePlayer

        private Ellipse load_piece()
        {
            Ellipse _piece = new Ellipse();
            _piece.Height = 24;
            _piece.Width = 24;
            _piece.Stroke = Brushes.Gray;
            theCanvas.Children.Add(_piece);
            Canvas.SetLeft(_piece, 0);
            Canvas.SetTop(_piece, 0);
            return _piece;
        } // load_piece

        private void insert_pieces()
        {
            int rad = 24;
            for (int i = 0; i < 15; i++)
            {               
                Ellipse _piece = load_piece();
                _piece.Fill = piece_light;
                if (i < 5)
                {
                    Canvas.SetLeft(_piece, 3);
                    Canvas.SetTop(_piece, rad * (i % 5));
                    white._laces[11] = 5;
                }
                else if (i < 7)
                {
                    Canvas.SetLeft(_piece, 363);
                    Canvas.SetTop(_piece, rad * (i % 5));
                    white._laces[0] = 2;
                }
                else if (i < 10)
                {
                    Canvas.SetLeft(_piece, 123);
                    Canvas.SetTop(_piece, 248 + (rad * (i % 7)));
                    white._laces[16] = 3;
                }
                else
                {
                    Canvas.SetLeft(_piece, 213);
                    Canvas.SetTop(_piece, 200 + (rad * (i % 5)));
                    white._laces[18] = 5;
                }
            }
            for (int i = 0; i < 15; i++)
            {
                Ellipse _piece = load_piece();
                _piece.Fill = piece_dark;
                if (i < 5)
                {
                    Canvas.SetLeft(_piece, 3);
                    Canvas.SetTop(_piece, 200 + (rad * (i % 5)));
                    black._laces[12] = 5;
                }
                else if (i < 7)
                {
                    Canvas.SetLeft(_piece, 363);
                    Canvas.SetTop(_piece, 272 + (rad * (i % 5)));
                    black._laces[23] = 2;
                }
                else if (i < 10)
                {
                    Canvas.SetLeft(_piece, 123);
                    Canvas.SetTop(_piece, rad * (i % 7));
                    black._laces[7] = 3;
                }
                else
                {
                    Canvas.SetLeft(_piece, 213);
                    Canvas.SetTop(_piece, rad * (i % 5));
                    black._laces[5] = 5;
                }
            }
        } // insert_pieces

        private void remove_pieces()
        {
            _totalChildren = theCanvas.Children.Count - 1;
            for (int i = _totalChildren; i > 0; i--)
            {
                if (theCanvas.Children[i].GetType() == typeof(Ellipse))
                {
                    Ellipse _piece = (Ellipse)theCanvas.Children[i];
                    theCanvas.Children.Remove(_piece);
                }
            }
            for (int i = 0; i < 24; i++)
            {
                black._laces[i] = 0;
                white._laces[i] = 0;
            }
        } // remove_pieces

        private void resume_game( object sender, RoutedEventArgs e )
        {
            bool noEllipse = true;
            int _totalChildren = theCanvas.Children.Count - 1;
            for (int i = _totalChildren; i > 0; i--)
            {
                if (theCanvas.Children[i].GetType() == typeof(Ellipse))
                {
                    noEllipse = false;
                    break;
                }
            }

            if (!noEllipse)
            {
                menuGrid.IsHitTestVisible = false;
                menuGrid.IsEnabled = false;
                menuGrid.Opacity = 0;
                theCanvas.Opacity = 1;
                theCanvas.IsEnabled = true;
                theCanvas.IsHitTestVisible = true;
                theCanvas.Effect = null;
            }
        } // resume_game

        private void new_game(object sender, RoutedEventArgs e)
        {
            menuGrid.IsHitTestVisible = false;
            menuGrid.IsEnabled = false;
            menuGrid.Opacity = 0;
            theCanvas.Opacity = 1;
            theCanvas.IsEnabled = true;
            theCanvas.IsHitTestVisible = true;
            theCanvas.Effect = null;

            DiceRoll.IsEnabled = true;
            DiceRoll.Opacity = 1;
            DiceView1.Opacity = 0;
            DiceView2.Opacity = 0;
            dice1 = 0;
            dice2 = 0;

            blackTurnArrow.Opacity = 1;
            whiteTurnArrow.Opacity = 0;

            lightdown_polygons();

            remove_pieces();
            insert_pieces();
            activePlayer = black;
            inactivePlayer = white;
            homeGrid = blackHome;
            start = 0;
        } // new_game

        private void exit_game(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        } // exit_game

        private void menu_action(object sender, RoutedEventArgs e)
        {
            theCanvas.Opacity = 1;
            theCanvas.IsEnabled = false;
            theCanvas.IsHitTestVisible = false;
            theCanvas.Effect = blur;
            menuGrid.IsHitTestVisible = true;
            menuGrid.IsEnabled = true;
            menuGrid.Opacity = 1;
        } // menu_action

        private void MovePiece( Ellipse el, double newX, double newY )
        {
            double top = Canvas.GetTop( el );
            double left = Canvas.GetLeft( el );
            var SB = new Storyboard
            {
                Duration = TimeSpan.FromSeconds(1.5)
            };
            DoubleAnimation anim1 = new DoubleAnimation( left, newX, TimeSpan.FromSeconds( 1.5 ) );
            DoubleAnimation anim2 = new DoubleAnimation( top, newY, TimeSpan.FromSeconds( 1.5 ) );
            Storyboard.SetTarget(anim1, el);
            Storyboard.SetTargetProperty(anim1, new PropertyPath(Canvas.LeftProperty));
            SB.Children.Add(anim1);
            Storyboard.SetTarget(anim2, el);
            Storyboard.SetTargetProperty(anim2, new PropertyPath(Canvas.TopProperty));
            SB.Children.Add(anim2);
            el.BeginStoryboard( SB );
        } // MovePiece Animation

        private void playMusic()
        {
            music.PlayLooping();
            soundOn = true;
        }
        private void btn_Off(object sender, RoutedEventArgs e)
        {
            if (soundOn == true)
            {
                Image img = new Image();
                Uri uri = new Uri(@"Grafik/sound.png", UriKind.Relative);
                ImageSource imgSource = new BitmapImage(uri);
                img.Source = imgSource;
                Btn_sound.Content = img;
                music.Stop();
                soundOn = false;
            }
            else
            {
                Image img1 = new Image();
                music.PlayLooping();
                Uri uri = new Uri(@"Grafik/noSound.png", UriKind.Relative);
                ImageSource imgSource = new BitmapImage(uri);
                img1.Source = imgSource;
                Btn_sound.Content = img1;
                soundOn = true;
            }
        }
        private void Mouse_leave_Menu(object sender, MouseEventArgs e)
        {
            DoubleAnimation Anim = new DoubleAnimation(0.2, TimeSpan.FromSeconds(2));
            menu_button.BeginAnimation(Button.OpacityProperty, Anim);
        }
        private void Mouse_enter_menu(object sender, MouseEventArgs e)
        {
            DoubleAnimation Anim = new DoubleAnimation(1, TimeSpan.FromSeconds(2));
            menu_button.BeginAnimation(Button.OpacityProperty, Anim);
        }
        }
    }




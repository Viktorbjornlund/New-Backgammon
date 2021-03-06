﻿using System;
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

        Storyboard SB;
        Storyboard story;

        private int dice1, dice2, start;
        private int d1, d2, d3;
        int _totalChildren = 0;

        System.Media.SoundPlayer music = new System.Media.SoundPlayer(@"Ljud\musik.wav");
        bool soundOn;

        ImageBrush piece_light = new ImageBrush();
        ImageBrush piece_dark = new ImageBrush();
        ImageBrush polygon_light = new ImageBrush();
        ImageBrush polygon_dark = new ImageBrush();
        ImageBrush home_image = new ImageBrush();
        DropShadowEffect SE = new DropShadowEffect();
        DropShadowEffect SE_light = new DropShadowEffect();

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

        void MoveFinished( Object sender, EventArgs args )
        {
            lightup_pieces();
        }

        public GameBoard()
        {
            App.Current.Activated += Active;
            App.Current.Deactivated += InActive;
            polygon_light.ImageSource = new BitmapImage( new Uri( @"Grafik/metal-light.jpg", UriKind.Relative ) );
            polygon_dark.ImageSource = new BitmapImage( new Uri( @"Grafik/metal-dark.jpg", UriKind.Relative ) );
            piece_light.ImageSource = new BitmapImage( new Uri( @"Grafik/piece-white.jpg", UriKind.Relative ) );
            piece_dark.ImageSource = new BitmapImage( new Uri( @"Grafik/piece-black.png", UriKind.Relative ) );
            home_image.ImageSource = new BitmapImage( new Uri( @"Grafik/metal-surface.jpg", UriKind.Relative ) );
            SB = new Storyboard();
            story = new Storyboard();
            SE.BlurRadius = 3;
            SE.ShadowDepth = 0;
            SE_light.BlurRadius = 25;
            SE_light.ShadowDepth = 0;
            SE_light.Color = Color.FromRgb( 255, 255, 235);
            playMusic();
            
            InitializeComponent();
            SB.Completed += new EventHandler( MoveFinished );
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

        // lyser upp de brickor som kan flyttas
        private void lightup_pieces()
        {
            lightdown_pieces();

            if (moveToFinish())
                activePlayer._goalReady = true;
            else
                activePlayer._goalReady = false;

            if (activePlayer._out == 0 && DiceRoll.IsEnabled == false)
            {
                for (int i = 0; i < 24; i++)
                {
                    if (activePlayer._laces[i] > 0)
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
                        if ((d1 >= 0 && d1 <= 23 && inactivePlayer._laces[d1] <= 1 && dice1 != 0) || (d2 >= 0 && d2 <= 23 && inactivePlayer._laces[d2] <= 1 && dice2 != 0) || (activePlayer._goalReady == true && ((d1 < 0 || d2 < 0 || d3 < 0) || (d1 > 23 || d2 > 23 || d3 > 23))))
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
                                el.Effect = SE_light;
                                el.Stroke = Brushes.PaleGreen;
                            }
                        }
                    }
                }
            }
            else if (activePlayer._out > 0 && DiceRoll.IsEnabled == false && lightup_out_possible() == true && activePlayer._goalReady == false)
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
                    el.Effect = SE_light;
                    el.Stroke = Brushes.PaleGreen;
                }
            }
            else if (activePlayer._out > 0 && DiceRoll.IsEnabled == false && lightup_out_possible() == false)
            {
                changePlayer();
            }
        } // lightup_pieces

        // släcker alla brickor
        private void lightdown_pieces()
        {
            _totalChildren = theCanvas.Children.Count - 1;
            for (int i = _totalChildren; i > 0; i--)
            {
                if (theCanvas.Children[i].GetType() == typeof( Ellipse ))
                {
                    Ellipse el = (Ellipse)theCanvas.Children[i];
                    el.Effect = SE;
                    el.Stroke = Brushes.Black;
                }
            }
        } // lightdown_pieces

        // lyser upp de trianglar som den markerade brickan kan flyttas till
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
            {
                polygons[d1].Effect = SE_light;
                polygons[d1].Stroke = Brushes.PaleGreen;
                //polygonPulse( polygons[d1] );
            }
            if (d2 >= 0 && d2 <= 23 && dice2 != 0 && inactivePlayer._laces[d2] <= 1)
            {
                polygons[d2].Effect = SE_light;
                polygons[d2].Stroke = Brushes.PaleGreen;
                //polygonPulse( polygons[d2] );
            }
            if (d3 >= 0 && d3 <= 23 && inactivePlayer._laces[d3] <= 1)
            {
                polygons[d3].Effect = SE_light;
                polygons[d3].Stroke = Brushes.PaleGreen;
                //polygonPulse( polygons[d3] );
            }
            // Om jämnt tal till mål med antincen dice1, dice2 eller dice1+2 lys rött. 
            if (activePlayer._goalReady == true && ((d1 == -1 || d2 == -1 || d3 == -1) || (d1 == 24 || d2 == 24 || d3 == 24)))
            {
                homeGrid.Effect = SE_light;
            }
            // om jämnt eller högre tal till mål med antingen dice1, dice2 eller dice1+2 OCH inga gula polynom 
            if (activePlayer._goalReady == true && ((d1 < -1 || d2 < -1 || d3 < -1) || (d1 > 24 || d2 > 24 || d3 > 24)) && !isYellow())
                homeGrid.Effect = SE_light;
        } // lightup_polygons

        private bool isYellow()
        {
            for (int i = 0; i < 24; i++)
            {
                if (polygons[i].Effect == SE_light)
                {
                    return true;
                }
            }
            return false;
        }

        // släcker alla polygoner
        private void lightdown_polygons()
        {
            for (int i = 0; i < 24; i++)
            {
                if (polygons[i].Effect == SE_light)
                {
                    if (i % 2 == 0)
                    {
                        polygons[i].Effect = null;
                        polygons[i].Stroke = null;
                    }
                    else
                    {
                        polygons[i].Effect = null;
                        polygons[i].Stroke = null;
                    }
                }
            }
            homeGrid.Effect = SE;
        } // lightdown_polygons

        // lyser upp de trianglar som den utspelade brickan kan flyttas till
        private void lightup_out()
        {
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
                polygons[d1].Effect = SE_light;
                polygons[d1].Stroke = Brushes.PaleGreen;
            }
            if (d2 >= 0 && d2 <= 23 && dice2 != 0 && inactivePlayer._laces[d2] <= 1)
            {
                polygons[d2].Effect = SE_light;
                polygons[d2].Stroke = Brushes.PaleGreen;
            }
            if (d3 >= 0 && d3 <= 23 && dice1 != 0 && dice2 != 0 && inactivePlayer._laces[d3] <= 1)
            {
                polygons[d3].Effect = SE_light;
                polygons[d3].Stroke = Brushes.PaleGreen;
            }
        } // lightup_out

        // kontrollerar om spelaren kan flytta en utslagen bricka
        private bool lightup_out_possible()
        {
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
                return true;
            }
            if (d2 >= 0 && d2 <= 23 && dice2 != 0 && inactivePlayer._laces[d2] <= 1)
            {
                return true;
            }

            //OBS
            //if (d3 >= 0 && d3 <= 23 && dice1 != 0 && dice2 != 0 && inactivePlayer._laces[d3] <= 1)
            //{
            //    return true;
            //}
            return false;
        } // lightup_out_possible

        // markerar en upplyst bricka
        private void select_piece( Object obj )
        {
            Ellipse el = (Ellipse)obj;
            if (el.Effect == SE_light)
            {
                _pieceSelected = el; 
                _posOfEllipseOnHit.X = Math.Round(Canvas.GetLeft( _pieceSelected ));
                _posOfEllipseOnHit.Y = Math.Round(Canvas.GetTop( _pieceSelected ));
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
                    _pieceSelected.Effect = SE_light;
                    _pieceSelected.Stroke = Brushes.PaleGreen;
                    lightup_polygons( start );
                }
                else
                {
                    lightup_out();
                }
            }
        } // select_piece

        // flyttar brickan till en triangel
        private void moveToPolygon( int move )
        {
            if (polygons[move].Effect == SE_light)
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

                    if (activePlayer == black)
                    {
                        if (activePlayer._out > 1)
                            setNumber( 183, 252, activePlayer._out );
                        else
                            removeNumber( 183, 252 );
                    }
                    else
                    {
                        if (activePlayer._out > 1)
                            setNumber( 183, 92, activePlayer._out );
                        else
                            removeNumber( 183, 92 );
                    }
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
                if (move == d3 && dice1 != 0 && dice2 != 0)
                {
                    dice1 = 0;
                    dice2 = 0;
                    DiceView1.Effect = blur;
                    DiceView2.Effect = blur;
                }
                double spacing = 24.0;
                if (activePlayer._laces[move] > 5)
                {
                    if (move < 12)
                    {
                        MovePiece( _pieceSelected, model.pointX[move], spacing * 4 );
                        setNumber( model.pointX[move], 120, activePlayer._laces[move] );
                    }
                    else
                    {
                        MovePiece( _pieceSelected, model.pointX[move], 320 - spacing * 5 );
                        setNumber( model.pointX[move], 170, activePlayer._laces[move] );
                    }
                }
                else
                {
                    if (move < 12)
                    {
                        MovePiece( _pieceSelected, model.pointX[move], spacing * (activePlayer._laces[move] - 1) );
                    }
                    else
                    {
                        MovePiece( _pieceSelected, model.pointX[move], 320 - spacing * activePlayer._laces[move] );
                    }
                }
                if (activePlayer._laces[start] > 5)
                {
                    if (start < 12)
                    {
                        setNumber( model.pointX[start], 120, activePlayer._laces[start] );
                    }
                    else
                    {
                        setNumber( model.pointX[start], 170, activePlayer._laces[start] );
                    }
                }
                else if (activePlayer._laces[start] == 5)
                {
                    if (start < 12)
                    {
                        removeNumber( model.pointX[start], 120 );
                    }
                    else
                    {
                        removeNumber( model.pointX[start], 170 );
                    }
                }
                homeGrid.Effect = SE;
            }
            else
            {
                lightup_pieces();
            }
        } // moveToPolygon

        // lägger ut ett nummer på planen
        private void setNumber(double newX, double newY, int i)
        {
            removeNumber( newX, newY );
            TextBlock numberOfPieces = new TextBlock();
            numberOfPieces.Height = 24;
            numberOfPieces.Width = 24;
            numberOfPieces.FontSize = 20;
            numberOfPieces.TextAlignment = TextAlignment.Center;
            numberOfPieces.Foreground = Brushes.Yellow;
            numberOfPieces.Effect = SE;
            numberOfPieces.Text = i.ToString();
            Canvas.SetLeft( numberOfPieces, newX );
            Canvas.SetTop( numberOfPieces, newY );
            theCanvas.Children.Remove( numberOfPieces );
            theCanvas.Children.Add( numberOfPieces );
        } //setNumber

        // tar bort ett nummer på planen
        private void removeNumber( double oldX, double oldY)
        {
            _totalChildren = theCanvas.Children.Count - 1;
            for (int j = _totalChildren; j > 0; j--)
            {
                if (theCanvas.Children[j].GetType() == typeof( TextBlock ))
                {
                    TextBlock tb = (TextBlock)theCanvas.Children[j];
                    if (oldX == Canvas.GetLeft( tb ) && oldY == Canvas.GetTop( tb ))
                    {
                        theCanvas.Children.Remove( tb );
                    }
                }
            }
        } // removeNumber

        // flyttar utslagen bricka
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
                                MovePiece( el, 183, 68);
                                white._laces[i]--;
                                white._out++;
                            }
                            else if (activePlayer == white && el.Fill == piece_dark)
                            {
                                MovePiece( el, 183, 228);
                                black._laces[i]--;
                                black._out++;
                            }
                        }
                        else if (i > 11 && Canvas.GetLeft( el ) == model.pointX[i] && Canvas.GetTop( el ) == 296)
                        {
                            if (activePlayer == black && el.Fill == piece_light)
                            {
                                MovePiece( el, 183, 68);
                                white._laces[i]--;
                                white._out++;
                            }
                            else if (activePlayer == white && el.Fill == piece_dark)
                            {
                                MovePiece( el, 183, 228);
                                black._laces[i]--;
                                black._out++;
                            }
                        }
                    }
                }
            }
            if (inactivePlayer._out > 1)
            {
                if (inactivePlayer == black)
                {
                    setNumber( 183, 252, inactivePlayer._out );
                }
                else
                {
                    setNumber( 183, 92, inactivePlayer._out );
                }
            }
        } // putOut

        private void moveHome()
        {
            int distance = 0;
            double homeX = 428;
            double homeY = 0;
            if (activePlayer == black)
            {
                distance = start - (-1);
                homeY = 58;
            }
            else
            {
                distance = 24 - start;
                homeY = 238;
            }
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
            MovePiece( _pieceSelected, homeX, homeY );
            setNumber( homeX, homeY + 24, 15 - activePlayer._bricksAmount );
            if (black._bricksAmount == 0)
            {
                MessageBoxResult result = MessageBox.Show( "Congratulations black player, you won!", "Congratulations", MessageBoxButton.OK );
                if (result == MessageBoxResult.OK)
                {
                    Application.Current.Shutdown();
                }
            }
            else if (white._bricksAmount == 0)
            {
                MessageBoxResult result = MessageBox.Show( "Congratulations white player, you won!", "Congratulations", MessageBoxButton.OK );
                if (result == MessageBoxResult.OK)
                {
                    Application.Current.Shutdown();
                }
            }
        } // moveHome

        // kontrollerar om spelaren är redo att flytta in i mål
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

        // musklick
        private void Canvas_MouseDown_1( object sender, MouseButtonEventArgs e )
        {
            Point pt = e.GetPosition( theCanvas );

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
                int topY = 0;
                int bottomY = 0;
                if (activePlayer == black)
                {
                    topY = 10;
                    bottomY = 130;
                }
                else
                {
                    topY = 190;
                    bottomY = 310;
                }
                if (_posOfMouseOnHit.X > 415 && _posOfMouseOnHit.X < 465 && _posOfMouseOnHit.Y > topY && _posOfMouseOnHit.Y < bottomY && homeGrid.Effect == SE_light)
                {
                    moveHome();
                }
                else
                {
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
                    putOut( move );
                    
                }
                homeGrid.Effect = SE;

                if(activePlayer._bricksAmount==0)
                {
                //win
                }

                if (dice1 == 0 && dice2 == 0)
                {
                    changePlayer();
                }
                _pieceSelected = null;
                lightdown_polygons();
                lightup_pieces();
            }
            
        } // Mouse Down

        // slår tärningarna
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

        // byter spelare
        private void changePlayer()
        {
            if (activePlayer == white)
            {
                activePlayer = black;
                inactivePlayer = white;
                blackTurnArrow.Opacity = 1;
                whiteTurnArrow.Opacity = 0;
                story.Stop();
                homeGrid.Background = home_image;
                homeGrid = blackHome;
                homeGrid.Background = Brushes.PaleGreen;
            }
            else
            {
                activePlayer = white;
                inactivePlayer = black;
                blackTurnArrow.Opacity = 0;
                whiteTurnArrow.Opacity = 1;
                story.Stop();
                homeGrid.Background = home_image;
                homeGrid = whiteHome;
                homeGrid.Background = Brushes.PaleGreen;
            }
            dice1 = 0;
            dice2 = 0;
            DiceRoll.IsEnabled = true;
            DiceRoll.Opacity = 1;
            DiceView1.Opacity = 0;
            DiceView2.Opacity = 0;
        } // changePlayer

        // skapar en bricka
        private Ellipse load_piece()
        {
            Ellipse _piece = new Ellipse();
            _piece.Height = 24;
            _piece.Width = 24;
            _piece.StrokeThickness = 0.5;
            _piece.Stroke = Brushes.Black; 
            _piece.Effect = SE;
            theCanvas.Children.Add(_piece);
            Canvas.SetLeft(_piece, 0);
            Canvas.SetTop(_piece, 0);
            return _piece;
        } // load_piece

        // lägger ut alla brickor på spelplanen
        private void insert_pieces()
        {
            int rad = 24;
            for (int i = 0; i < 15; i++)
            {               
                Ellipse el = load_piece();
                el.Fill = piece_light;
                if (i < 5)
                {
                    Layout( el, 3, rad * (i % 5));
                    white._laces[11] = 5;
                }
                else if (i < 7)
                {
                    Layout( el, 363, rad * (i % 5));
                    white._laces[0] = 2;
                }
                else if (i < 10)
                {
                    Layout( el, 123, 248 + (rad * (i % 7)));
                    white._laces[16] = 3;
                }
                else
                {
                    Layout( el, 213, 200 + (rad * (i % 5)));
                    white._laces[18] = 5;
                }
            }
            for (int i = 0; i < 15; i++)
            {
                Ellipse el = load_piece();
                el.Fill = piece_dark;
                if (i < 5)
                {
                    Layout( el, 3, 200 + (rad * (i % 5)));
                    black._laces[12] = 5;
                }
                else if (i < 7)
                {
                    Layout( el, 363, 272 + (rad * (i % 5)));
                    black._laces[23] = 2;
                }
                else if (i < 10)
                {
                    Layout( el, 123, rad * (i % 7));
                    black._laces[7] = 3;
                }
                else
                {
                    Layout( el, 213, rad * (i % 5));
                    black._laces[5] = 5;
                }
            }
        } // insert_pieces

        // tar bort alla brickor och siffror
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
                else if (theCanvas.Children[i].GetType() == typeof( TextBlock ))
                {
                    TextBlock tb = (TextBlock)theCanvas.Children[i];
                    theCanvas.Children.Remove( tb );
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
            start = 0;

            blackTurnArrow.Opacity = 1;
            whiteTurnArrow.Opacity = 0;

            black._out = 0;
            white._out = 0;
            black._goalReady = false;
            white._goalReady = false;
            black._bricksAmount = 15;
            white._bricksAmount = 15;
            activePlayer = black;
            inactivePlayer = white;
            homeGrid = blackHome;
            homeGrid.Background = Brushes.PaleGreen;

            lightdown_polygons();
            remove_pieces();
            insert_pieces();
            blackHome.Effect = SE;
            whiteHome.Effect = SE;
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

        // animerar ut brickorna på planen vid start
        private void Layout( Ellipse el, double newX, double newY )
        {
            double top = Canvas.GetTop( el );
            double left = Canvas.GetLeft( el );
            Storyboard layout = new Storyboard();

            DoubleAnimation anim1 = new DoubleAnimation( left, newX, TimeSpan.FromSeconds( 1 ) );
            DoubleAnimation anim2 = new DoubleAnimation( top, newY, TimeSpan.FromSeconds( 1 ) );

            ElasticEase ease = new ElasticEase();
            ease.EasingMode = EasingMode.EaseOut;
            ease.Oscillations = 3;
            ease.Springiness = 15;
            anim2.Duration = TimeSpan.FromSeconds( 0 );
            anim1.Duration = TimeSpan.FromSeconds( 2 );
            anim1.EasingFunction = ease;

            Storyboard.SetTarget( anim1, el );
            Storyboard.SetTargetProperty( anim1, new PropertyPath( Canvas.LeftProperty ) );
            layout.Children.Add( anim1 );
            Storyboard.SetTarget( anim2, el );
            Storyboard.SetTargetProperty( anim2, new PropertyPath( Canvas.TopProperty ) );
            layout.Children.Add( anim2 );
            layout.Begin();
        }

        // animerar flytt av bricka
        private void MovePiece( Ellipse el, double newX, double newY)
        {
            double top = Canvas.GetTop( el );
            double left = Canvas.GetLeft( el );
            SB.Children.Clear();
            SB.Duration = TimeSpan.FromSeconds( 1 );
            DoubleAnimation anim1 = new DoubleAnimation( left, newX, TimeSpan.FromSeconds( 1 ) );
            DoubleAnimation anim2 = new DoubleAnimation( top, newY, TimeSpan.FromSeconds( 1 ) );
            Storyboard.SetTarget(anim1, el);
            Storyboard.SetTargetProperty(anim1, new PropertyPath(Canvas.LeftProperty));
            SB.Children.Add(anim1);
            Storyboard.SetTarget(anim2, el);
            Storyboard.SetTargetProperty(anim2, new PropertyPath(Canvas.TopProperty));
            SB.Children.Add(anim2);
            SB.Begin();
        } // MovePiece Animation

        //private void Pulse( Grid grid )
        //{
        //    DoubleAnimation anim1 = new DoubleAnimation( 1, 0.2, TimeSpan.FromSeconds( 0.5 ) );
        //    //DoubleAnimation anim2 = new DoubleAnimation( 1, 0, TimeSpan.FromSeconds( 0.5 ) );

        //    story.Children.Clear();
        //    story.Duration = TimeSpan.FromSeconds( 1 );
        //    story.RepeatBehavior = RepeatBehavior.Forever;
        //    anim1.AutoReverse = true;
        //    Storyboard.SetTarget( anim1, grid);
        //    Storyboard.SetTargetProperty( anim1, new PropertyPath( Grid.OpacityProperty ) );
        //    story.Children.Add( anim1 );
        //    //Storyboard.SetTarget( anim2, poly );
        //    //Storyboard.SetTargetProperty( anim2, new PropertyPath( Polygon.OpacityProperty ) );
        //    //story.Children.Add( anim2 );
        //    story.Begin();
        //}

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
            DoubleAnimation Anim = new DoubleAnimation(0.4, TimeSpan.FromSeconds(2));
            menu_button.BeginAnimation(Button.OpacityProperty, Anim);
        }
        private void Mouse_enter_menu(object sender, MouseEventArgs e)
        {
            DoubleAnimation Anim = new DoubleAnimation(1, TimeSpan.FromSeconds(1));
            menu_button.BeginAnimation(Button.OpacityProperty, Anim);
        }

        private void HowToPlay(object sender, RoutedEventArgs e)
        {
            HowTo op = new HowTo();
            
            op.Show();
            op.Left = 270;
            op.Top = 150;
            
        }
        }
    }




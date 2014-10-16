using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows.Controls;
using System.Windows.Media.Imaging;

using System.Windows.Threading;
using System.Threading;

namespace CreatureKingdom {
    class KnabeConnorCreature :Creature {
        


        Image dogImage;
        BitmapImage leftBitmap;
        BitmapImage rightBitmap;
        private Thread posnThread = null;
        private Boolean goRight = true;
        double dogWidth = 356;
        private Int32 waitTime;
        double incrementSize = 2.0;
        double maxX = 0.0;


        public KnabeConnorCreature(Canvas kingdom, Dispatcher dispatcher, Int32 waitTime = 100)
            : base(kingdom, dispatcher, waitTime) {

                //this.waitTime = waitTime;
                dogImage = new Image();
                dogImage.Width = dogWidth;

                rightBitmap = LoadBitmap(@"SmithMolly\left.png", 356);
                //leftBitmap = LoadBitmap(@"KnabeConnor\dogLeft.gif", 356);

        }

        
        

        public override void Shutdown(){
            if (posnThread != null) {
                posnThread.Abort();
            }
        }



        public override void Place(double x, double y){
            String direction = "right";


            switch (direction) {
                case "right": {
                        dogImage.Source = rightBitmap;
                        goRight = true;
                        break;
                    }
                case "left": {
                        dogImage.Source = leftBitmap;
                        goRight = false;
                        break;
                    }
                default: {
                        dogImage.Source = rightBitmap;
                        goRight = true;
                        break;
                    }
            }

            this.waitTime = 100;
            this.x = x;
            this.y = y;
            kingdom.Children.Add(dogImage);
            dogImage.SetValue(Canvas.LeftProperty, this.x);
            dogImage.SetValue(Canvas.TopProperty, this.y);
            /*
            posnThread = new Thread(Position);

            posnThread.Start();*/
        }

        void Position() {
            while (true) {
                if (goRight) {
                    x += incrementSize;

                    if (x > maxX) {
                        goRight = false;
                        SwitchBitmap(leftBitmap);
                    }
                } else {
                    x -= incrementSize;

                    if (x < 0) {
                        goRight = true;
                        SwitchBitmap(rightBitmap);
                    }
                }

                UpdatePosition();
                Thread.Sleep(waitTime);
            }
        }

        void UpdatePosition() {
            Action action = () => { dogImage.SetValue(Canvas.LeftProperty, x); dogImage.SetValue(Canvas.TopProperty, y); };
            dispatcher.BeginInvoke(action);
        }

        void SwitchBitmap(BitmapImage theBitmap) {
            Action action = () => { dogImage.Source = theBitmap; };
            dispatcher.BeginInvoke(action);
        }

        
    }
}

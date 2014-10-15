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
        private Thread posnThread = null;

        protected Canvas kingdom;
        protected Dispatcher dispatcher;
        protected double x, y;
        Int32 waitTime;
        Boolean paused = false;

        BitmapImage leftBitmap;
        BitmapImage rightBitmap;
        double aquariumWidth = 0.0;
        double fishWidth = 100.0;
        double maxX = 0.0;
        double incrementSize = 2.0;

        private double y;
        private double x;


        public KnabeConnorCreature(Canvas kingdom, Dispatcher dispatcher, Int32 waitTime)
            : base(kingdom, dispatcher, waitTime) {

                this.kingdom = kingdom;
                this.dispatcher = dispatcher;
                this.waitTime = waitTime;
        }



        public override void Shutdown(){
            if (posnThread != null) {
                posnThread.Abort();
            }
        }



        public void Place(double x = 100.0, double y = 200.0, String direction = "right", Int32 wait = 100) {
            switch (direction) {
                case "right": {
                        fishImage.Source = rightBitmap;
                        goRight = true;
                        break;
                    }
                case "left": {
                        fishImage.Source = leftBitmap;
                        goRight = false;
                        break;
                    }
                default: {
                        fishImage.Source = rightBitmap;
                        goRight = true;
                        break;
                    }
            }

            this.waitTime = wait;
            this.x = x;
            this.y = y;
            aquarium.Children.Add(fishImage);
            fishImage.SetValue(Canvas.LeftProperty, this.x);
            fishImage.SetValue(Canvas.TopProperty, this.y);

            posnThread = new Thread(Position);

            posnThread.Start();
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
            Action action = () => { fishImage.SetValue(Canvas.LeftProperty, x); fishImage.SetValue(Canvas.TopProperty, y); };
            dispatcher.BeginInvoke(action);
        }

        void SwitchBitmap(BitmapImage theBitmap) {
            Action action = () => { fishImage.Source = theBitmap; };
            dispatcher.BeginInvoke(action);
        }


    }
}

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
        double maxX = 0;

        public KnabeConnorCreature(Canvas kingdom, Dispatcher dispatcher, Int32 waitTime = 100)
            : base(kingdom, dispatcher, waitTime) {

                dogImage = new Image();

                leftBitmap = LoadBitmap(@"KnabeConnor\dogLeft.gif", dogWidth);
                rightBitmap = LoadBitmap(@"KnabeConnor\dogRight.gif", dogWidth);
        }


        public override void Shutdown(){
            if (posnThread != null) {
                posnThread.Abort();
            }
        }

        public override void Place(double x, double y){
            dogImage.Source = rightBitmap;
            goRight = true;

            this.waitTime = 100;
            this.x = x;
            this.y = y;
            kingdom.Children.Add(dogImage);
            dogImage.SetValue(Canvas.LeftProperty, this.x);
            dogImage.SetValue(Canvas.TopProperty, this.y);

            
            posnThread = new Thread(Position);
            posnThread.Start();
        }

        void Position() {
            while (true) {
                if (goRight && !Paused) {
                    x += incrementSize;
                    if (x > maxX) {
                        goRight = false;
                        SwitchBitmap(leftBitmap);
                    }
                } else if (!Paused) {
                    x -= incrementSize;
                    if (x < 0) {
                        goRight = true;
                        SwitchBitmap(rightBitmap);
                    }
                }
                maxX = kingdom.RenderSize.Width - dogWidth;
                Console.WriteLine("KINGDOM in place" + kingdom.RenderSize.Width);

                UpdatePosition();
                Thread.Sleep(10);
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

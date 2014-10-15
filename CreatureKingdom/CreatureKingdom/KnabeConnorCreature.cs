using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows.Controls;
using System.Windows.Media.Imaging;

using System.Windows.Threading;

namespace CreatureKingdom {
    class KnabeConnorCreature :Creature {
        
        public KnabeConnorCreature(Canvas kingdom, Dispatcher dispatcher, Int32 waitTime)
            : base(kingdom, dispatcher, waitTime) {
        }

        public override void Shutdown(){


        }

    }
}

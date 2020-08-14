
using BattleTech;

namespace PitCrew
{

    public static class ModState
    {

        private static SimGameState sgs = null;

        public static SimGameState GetSimGameState()
        {
            if (sgs == null)
            {
                sgs = UnityGameInstance.BattleTechGame.Simulation;
            }
            return sgs;
        }

        public static void Reset()
        {
            // Reinitialize state
            sgs = null;
        }
    }

}



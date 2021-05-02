using Deli.Immediate;
using Deli.Setup;
using Deli.H3VR.Api;
using FistVR;

namespace Deli.GatherButton
{
	public class Gather : DeliBehaviour
	{
		public Gather()	{
            WristMenu.RegisterWristMenuButton("Custom Button!", WristMenuButtonClicked);
        }

        private void WristMenuButtonClicked(FVRWristMenu wristMenu) {
            Logger.LogMessage("Clicked!");
        }
    }
}
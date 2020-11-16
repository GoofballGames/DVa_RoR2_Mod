using BepInEx;
using RoR2;

namespace GoofballGames
{
    [BepInDependency("com.bepis.r2api")]
    //Change these
    [BepInPlugin("com.GoofballGames.DVa_RoR2", "D.Va", "0.1.0")]
    public class DVa_RoR2 : BaseUnityPlugin
    {
        public void Awake()
        {
            //Testing my grasp on the codebase.
            Chat.AddMessage("Croco Smack Activated!");
            On.EntityStates.Croco.Slash.OnEnter += (orig, self) =>
            {
                self.step = 2;
                orig(self);
            };
        }
    }
}
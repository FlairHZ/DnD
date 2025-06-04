using ClassesManagerReborn;
using DnD.Cards;
using Photon.Pun.Demo.Procedural;
using System.Collections;

namespace DnD.Cards
{
    class BarbarianClass : ClassHandler
    {
        internal static string name = "Barbarian";

        public override IEnumerator Init()
        {
            while (!(Barbarian.Card)) yield return null;
            ClassesRegistry.Register(Barbarian.Card, CardType.Entry);
            ClassesRegistry.Register(Sword.Card, CardType.Card, Barbarian.Card);
        }
        public override IEnumerator PostInit()
        {
            yield break;
        }
    }
}
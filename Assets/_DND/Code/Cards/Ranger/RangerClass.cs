using ClassesManagerReborn;
using DnD.Cards;
using System.Collections;

namespace DnD.Cards
{
    class RangerClass : ClassHandler
    {
        internal static string name = "Ranger";

        public override IEnumerator Init()
        {
            while (!(Ranger.Card)) yield return null;
            ClassesRegistry.Register(Ranger.Card, CardType.Entry);
        }
        public override IEnumerator PostInit()
        {
            yield break;
        }
    }
}
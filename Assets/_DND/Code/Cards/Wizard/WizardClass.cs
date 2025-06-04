using ClassesManagerReborn;
using DnD.Cards;
using System.Collections;

namespace DnD.Cards
{
    class WizardClass : ClassHandler
    {
        internal static string name = "Wizard";

        public override IEnumerator Init()
        {
            while (!(Wizard.Card)) yield return null;
            ClassesRegistry.Register(Wizard.Card, CardType.Entry);
        }
        public override IEnumerator PostInit()
        {
            yield break;
        }
    }
}
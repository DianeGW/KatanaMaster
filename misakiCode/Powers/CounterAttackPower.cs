

using System.Linq;
using System.Threading.Tasks;
using BaseLib.Utils; // Required for UnstableShuffle
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

namespace misaki.misakiCode.Powers;


public sealed class CounterAttackPower : misakiPower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.None; 

    public async Task TriggerCounterAttack(PlayerChoiceContext choiceContext)
    {
        CardPile discardPile = PileType.Discard.GetPile(base.Owner.Player);
        var discardAttacks = discardPile.Cards.Where(c => c.Type == CardType.Attack).ToList();
        if (discardAttacks.Count > 0)
        {
            CardModel selectedCard = discardAttacks
                .UnstableShuffle(base.Owner.Player.RunState.Rng.CombatCardSelection)
                .First();
            await CardPileCmd.Add(selectedCard, PileType.Hand);
            selectedCard.EnergyCost.SetUntilPlayed(0);
        }
    }
}
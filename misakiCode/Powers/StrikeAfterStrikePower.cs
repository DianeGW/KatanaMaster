using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

using misaki.misakiCode.Cards;
namespace misaki.misakiCode.Powers;

public sealed class StrikeAfterStrikePower : misakiPower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;
    public override async Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (cardPlay.Card.Tags.Contains(CardTag.Strike) && cardPlay.Card.Owner.Creature == base.Owner)
        {
            if (cardPlay.Target != null && cardPlay.Target.IsAlive)
            {
                await DamageCmd.Attack(cardPlay.Card.DynamicVars.Damage.BaseValue)
                    .FromCard(cardPlay.Card)
                    .Targeting(cardPlay.Target)
                    .WithHitCount(base.Amount) 
                    .WithHitFx("vfx/vfx_attack_slash") 
                    .Execute(choiceContext);
            }
        }
    }
}
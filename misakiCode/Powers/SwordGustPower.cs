using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Hooks;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
namespace misaki.misakiCode.Powers;


public class SwordGustPower : misakiPower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;
    public override bool AllowNegative => false;
    private int GetCurrentStrengthBonus()
    {
        if (base.Amount >= 9) return 3;
        if (base.Amount >= 6) return 2;
        if (base.Amount >= 3) return 1;
        return 0;
    }
    public override async Task AfterPowerAmountChanged(PlayerChoiceContext choiceContext, PowerModel power, decimal amount, Creature? applier, CardModel? cardSource)
    {
        if (this.Amount > 9)
        {
            this.SetAmount(9); 
        }
        await base.AfterPowerAmountChanged(choiceContext, power, amount, applier, cardSource);
    }
    public override decimal ModifyDamageAdditive(Creature? target, decimal amount, ValueProp props, Creature? dealer,
        CardModel? cardSource)
    {
        if (base.Owner != dealer || !props.IsPoweredAttack())
        {
            return 0m;
        }

        return (decimal)GetCurrentStrengthBonus();
    }
}

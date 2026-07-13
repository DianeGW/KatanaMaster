using System;
using System.Linq;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Combat.History.Entries;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.ValueProps;
namespace misaki.misakiCode.Powers;

public sealed class CounterPower : misakiPower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;
 private HashSet<Creature> _attackedMe = new HashSet<Creature>();
    private HashSet<Creature> _brokeMyBlock = new HashSet<Creature>();
    public override async Task AfterDamageReceived(PlayerChoiceContext choiceContext, Creature target, DamageResult result, ValueProp props, Creature? dealer, CardModel? cardSource)
    {
        if (target == base.Owner && props.IsPoweredAttack() && dealer != null)
        {
            if (result.BlockedDamage > 0)
            {
                _attackedMe.Add(dealer);
            }
            if (result.UnblockedDamage > 0)
            {
                _brokeMyBlock.Add(dealer);
            }
        }
    }public override async Task AfterAttack(PlayerChoiceContext choiceContext, AttackCommand command)
    {
        Creature attacker = command.Attacker; 

        if (attacker != null && attacker.Side == CombatSide.Enemy && _attackedMe.Contains(attacker))
        {
            if (!_brokeMyBlock.Contains(attacker) && attacker.IsAlive)
            {
                int totalBuffStacks = base.Owner.Powers
                    .Where(power => power.Type == PowerType.Buff)
                    .Sum(power => power.Amount > 0 ? power.Amount : 1);
                if (totalBuffStacks > 0)
                {
                    await CreatureCmd.TriggerAnim(base.Owner, "Attack", 0.3f);
                    await Cmd.CustomScaledWait(0.1f, 0.25f);
                    
                    await CreatureCmd.Damage(
                        choiceContext, 
                        attacker, 
                        (decimal)totalBuffStacks, 
                        ValueProp.Unpowered, 
                        base.Owner, 
                        null
                    );
                    await PowerCmd.Decrement(this);
                    var counterAttackPower = base.Owner.GetPower<CounterAttackPower>();
                    if (counterAttackPower != null)
                    {
                        await counterAttackPower.TriggerCounterAttack(choiceContext);
                    }
                }
            }
            _attackedMe.Remove(attacker);
            _brokeMyBlock.Remove(attacker);
        }
    }
}
    

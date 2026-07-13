using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.ValueProps;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.Characters;
using misaki.misaki.scenes.vfx;

namespace misaki.misakiCode.Powers;


public class DelayedCut: misakiPower
{

    public override PowerType Type => PowerType.Debuff;
    public override PowerStackType StackType => PowerStackType.Single;
    public override PowerInstanceType InstanceType => PowerInstanceType.Instanced;
    public override async Task AfterSideTurnStart(CombatSide side, IReadOnlyList<Creature> participants, ICombatState combatState)
    {
        if (!participants.Any(p => p.IsPlayer))
        {
            return;
        }
        Slashes.Slash(base.Owner);
        await Cmd.CustomScaledWait(0.1f, 0.25f); 

        await CreatureCmd.Damage(
            new ThrowingPlayerChoiceContext(), 
            base.Owner, 
            6m, 
            ValueProp.Unpowered, 
            null, 
            null
        );
        await PowerCmd.Remove(this);
    }
}

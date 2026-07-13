
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using misaki.misakiCode.Powers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.ValueProps;
using misaki.misaki.scenes.vfx;

namespace misaki.misakiCode.Cards;

public class Tsumukari: misakiCard
{
    public Tsumukari()
        : base(2, CardType.Attack, CardRarity.Rare, TargetType.AllEnemies)
    {
    }
    protected override IEnumerable<DynamicVar> CanonicalVars => new DynamicVar[]
    {
        new DamageVar(5m, ValueProp.Move),
    };
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        IReadOnlyList<Creature> hittableEnemies = base.CombatState.HittableEnemies;
        await CreatureCmd.TriggerAnim(base.Owner.Creature, "Attack", 0.2f);
        if (hittableEnemies.Count > 0)
        {
            Afterimage.PlayOn(hittableEnemies[0]);
        }
        await Cmd.CustomScaledWait(0.3f, 0.3f);
        foreach (Creature enemy in hittableEnemies)
        {
            await CreatureCmd.TriggerAnim(enemy, "Hit", 0.0f); 
        }
        List<Task> shatter = new List<Task>();
        foreach (Creature enemy in hittableEnemies)
        {
            if (enemy.Block > 0)
            {
                shatter.Add(CreatureCmd.LoseBlock(enemy, enemy.Block));
            }
            if (enemy.HasPower<ThornsPower>()) shatter.Add(PowerCmd.Remove<ThornsPower>(enemy));
            if (enemy.HasPower<CurlUpPower>()) shatter.Add(PowerCmd.Remove<CurlUpPower>(enemy));
            if (enemy.HasPower<HardToKillPower>()) shatter.Add(PowerCmd.Remove<HardToKillPower>(enemy));
            if (enemy.HasPower<HardenedShellPower>()) shatter.Add(PowerCmd.Remove<HardenedShellPower>(enemy));
            if (enemy.HasPower<IntangiblePower>()) shatter.Add(PowerCmd.Remove<IntangiblePower>(enemy));
            if (enemy.HasPower<PlatingPower>()) shatter.Add(PowerCmd.Remove<PlatingPower>(enemy));
            if (enemy.HasPower<SoarPower>()) shatter.Add(PowerCmd.Remove<SoarPower>(enemy));
            if (enemy.HasPower<SlipperyPower>()) shatter.Add(PowerCmd.Remove<SlipperyPower>(enemy));
            if (enemy.GetPower<FlutterPower>() is FlutterPower f) for (int i = f.Amount; i > 1; i--) shatter.Add(PowerCmd.Decrement(f));
        }
        await Task.WhenAll(shatter);
        await Cmd.CustomScaledWait(1.2f, 1.2f);
        foreach (Creature item in hittableEnemies)
        {
            NCombatRoom.Instance?.CombatVfxContainer.AddChildSafely(NGroundFireVfx.Create(item));
        }
        await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue)
            .FromCard(this)
            .TargetingAllOpponents(base.CombatState)
            .WithHitCount(3)
            .WithAttackerAnim(null, 0f)
            .Execute(choiceContext);
    }
    protected override void OnUpgrade()
    {
        base.DynamicVars.Damage.UpgradeValueBy(2m);
    }
}
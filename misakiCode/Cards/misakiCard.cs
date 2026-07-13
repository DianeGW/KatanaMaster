using BaseLib.Abstracts;
using BaseLib.Extensions;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using misaki.misakiCode.Character;
using misaki.misakiCode.Extensions;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Cards.Holders;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using misaki.misaki.scenes.vfx;

namespace misaki.misakiCode.Cards;

[Pool(typeof(misakiCardPool))]
public abstract class misakiCard(int cost, CardType type, CardRarity rarity, TargetType target) :
    CustomCardModel(cost, type, rarity, target)
{
    //Image size:
    //Normal art: 1000x760 (Using 500x380 should also work, it will simply be scaled.)
    //Full art: 606x852
    public override string CustomPortraitPath => $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".BigCardImagePath();

    //Smaller variants of card images for efficiency:
    //Smaller variant of fullart: 250x350
    //Smaller variant of normalart: 250x190

    //Uses card_portraits/card_name.png as image path. These should be smaller images.
    public override string PortraitPath => $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".CardImagePath();
    public override string BetaPortraitPath => $"beta/{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".CardImagePath();
    public void DoFlash()
    {
        if (NCombatRoom.Instance != null)
        {
            var hand = NCombatRoom.Instance.Ui.Hand;
            if (hand.GetCardHolder(this) is NHandCardHolder holder)
            {
                holder.Flash();
            }
        }
    }
    protected async Task DealPierceDamage(PlayerChoiceContext choiceContext, Creature target, int damageAmount, int hitCount = 1)
    {
        bool hadBlock = target != null && target.Block > 0;

        await DamageCmd.Attack(damageAmount)
            .FromCard(this)
            .Targeting(target)
            .WithHitCount(hitCount)
            .Execute(choiceContext);
        if (hadBlock && target != null && target.Block == 0)
        {
            await Cmd.CustomScaledWait(0.2f, 0.2f);
            await DamageCmd.Attack(damageAmount)
                .FromCard(this)
                .Targeting(target)
                .WithHitCount(hitCount)
                .Execute(choiceContext);
        }
    }
}

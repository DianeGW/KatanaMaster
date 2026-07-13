using MegaCrit.Sts2.addons.mega_text;
using Godot;
namespace misaki.misakiCode.Character;

public partial class MisakiEnergyLabel : MegaLabel
{
    public override void _Ready()
    {
        MinFontSize = 20;
        MaxFontSize = 36;
        base._Ready();
    }
}

using Godot;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.TestSupport;

namespace misaki.misaki.scenes.vfx;

public static class Afterimage
{
    private const string Root = "res://misaki/scenes/vfx/";

    public static Node2D? PlayOn(Creature target)
    {
        var node = Create(target, "afterimage");
        
        if (node != null)
        {
            NCombatRoom.Instance?.CombatVfxContainer.AddChildSafely(node);
            
            var anim = node.GetNodeOrNull<AnimationPlayer>("Node2D/Sprite2D/AnimationPlayer");
            
            anim?.Play("afterimage");
        }
        
        return node;
    }
    private static Node2D? Create(Creature target, string name)
    {
        if (TestMode.IsOn) return null;
        string path = $"{Root}{name}.tscn";
        var scene = GD.Load<PackedScene>(path);
        if (scene == null)
        {
            GD.PushError($"[Afterimage] Failed to load VFX: {path}");
            return null;
        }
        var node = scene.Instantiate<Node2D>();
        var creatureNode = target.GetCreatureNode();
        if (creatureNode != null)
        {
            node.GlobalPosition = creatureNode.VfxSpawnPosition;
        }
        return node;
    }
}
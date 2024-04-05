using Flax.Build;

public class BlenderLinkTarget : GameProjectEditorTarget
{
    /// <inheritdoc />
    public override void Init()
    {
        base.Init();

        // Reference the modules for editor
        Modules.Add("BlenderLink");
    }
}
